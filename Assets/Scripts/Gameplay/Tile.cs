using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public MapGenerator.Coordinate coord;
    public MapGenerator.TileType type;
    public GameObject obj;
    public int hCost = 0;
    public int gCost = 0;
    public Tile parent = null;
    public int neighbourWalls = 0;

    public int fCost()
    {
        return gCost + hCost + (neighbourWalls * 5);
    }

    public bool isEqual(Tile other)
    {
        if (this.coord.tileX == other.coord.tileX && this.coord.tileY == other.coord.tileY)
        {
            return true;
        }
        return false;
    }
}
