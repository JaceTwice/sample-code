using UnityEngine;
using System.Collections;

public class Tile{

    public const float TILE_SIZE = 1.5f;
    public const float TILE_HEIGHT = 3f;

    public TileData Data;

    #region constructors

    public Tile(Vector3 pos, TileRotDirections tDir): this(new TileData(pos, tDir))
    {

    }

    public Tile(TileData TData)
    {
        Data = TData;
    }

    #endregion

    #region methods

    #endregion

}