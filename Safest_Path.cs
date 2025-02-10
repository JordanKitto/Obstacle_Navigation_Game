using System;

using System.Collections.Generic;

using System.Linq;



/// <summary>

/// Represents an obstacle in the game world that can compromise an agent's location.

/// </summary>

public interface IObstacle

{

    /// <summary>

    /// Determines whether a given point is compromised by the obstacle.

    /// </summary>

    bool IsPointCompromised(int x, int y);

}



/// <summary>

/// Represents a guard in the game world.

/// </summary>

public class Guard : IObstacle

{

    /// <summary>

    /// Gets or sets the X-coordinate of the guard.

    /// </summary>

    public int X { get; set; }



    /// <summary>

    /// Gets or sets the Y-coordinate of the guard.

    /// </summary>

    public int Y { get; set; }



    /// <summary>

    /// Checks if a given point is compromised by the guard's presence.

    /// </summary>

    public bool IsPointCompromised(int x, int y)

    {

        return X == x && Y == y;

    }

}



/// <summary>

/// Represents a fence in the game world.

/// </summary>

public class Fence : IObstacle

{

    public int StartX { get; set; }

    public int StartY { get; set; }

    public int EndX { get; set; }

    public int EndY { get; set; }



    // Determines if a point lies on the fence

    public bool IsPointOnFence(int x, int y)

    {

        // Check for vertical fence

        if (StartX == EndX && x == StartX)

        {

            int minY = Math.Min(StartY, EndY);

            int maxY = Math.Max(StartY, EndY);

            return y >= minY && y <= maxY;

        }

        // Check for horizontal fence

        if (StartY == EndY && y == StartY)

        {

            int minX = Math.Min(StartX, EndX);

            int maxX = Math.Max(StartX, EndX);

            return x >= minX && x <= maxX;

        }



        return false;

    }



    // Checks if a given point is compromised by the fence's presence

    public bool IsPointCompromised(int x, int y)

    {

        return IsPointOnFence(x, y);

    }

}



/// <summary>

/// Represents a camera in the game world with a vision directed in one of the cardinal directions (N, S, E, W).

/// </summary>

public class Camera : IObstacle

{

    public int X { get; set; }

    public int Y { get; set; }

    public char Direction { get; set; } // nsew



    // Determines if a point is within the camera's vision based on its direction

    public bool IsPointInVision(double x, double y)

    {

        switch (Direction)

        {

            case 'n':

                if (y <= Y && x >= X - (Y - y) && x <= X + (Y - y))

                    return true;

                break;



            case 's':

                if (y >= Y && x >= X - (y - Y) && x <= X + (y - Y))

                    return true;

                break;



            case 'e':

                if (x >= X && y >= Y - (x - X) && y <= Y + (x - X))

                    return true;

                break;



            case 'w':

                if (x <= X && y >= Y - (X - x) && y <= Y + (X - x))

                    return true;

                break;

        }



        return false;

    }



    // Checks if a given point is compromised by the camera's vision

    public bool IsPointCompromised(int x, int y)

    {

        return IsPointInVision(x, y);

    }

}





// <summary>

/// Represents a sensor in the game world with a circular detection range.

/// </summary>

public class Sensor : IObstacle

{

    public int X { get; set; }

    public int Y { get; set; }

    public double Range { get; set; }



    // Determines if a point is within the sensor's range

    public bool IsWithinRange(int x, int y)

    {

        double distance = Math.Sqrt(Math.Pow(X - x, 2) + Math.Pow(Y - y, 2));

        return distance <= Range;

    }



    // Checks if a given point is compromised by the sensor's detection range

    public bool IsPointCompromised(int x, int y)

    {

        return IsWithinRange(x, y);

    }

}



/// <summary>

/// Represents a laser barrier in the game world with a direction and range.

/// </summary>

public class LaserBarrier : IObstacle

{

    public int X { get; set; }

    public int Y { get; set; }

    public char Direction { get; set; }

    public int Range { get; set; }



    // Checks if a given point is compromised by the laser barrier

    public bool IsPointCompromised(int x, int y)

