# Virtual Eye Clinic Walkthrough

This project is an interactive 3D smartphone application developed in Unity. It serves as a virtual tour of an eye clinic, demonstrating core interactive environment design, state management, basic animations, haptic feedback, and user interface integration.

## Features

- **Interactive 3D Environment**: Walk around a fully modeled eye clinic using intuitive on-screen touch controls.
- **First-Person Navigation**: Immersive head-bobbing animation combined with smooth camera rotation gives a realistic first-person feel.
- **Object Interaction**: Explore and interact with 5 core pieces of eye clinic equipment (Autorefractor, Doctor's Desk, Snellen Eye Chart, Retina Image, Slit Lamp).
- **Haptic & Visual Polish**: Tapping an object provides mobile haptic vibration feedback and triggers informative UI pop-ups and custom animations.
- **State Management (Save/Load)**: The game actively tracks the equipment you have discovered. It saves this progress persistently using `PlayerPrefs`, culminating in a congratulatory completion popup once all items are found.
<img width="979" height="552" alt="Screenshot 2026-05-19 at 5 27 35 PM" src="https://github.com/user-attachments/assets/a75f6aee-9d37-47e1-9a62-f4cc8a3203cb" />

## Architecture

The project adheres to clean architecture principles:
- **Core Systems**: Managed by singletons like `GameManager` (handles save states and scene transitions) and `AudioManager` (handles SFX and BGM).
- **Modularity**: Interaction logic is decoupled via the `IInteractable` interface. Each piece of equipment manages its own specific interactions and animations (e.g., `SlitLamp.cs`, `Autorefractor.cs`), avoiding monolithic scripts.
- **UI Management**: `UIManager.cs` acts as a central hub for all interface operations, handling popups, crosshairs, and dynamic messaging cleanly.

## Challenges Faced

1. **Input Handling**: Differentiating between camera rotation touches and UI interaction touches required careful tracking of Touch ID instances.
2. **State Persistence**: Ensuring the game accurately remembered exactly which objects were interacted with across multiple sessions required a robust HashSet linked to Unity's `PlayerPrefs`.
<img width="1685" height="976" alt="Screenshot 2026-05-19 at 5 26 38 PM" src="https://github.com/user-attachments/assets/41523137-f7b1-46b5-af65-46cb4cf25414" />

## Future Improvements

Given more time, the project could be expanded into a richer VR or AR experience by adding:
- **AR Foundation**: Overlaying the interactive equipment in the real world.
- **Voice Instructions**: Implementing spatial audio guides or dictation for a truly accessible clinic tour.
- **Complex Shaders**: Utilizing customized shaders for the Slit Lamp light beams to create an ultra-realistic rendering.
