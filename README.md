# Stealth Agent: Obstacle Navigation Game

**Stealth Agent** is a console-based game developed for QUT's IFB104 "Building IT Systems" (Semester 1, 2023). In this game, you guide an agent through a grid-based environment populated with various obstacles—such as Guards, Fences, Sensors, Cameras, and Laser Barriers—that can compromise safe movement. The game features interactive obstacle creation, safe direction analysis, and pathfinding using a breadth-first search (BFS) algorithm.

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Technologies Used](#technologies-used)
- [Installation](#installation)
- [Usage](#usage)
- [Game Mechanics](#game-mechanics)
- [Code Structure](#code-structure)


## Overview

Stealth Agent challenges you to safely navigate a hostile environment by adding and managing obstacles on a grid. The agent can query its current position for safe directions, view a map of obstacles, or calculate a safe path to a target location—all while avoiding detection or compromise by the obstacles around it.

## Features

- **Dynamic Obstacle Management:**  
Add various types of obstacles including:
- **Guards:** Compromise a single grid point.
- **Fences:** Occupy a straight line between two points.
- **Sensors:** Detect within a circular range.
- **Cameras:** Monitor a cone of vision in one of the cardinal directions (n, s, e, w).
- **Laser Barriers:** Project a directional beam for a specified range.

- **Interactive Console Interface:**  
A text-based menu system for adding obstacles, checking safe directions, displaying a portion of the obstacle map, and finding a safe path using BFS.

- **Safe Direction Analysis:**  
Evaluate the agent’s current position and determine which of the four cardinal directions (N, S, E, W) are safe to move.

- **Pathfinding:**  
Use a breadth-first search algorithm to compute and display a safe path from the agent’s current location to a specified objective.

## Technologies Used

- **C#** – The primary programming language.
- **.NET** – For compiling and running the console application.
- **Object-Oriented Programming** – Implementing obstacles using interfaces and classes.
- **Algorithms** – Breadth-first search (BFS) for pathfinding.

## Installation

### Using Visual Studio

1. **Create a New Project:**
- Open Visual Studio.
 - Create a new **Console App (.NET Framework)** or **Console App (.NET Core)** project in C#.

2. **Add the Code:**
 - Replace the contents of `Program.cs` with your game code.

4. **Build and Run:**
- Build the solution and run the application.

4. **Usage:**
When you run the game, a menu with several options is displayed in the console:

- g: Add a Guard obstacle.
- f: Add a Fence obstacle.
- s: Add a Sensor obstacle.
- c: Add a Camera obstacle.
- l: Add a Laser Barrier.
- d: Show safe directions from your current location.
- m: Display a portion of the obstacle map.
- p: Find a safe path to your objective.
- x: Exit the game.

Simply type the corresponding letter and follow the on-screen prompts to enter coordinates and other parameters.

5. **Game Mechanics:**

Obstacles & Compromise:
- Each obstacle implements the IObstacle interface to determine whether a given point on the grid is compromised.

Safe Direction Analysis:
- The game checks the four cardinal directions from the agent's position to identify safe moves.

Pathfinding:
- A breadth-first search (BFS) algorithm is used to find a safe path from the agent's start location to the specified objective.


6. **Code Structure:**

Interfaces & Classes:

- IObstacle: Defines a contract for obstacles.
- Guard, Fence, Sensor, Camera, and LaserBarrier: Concrete implementations with unique logic.

Program Class:

- Contains the main game loop, menu display, user interaction methods, and pathfinding logic.