    {

        switch (Direction)

        {

            case 'n':

                return x == X && y < Y && y >= Y - Range;

            case 's':

                return x == X && y > Y && y <= Y + Range;

            case 'e':

                return y == Y && x > X && x <= X + Range;

            case 'w':

                return y == Y && x < X && x >= X - Range;

        }

        return false;

    }

}





/// <summary>

/// The main class containing the game loop, obstacle management, and user interactions.

/// </summary>

class Program

{

    // Collections to store different types of obstacles

    static List<IObstacle> obstacles = new List<IObstacle>();

    static List<Guard> guards = new List<Guard>();

    static List<Fence> fences = new List<Fence>();

    static List<Sensor> sensors = new List<Sensor>();

    static List<Camera> cameras = new List<Camera>();





    /// <summary>

    /// The main entry point of the application.

    /// </summary>

    static void Main()

    {

        // Populate obstacles list with your existing obstacles

        obstacles.AddRange(guards);

        obstacles.AddRange(fences);

        obstacles.AddRange(sensors);

        obstacles.AddRange(cameras);



        // Game loop to interact with the user

        char choice;

        do

        {

            DisplayMenu();// Show the main menu



            choice = GetMenuChoice();// Get the user's choice



            // Handle the user's choice

            switch (choice)

            {

                case 'g':

                    AddGuard();

                    break;

                case 'f':

                    AddFence();

                    break;

                case 's':

                    AddSensor();

                    break;

                case 'c':

                    AddCamera();

                    break;

                case 'd':

                    var results = ShowSafeDirections();

                    DisplaySafeDirectionResults(results.Compromised, results.SafeDirections);

                    break;

                case 'm':

                    DisplayObstacleMap();

                    break;

                case 'p':

                    FindSafePath();

                    break;

                case 'l':

                    AddLaserBarrier();

                    break;

                case 'x':

                    return; // Exit the game loop and terminate the application

            }

        } while (choice != 'x');

    }



    /// <summary>

    /// Displays the main menu to the user.

    /// </summary>

    static void DisplayMenu()

    {

        Console.WriteLine("Select one of the following options:");

        Console.WriteLine("g) Add 'Guard' obstacle");

        Console.WriteLine("f) Add 'Fence' obstacle");

        Console.WriteLine("s) Add 'Sensor' obstacle");

        Console.WriteLine("c) Add 'Camera' obstacle");

        Console.WriteLine("d) Show safe directions");

        Console.WriteLine("m) Display obstacle map");

        Console.WriteLine("p) Find safe path");

        Console.WriteLine("l) Laser barrier");

        Console.WriteLine("x) Exit");

        Console.Write("Enter code: ");

    }



    /// <summary>

    /// Adds a Guard obstacle based on user input.

    /// </summary>

    static void AddGuard()

    {

        Console.Write("Enter Guard's location X,Y: ");

        while (true)

        {

            string input = Console.ReadLine();

            string[] parts = input.Split(',');

            if (parts.Length != 2 || !int.TryParse(parts[0], out int x) || !int.TryParse(parts[1], out int y))

            {

                Console.WriteLine("Invalid input.");

                Console.Write("Enter Guard's location X,Y: ");

                continue;

            }



            Guard guard = new Guard { X = x, Y = y };

            guards.Add(guard); // Add to the list of guards

            obstacles.Add(guard); // Update obstacles list

            break;

        }

    }



    // Adds a Fence obstacle based on user input

    static void AddFence()

    {

        Console.Write("Enter starting location of the Fence X,Y: ");

        if (TryParseCoordinates(Console.ReadLine(), out var startCoords))

        {

            Console.Write("Enter ending location of the Fence X,Y: ");

            if (TryParseCoordinates(Console.ReadLine(), out var endCoords))

            {

                Fence fence = new Fence

                {

                    StartX = startCoords.Item1,

                    StartY = startCoords.Item2,

                    EndX = endCoords.Item1,

                    EndY = endCoords.Item2

                };

                fences.Add(fence); // Add to the list of fences

                obstacles.Add(fence); // Update obstacles list

            }

            else

            {

                Console.WriteLine("Invalid ending location.");

            }

        }

        else

        {

            Console.WriteLine("Invalid starting location.");

        }

    }



