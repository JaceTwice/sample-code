using UnityEngine;
using System.Collections;

public class TileSetManager : MonoBehaviour {

    public GameObject Default;
    public GameObject Green;
    public GameObject Greek;

    private TileSet DefaultSet;
    private TileSet GreenSet;
    private TileSet GreekSet;

    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "Grass");
    }

    void Awake()
    {
        DefaultSet = Default.GetComponent<TileSet>();
        GreenSet = Green.GetComponent<TileSet>();
        GreekSet = Greek.GetComponent<TileSet>();
    }

    public GameObject GetTileInTileset(TileSets ts, TileRenderTypes tt)
    {
        Debug.Log("TileSet: " + ts + " Type: " + tt);
        switch (ts)
        {
            case TileSets.Default:
                return DefaultSet.checkIfTileExists(tt);
            case TileSets.Green:
                return GreenSet.checkIfTileExists(tt);
            case TileSets.Greek:
                return GreekSet.checkIfTileExists(tt);
            default:
                return null;
        }
    }
}

public enum TileSets
{
    Default,
    Green,
    Greek,
}
