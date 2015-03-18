using UnityEngine;
using System.Collections;

public class TileSet : MonoBehaviour {

    public GameObject Floor;
    public GameObject Wall;
    public GameObject TorchWall;
    public GameObject Corner;
    public GameObject Door;
    public GameObject Stair;
    public GameObject Trap;
    public GameObject Spawn;

    public GameObject checkIfTileExists(TileRenderTypes tile)
    {
        switch(tile)
        {
            case TileRenderTypes.Floor:
                return Floor != null ? Floor : null;
            case TileRenderTypes.Wall:
                return Wall != null ? Wall : null;
            case TileRenderTypes.TorchWall:
                return TorchWall != null ? TorchWall : null;
            case TileRenderTypes.Corner:
                return Corner != null ? Corner : null;
            case TileRenderTypes.Door:
                return Door != null ? Door : null;
            case TileRenderTypes.Stair:
                return Stair != null ? Stair : null;
            case TileRenderTypes.Trap:
                return Trap != null ? Trap : null;
            case TileRenderTypes.Spawn:
                return Spawn != null ? Spawn : null;
            default:
                return null;
        }
    }
}

public enum TileRenderTypes
{
    Floor,
    Wall,
    TorchWall,
    Corner,
    Door,
    Stair,
    Trap,
    Spawn,
}