    // Adds a Sensor obstacle based on user input

    static void AddSensor()

    {

        Console.Write("Enter Sensor's location X,Y: ");

        while (true)

        {

            if (TryParseCoordinates(Console.ReadLine(), out var coords))

            {

                Console.Write("Enter the sensor's range (in klicks): ");

                if (double.TryParse(Console.ReadLine(), out double range) && range > 0)

                {

                    Sensor sensor = new Sensor { X = coords.Item1, Y = coords.Item2, Range = range };

                    sensors.Add(sensor); // Add to the list of sensors

                    obstacles.Add(sensor); // Update obstacles list

                    break;

                }

                else

                {

                    Console.WriteLine("Invalid range.");

                }

            }

            else

            {

                Console.WriteLine("Invalid location.");

            }

        }

    }



    /// <summary>

    /// Adds a Camera obstacle based on user input.

    /// </summary>

    static void AddCamera()

    {

        Console.Write("Enter the camera's location X,Y: ");

        if (TryParseCoordinates(Console.ReadLine(), out var coords))

        {

            Console.Write("Enter the direction the camera is facing (nsew): ");

            char direction = GetCameraDirection();

            Camera cam = new Camera { X = coords.Item1, Y = coords.Item2, Direction = direction };

            cameras.Add(cam); // Add to the list of cameras

            obstacles.Add(cam); // Update obstacles list

        }

        else

        {

            Console.WriteLine("Invalid location.");

        }

    }



    /// <summary>

    /// Adds a LaserBarrier obstacle based on user input.

    /// </summary>

    static void AddLaserBarrier()

    {

        Console.Write("Enter the starting location of the Laser Barrier X,Y: ");

        if (TryParseCoordinates(Console.ReadLine(), out var coords))

        {

            Console.Write("Enter the direction the laser is facing (nsew): ");

            char direction = GetCameraDirection();

            Console.Write("Enter the laser's range (in klicks): ");

            if (int.TryParse(Console.ReadLine(), out int range) && range > 0)

            {

                LaserBarrier laser = new LaserBarrier

                {

                    X = coords.Item1,

                    Y = coords.Item2,

                    Direction = direction,

                    Range = range

                };

                obstacles.Add(laser); // Update the main obstacles list

            }

            else

            {

                Console.WriteLine("Invalid range.");

            }

        }

        else

        {

            Console.WriteLine("Invalid location.");

        }

    }





    /// <summary>

    /// Prompts the user to provide a valid camera direction (n, s, e, w).

    /// </summary>

    static char GetCameraDirection()

    {

        while (true)

        {

            string dirString = Console.ReadLine().ToLower();



            if (!string.IsNullOrEmpty(dirString) && "nsew".Contains(dirString[0]))

            {

                return dirString[0];

            }

            else

            {

                Console.WriteLine("Invalid direction. Enter nsew:");

            }

        }

    }

    /// <summary>

    /// Gets the user's menu choice.

    /// </summary>

    static char GetMenuChoice()

    {

        string input = Console.ReadLine();

        if (!string.IsNullOrEmpty(input))

        {

            return input[0];

        }

        return ' '; // default or invalid choice

    }



    /// <summary>

    /// Checks if a point is compromised by any obstacle.

    /// </summary>

    static bool IsPointCompromisedByAnyObstacle(int x, int y)

    {

        return obstacles.Any(obstacle => obstacle.IsPointCompromised(x, y));

    }



    /// <summary>

    /// Displays the possible safe directions or warns the agent if they are compromised.

    /// </summary>

    /// <param name="compromised">Indicates whether the agent's location is compromised.</param>

    /// <param name="directions">List of safe directions for the agent.</param>

    static void DisplaySafeDirectionResults(bool compromised, List<char> directions)

