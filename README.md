# Divine Meltdown

A low-poly, third-person action-adventure with puzzle and stealth elements, built in **Unity 6**.

Players take on the role of **Aya**, a low-ranking goddess with light powers. Guided by her companion **Tavi**, a small lion with a golden-white mane, she sets out on her own initiative to recover four divine artifacts and restore balance to a world thrown into chaos after the Elder God's death.

| | |
|---|---|
| **Genre** | Third-Person Action Adventure / Puzzle / Stealth |
| **Setting** | Original fantasy world |
| **Art Style** | Low-poly, golden palette (white, yellow, gold) |
| **Engine** | Unity 6000.2.8f1 |
| **Status** | Work in progress: full rebuild |

---

## Project Status: Rebuild in Progress

The project is currently being **rebuilt from the ground up**.

This is not a restart from zero. The goal is a clean, maintainable foundation that the rest of Chapter 1 can be built on. Everything that is worth keeping gets carried over, but only after it has been reviewed and cleaned up.

### What the cleanup means

- **Code and the Character Controller** are the main things being carried over. Every script is reviewed, refactored to current conventions and only then moved into the new structure.
- **Most legacy 3D assets have been discarded.** New models are created as part of the rebuild.
- **Legacy patterns are replaced, not patched.** Examples:
  - Fragmented `SaveAndLoad` and `DontDestroyOnLoad` scripts → a single `GameManager` singleton
  - `BinaryFormatter` saves → JSON via `JsonUtility`
  - Boolean-driven animation states → a `speed`-based Blend Tree

### Migration rule of thumb

A script only lands in `_Project/` when it:

1. Has a single, clear responsibility
2. Follows the coding conventions below
3. Works with the new architecture (GameManager, interaction system, camera setup)

If a script cannot meet these without a full rewrite, it is rewritten instead of migrated.

---

## Getting Started

### Requirements

- **Unity 6000.2.8f1**
- Packages / plugins included in the project:
  - Cinemachine 3.x
  - ProBuilder
  - Dialogue Manager

### Setup

1. Clone the repository
2. Open the project with Unity **6000.2.8f1** via Unity Hub
3. Open the `StartUpMenu` scene and press Play

---

## Folder Structure

```
Assets/
├── _Project/     Custom assets, scripts and scenes created for Divine Meltdown
└── ThirdParty/   External assets and plugins
```

Everything made for this game goes into `_Project/`. Third-party content stays isolated in `ThirdParty/` and is not modified directly.

---

## Scenes

| Scene | Purpose |
|---|---|
| `StartUpMenu` | Main menu: New Game, Continue, Options, Credits |
| `Tutorial` | Introductory island: Tavi intro, basic mechanics taught through play |
| `MainGame` | Main island: puzzles that teach the boss fight mechanics |
| `BossArena` | Stealth corridor and Throne Room boss fight |
| `Credits` | Ending cutscene |

---

## Core Architecture

### GameManager

A singleton that persists across all scenes. It restores the player state and tracks game progress through a unified `GameData` class:

- Player position, health, mana and scene index
- Completed puzzles, open doors, watched cutscenes, boss defeated

### Save System

- Stored as JSON via `JsonUtility`
- Auto-save is triggered at Meditation Spots

### Player & Camera

- Movement uses a `CharacterController`; ground detection via `controller.isGrounded`
- Souls-like third-person camera built with **Cinemachine 3.x** (`CinemachineCamera`)
- The `CameraTarget` is an empty GameObject at **scene root**, not a child of the Player. It rotates independently via mouse input, and the `CinemachineCamera` tracks it with `Rotation Control: None`. This decoupling prevents rotation feedback loops.

### Animation

- Locomotion runs on a Blend Tree driven by a `speed` float
- States: Locomotion, Jump, Fall, Landing
- Import setting for landing animations: `Root Transform Position (Y)` → `Bake Into Pose: OFF`, `Based Upon: Feet`

### Interaction

- All interactions are **raycast-based**; no proximity triggers for interacting
- Interactable objects implement the `IInteractable` interface
- An interaction prompt appears when the raycast hits a valid target

---

## Controls

| Action | Input |
|---|---|
| Move | `W` `A` `S` `D` (relative to camera) |
| Camera | Mouse |
| Sprint | `Left Shift` |
| Jump | `Space` |
| Interact | `E` |
| Light Power / QTE | `F` |

---

## Coding Conventions

- **All code is written in English**: names, strings, logs
- One responsibility per class
- Prefer modern Unity patterns over legacy ones
- For packages like Cinemachine, follow the official documentation for the installed version

---

## Git Workflow

- `main` is the only long-lived branch
- New work happens on **feature branches** and is merged back into `main`

---

## Roadmap

1. Rebuild the dialogue system (multi-speaker, branching, Tavi telepathy overlay)
2. Refine existing puzzles (Pin NPC quest chain, Pressure Plate feedback)
3. Design and implement new puzzles (Mirror Light, NPC Quest Chain, Distraction)
4. Mana / Light Power system
5. Boss fight redesign (stealth corridor + Throne Room)
6. Tavi model setup
7. NPC models
8. New UI (Mana bar, Health, Interaction prompts)
9. Meditation Spot system (restore + auto-save)
10. Artifact system design
11. Rebuild ending cutscene
12. World building and lore
