using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileData
{
    public static TileRotDirections[] Cardinal = {TileRotDirections.North,
                                                  TileRotDirections.East,
                                                  TileRotDirections.South,
                                                  TileRotDirections.West
                                                 };

    private TileRotDirections _tileDir;
    public TileRotDirections TileDir
    {
        get { return _tileDir; }
        set { _tileDir = value; }
    }

    private TileDataTypes _tileType;
    public TileDataTypes TileType
    {
        get { return _tileType; }
        set { _tileType = value; }
    }

    private Vector3 _dungeonPosition;
    public Vector3 DungeonPosition
    {
        get { return _dungeonPosition; }
    }

    //High level feilds are pointed to multiple tile types
    #region High level fields

    public bool OpenSpace
    {
        get
        {
            if (_tileType == TileDataTypes.Room || 
                _tileType == TileDataTypes.Corridor)
                return true;
            else
                return false;
        }
    }

    public bool DoorSpace
    {
        get
        {
            if (_tileType == TileDataTypes.Arch ||
                _tileType == TileDataTypes.Door)
                return true;
            else
                return false;
        }
    }

    public bool EntranceSpace
    {
        get
        {
            if (_tileType == TileDataTypes.Entrance ||
                DoorSpace)
                return true;
            else
                return false;
        }
    }

    #endregion

    public TileData(Vector3 pos, TileRotDirections tDir)
    {
        _dungeonPosition = pos;
        _tileDir = tDir;
    }


}

public enum TileDataTypes
{
    Nothing,
    Blocked,
    Room,
    Corridor,
    Perimeter,
    Entrance,
    Arch,
    Door,
    Stair,
    Trap,
    Spawn
}

public enum TileRotDirections :short
{
    North = 90,
    East = 0,
    South = 270,
    West = 180,
}