    {

        Console.WriteLine();

        if (compromised)

        {

            // Agent's current position is compromised



            Console.WriteLine("Agent, your location is compromised. Abort mission.");

            return;

        }

        // Check if there are no safe directions available

        if (directions == null || directions.Count == 0)

        {

            Console.WriteLine("You cannot safely move in any direction. Abort mission.");

            return;

        }

        // Display the safe directions for the agent

        Console.WriteLine($"You can safely take any of the following directions: {string.Join("", directions)}");

        Console.WriteLine();

    }

    /// <summary>

    /// Tries to parse a string input into coordinates (X,Y).

    /// </summary>

    /// <param name="input">The string input to be parsed.</param>

    /// <param name="coordinates">The parsed coordinates.</param>

    /// <returns>Returns true if parsing is successful, otherwise false.</returns>

    static bool TryParseCoordinates(string input, out (int, int) coordinates)

    {

        coordinates = (0, 0);

        string[] parts = input.Split(',');

        if (parts.Length == 2 && int.TryParse(parts[0], out int x) && int.TryParse(parts[1], out int y))

        {

            coordinates = (x, y);

            return true;

        }

        return false;

    }



    /// <summary>

    /// Determines which directions are safe for the agent to move in.

    /// </summary>

    /// <returns>Returns a tuple indicating if the agent is compromised and a list of safe directions.</returns>

    static (bool Compromised, List<char> SafeDirections) ShowSafeDirections()

    {

        Console.Write("Enter your current location X,Y: ");

        if (TryParseCoordinates(Console.ReadLine(), out var coords))

        {

            List<char> safeDirections = new List<char>();

            // Check the surroundings (North, South, East, West) to determine safe directions

            // North

            if (!IsPointCompromisedByAnyObstacle(coords.Item1, coords.Item2 - 1))

                safeDirections.Add('n');



            // South

            if (!IsPointCompromisedByAnyObstacle(coords.Item1, coords.Item2 + 1))

                safeDirections.Add('s');



            // East

            if (!IsPointCompromisedByAnyObstacle(coords.Item1 + 1, coords.Item2))

                safeDirections.Add('e');



            // West

            if (!IsPointCompromisedByAnyObstacle(coords.Item1 - 1, coords.Item2))

                safeDirections.Add('w');

            // Check if the agent's current position is compromised

            if (IsPointCompromisedByAnyObstacle(coords.Item1, coords.Item2))

            {

                return (true, null);

            }



            if (safeDirections.Count == 0)

            {

                Console.WriteLine("You cannot safely move in any direction. Abort mission.");

                return (false, null);

            }



            return (false, safeDirections);

        }

        else

        {

            return (false, null); // if invalid input

        }

    }

    /// <summary>

    /// Prompt the user to specify a portion of the map to display.

    /// </summary>

    static void DisplayObstacleMap()

    {

        Console.WriteLine("Enter the location of the top-left cell of the map X,Y:");

        if (TryParseCoordinates(Console.ReadLine(), out var topLeftCoords))

        {

            Console.WriteLine("Enter the location of the bottom-right cell of the map (X,Y):");



            if (TryParseCoordinates(Console.ReadLine(), out var bottomRightCoords))

            {

                if (bottomRightCoords.Item1 >= topLeftCoords.Item1 && bottomRightCoords.Item2 >= topLeftCoords.Item2)

                {

                    PrintMap(topLeftCoords, bottomRightCoords);

                }

                else

                {

                    Console.WriteLine("Invalid map specification.");

                }

            }

            else

            {

                Console.WriteLine("Invalid input.");

            }

        }

    }

    /// <summary>

    /// Print the specified portion of the map with the obstacles.

    /// </summary>

    /// <param name="topLeft">The top-left corner of the map portion.</param>

    /// <param name="bottomRight">The bottom-right corner of the map portion.</param>

    static void PrintMap((int, int) topLeft, (int, int) bottomRight)

