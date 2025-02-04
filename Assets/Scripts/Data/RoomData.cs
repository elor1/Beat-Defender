﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomData
{
    private List<MapGenerator.Coordinate> _tiles; //List of coordinates in room
    private List<MapGenerator.Coordinate> _edgeTiles; //List of coordinates on edge of room
    private List<RoomData> _connectedRooms; //List of connected rooms

    public List<MapGenerator.Coordinate> EdgeTiles { get { return _edgeTiles; } }

    public RoomData() { }

    /// <summary>
    /// Room constructor
    /// </summary>
    /// <param name="roomTiles">List of coordinates of tiles in room</param>
    /// <param name="mapGrid">Array of TileTypes with map grid data</param>
    public RoomData(List<MapGenerator.Coordinate> roomTiles, Tile[,] mapGrid)
    {
        _tiles = roomTiles;
        _connectedRooms = new List<RoomData>();

        _edgeTiles = new List<MapGenerator.Coordinate>();
        foreach (MapGenerator.Coordinate tile in _tiles)
        {
            for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++)
            {
                for (int y = tile.tileY - 1; y <= tile.tileY + 1; y++)
                {
                    if (x == tile.tileX || y == tile.tileY)
                    {
                        if (mapGrid[x, y].Type == MapGenerator.TileType.Wall)
                        {
                            _edgeTiles.Add(tile);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Adds a connection between two rooms
    /// </summary>
    /// <param name="room1">1st room to connect</param>
    /// <param name="room2">2nd room to connect</param>
    public static void ConnectRooms(RoomData room1, RoomData room2)
    {
        room1._connectedRooms.Add(room2);
        room2._connectedRooms.Add(room1);
    }

    /// <summary>
    /// Checks whether this room is connected to another given room
    /// </summary>
    /// <param name="otherRoom">Room to check connections for</param>
    /// <returns>True or false</returns>
    public bool IsConnected(RoomData otherRoom)
    {
        return _connectedRooms.Contains(otherRoom);
    }
}
