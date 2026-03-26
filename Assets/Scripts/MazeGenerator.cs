using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine.AI;
using System.Collections;

public class MazeGenerator : MonoBehaviour
{
    public NavMeshSurface navMeshSurface;
    public GameObject monsterPrefab;
    public SpriteRenderer backgroundSR;
    
    [Header("Maze Dimensions")]
    public int width = 21;
    public int height = 21; 

    [Header("Path Settings")]
    [Range(0, 0.3f)]
    public float pathAbundance = 0.1f;

    [Header("References")]
    public Tilemap tilemap;
    public TileBase wallTile;
    public GameObject entrancePrefab;
    public GameObject exitPrefab;
    public GameObject pickupPrefab;

    private int[,] maze;

    void Start()
    {

        StartCoroutine(SetupMazeRoutine());
    }

    IEnumerator SetupMazeRoutine()
    {
        // 1. Generate the maze first
        GenerateMaze(); 
    
        // Wait for the tilemap to finish placing tiles
        yield return new WaitForEndOfFrame();
    
        // 2. Place the player, monster, and items
        PlaceObjects();
    
        // 3. Fit the camera and background once
        FitCameraToMaze();
        //ScaleBackground();  //removed
    }
    
    /*void ScaleBackground()
    {
        if (backgroundSR == null) return;

        // 1. Get the actual world-space size of the maze (handles the 1.5 cell size automatically)
        tilemap.CompressBounds();
        Bounds bounds = tilemap.localBounds;

        // 2. Center the background on the actual center of the tiles
        backgroundSR.transform.position = new Vector3(bounds.center.x, bounds.center.y, 5f);

        // 3. Get the sprite's size in its own local units
        float spriteWidth = backgroundSR.sprite.bounds.size.x;
        float spriteHeight = backgroundSR.sprite.bounds.size.y;

        // 4. Calculate scale by dividing the Maze Bounds by the Sprite's Size
        // We add +3f here as a "safety buffer" so the background overlaps the edges slightly
        float targetWidth = bounds.size.x + 3f;
        float targetHeight = bounds.size.y + 3f;

        backgroundSR.transform.localScale = new Vector3(targetWidth / spriteWidth, targetHeight / spriteHeight, 1f);
    }*/
    
    void FitCameraToMaze()
    {
        float padding = 1.5f; // Reduced padding so it's not too tiny

        // 1. Tell the tilemap to recalculate its area based on the tiles you just drew
        tilemap.CompressBounds();
        Bounds bounds = tilemap.localBounds;

        // 2. Calculate size based on the ACTUAL bounds of the tiles
        float verticalSize = (bounds.size.y / 2f) + padding;
        float horizontalSize = ((bounds.size.x / 2f) + padding) / Camera.main.aspect;

        // 3. Set the zoom
        Camera.main.orthographicSize = Mathf.Max(verticalSize, horizontalSize);

        // 4. Center the camera on the ACTUAL center of the bounds
        // This fixes the "shifted corner" issue
        Camera.main.transform.position = new Vector3(bounds.center.x, bounds.center.y, -10f);
    }

    // This is the function that was missing!
    void GenerateMaze()
    {
        maze = new int[width, height];
        tilemap.ClearAllTiles();

        CarvePath(1, 1);
        BraidMaze();
        DrawWalls();
    }

    void CarvePath(int x, int y)
    {
        maze[x, y] = 1;
        Vector2Int[] dirs = { Vector2Int.up * 2, Vector2Int.right * 2, Vector2Int.down * 2, Vector2Int.left * 2 };
        Shuffle(dirs);

        foreach (var dir in dirs)
        {
            int nx = x + dir.x;
            int ny = y + dir.y;
            if (nx > 0 && nx < width - 1 && ny > 0 && ny < height - 1 && maze[nx, ny] == 0)
            {
                maze[x + dir.x / 2, y + dir.y / 2] = 1;
                CarvePath(nx, ny);
            }
        }
    }

    void BraidMaze()
    {
        for (int x = 1; x < width - 1; x++)
        {
            for (int y = 1; y < height - 1; y++)
            {
                if (maze[x, y] == 0 && Random.value < pathAbundance)
                {
                    maze[x, y] = 1;
                }
            }
        }
    }

    void DrawWalls()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (maze[x, y] == 0)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), wallTile);
                }
            }
        }
    }
    
    void PlaceObjects()
    {
        
        Vector3 offset = new Vector3(0.5f, 0.5f, 0);
        // Use the Tilemap's grid to get the exact center of a tile
        // Since your cell size is 1.5, we need to use the tilemap's center logic
    
        // 1. Force the corner locations to be path tiles so the player doesn't spawn in a wall
        maze[1, 1] = 1; 
        maze[width - 2, height - 2] = 1;
        maze[1, height - 2] = 1;
        maze[width - 2, 1] = 1;

        // Redraw tiles to clear any wall that was at the entrance
        DrawWalls();

        // Define positions (still using the logic to rotate corners)
        Vector3Int startCell = (GameManager.currentLevel % 2 == 1) ? new Vector3Int(1, 1, 0) : new Vector3Int(1, height - 2, 0);
        Vector3Int exitCell = (GameManager.currentLevel % 2 == 1) ? new Vector3Int(width - 2, height - 2, 0) : new Vector3Int(width - 2, 1, 0);

        // Get world positions for centering
        Vector3 spawnPos = tilemap.GetCellCenterWorld(startCell);
        Vector3 exitPos = tilemap.GetCellCenterWorld(exitCell);

        // 2. Spawn and align
        GameObject entrance = Instantiate(entrancePrefab, spawnPos, Quaternion.identity);
        Instantiate(exitPrefab, exitPos, Quaternion.identity);
        Instantiate(monsterPrefab, exitPos, Quaternion.identity);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = spawnPos;
            // Reset physics completely
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null) {
                rb.position = spawnPos;
                rb.linearVelocity = Vector2.zero;
            }
        }
    
        // Spawn 3 Pickups in random PATH locations (not walls)
        for (int i = 0; i < 3; i++)
        {
            Vector3Int pickupCell = GetRandomPathTile();
            Instantiate(pickupPrefab, tilemap.CellToWorld(pickupCell) + offset, Quaternion.identity);
        }
    }
    
    
    Vector3Int GetRandomPathTile()
    {
        List<Vector3Int> pathTiles = new List<Vector3Int>();

        for (int x = 1; x < width - 1; x++)
        {
            for (int y = 1; y < height - 1; y++)
            {
                if (maze[x, y] == 1)
                {
                    pathTiles.Add(new Vector3Int(x, y, 0));
                }
            }
        }

        int randomIndex = Random.Range(0, pathTiles.Count);
        return pathTiles[randomIndex];
    }

    void Shuffle<T>(T[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int rnd = Random.Range(0, i + 1);
            T temp = array[i];
            array[i] = array[rnd];
            array[rnd] = temp;
        }
    }
}