    {

        for (int y = topLeft.Item2; y <= bottomRight.Item2; y++)

        {

            for (int x = topLeft.Item1; x <= bottomRight.Item1; x++)

            {

                if (IsPointCompromisedByAnyObstacle(x, y))

                {

                    var obstacle = obstacles.FirstOrDefault(o => o.IsPointCompromised(x, y));

                    if (obstacle is Guard)

                        Console.Write('g');

                    else if (obstacle is Fence)

                        Console.Write('f');

                    else if (obstacle is Sensor)

                        Console.Write('s');

                    else if (obstacle is Camera)

                        Console.Write('c');

                    else

                        Console.Write('X');  // This should not occur, but added just in case

                }

                else

                {

                    Console.Write('.');

                }

            }

            Console.WriteLine();

        }

    }

    /// <summary>

    /// Find and print the safe path from the agent's current position to the objective.

    /// </summary>

    static void FindSafePath()

    {

        Console.Write("Enter your current location (X,Y): ");

        if (TryParseCoordinates(Console.ReadLine(), out var start))

        {

            Console.Write("Enter the location of your objective (X,Y): ");

            if (TryParseCoordinates(Console.ReadLine(), out var end))

            {

                if (start == end)

                {

                    Console.WriteLine("Agent, you are already at the objective.");

                    return;

                }



                if (IsPointCompromisedByAnyObstacle(end.Item1, end.Item2))

                {

                    Console.WriteLine("The objective is blocked by an obstacle and cannot be reached.");

                    return;

                }



                var path = BFS(start, end);

                if (path != null)

                {

                    Console.WriteLine("The following path will take you to the objective:");

                    Console.WriteLine(path);

                }

                else

                {

                    Console.WriteLine("There is no safe path to the objective.");

                }

            }

            else

            {

                Console.WriteLine("Invalid input.");

            }

        }

        else

        {

            Console.WriteLine("Invalid input.");

        }

    }



    /// <summary>

    /// Breadth-first search to find a path from start to end.

    /// </summary>

    /// <param name="start">The starting point.</param>

    /// <param name="end">The ending point.</param>

    /// <returns>Returns the found path as a string.</returns>

    static string BFS((int, int) start, (int, int) end)

    {

        var visited = new HashSet<(int, int)>();

        var queue = new Queue<(int, int)>();

        var parent = new Dictionary<(int, int), (int, int)>();



        queue.Enqueue(start);



        while (queue.Count > 0)

        {

            var current = queue.Dequeue();

            if (current == end)

            {

                return ReconstructPath(parent, start, end);

            }



            var neighbors = GetNeighbors(current);



            foreach (var neighbor in neighbors)

            {

                if (!visited.Contains(neighbor) && !IsPointCompromisedByAnyObstacle(neighbor.Item1, neighbor.Item2))

                {

                    queue.Enqueue(neighbor);

                    visited.Add(neighbor);

                    parent[neighbor] = current;

                }

            }

        }



        return null;

    }



    /// <summary>

    /// Get the neighboring points of the given point.

    /// </summary>

    /// <param name="point">The given point.</param>

    /// <returns>Returns a list of neighboring points.</returns>

    static List<(int, int)> GetNeighbors((int, int) point)

    {

        return new List<(int, int)>

    {

        (point.Item1, point.Item2 - 1), // North

        (point.Item1, point.Item2 + 1), // South

        (point.Item1 + 1, point.Item2), // East

        (point.Item1 - 1, point.Item2)  // West

    };

    }



    /// <summary>

    /// Reconstruct the path from the end to the start using the parent map.

    /// </summary>

    /// <param name="parent">The parent map used for path reconstruction.</param>

    /// <param name="start">The starting point.</param>

    /// <param name="end">The ending point.</param>

    /// <returns>Returns the reconstructed path as a string.</returns>

    static string ReconstructPath(Dictionary<(int, int), (int, int)> parent, (int, int) start, (int, int) end)

    {

        var path = new List<char>();

        var current = end;



        while (current != start)

        {

            var prev = parent[current];

            if (current.Item1 > prev.Item1)

                path.Add('E');

            else if (current.Item1 < prev.Item1)

                path.Add('W');

            else if (current.Item2 > prev.Item2)

                path.Add('S');

            else if (current.Item2 < prev.Item2)

                path.Add('N');



            current = prev;

        }



        path.Reverse();

        return new string(path.ToArray());

    }

}

