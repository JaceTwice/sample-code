using UnityEngine;
using System.Collections;

public class PlayerTileRaycast : MonoBehaviour {

    private Vector3 oldPos;
    private DungeonGenerator DG;
    public float yOffset = 1;

	// Use this for initialization
	void Start () {
        DG = GameObject.Find("Dungeon Gen").GetComponent<DungeonGenerator>();
        oldPos = Vector3.zero;
	}

	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        Vector3 vec = new Vector3(transform.position.x, transform.position.y - yOffset, transform.position.z);
        if (Physics.Raycast((vec), Vector3.down, out hit, 10.0f))
        {
            Debug.DrawLine(vec, hit.point);
            Vector3 pos = hit.transform.parent.position;
            //Debug.Log(pos);

            if (pos != oldPos)
            {
                Vector3 DunPos = CalulateDungeonTilePos(pos);
                //Debug.Log(hit.transform.parent.name);
                //Debug.Log(DunPos);
                DatabaseCommunicator.SendCharPos(DunPos, 1);
                oldPos = pos;
            }
        }//end IF physics.Raycast
	}//end update()

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
