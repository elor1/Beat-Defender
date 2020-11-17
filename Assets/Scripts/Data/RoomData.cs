using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomData
{
    public List<MapGenerator.Coordinate> tiles;
    public List<MapGenerator.Coordinate> edgeTiles;
    public List<RoomData> connectedRooms;
    public int roomSize;

    public RoomData() { }

    /// <summary>
    /// Room constructor
    /// </summary>
    /// <param name="roomTiles">List of coordinates of tiles in room</param>
    /// <param name="mapGrid">Array of TileTypes with map grid data</param>
    public RoomData(List<MapGenerator.Coordinate> roomTiles, MapGenerator.TileType[,] mapGrid)
    {
        tiles = roomTiles;
        roomSize = tiles.Count;
        connectedRooms = new List<RoomData>();

        edgeTiles = new List<MapGenerator.Coordinate>();
        foreach (MapGenerator.Coordinate tile in tiles)
        {
            for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++)
            {
                for (int y = tile.tileY - 1; y <= tile.tileY + 1; y++)
                {
                    if (x == tile.tileX || y == tile.tileY)
                    {
                        if (mapGrid[x, y] == MapGenerator.TileType.Wall)
                        {
                            edgeTiles.Add(tile);
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
        room1.connectedRooms.Add(room2);
        room2.connectedRooms.Add(room1);
    }

    /// <summary>
    /// Checks whether this room is connected to another given room
    /// </summary>
    /// <param name="otherRoom">Room to check connections for</param>
    /// <returns>True or false</returns>
    public bool IsConnected(RoomData otherRoom)
    {
        return connectedRooms.Contains(otherRoom);
    }
}
