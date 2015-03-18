using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {

    protected DungeonGenerator DG;
    protected EntityTypes EntType;
    private int id = -1;
    public int ID
    {
        get { return id; }
        set { id = value; }
    }

    void Awake()
    {
        DG = GameObject.Find("Dungeon Gen").GetComponent<DungeonGenerator>();
    }

    public void Init(string itemName = null)
    {
        
        RaycastHit hit;
        Vector3 vec = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
        if (Physics.Raycast((vec), Vector3.down, out hit, 10.0f))
        {
            Vector3 pos = hit.transform.parent.position;
            Vector3 DunPos = CalulateDungeonTilePos(pos);
            if (itemName != null)
            {
                DatabaseCommunicator.SendEntityItemSpawn(itemName, id, DunPos);
            }
            else
            {
                DatabaseCommunicator.SendEntitySpawn(EntType.ToString(), id, DunPos);
            }
        }
    }

    private Vector3 CalulateDungeonTilePos(Vector3 transformPos)
    {
        Vector3 startPos = DG.DungeonStartingPosition;
        float tileSize = Tile.TILE_SIZE;

        Vector3 dungeonPos = Vector3.zero;
        dungeonPos.x = (transformPos.x / tileSize) - startPos.x;
        dungeonPos.z = (transformPos.z / tileSize) - startPos.z;

        return dungeonPos;
    }
}

public enum EntityTypes
{
    Urn,
    Item,
    TreasureChest,
}
