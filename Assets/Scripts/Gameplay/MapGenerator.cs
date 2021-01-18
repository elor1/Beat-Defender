using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    private const int WALL_PERCENT = 47; //Chance that a tile will be a wall
    private const int SMOOTH_ITERATIONS = 20; //Number of times the SmootWalls method is called when generating a new map
    private const int PATH_RADIUS = 3; //The width of connections made between rooms
    public const int TILE_SIZE = 1; //Size of each tile in map
    public const int BORDER_SIZE = 7; //Number of wall blocks around each edge

    public enum TileType
    {
        Room,
        Wall,
        Border
    }

    public struct Coordinate
    {
        public int tileX; //X coordinate of tile
        public int tileY; //Y coordinate of tile

        public Coordinate(int x, int y)
        {
            tileX = x;
            tileY = y;
        }
    }

    [SerializeField] private GameObject _wallPrefab; //Drag the prefab for the wall into this field in the inspector
    [SerializeField] private GameObject _playerPrefab; //Drag the prefab for the player into this field in the inspector
    private static int _width; //Width of map
    private static int _height; //Height of map
    private static Tile[,] _mapGrid; //Stores tile data for map
    private static int _wallThreshold; //Any wall regions with a smaller number of tiles than this will be removed from the map
    private static int _roomThreshold; //Any room regions with a smaller number of tiles than this will be removed from the map
    private static GameObject _playerModel; //Stores the actual player object

    public static int Width { get { return _width; } }
    public static int Height { get { return _height; } }
    public static Tile[,] MapGrid { get { return _mapGrid; } }

    private void Awake()
    {
        _width = 80 + BORDER_SIZE * 2;
        _height = 60 + BORDER_SIZE * 2;
    }

    // Start is called before the first frame update
    private void Start()
    {
        _wallThreshold = (_width * _height) / 100;
        _roomThreshold = (_width * _height) / 150;

        _mapGrid = new Tile[_width, _height];
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                _mapGrid[x, y] = new Tile();
                _mapGrid[x, y]._coord.tileX = x;
                _mapGrid[x, y]._coord.tileY = y;
            }
        }

        //Set borders in map grid
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                if (x < BORDER_SIZE || x >= _width - BORDER_SIZE || y < BORDER_SIZE || y >= _height - BORDER_SIZE)
                {
                    _mapGrid[x, y].Type = TileType.Border;
                }
            }
        }

        _playerModel = Instantiate(_playerPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
       
        //Set camera to follow player
        CameraFollowScript.Target = _playerModel;

        InstantiateWalls();
        GenerateMap();
    }

    /// <summary>
    /// Checks whether given indexes are within the mapGrid array (excludes borders)
    /// </summary>
    /// <param name="x">X index</param>
    /// <param name="y">Y index</param>
    /// <returns>True or false</returns>
    public static bool IsInBounds(int x, int y)
    {
        return (x >= BORDER_SIZE && x < _width - BORDER_SIZE & y >= BORDER_SIZE && y < _height - BORDER_SIZE);
    }

    /// <summary>
    /// Instantiates wall prefabs for map grid
    /// </summary>
    private void InstantiateWalls()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                //Create a wall block for each tile in mapGrid
                _mapGrid[x, y].Obj = Instantiate(_wallPrefab, new Vector3(x * TILE_SIZE, 0.0f, y * TILE_SIZE), Quaternion.identity) as GameObject;
            }
        }
    }

    /// <summary>
    /// Generates a new map and spawns player
    /// </summary>
    public static void GenerateMap()
    {
        GameManager.WaveNumber++;
        UpdateHUD.UpdateWaveNumber();

        FillMap();

        for (int i = 0; i < SMOOTH_ITERATIONS; i++)
        {
            SmoothWalls();
        }

        RemoveSmallRegions(TileType.Wall, _wallThreshold);
        RemoveSmallRegions(TileType.Room, _roomThreshold);
        HideWalls();
        SpawnPlayer();

        ScaleCubes.ResetScales();
        AudioAnalyser.ResetAmplitude();
    }

    /// <summary>
    /// Generates random data for new map
    /// </summary>
    private static void FillMap()
    {
        System.Random randomNumber = new System.Random(System.DateTime.Now.GetHashCode());

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                if (IsInBounds(x, y))
                {
                    if (x == BORDER_SIZE || x == _width - BORDER_SIZE - 1 || y == BORDER_SIZE || y == _height - BORDER_SIZE - 1)
                    {
                        //Always set edge tiles to walls
                        _mapGrid[x, y].Type = TileType.Wall;
                    }
                    else
                    {
                        //For each tile in the map grid, if the randomly generated number is less than the wall percent, set its value to Wall
                        if (randomNumber.Next(0, 100) < WALL_PERCENT)
                        {
                            _mapGrid[x, y].Type = TileType.Wall;
                        }
                        else
                        {
                            _mapGrid[x, y].Type = TileType.Room;
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Finds the number of walls surrounding the given tile
    /// </summary>
    /// <param name="gridX">X coordinate in grid map</param>
    /// <param name="gridY">Y coordinate in grid map</param>
    /// <returns>Number of walls surrounding given tile</returns>
    private static int GetNumberOfSurroundingWalls(int gridX, int gridY)
    {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if ((neighbourX != gridX || neighbourY != gridY) && (_mapGrid[neighbourX, neighbourY].Type != TileType.Room))
                {
                        //If the given tile is within the bounds of the map, for each of its surrounding tiles, add its grid value to wallCount
                        wallCount += (int)_mapGrid[neighbourX, neighbourY].Type;
                }
            }
        }

        return wallCount;
    }

    /// <summary>
    /// Uses cellular automata to create more room-like shapes
    /// </summary>
    private static void SmoothWalls()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                //Do not check borders
                if (IsInBounds(x, y))
                {
                    int surroundingWallCount = GetNumberOfSurroundingWalls(x, y);

                    if (surroundingWallCount > 4)
                    {
                        _mapGrid[x, y].Type = TileType.Wall;
                    }
                    else if (surroundingWallCount < 4)
                    {
                        _mapGrid[x, y].Type = TileType.Room;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Flood fill algorithm to find each tile in a region
    /// </summary>
    /// <param name="startX">Starting x coordinate of region</param>
    /// <param name="startY">Starting y coordinate of region</param>
    /// <returns>List of tiles in a region</returns>
    private static List<Coordinate> GetRegionTiles(int startX, int startY)
    {
        List<Coordinate> tiles = new List<Coordinate>();
        int[,] checkedTiles = new int[_width, _height];
        TileType tileType = _mapGrid[startX, startY].Type;

        Queue<Coordinate> fillQueue = new Queue<Coordinate>();
        fillQueue.Enqueue(new Coordinate(startX, startY));
        checkedTiles[startX, startY] = 1;

        while (fillQueue.Count > 0)
        {
            Coordinate tile = fillQueue.Dequeue();
            tiles.Add(tile);

            for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++)
            {
                for (int y = tile.tileY - 1; y <= tile.tileY + 1; y++)
                {
                    if (IsInBounds(x, y) && (y == tile.tileY || x == tile.tileX))
                    {
                        if (checkedTiles[x, y] == 0 && _mapGrid[x, y].Type == tileType)
                        {
                            checkedTiles[x, y] = 1;
                            fillQueue.Enqueue(new Coordinate(x, y));
                        }
                    }
                }
            }
        }

        return tiles;
    }

    /// <summary>
    /// Finds all regions in map
    /// </summary>
    /// <param name="tileType">Type of tile: Wall or Room</param>
    /// <returns>List of regions</returns>
    private static List<List<Coordinate>> GetRegions(TileType tileType)
    {
        List<List<Coordinate>> regions = new List<List<Coordinate>>();
        int[,] checkedTiles = new int[_width, _height];

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                if (checkedTiles[x, y] == 0 && _mapGrid[x, y].Type == tileType)
                {
                    List<Coordinate> newRegion = GetRegionTiles(x, y);
                    regions.Add(newRegion);

                    foreach (Coordinate tile in newRegion)
                    {
                        checkedTiles[tile.tileX, tile.tileY] = 1;
                    }
                }
            }
        }
        return regions;
    }

    /// <summary>
    /// Removes any regions that are smaller than the given threshold
    /// </summary>
    /// <param name="tileType">Type of tile: Wall or Room</param>
    /// <param name="threshold">Number of tiles in a region for it to not be removed</param>
    private static void RemoveSmallRegions(TileType tileType, int threshold)
    {
        List<List<Coordinate>> regions = GetRegions(tileType);
        List<RoomData> survivingRooms = new List<RoomData>();
        foreach (List<Coordinate> region in regions)
        {
            //If a region has less tiles than the threshold, swap the tile type for all tiles in that region
            if (region.Count < threshold)
            {
                foreach (Coordinate tile in region)
                {
                    if (tileType == TileType.Wall)
                    {
                        _mapGrid[tile.tileX, tile.tileY].Type = TileType.Room;
                    }
                    else
                    {
                        _mapGrid[tile.tileX, tile.tileY].Type = TileType.Wall;
                    }
                }
            }
            else if (tileType == TileType.Room)
            {
                //Add surviving rooms to a list so that they can be checked for connections
                survivingRooms.Add(new RoomData(region, _mapGrid));
                connectClosestRooms(survivingRooms);
            }
        }
    }

    /// <summary>
    /// Connects each room to the nearest other room
    /// </summary>
    /// <param name="rooms">List of surviving rooms</param>
    private static void connectClosestRooms(List<RoomData> rooms)
    {
        float minDistance = 0.0f;
        Coordinate bestTile1 = new Coordinate();
        Coordinate bestTile2 = new Coordinate();
        RoomData bestRoom1 = new RoomData();
        RoomData bestRoom2 = new RoomData();
        bool connectionFound = false;

        foreach (RoomData room1 in rooms)
        {
            connectionFound = false;
            foreach (RoomData room2 in rooms)
            {
                if (room1 == room2)
                {
                    //Can't connect a room to itself
                    continue;
                }

                if (room1.IsConnected(room2))
                {
                    //Don't connect the same rooms twice
                    connectionFound = false;
                    break;
                }

                //If rooms are not connected, find the tiles from each room with the minimum distance
                for (int tileIndex1 = 0; tileIndex1 < room1.EdgeTiles.Count; tileIndex1++)
                {
                    for (int tileIndex2 = 0; tileIndex2 < room2.EdgeTiles.Count; tileIndex2++)
                    {
                        Coordinate tile1 = room1.EdgeTiles[tileIndex1];
                        Coordinate tile2 = room2.EdgeTiles[tileIndex2];
                        int distance = (tile1.tileX - tile2.tileX) * (tile1.tileX - tile2.tileX) + (tile1.tileY - tile2.tileY) * (tile1.tileY - tile2.tileY);

                        if (distance < minDistance || !connectionFound)
                        {
                            minDistance = distance;
                            connectionFound = true;
                            bestTile1 = tile1;
                            bestTile2 = tile2;
                            bestRoom1 = room1;
                            bestRoom2 = room2;
                        }

                    }
                }
            }

            if (connectionFound)
            {
                CreateConnection(bestRoom1, bestRoom2, bestTile1, bestTile2);
            }
        }
    }

    /// <summary>
    /// Connects tiles from two rooms together
    /// </summary>
    /// <param name="room1">1st room to be connected</param>
    /// <param name="room2">2nd room to be connected</param>
    /// <param name="tile1">1st room's tile to be connected</param>
    /// <param name="tile2">2nd room's tile to be connected</param>
    private static void CreateConnection(RoomData room1, RoomData room2, Coordinate tile1, Coordinate tile2)
    {
        RoomData.ConnectRooms(room1, room2);

        List<Coordinate> line = GetConnectionLine(tile1, tile2);
        foreach (Coordinate center in line)
        {
            DrawCircle(center, PATH_RADIUS);
        }
    }

    /// <summary>
    /// Sets all tiles within a given radius to room tiles
    /// </summary>
    /// <param name="center">Tile path should be cleared around</param>
    /// <param name="radius">Size of path to be cleared</param>
    private static void DrawCircle(Coordinate center, int radius)
    {
        for (int x = -radius; x <= radius; x++)
        {
            for (int y = -radius; y <= radius; y++)
            {
                if (x * x + y * y <= radius * radius)
                {
                    //If coordinate is inside the circle
                    int xPoint = center.tileX + x;
                    int yPoint = center.tileY + y;
                    if (IsInBounds(xPoint, yPoint))
                    {
                        _mapGrid[xPoint, yPoint].Type = TileType.Room;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Calculates a line between two points
    /// </summary>
    /// <param name="start">Starting coordinate of line</param>
    /// <param name="end">Ending coordinate of line</param>
    /// <returns>List of coordinates in line</returns>
    private static List<Coordinate> GetConnectionLine(Coordinate start, Coordinate end)
    {
        List<Coordinate> line = new List<Coordinate>();

        int x = start.tileX;
        int y = start.tileY;

        int dx = end.tileX - start.tileX;
        int dy = end.tileY - start.tileY;

        bool isInverted = false;
        int step = System.Math.Sign(dx);
        int gradientStep = System.Math.Sign(dy);

        int longest = Mathf.Abs(dx);
        int shortest = Mathf.Abs(dy);

        if (longest < shortest)
        {
            isInverted = true;
            longest = Mathf.Abs(dy);
            shortest = Mathf.Abs(dx);

            step = System.Math.Sign(dy);
            gradientStep = System.Math.Sign(dx);
        }

        int gradientAccumulation = longest / 2;
        for (int i = 0; i < longest; i++)
        {
            line.Add(new Coordinate(x, y));

            if (isInverted)
            {
                y += step;
            }
            else
            {
                x += step;
            }

            gradientAccumulation += shortest;
            if (gradientAccumulation >= longest)
            {
                if (isInverted)
                {
                    x += gradientStep;
                }
                else
                {
                    y += gradientStep;
                }
                gradientAccumulation -= longest;
            }
        }
        return line;
    }

    /// <summary>
    /// Sets each wall game object to active or unactive depending on whether it should be visible or not
    /// </summary>
    private static void HideWalls()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                if (_mapGrid[x, y].Type == TileType.Room)
                {
                    _mapGrid[x, y].Obj.SetActive(false);
                }
                else
                {
                    _mapGrid[x, y].Obj.SetActive(true);
                }
            }
        }
    }

    /// <summary>
    /// Moves player to a random spawn point
    /// </summary>
    private static void SpawnPlayer()
    {
        Coordinate spawnPoint = GetRandomRoomTile();

        //Move player to new spawn point
        _playerModel.transform.position = new Vector3(spawnPoint.tileX * TILE_SIZE, 0.0f, spawnPoint.tileY * TILE_SIZE);
    }

    /// <summary>
    /// Finds a random valid spawn point
    /// </summary>
    /// <returns>Spawn point coordinates</returns>
    public static Coordinate GetRandomRoomTile()
    {
        Coordinate spawnPoint = new Coordinate();
        System.Random randomNumber = new System.Random(System.DateTime.Now.GetHashCode());

        //Generate random spawn points until one is found within the walls
        while (_mapGrid[spawnPoint.tileX, spawnPoint.tileY].Type != TileType.Room)
        {
            spawnPoint.tileX = randomNumber.Next(0 + BORDER_SIZE, _width - BORDER_SIZE - 1);
            spawnPoint.tileY = randomNumber.Next(0 + BORDER_SIZE, _height - BORDER_SIZE - 1);
        }

        return spawnPoint;
    }
}
