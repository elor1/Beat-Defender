using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
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

    [SerializeField] private int _wallPercent = 45; //Chance that a tile will be a wall

    public static int _width = 80; //Width of map
    public static int _height = 60; //Height of map

    public static TileType[,] _mapGrid; //Wall data for each tile
    public GameObject[,] _walls; //Stores each instance of a wall
    public static int _tileSize = 1; //Size of each tile in map
    public static int _borderSize = 7; //Number of wall blocks around each edge. Used so that the camera can't see over the edge of the world
    [SerializeField] private int _smoothIterations = 20; //Number of times the SmootWalls method is called when generating a new map

    private int _wallThreshold; //Any wall regions with a smaller number of tiles than this will be removed from the map
    private int _roomThreshold; //Any room regions with a smaller number of tiles than this will be removed from the map
    [SerializeField] private int _pathRadius = 1; //The width of connections made between rooms

    [SerializeField] private GameObject _wallPrefab; //Drag the prefab for the wall into this field in the inspector
    [SerializeField] private GameObject _playerPrefab; //Drag the prefab for the player into this field in the inspector

    private GameObject _playerModel; //Stores the actual player object

    // Start is called before the first frame update
    private void Start()
    {
        _wallThreshold = (_width * _height) / 100;
        _roomThreshold = (_width * _height) / 150;
        _width += _borderSize * 2;
        _height += _borderSize * 2;
        _mapGrid = new TileType[_width, _height];

        //Set borders in map grid
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                if (x < _borderSize || x >= _width - _borderSize || y < _borderSize || y >= _height - _borderSize)
                {
                    _mapGrid[x, y] = TileType.Border;
                }
            }
        }

        _playerModel = Instantiate(_playerPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        if (_playerModel == null)
        {
            Debug.Log("NULL :(");
        }
        //Set camera to follow player
        CameraFollowScript._target = _playerModel;
        InstantiateWalls();

        GenerateMap();
    }

    // Update is called once per frame
    private void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    GenerateMap();

        //}
    }

    /// <summary>
    /// Checks whether given indexes are within the mapGrid array (excludes borders)
    /// </summary>
    /// <param name="x">X index</param>
    /// <param name="y">Y index</param>
    /// <returns>True or false</returns>
    private bool IsInBounds(int x, int y)
    {
        return (x >= _borderSize && x < _width - _borderSize & y >= _borderSize && y < _height - _borderSize);
    }

    private void InstantiateWalls()
    {
        _walls = new GameObject[_width, _height];
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                //Create a wall block for each tile in mapGrid
                _walls[x, y] = Instantiate(_wallPrefab, new Vector3(x * _tileSize, 0.0f, y * _tileSize), Quaternion.identity) as GameObject;
            }
        }
    }

    private void GenerateMap()
    {
        FillMap();

        for (int i = 0; i < _smoothIterations; i++)
        {
            SmoothWalls();
        }

        RemoveSmallRegions(TileType.Wall, _wallThreshold);
        RemoveSmallRegions(TileType.Room, _roomThreshold);
        HideWalls();
        SpawnPlayer();

        //gameManager.currentState = GameManager.GameState.Playing;
    }

    /// <summary>
    /// Generates random data for new map
    /// </summary>
    private void FillMap()
    {
        System.Random randomNumber = new System.Random(System.DateTime.Now.GetHashCode());

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                if (IsInBounds(x, y))
                {
                    if (x == _borderSize || x == _width - _borderSize - 1 || y == _borderSize || y == _height - _borderSize - 1)
                    {
                        //Always set edge tiles to walls
                        _mapGrid[x, y] = TileType.Wall;
                    }
                    else
                    {
                        //For each tile in the map grid, if the randomly generated number is less than the wallPercent, set its value to Wall
                        if (randomNumber.Next(0, 100) < _wallPercent)
                        {
                            _mapGrid[x, y] = TileType.Wall;
                        }
                        else
                        {
                            _mapGrid[x, y] = TileType.Room;
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
    /// <returns>Number of walls surrounding give tile</returns>
    private int GetNumberOfSurroundingWalls(int gridX, int gridY)
    {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if ((neighbourX != gridX || neighbourY != gridY) && (_mapGrid[neighbourX, neighbourY] != TileType.Room))
                {
                        //If the given tile is within the bounds of the map, for each of its surrounding tiles, add its grid value to wallCount
                        wallCount += (int)_mapGrid[neighbourX, neighbourY];
                }
            }
        }

        return wallCount;
    }

    /// <summary>
    /// Uses cellular automata to create more room-like shapes
    /// </summary>
    private void SmoothWalls()
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
                        _mapGrid[x, y] = TileType.Wall;
                    }
                    else if (surroundingWallCount < 4)
                    {
                        _mapGrid[x, y] = TileType.Room;
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
    private List<Coordinate> GetRegionTiles(int startX, int startY)
    {
        List<Coordinate> tiles = new List<Coordinate>();
        int[,] checkedTiles = new int[_width, _height];
        TileType tileType = _mapGrid[startX, startY];

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
                        if (checkedTiles[x, y] == 0 && _mapGrid[x, y] == tileType)
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
    private List<List<Coordinate>> GetRegions(TileType tileType)
    {
        List<List<Coordinate>> regions = new List<List<Coordinate>>();
        int[,] checkedTiles = new int[_width, _height];

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                if (checkedTiles[x, y] == 0 && _mapGrid[x, y] == tileType)
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
    private void RemoveSmallRegions(TileType tileType, int threshold)
    {
        List<List<Coordinate>> regions = GetRegions(tileType);
        List<RoomData> survivingRooms = new List<RoomData>();
        foreach (List<Coordinate> region in regions)
        {
            if (region.Count < threshold)
            {
                foreach (Coordinate tile in region)
                {
                    if (tileType == TileType.Wall)
                    {
                        _mapGrid[tile.tileX, tile.tileY] = TileType.Room;
                    }
                    else
                    {
                        _mapGrid[tile.tileX, tile.tileY] = TileType.Wall;
                    }
                }
            }
            else if (tileType == TileType.Room)
            {
                //Add surviving rooms to a list so that they can be check for connections
                survivingRooms.Add(new RoomData(region, _mapGrid));
                connectClosestRooms(survivingRooms);
            }
        }
    }

    /// <summary>
    /// Connects each room to the nearest other room
    /// </summary>
    /// <param name="rooms">List of surviving rooms</param>
    private void connectClosestRooms(List<RoomData> rooms)
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
                    continue;
                }

                if (room1.IsConnected(room2))
                {
                    connectionFound = false;
                    break;
                }

                for (int tileIndex1 = 0; tileIndex1 < room1._edgeTiles.Count; tileIndex1++)
                {
                    for (int tileIndex2 = 0; tileIndex2 < room2._edgeTiles.Count; tileIndex2++)
                    {
                        Coordinate tile1 = room1._edgeTiles[tileIndex1];
                        Coordinate tile2 = room2._edgeTiles[tileIndex2];
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
    private void CreateConnection(RoomData room1, RoomData room2, Coordinate tile1, Coordinate tile2)
    {
        RoomData.ConnectRooms(room1, room2);
        Debug.DrawLine(CoordToWorldPoint(tile1), CoordToWorldPoint(tile2), Color.green, 15);

        List<Coordinate> line = GetConnectionLine(tile1, tile2);
        foreach (Coordinate center in line)
        {
            DrawCircle(center, _pathRadius);
        }
    }

    /// <summary>
    /// Sets all tiles within a given radius to room tiles
    /// </summary>
    /// <param name="center">Tile path should be cleared around</param>
    /// <param name="radius">Size of path to be cleared</param>
    private void DrawCircle(Coordinate center, int radius)
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
                        _mapGrid[xPoint, yPoint] = TileType.Room;
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
    /// <returns>List coordinates in line</returns>
    private List<Coordinate> GetConnectionLine(Coordinate start, Coordinate end)
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
    /// Converts a coordinate to a world point
    /// </summary>
    /// <param name="tile">Cooridinates of tile to convert</param>
    /// <returns>Vector3 of position</returns>
    private Vector3 CoordToWorldPoint(Coordinate tile)
    {
        return new Vector3(tile.tileX * _tileSize, 1.0f, tile.tileY * _tileSize);
    }

    /// <summary>
    /// Sets each wall game object to active or unactive depending on whether it should be visible or not
    /// </summary>
    private void HideWalls()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                if (_mapGrid[x, y] == TileType.Room)
                {
                    _walls[x, y].SetActive(false);
                }
                else
                {
                    _walls[x, y].SetActive(true);
                }
            }
        }
    }

    /// <summary>
    /// Moves player to a random spawn point
    /// </summary>
    private void SpawnPlayer()
    {
        Coordinate spawnPoint = new Coordinate();
        System.Random randomNumber = new System.Random(System.DateTime.Now.GetHashCode());

        //Generate random spawn points until one is found within the walls
        while (_mapGrid[spawnPoint.tileX, spawnPoint.tileY] != TileType.Room)
        {
            Debug.Log("New spawn point");
            spawnPoint.tileX = randomNumber.Next(0 + _borderSize, _width - _borderSize - 1);
            spawnPoint.tileY = randomNumber.Next(0 + _borderSize, _height - _borderSize - 1);
        }

        //Move player to new spawn point
        _playerModel.transform.position = new Vector3(spawnPoint.tileX * _tileSize, 0.0f, spawnPoint.tileY * _tileSize);
    }
}
