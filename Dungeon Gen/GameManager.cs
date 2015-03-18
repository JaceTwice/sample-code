using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

    public GameObject DungeonGenerator;
    public List<GameObject> items;
    public GameObject DirectionArrow;
    public GameObject respawn;
    public int spawn = 0;
    public GameObject MainCamera;
    public bool lockCamera = true;

    public float locx = 0;
    public float locz = 0;

    private Vector3 teleportX;
    private Vector3 teleportZ;

    public static int itemIDCount = 0;
    public static int arrowIDCount = 0;

    private DungeonGenerator DG;

    public static int incrementItemID()
    {
        itemIDCount++;
        return itemIDCount;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "Alien");
    }

    // Use this for initialization
    void Start()
    {
        DG = DungeonGenerator.GetComponent<DungeonGenerator>();
        DG.SetupDungeon();

        Screen.lockCursor = true;
    }

    public void SetupGame()
    {
        GetRespawns();
    }

    private void GetRespawns()
    {
        Debug.Log("-------------Getting spawns-------------");
        respawn = GameObject.FindGameObjectWithTag("Respawn");
        respawn.GetComponent<PlayerSpawn>().SpawnPlayer();
        MainCamera.camera.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            lockCamera = true;

        if (Input.GetKeyDown("escape"))
            lockCamera = false;

        if (lockCamera)
            Screen.lockCursor = true;
        else
            Screen.lockCursor = false;
    }

    public void TeleportX(float x)
    {
        locx = x;
    }

    public void TeleportZ(float z)
    {
        locz = z;
    }

    #region ItemInstantiators
    public void instantiateGFleece()
    {
        GameObject item;
        item = items[0];
        Vector3 spawnPos = CalculateGlobalDungeonCoord(new Vector3(locx, 1.0f, locz));
        spawnPos.y = 1;
        Instantiate(item, spawnPos, Quaternion.identity);
    }
    public void instantiateHPotion()
    {
        GameObject item;
        item = items[1];
        Vector3 spawnPos = CalculateGlobalDungeonCoord(new Vector3(locx, 1.0f, locz));
        spawnPos.y = 1;
        Instantiate(item, spawnPos, Quaternion.identity);
    }
    public void instantiateHOHelm()
    {
        GameObject item;
        item = items[2];
        Vector3 spawnPos = CalculateGlobalDungeonCoord(new Vector3(locx, 1.0f, locz));
        spawnPos.y = 1;
        Instantiate(item, spawnPos, Quaternion.identity);
    }
    public void instantiateHSandals()
    {
        GameObject item;
        item = items[3];
        Vector3 spawnPos = CalculateGlobalDungeonCoord(new Vector3(locx, 1.0f, locz));
        spawnPos.y = 1;
        Instantiate(item, spawnPos, Quaternion.identity);
    }
    public void instantiateHDHelm()
    {
        GameObject item;
        item = items[4];
        Vector3 spawnPos = CalculateGlobalDungeonCoord(new Vector3(locx, 1.0f, locz));
        spawnPos.y = 1;
        Instantiate(item, spawnPos, Quaternion.identity);
    }
    public void instantiateJSandals()
    {
        GameObject item;
        item = items[5];
        Vector3 spawnPos = CalculateGlobalDungeonCoord(new Vector3(locx, 1.0f, locz));
        spawnPos.y = 1;
        Instantiate(item, spawnPos, Quaternion.identity);
    }
    public void instantiatePPotion()
    {
        GameObject item;
        item = items[6];
        Vector3 spawnPos = CalculateGlobalDungeonCoord(new Vector3(locx, 1.0f, locz));
        spawnPos.y = 1;
        Instantiate(item, spawnPos, Quaternion.identity);
    }
    public void instantiateSFleece()
    {
        GameObject item;
        item = items[7];
        Vector3 spawnPos = CalculateGlobalDungeonCoord(new Vector3(locx, 1.0f, locz));
        spawnPos.y = 1;
        Instantiate(item, spawnPos, Quaternion.identity);
    }
    public void instantiateSpShoes()
    {
        GameObject item;
        item = items[8];
        Vector3 spawnPos = CalculateGlobalDungeonCoord(new Vector3(locx, 1.0f, locz));
        spawnPos.y = 1;
        Instantiate(item, spawnPos, Quaternion.identity);
    }

    public void instantiateStShoes()
    {
        GameObject item;
        item = items[9];
        Vector3 spawnPos = CalculateGlobalDungeonCoord(new Vector3(locx, 1.0f, locz));
        spawnPos.y = 1;
        Instantiate(item, spawnPos, Quaternion.identity);
    }
#endregion

    #region ArrowInstantiators
    public void instantiateUpArrow()
    {
        Vector3 spawnPos = CalculateGlobalDungeonCoord(new Vector3(locx, 1.0f, locz));
        spawnPos.y = 1;
        Instantiate(DirectionArrow, spawnPos, Quaternion.identity);
    }

    public void instantiateDownArrow()
    {
        Vector3 spawnPos = CalculateGlobalDungeonCoord(new Vector3(locx, 1.0f, locz));
        spawnPos.y = 1;
        Instantiate(DirectionArrow, spawnPos, Quaternion.Euler(new Vector3(0, 180, 0)));
    }

    public void instantiateLeftArrow()
    {
        Vector3 spawnPos = CalculateGlobalDungeonCoord(new Vector3(locx, 1.0f, locz));
        spawnPos.y = 1;
        Instantiate(DirectionArrow, spawnPos, Quaternion.Euler(new Vector3(0, 270, 0)));
    }

    public void instantiateRightArrow()
    {
        Vector3 spawnPos = CalculateGlobalDungeonCoord(new Vector3(locx, 1.0f, locz));
        spawnPos.y = 1;
        Instantiate(DirectionArrow, spawnPos, Quaternion.Euler(new Vector3(0, 90, 0)));
    }
    #endregion

    private Vector3 CalculateGlobalDungeonCoord(Vector3 dungeonPos)
    {
        Vector3 startPos = DG.DungeonStartingPosition;
        float tileSize = Tile.TILE_SIZE;

        Vector3 transformPos = Vector3.zero;
        transformPos.x = (dungeonPos.x * tileSize) - tileSize;
        transformPos.z = (dungeonPos.z * tileSize) - tileSize;

        return transformPos;
    }
}
