using UnityEngine;
using System.Collections.Generic;

public class Room {

    private int _roomID;  //the unique number for the room
    public int RoomID
    {
        get { return _roomID; }
    }

    private Vector2 _roomSize;
    public Vector2 RoomSize
    {
        get { return _roomSize; }
    }

    public List<Tile> PerimiterTiles;
    public List<Tile> RoomTiles;

    public Room(int id, int width, int depth)
    {
        _roomID = id;
        _roomSize = new Vector2(width, depth);

        PerimiterTiles = new List<Tile>();
        RoomTiles = new List<Tile>();
    }
}
