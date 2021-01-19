using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    private Tile[,] _nodes; //Array of map nodes

    private void Start()
    {
        _nodes = new Tile[MapGenerator.Width, MapGenerator.Height];

        //Set nodes to equivelent tile types
        for (int x = 0; x < MapGenerator.Width; x++)
        {
            for (int y = 0; y < MapGenerator.Height; y++)
            {
                Tile tile = new Tile();
                tile.Coord.tileX = x;
                tile.Coord.tileY = y;
                tile.Type = MapGenerator.MapGrid[x, y].Type;
                _nodes[x, y] = tile;
            }
        }

        //Discourage enemies from hugging walls by giving edge tiles a higher cost
        for (int x = 0; x < MapGenerator.Width; x++)
        {
            for (int y = 0; y < MapGenerator.Height; y++)
            {
                for (int i = -5; i <= 5; i++)
                {
                    for (int j = -5; j <= 5; j++)
                    {
                        if (i == 0 && j == 0)
                        {
                            continue;
                        }
                        if (!MapGenerator.IsInBounds(x + i, y + j))
                        {
                            continue;
                        }
                        if (_nodes[x + i, y + j].Type != MapGenerator.TileType.Room)
                        {
                            _nodes[x, y].NeighbourWalls++;
                        }
                    }
                }
            }
        }
    }
    
    /// <summary>
    /// Finds a list of tiles to move between the start and end point around walls
    /// </summary>
    /// <param name="obj">Path start location</param>
    /// <param name="target">Target location</param>
    /// <returns>List of tiles to navigate</returns>
    public List<Tile> FindPath(Vector3 obj, Vector3 target)
    {
        Tile start = _nodes[(int)obj.x, (int)obj.z];
        Tile goal = _nodes[(int)target.x, (int)target.z];

        List<Tile> openList = new List<Tile>();
        HashSet<Tile> closedList = new HashSet<Tile>();
        openList.Add(start);

        while (openList.Count > 0)
        {
            Tile currentTile = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].FCost < currentTile.FCost || openList[i].FCost == currentTile.FCost)
                {
                    if (openList[i].HCost < currentTile.HCost)
                    {
                        currentTile = openList[i];
                    }
                }
            }

            openList.Remove(currentTile);
            closedList.Add(currentTile);

            if (currentTile.isEqual(goal))
            {
                return CreatePath(start, goal);
            }

            foreach (Tile neighbour in GetNeighbours(currentTile))
            {
                if (neighbour.Type != MapGenerator.TileType.Room || closedList.Contains(neighbour))
                {
                    continue;
                }

                int cost = currentTile.GCost + CalculateDistance(currentTile, neighbour);
                if (cost < neighbour.GCost || !openList.Contains(neighbour))
                {
                    neighbour.GCost = cost;
                    neighbour.HCost = CalculateDistance(neighbour, goal);
                    neighbour.Parent = currentTile;

                    if (!openList.Contains(neighbour))
                    {
                        openList.Add(neighbour);
                    }
                }
            }
        }

        //If path can't be found, send enemy to random tile
        MapGenerator.Coordinate randomTile = MapGenerator.GetRandomRoomTile();
        return FindPath(obj, new Vector3(randomTile.tileX, 0.0f, randomTile.tileY));
    }

    /// <summary>
    /// Finds the neighbouring tiles of a given tile
    /// </summary>
    /// <param name="tile"></param>
    /// <returns>List of neighbouring tiles</returns>
    private List<Tile> GetNeighbours(Tile tile)
    {
        List<Tile> neighbours = new List<Tile>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }

                if (MapGenerator.IsInBounds(tile.Coord.tileX + x, tile.Coord.tileY + y))
                {
                    neighbours.Add(_nodes[tile.Coord.tileX + x, tile.Coord.tileY + y]);
                }
            }
        }
        return neighbours;
    }

    /// <summary>
    /// Calculates the distance between two tiles
    /// </summary>
    /// <param name="tileA">Start tile</param>
    /// <param name="tileB">End tile</param>
    /// <returns>Distance cost betwen the tiles</returns>
    private int CalculateDistance(Tile tileA, Tile tileB)
    {
        int distanceX = Mathf.Abs(tileA.Coord.tileX - tileB.Coord.tileX);
        int distanceY = Mathf.Abs(tileA.Coord.tileY - tileB.Coord.tileY);

        if (distanceX > distanceY)
            return 14 * distanceY + 10 * (distanceX - distanceY);
        return 14 * distanceX + 10 * (distanceY - distanceX);
    }

    /// <summary>
    /// Builds a list of tiles for a path
    /// </summary>
    /// <param name="start">Path start</param>
    /// <param name="end">Path end</param>
    /// <returns>List of tiles in the path</returns>
    private List<Tile> CreatePath(Tile start, Tile end)
    {
        List<Tile> path = new List<Tile>();
        Tile currentTile = end;

        while (currentTile != start)
        {
            path.Add(currentTile);
            currentTile = currentTile.Parent;
        }

        path.Reverse();
        return path;
    }
}
