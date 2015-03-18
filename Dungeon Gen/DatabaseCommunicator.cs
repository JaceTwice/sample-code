using UnityEngine;
using System.Collections;

public static class DatabaseCommunicator : object {

    public static void SendCharPos(Vector3 playerPos, int playerID)
    {
        int playerX = (int)playerPos.x;
        int playerZ = (int)playerPos.z;

        Application.ExternalCall("PlayerLOCUpdate", playerX, playerZ, playerID);
    }

    // xAR - ARray of X coordinates
    // zAR - ARray of Z coordinates
    // nAR - ARray of tile Names
    // dAR - ARray of tile Directions
    public static void SendDungeon(int[] xAR, int[] zAR, string[] nAR, string[] dAR)
    {
        Application.ExternalCall("BuildDungeon", xAR, zAR, nAR, dAR);
    }

    public static void SendEntitySpawn(string entityType, int entityID, Vector3 entityDungeonPos)
    {
        int entX = (int)entityDungeonPos.x;
        int entZ = (int)entityDungeonPos.z;

        Application.ExternalCall("CreateEntity", entityType, entityID, entX, entZ);
    }

    public static void SendEntityItemSpawn(string itemName, int entityID, Vector3 entityDungeonPos)
    {
        int entX = (int)entityDungeonPos.x;
        int entZ = (int)entityDungeonPos.z;

        Application.ExternalCall("CreateItemEntity", itemName, entityID, entX, entZ);
    }

    public static void SendPlayerPickUpItem(string itemName, int itemID, int playerID)
    {
        Application.ExternalCall("PlayerPickUp", itemName, itemID, playerID);
    }

    public static void SendPlayerDropItem(string itemName, int playerID)
    {
        Application.ExternalCall("PlayerDrop", itemName, playerID);
    }

    public static void SendArrowDestroy(int arrowID)
    {
        Application.ExternalCall("DestroyArrow", arrowID);
    }

    //0 = lose
    //1 = win
    public static void SendEndState(int endState)
    {
        Application.ExternalCall("GameEnd", endState);
    }
}
