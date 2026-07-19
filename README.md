
# Labyrinth Crawler: Multiplayer Terminal RPG

## Overview

A multiplayer, terminal-based RPG engine written in C# (.NET 8/9). The project features a custom-built, authoritative TCP/IP server and a decoupled client, utilizing standard console rendering. 

The primary goal of this project was to design a highly modular, Object-Oriented game engine without relying on external frameworks (like Unity) or runtime type identification (RTTI/type casting). It strictly enforces **SOLID** principles and utilizes an extensive set of **Gang of Four (GoF) Design Patterns** to handle procedural generation, reactive enemies, and complex combat mechanics.

## Design Patterns & Architecture

The game architecture is built upon the **Model-View-Controller (MVC)** paradigm, strictly separating the authoritative game state (Model) from the network/input handlers (Controller) and the console rendering (View).

To avoid anti-patterns and `if/switch` type-checking, the following design patterns are heavily utilized:

*   **Abstract Factory:** (`IThemeFactory`) Drives the procedural generation by swapping biomes (Overworld, Nether, End), creating theme-specific enemies, items, and artifacts.
*   **Builder:** (`IMapBuilder` & `MapDirector`) Constructs the dungeon layout step-by-step (adding walls, rooms, corridors, loot, and enemies).
*   **Strategy:** (`IMapGenerationStrategy`) Defines different algorithmic approaches for generating map layouts based on the selected theme.
*   **Visitor:** (`IAttackVisitor`) Elegantly resolves combat damage calculations between different weapon types (Heavy, Light, Magic) and attack styles (Normal, Stealth, Magic) without type casting.
*   **Decorator:** (`WeaponDecorator`) Dynamically attaches stat modifiers (e.g., *Strong*, *Agile*, *Unlucky*) to items at runtime.
*   **Observer:** (`ISoundPublisher`, `ISpeciesPublisher`) Manages an entity-agnostic event system. Used for BFS-based sound propagation (noise alerts nearby enemies) and enemies morale changes upon faction member death.
*   **Chain of Responsibility:** (`BaseHandler`) Intercepts and processes user input commands hierarchically.
*   **Singleton:** (`GameLogger`) Provides centralized, thread-safe event logging.

## Network & Synchronization

*   **TCP Sockets:** Implemented a multi-threaded TCP server (`TcpListener`) handling up to 9 concurrent client connections.
*   **JSON Serialization:** Real-time state broadcasting using `System.Text.Json`. 
*   **Thread Safety:** Utilizes `lock` mechanisms and `ConcurrentQueue` to prevent race conditions during simultaneous player inputs and server tick updates.

## Configuration

Before starting the server or playing locally, you can customize the game via the `config.json` file located in the working directory.

```json
{
  "PlayerName": "Hero",
  "LogDirectory": "./Logs",
  "Theme": "Nether"
}
```
*   `Theme`: Determines the map generation algorithm, loot tables, and enemy types (Available: `Overworld`, `Nether`, `End`).
*   `LogDirectory`: Output path for the detailed combat and event diary saved after the game ends.

## Interactive Commands

| Command | Action |
| :--- | :--- |
| `W`, `A`, `S`, `D` | Move the character across the grid. |
| `E` / `Q` | Interact with the environment (Pick up / Drop items). |
| `K`, `L` | Equip items to Left or Right hand (supports two-handed weapons). |
| `B`, `N`, `M` | Execute combat maneuvers (Normal, Stealth, Magic attacks). |
| `J` | Toggle the Event Diary. |
| `ESC` | Exit the game. |

## Build and Run

To run the project, ensure you have the .NET SDK (8.0 or 9.0) installed. 

**1. Clone the repository and build:**
```bash
git clone https://github.com/YourUsername/LabyrinthCrawler.git
cd LabyrinthCrawler
dotnet build
```

**2. Start the game:**
Run the executable. You will be prompted via the console to choose the launch mode:
*   **Server Mode `(S)`:** Starts an authoritative TCP server on port `5555`. Generates the map, waits for players to join, and acts as the game host (press `S` again to start the game).
*   **Client Mode `(C)`:** Connects to the server.

*(Alternatively, you can launch with CLI arguments: `dotnet run -- --server 5555` or `dotnet run -- --client 127.0.0.1:5555`)*

> **Note on Rendering:** Currently, the game uses standard console buffer rendering (`Console.SetCursorPosition`). While optimized to redrawn only dynamic tiles, you might notice flickering on certain terminal emulators.
