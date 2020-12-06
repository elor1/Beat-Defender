using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{

    //public static List<Tile> FindPath(GameObject obj, GameObject target)
    //{
    //    Tile start = MapGenerator._mapGrid[(int)obj.transform.position.x, (int)obj.transform.position.z];
    //    Tile end = MapGenerator._mapGrid[(int)target.transform.position.x, (int)target.transform.position.z];
    //    start.hCost = (int)(new Vector3(end.coord.tileX, 0.0f, end.coord.tileY) - new Vector3(start.coord.tileX, 0.0f, start.coord.tileY)).magnitude;

    //    List<Tile> openSet = new List<Tile>();
    //    HashSet<Tile> closedSet = new HashSet<Tile>();

    //    openSet.Add(start);

    //    while (openSet.Count > 0)
    //    {
    //        Tile currentTile = GetBestTile(openSet);
    //        //for (int i = 1; i < openSet.Count; i++)
    //        //{
    //        //    if (openSet[i].fCost() < currentTile.fCost() || openSet[i].fCost() == currentTile.fCost())
    //        //    {
    //        //        if (openSet[i].hCost < currentTile.hCost)
    //        //        {
    //        //            currentTile = openSet[i];
    //        //        } 
    //        //    }
    //        //}

    //        openSet.Remove(currentTile);
    //        //closedSet.Add(currentTile);

    //        //if (currentTile == end)
    //        //{
    //        //    return RetracePath(start, end);

    //        //}

    //        foreach (Tile neighbour in GetNeighbours(currentTile))
    //        {
    //            //if (neighbour.type != MapGenerator.TileType.Room || closedSet.Contains(neighbour))
    //            //{
    //            //    continue;
    //            //}

    //            //int newCost = currentTile.gCost + GetDistance(currentTile, neighbour);
    //            //if (newCost < neighbour.gCost || !openSet.Contains(neighbour))
    //            //{
    //            //    neighbour.gCost = newCost;
    //            //    neighbour.hCost = GetDistance(neighbour, end);
    //            //    neighbour.parent = currentTile;

    //            //    if (!openSet.Contains(neighbour))
    //            //    {
    //            //        openSet.Add(neighbour);
    //            //    }
    //            //}

    //            if (neighbour.isEqual(end))
    //            {
    //                neighbour.parent = currentTile;
    //                return Backtrack(neighbour);
    //            }
    //            int g = currentTile.gCost + (int)(new Vector3(neighbour.coord.tileX, 0.0f, neighbour.coord.tileY) - new Vector3(currentTile.coord.tileX, 0.0f, currentTile.coord.tileY)).magnitude;
    //            int h = (int)(new Vector3(end.coord.tileX, 0.0f, end.coord.tileY) - new Vector3(neighbour.coord.tileX, 0.0f, neighbour.coord.tileY)).magnitude;

    //            if (openSet.Contains(neighbour) && neighbour.fCost() < (g + h))
    //            {
    //                continue;
    //            }
    //            if (closedSet.Contains(neighbour) && neighbour.fCost() < (g + h))
    //            {
    //                continue;
    //            }

    //            neighbour.gCost = g;
    //            neighbour.hCost = h;
    //            neighbour.parent = currentTile;

    //            if (!openSet.Contains(neighbour))
    //            {
    //                openSet.Add(neighbour);
    //            }
    //        }
    //        if (!closedSet.Contains(currentTile))
    //        {
    //            closedSet.Add(currentTile);
    //        }
    //    }
    //    Debug.LogError("Path not found");
    //    return null;
    //}

    //private static List<Tile> GetNeighbours(Tile currentTile)
    //{
    //    List<Tile> neighbours = new List<Tile>();
    //    for (int x = -1; x <= 1; x++)
    //    {
    //        for (int y = -1; y <= 1; y++)
    //        {
    //            if (!(x== 0 && y== 0))
    //            {
    //                if (MapGenerator.IsInBounds(currentTile.coord.tileX + x, currentTile.coord.tileY + y))
    //                {
    //                    neighbours.Add(MapGenerator._mapGrid[currentTile.coord.tileX + x, currentTile.coord.tileY + y]);
    //                }
    //            }
    //        }
    //    }

    //    return neighbours;
    //}

    //private static int GetDistance (Tile tileA, Tile tileB)
    //{
    //    int distanceX = Mathf.Abs(tileA.coord.tileX - tileB.coord.tileX);
    //    int distanceY = Mathf.Abs(tileA.coord.tileY - tileB.coord.tileY);

    //    if (distanceX > distanceY)
    //    {
    //        return 14 * distanceY + 10 * (distanceX - distanceY);
    //    }
    //    return 14 * distanceX + 10 * (distanceY - distanceX);
    //}

    //private static List<Tile> RetracePath(Tile start, Tile end)
    //{
    //    List<Tile> path = new List<Tile>();
    //    Tile currentTile = end;

    //    while(currentTile != start){
    //        path.Add(currentTile);
    //        currentTile = currentTile.parent;
    //    }

    //    //path.Reverse();
    //    return path;
    //}

    //private static List<Tile> Backtrack(Tile tile)
    //{
    //    List<Tile> path = new List<Tile>() { tile };
    //    while (tile.parent != null)
    //    {
    //        tile = tile.parent;
    //        path.Add(tile);
    //    }
    //    path.Reverse();
    //    return path;
    //}

    //private static Tile GetBestTile(List<Tile> openList)
    //{
    //    Tile best = null;

    //    if (openList.Count > 0)
    //    {
    //        best = openList[0];

    //        foreach (Tile t in openList)
    //        {
    //            if (t.fCost() < best.fCost())
    //            {
    //                best = t;
    //            }
    //        }
    //    }

    //    return best;
    //}

    //public static List<Tile> FindPath(GameObject obj, GameObject target)
    //{
    //    Tile start = MapGenerator._mapGrid[(int)obj.transform.position.x, (int)obj.transform.position.z];
    //    Tile goal = MapGenerator._mapGrid[(int)target.transform.position.x, (int)target.transform.position.z];

    //    List<Tile> openList = new List<Tile>();
    //    List<Tile> closedList = new List<Tile>();

    //    openList.Add(start);

    //    while (openList.Count > 0)
    //    {
    //        Tile q = FindBestInList(openList);
    //        openList.Remove(q);

    //        foreach (Tile neighbour in GetNeighbours(q))
    //        {
    //            neighbour.parent = q;
    //            if (neighbour.isEqual(goal))
    //            {
    //                neighbour.gCost = q.gCost + CalculateDistance(neighbour, q);
    //                neighbour.hCost = CalculateDistance(goal, neighbour);
    //                return CreatePath(neighbour);
    //            }

    //            if (openList.Contains(neighbour))
    //            {
    //                continue;
    //            }

    //            if (closedList.Contains(neighbour))
    //            {
    //                continue;
    //            }

    //            if (openList.Count > 30)
    //            {
    //                neighbour.gCost = q.gCost + CalculateDistance(neighbour, q);
    //                neighbour.hCost = CalculateDistance(goal, neighbour);
    //                return CreatePath(neighbour);
    //            }
    //            openList.Add(neighbour);
    //        }
    //        closedList.Add(q);
    //    }
    //    Debug.LogError("Path not found");
    //    return null;
    //}

    //public static Tile FindBestInList(List<Tile> list)
    //{
    //    Tile best = null;

    //    if (list.Count > 0)
    //    {
    //        best = list[0];
    //        foreach (Tile t in list)
    //        {
    //            if (t.fCost() < best.fCost())
    //            {
    //                best = t;
    //            }
    //        }
    //    }

    //    return best;
    //}

    //public static List<Tile> GetNeighbours(Tile tile)
    //{
    //    List<Tile> neighbours = new List<Tile>();

    //    for (int i = -1; i <= 1; i++)
    //    {
    //        for (int j = -1; j <= 1; j++)
    //        {
    //            if (!(i == 0 && j == 0))
    //            {
    //                if (MapGenerator.IsInBounds(tile.coord.tileX + i, tile.coord.tileY + j))
    //                {
    //                    neighbours.Add(MapGenerator._mapGrid[tile.coord.tileX + i, tile.coord.tileY + j]);
    //                }

    //            }
    //        }
    //    }

    //    return neighbours;
    //}

    //public static int CalculateDistance(Tile start, Tile end)
    //{
    //    return Mathf.Max(Mathf.Abs(start.coord.tileX - end.coord.tileX), Mathf.Abs(start.coord.tileY - end.coord.tileY));
    //}

    //public static List<Tile> CreatePath(Tile currentTile)
    //{
    //    List<Tile> path = new List<Tile>();
    //    path.Add(currentTile);

    //    while (currentTile.parent != null)
    //    {
    //        currentTile = currentTile.parent;
    //        path.Add(currentTile);
    //    }
    //    return path;
    //}
    public Tile[,] _nodes;

    private void Start()
    {
        _nodes = new Tile[MapGenerator._width, MapGenerator._height];
        for (int x = 0; x < MapGenerator._width; x++)
        {
            for (int y = 0; y < MapGenerator._height; y++)
            {
                Tile tile = new Tile();
                tile.coord.tileX = x;
                tile.coord.tileY = y;
                tile.type = MapGenerator._mapGrid[x, y].type;
                _nodes[x, y] = tile;
            }
        }

        for (int x = 0; x < MapGenerator._width; x++)
        {
            for (int y = 0; y < MapGenerator._height; y++)
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
                        if (_nodes[x + i, y + j].type != MapGenerator.TileType.Room)
                        {
                            _nodes[x, y].neighbourWalls++;
                        }
                    }
                }
            }
        }
    }

    //public List<Tile> FindPath(GameObject obj, GameObject target)
    //{

    //    Tile start = _nodes[(int)obj.transform.position.x, (int)obj.transform.position.z];
    //    Tile goal = _nodes[(int)target.transform.position.x, (int)target.transform.position.z];

    //    List<Tile> openList = new List<Tile>();
    //    List<Tile> closedList = new List<Tile>();
    //    openList.Add(start);

    //    while (openList.Count > 0)
    //    {
    //        Tile bestTile = GetBestTile(openList);
    //        if (bestTile.isEqual(goal))
    //        {
    //            Debug.Log("PATH FOUND!");
    //            return CreatePath(bestTile);
    //        }

    //        closedList.Add(bestTile);
    //        openList.Remove(bestTile);

    //        for (int x = -1; x <= 1; x++)
    //        {
    //            for (int y = -1; y <= 1; y++)
    //            {
    //                if (!MapGenerator.IsInBounds(bestTile.coord.tileX + x, bestTile.coord.tileY + y))
    //                {
    //                    continue;
    //                }

    //                Tile neighbour = _nodes[bestTile.coord.tileX + x, bestTile.coord.tileY + y];

    //                if (neighbour.type != MapGenerator.TileType.Room)
    //                {
    //                    continue;
    //                }

    //                if (closedList.Contains(neighbour))
    //                {
    //                    continue;
    //                }

    //                int cost = bestTile.gCost + CalculateDistance(bestTile, neighbour);

    //                if (openList.Contains(neighbour) && cost < neighbour.gCost)
    //                {
    //                    openList.Remove(neighbour);
    //                }

    //                if (closedList.Contains(neighbour) && cost < neighbour.gCost)
    //                {
    //                    closedList.Remove(neighbour);
    //                }

    //                if (!openList.Contains(neighbour) && !closedList.Contains(neighbour))
    //                {
    //                    openList.Add(neighbour);
    //                    neighbour.gCost = cost;
    //                    neighbour.hCost = CalculateDistance(neighbour, goal);
    //                    //neighbour.parent = bestTile;
    //                }

    //                //if (cost < neighbour.gCost || !openList.Contains(neighbour))
    //                //{
    //                //    neighbour.gCost = cost;
    //                //    neighbour.hCost = CalculateDistance(neighbour, goal);
    //                //    neighbour.parent = bestTile;
    //                //}
    //            }
    //        }
    //    }
    //    Debug.LogError("Path not found");
    //    return null;
    //}

    //private Tile GetBestTile(List<Tile> openList)
    //{
    //    Tile best = null;

    //    if (openList.Count > 0)
    //    {
    //        best = openList[0];

    //        foreach (Tile t in openList)
    //        {
    //            if (t.fCost() <= best.fCost())
    //            {
    //                if (t.hCost < best.hCost)
    //                {
    //                    best = t;
    //                }
    //            }
    //        }
    //    }

    //    return best;
    //}

    //public int CalculateDistance(Tile start, Tile end)
    //{
    //    return Mathf.Max(Mathf.Abs(start.coord.tileX - end.coord.tileX), Mathf.Abs(start.coord.tileY - end.coord.tileY));
    //}

    //public List<Tile> CreatePath(Tile currentTile)
    //{
    //    List<Tile> path = new List<Tile>();
    //    path.Add(currentTile);

    //    while (currentTile.parent != null)
    //    {
    //        currentTile = currentTile.parent;
    //        path.Add(currentTile);
    //    }
    //    path.Reverse();
    //    return path;
    //}

    public List<Tile> FindPath(Vector3 obj, Vector3 target)
    {

        Tile start = _nodes[(int)obj.x, (int)obj.z];
        Tile goal = _nodes[(int)target.x, (int)target.z];

        List<Tile> openList = new List<Tile>();
        List<Tile> closedList = new List<Tile>();
        openList.Add(start);

        while (openList.Count > 0)
        {
            Tile currentTile = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].fCost() < currentTile.fCost() || openList[i].fCost() == currentTile.fCost())
                {
                    if (openList[i].hCost < currentTile.hCost)
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
                if (neighbour.type != MapGenerator.TileType.Room || closedList.Contains(neighbour))
                {
                    continue;
                }

                int cost = currentTile.gCost + CalculateDistance(currentTile, neighbour);
                if (cost < neighbour.gCost || !openList.Contains(neighbour))
                {
                    neighbour.gCost = cost;
                    neighbour.hCost = CalculateDistance(neighbour, goal);
                    neighbour.parent = currentTile;

                    if (!openList.Contains(neighbour))
                    {
                        openList.Add(neighbour);
                    }
                }
            }
        }
        Debug.LogError("Path not found");
        return null;
    }

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

                if (MapGenerator.IsInBounds(tile.coord.tileX + x, tile.coord.tileY + y))
                {
                    neighbours.Add(_nodes[tile.coord.tileX + x, tile.coord.tileY + y]);
                }
            }
        }
        return neighbours;
    }

    private int CalculateDistance(Tile tileA, Tile tileB)
    {
        int distanceX = Mathf.Abs(tileA.coord.tileX - tileB.coord.tileX);
        int distanceY = Mathf.Abs(tileA.coord.tileY - tileB.coord.tileY);

        if (distanceX > distanceY)
            return 14 * distanceY + 10 * (distanceX - distanceY);
        return 14 * distanceX + 10 * (distanceY - distanceX);
    }

    private List<Tile> CreatePath(Tile start, Tile end)
    {
        List<Tile> path = new List<Tile>();
        Tile currentTile = end;

        while (currentTile != start)
        {
            path.Add(currentTile);
            currentTile = currentTile.parent;
        }

        path.Reverse();
        return path;
    }
}
