using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    private MapGenerator.TileType _type; //Tile type
    private GameObject _obj; //Cube game object on this tile
    private int _hCost = 0; //Tile's h cost
    private int _gCost = 0; //Tile's g cost
    private Tile _parent = null; //Previous tile in current path
    private int _neighbourWalls = 0; //Number of walls surrounding this tile

    public MapGenerator.Coordinate Coord; //Map coordinates of tile
    public MapGenerator.TileType Type { get { return _type; } set { _type = value; } }
    public GameObject Obj { get { return _obj; } set { _obj = value; } }
    public int HCost { get { return _hCost; } set { _hCost = value; } }
    public int GCost { get { return _gCost; } set { _gCost = value; } }
    public Tile Parent { get { return _parent; } set { _parent = value; } }
    public int NeighbourWalls { get { return _neighbourWalls; } set { _neighbourWalls = value; } }
    public int FCost { get { return _gCost + _hCost + (_neighbourWalls * 5); } }

    /// <summary>
    /// Checks if this tile is the same as a gaiven tile
    /// </summary>
    /// <param name="other">Tile to check</param>
    /// <returns>True of false</returns>
    public bool isEqual(Tile other)
    {
        if (this.Coord.tileX == other.Coord.tileX && this.Coord.tileY == other.Coord.tileY)
        {
            return true;
        }
        return false;
    }
}
