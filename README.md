# Modular FPS Framework

A highly modular and scalable first-person shooter framework built in Unity, emphasizing clean architecture, designer-friendly workflows, and performance-conscious design.

## Overview

- **Drop-in Weapon Creation**: Create new weapons entirely via `ScriptableObject` configs without additional code.
- **Modular Components**: Weapon behaviors like firing, reloading, recoil, spread, and aiming are separate components managed by a central `Weapon`.
- **Stance-based Movement**: Player stance (walk, sprint, crouch) dynamically influences weapon spread, speed, and animation.
- **Camera & Recoil System**: Dedicated `CameraController` manages FOV transitions and recoil recovery, ensuring smooth and decoupled camera movements.
- **Clean Data & Logic Separation**: Static data (fire rate, recoil settings) is stored in `ScriptableObject` configs, with components handling only runtime logic.

## Media

[![Watch the Demo](https://i.imgur.com/UadfrBZ.jpeg)](https://www.youtube.com/watch?v=zHEBJB2418g)

**[▶ Watch the Demo](https://www.youtube.com/watch?v=zHEBJB2418g)**

## Tech Stack

- Unity (6)
- C# (SOLID principles, event-driven architecture)
- ScriptableObject-based configuration
- Unity Animation Rigging Package
- Unity Input System

## Project Structure

**Weapon System**
- `Weapon.cs` - Central logic and component management.
- `BaseComponent.cs` - Generic base for modular weapon components.
- `FireComponent.cs` - Manages firing logic (single, burst, auto).
- `ReloadComponent.cs` - Controls reload timing and ammo management.
- `RecoilComponent.cs` - Applies recoil effects to weapons and camera.
- `SpreadComponent.cs` - Calculates bullet spread based on stance and aiming.
- `AimComponent.cs` - Manages aiming modes (ADS, HipFire) and related animations.

**Player**
- `PlayerController.cs` - Handles player input and interactions.
- `PlayerMovement.cs` - Manages character physics, movement, and stances.
- `Anchor.cs` - Controls weapon and camera pitch adjustments.

**Camera**
- `CameraController.cs` - Handles FOV changes, recoil application, and camera transitions.

**ScriptableObjects**
- `FireConfig` - Defines firing behavior parameters.
- `ReloadConfig` - Manages ammo counts and reload settings.
- `RecoilConfig` - Adjusts recoil intensity and recovery.
- `SpreadConfig` - Configures bullet spread across different player states.

## Highlights

- `BaseComponent<TConfig>`: Generic pattern allowing components to reference specific ScriptableObject configurations.
- Central `Weapon` class maintains shared weapon state and component references.
- Player movement and stance changes seamlessly integrate with weapon behaviors (e.g., spread adjustments).
- Clear separation of camera handling via `CameraController` and pitch adjustments through the `Anchor`.
- Animation events cleanly bridge animator states with game logic (reload completion, ADS transitions).

## Key Features

- **Hot-Swappable Components**: Easily add/remove behaviors without affecting existing functionality.
- **Data-Driven Design**: Rapidly tweak gameplay parameters via ScriptableObjects.
- **Event-Driven State Management**: Centralized weapon state (`WeaponState`) ensures consistent component communication.
- **Flexible Input System**: Quickly integrate Unity's new Input System or custom solutions.

## Notes

- Ideal for rapid prototyping and scalable for larger projects.
- Extensible design facilitates the addition of complex features (dynamic IK, advanced recoil patterns, multiplayer).
- Currently includes one demo weapon; easily extendable to diverse weapon types.

## Contact

- **Name**: João Yakubets  
- **Email**: unduke2@tuta.io

