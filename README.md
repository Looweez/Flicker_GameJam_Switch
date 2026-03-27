Overview:
Flicker is a high-tension, atmospheric horror maze-runner developed in Unity. Players must navigate an ever-shifting dark void while being pursued by a relentless,
invisible entity. Your only defense is a light that freezes time.

Core Mechanics:
The Light: Press Spacebar to emit a 3-second burst of light. This freezes the game world, allowing you to plan your route and spot pickups.

The Monster: The monster is only audible (breathing) and visible (faint glow) when the lights are off. Turning the lights on reveals the maze but
hides the hunter.

Ramping Difficulty: Every level cleared increases the monster's speed (up to a balanced cap of 2.4), forcing faster decision-making.

Installation & Run Instructions
To Play (Build):
Download the latest release from the Releases tab.

Extract the .zip file to your preferred location.

Run Flicker.exe.

To Develop (Unity):
Clone this repository:

git clone https://github.com/Looweez/Flicker.git

Open the project in Unity 2022.3 LTS (or newer).

Ensure the Universal Render Pipeline (URP) is active for the Light2D effects to render correctly.

Open Scenes/MainMenu and press Play.

Controls:
WASD / Arrow Keys: Movement

Spacebar: Toggle Light(Time-Stop)

Mouse: Navigate UI Menus

Credits:
Game Design & Programming: Louise Lee

Art: Hand-drawn backgrounds and sprites by Louise Lee

Audio: https://pixabay.com/sound-effects/film-special-effects-ruler-flick-98267/, https://pixabay.com/sound-effects/film-special-effects-ui-button-click-5-327756/, https://pixabay.com/sound-effects/film-special-effects-fire-magic-5-378639/, https://pixabay.com/sound-effects/film-special-effects-sci-fi-sound-effect-designed-circuits-hum-24-200825/, https://pixabay.com/sound-effects/household-light-switch-382712/, https://pixabay.com/sound-effects/horror-ghostbreath1-107236/

Development Assistant: Gemini (Google AI) - C# scripting and logic refinement.

AI Ethics & Originality Statement

AI Collaborator: Google Gemini (Generative AI)

1. Statement of Originality
The core concept, game design, and creative direction of Flicker are the original work of the developer. The unique mechanic where the monster is only visible/audible when the player’s light is deactivated was conceived by the developer. AI was utilized as a technical consultant to translate these creative requirements into functional C# code within the Unity Engine.

2. Fair Use & Transformation
All code snippets provided by the AI collaborator were based on standard Unity API documentation and public-domain programming patterns. These suggestions were not used "as-is" but were transformed, debugged, and integrated by the developer to fit the specific architectural needs of the project.

3. Ethical Use of AI
Transparency: The use of AI has been documented throughout the development process, including a dedicated section in the README.md and refinements-changes.md files.

Human Oversight: Every line of code generated via AI was reviewed for logic, security, and performance. The developer maintained full control over the project's "Save/Load" logic (PlayerPrefs) and scene transitions.

Asset Integrity: While AI assisted in the logic for audio triggers and sprite toggling, all visual assets (pixel art) and audio files were sourced or created independently by the developer, ensuring no copyright infringement via AI-generated imagery.

4. Conclusion
The developer asserts that Google Gemini functioned as a "Pair Programmer." This collaboration allowed the developer to focus on high-level game design and atmospheric balancing while using the AI to overcome technical hurdles and boilerplate implementation. The final product is a result of human-led creative decision-making.
