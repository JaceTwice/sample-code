using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DungeonGenerator : MonoBehaviour {

    //public feilds
    public int DungeonWidth = 50;                           //the number of tiles in the X direction
    public int DungeonDepth = 50;                           //the number of tiles in the Y direction
    public int DungeonHeight = 0;                           //the number of floors in the dungeon(unused)
    public Vector3 DungeonStartingPosition = Vector3.zero;  //the starting coordinate for the dungeon tiles
    public TileSets CurrentTileSet = TileSets.Default;      //the tileset to use in generating the dungeon

    public int roomMinSize = 3;
    public int roomMaxSize = 5;

    public int NumPots = 10;
    public GameObject Urn;
    public int NumChests = 4;
    public GameObject Chest;

    //properties
    private TileSetManager TSM;         //A reference to the TileSetManager object
    private GameManager GM;             //A reference to the GameManager object
    private Tile[,] DungeonTiles;       //2d array of all of the tiles in the dungeon
    private int DungeonArea;            //The number of tiles in the dungeon(used for serialization)

    //serilization properties
    private int[] sTilesX;               //x positions of all dungeon tiles
    private int[] sTilesZ;               //y positions of all dingeon tiles
    private string[] sTileNames;         //the string names of each tile
    private string[] sDirection;         //the cardinal direction each tile is facing

    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "Gear");
    }

    void Awake()
    {
        TSM = GameObject.Find("Tile Set Manager").GetComponent<TileSetManager>();
        GM = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

	// Use this for initialization
	void Start () {

        //TSM = GameObject.Find("Tile Set Manager").GetComponent<TileSetManager>();

        //SetupDungeon();
	}

    public void SetupDungeon()
    {
        Debug.Log("------------Setting up dungeon------------");
        DungeonTiles = new Tile[DungeonWidth, DungeonDepth];
        DungeonArea = DungeonTiles.Length;
        Debug.Log("Dungeon area is: " + DungeonArea);
        
        for (int i = 0; i < DungeonWidth; i++)
        {
            for (int j = 0; j < DungeonDepth; j++)
            {
                DungeonTiles[i, j] = new Tile(new Vector3(i, DungeonHeight, j), TileRotDirections.North);
            }
        }

        Debug.Log("------------Finished setting up dungeon------------");

        DoRoomPass();
        //DoTeeRoomPass();
    }

    private void DoRoomPass()
    {
        Debug.Log("------------Building Rooms------------");
        int numRooms = CalculateNumberOfRooms();

        for (int i = 0; i < numRooms; i++)
        {
            int xRand = 0;
            do
            {
                xRand = (int)Random.Range(0, DungeonWidth);
            } while (xRand < (DungeonWidth - roomMinSize - 2));
            int zRand = 0;
            do
            {
                zRand = (int)Random.Range(0, DungeonDepth);
            } while (zRand < (DungeonWidth - roomMinSize - 2));
            //Debug.Log(xRand + ", " + zRand);
        }

        MakeSingleRoom(1, 1, 10, 10);
        MakeSingleOverlapRoom(1, 6, 5, 5);
        MakeSingleRoom(10, 8, 6, 2);
        MakeSingleRoom(15, 6, 5, 7);
        MakeSingleRoom(2, 16, 14, 4);
        MakeSingleRoom(15, 12, 1, 4);
        MakeSingleRoom(8, 10, 2, 6);
        MakeSingleRoom(3, 10, 1, 6);
        MakeSingleOverlapRoom(18, 6, 2, 2);
        MakeSingleRoom(18, 7, 2, 3);

        PlaceFeatures();

        Debug.Log("------------Finished building rooms------------");
        StartCoroutine("PlaceDungeonTiles");
    }

    private int CalculateNumberOfRooms()
    {
        return 4;
    }

    private void DoTeeRoomPass()
    {
        Debug.Log("------------Building Tee Room------------");

        MakeGoalHall((int)Mathf.Ceil(Random.Range(1,5)) , "horizontal");


        MakeStartHall((int)Mathf.Ceil(Random.Range(1, 5)), "vertical");

        Debug.Log("------------Finished building tee room------------");
        StartCoroutine("PlaceDungeonTiles");
    }

    //----------------------------
    //Tile Placement Methods
    //----------------------------
    IEnumerator PlaceDungeonTiles()
    {
        Debug.Log("------------Placing Tiles------------");
        for (int i = 0; i < DungeonWidth; i++)
        {
            for (int j = 0; j < DungeonDepth; j++)
            {
                switch (DungeonTiles[i, j].Data.TileType)
                {
                    case TileDataTypes.Room:
                        Instantiate(Resources.Load(GetTilesetPrefabName(TileRenderTypes.Floor))
                                            , CalulateDungeonTilePosition(i, DungeonHeight, j)
                                            , CalculateDungeonTileRotation(DungeonTiles[i, j])
                                            );
                        break;
                    case TileDataTypes.Perimeter:
                        Instantiate(Resources.Load(GetTilesetPrefabName(PickWallType(i, j)))
                                            , CalulateDungeonTilePosition(i, DungeonHeight, j)
                                            , CalculateDungeonTileRotation(DungeonTiles[i, j])
                                            );
                        break;
                    case TileDataTypes.Trap:
                        Instantiate(Resources.Load(GetTilesetPrefabName(TileRenderTypes.Trap))
                                            , CalulateDungeonTilePosition(i, DungeonHeight, j)
                                            , CalculateDungeonTileRotation(DungeonTiles[i, j])
                                            );
                        break;
                    case TileDataTypes.Stair:
                        Instantiate(Resources.Load(GetTilesetPrefabName(TileRenderTypes.Stair))
                                            , CalulateDungeonTilePosition(i, DungeonHeight, j)
                                            , CalculateDungeonTileRotation(DungeonTiles[i, j])
                                            );
                        break;
                    case TileDataTypes.Spawn:
                        Instantiate(Resources.Load(GetTilesetPrefabName(TileRenderTypes.Spawn))
                                            , CalulateDungeonTilePosition(i, DungeonHeight, j)
                                            , CalculateDungeonTileRotation(DungeonTiles[i, j])
                                            );
                        break;
                }
                yield return null;
            }
        }
        Debug.Log("------------Finished placing Tiles------------");

        PlaceUrns();
        PlaceChests();

        DungeonComplete();
    }

    private TileRenderTypes PickWallType(int row, int col)
    {
        //get surrounding tiles
        bool[] neighbors = new bool[4];
        int numPerim = 0;

        //check West tile
        if (row != 0)
        {
            if (DungeonTiles[row - 1, col].Data.TileType == TileDataTypes.Perimeter)
                neighbors[0] = true;
            else
                neighbors[0] = false;
        }
        //check East tile
        if (row != DungeonWidth - 1)
        {
            if (DungeonTiles[row + 1, col].Data.TileType == TileDataTypes.Perimeter)
                neighbors[1] = true;
            else
                neighbors[1] = false;
        }
        //check south tile
        if (col != 0)
        {
            if (DungeonTiles[row, col - 1].Data.TileType == TileDataTypes.Perimeter)
                neighbors[2] = true;
            else
                neighbors[2] = false;
        }
        //check north tile
        if (col != DungeonDepth - 1)
        {
            if (DungeonTiles[row, col + 1].Data.TileType == TileDataTypes.Perimeter)
                neighbors[3] = true;
            else
                neighbors[3] = false;
        }

        //loop through neighbors and check to see how many of them are perimeters
        foreach (bool b in neighbors)
        {
            if (b == true)
            {
                numPerim++;
            }
        }

        // if the number of perimeters is greater then 2 change the current tile to a corner block
        if (numPerim != 2)
            return TileRenderTypes.Corner;
        else
        {
            float TorchPercent = .2f;
            float TorchRand = Random.value;
            if (neighbors[0] == true && neighbors[1] == true)
            {
                DungeonTiles[row, col].Data.TileDir = TileRotDirections.East;
                if (TorchRand > TorchPercent)
                    return TileRenderTypes.Wall;
                else
                    return TileRenderTypes.TorchWall;
            }
            else if (neighbors[2] == true && neighbors[3] == true)
            {
                DungeonTiles[row, col].Data.TileDir = TileRotDirections.North;
                if (TorchRand > TorchPercent)
                    return TileRenderTypes.Wall;
                else
                    return TileRenderTypes.TorchWall;
            }
            else
                return TileRenderTypes.Corner;
        }
    }

    private string GetTilesetPrefabName(TileRenderTypes tile)
    {
        switch (CurrentTileSet)
        {
            case TileSets.Default:
                return "Default Set/" + TSM.GetTileInTileset(CurrentTileSet, tile).name;
            case TileSets.Green:
                return "Green Set/" + TSM.GetTileInTileset(CurrentTileSet, tile).name;
            case TileSets.Greek:
                return "Greek Set/" + TSM.GetTileInTileset(CurrentTileSet, tile).name;
        }
        return "Error Block";
    }

    //-----------------------
    //Dungeon Area Methods
    //-----------------------
    private List<Tile> GetTilesOfType(TileDataTypes type)
    {
        List<Tile> tileList = new List<Tile>();
        for (int i = 0; i < DungeonWidth; i++)
        {
            for (int j = 0; j < DungeonDepth; j++)
            {
                Tile aTile = DungeonTiles[i, j];
                if (aTile.Data.TileType == type)
                    tileList.Add(aTile);
            }
        }
        Debug.Log(tileList.Count);
        return tileList;
    }

    private void PlaceFeatures()
    {
        List<Tile> floorList = GetTilesOfType(TileDataTypes.Room);

        for (int i = 0; i < 20; i++)
        {
            floorList[(int)(Random.value * floorList.Count)].Data.TileType = TileDataTypes.Trap;
        }
        floorList = GetTilesOfType(TileDataTypes.Room);
        floorList[(int)(Random.value * floorList.Count)].Data.TileType = TileDataTypes.Spawn;
        floorList = GetTilesOfType(TileDataTypes.Room);
        floorList[(int)(Random.value * floorList.Count)].Data.TileType = TileDataTypes.Stair;
    }

    private void PlaceUrns()
    {
        List<Tile> floorList = GetTilesOfType(TileDataTypes.Room);

        for (int i = 0; i < NumPots; i++)
        {
            Vector3 pos = floorList[(int)(Random.value * floorList.Count)].Data.DungeonPosition;
            pos.x *= Tile.TILE_SIZE;
            pos.z *= Tile.TILE_SIZE;
            GameObject aUrn = (GameObject)Instantiate(Urn, pos, Quaternion.identity);
            aUrn.GetComponent<Urn>().ID = i;
            aUrn.GetComponent<Urn>().Init();
        }
    }

    private void PlaceChests()
    {
        List<Tile> floorList = GetTilesOfType(TileDataTypes.Room);

        for (int i = 0; i < NumChests; i++)
        {
            Vector3 pos = floorList[(int)(Random.value * floorList.Count)].Data.DungeonPosition;
            pos.x *= Tile.TILE_SIZE;
            pos.y += 0.6f;
            pos.z *= Tile.TILE_SIZE;
            GameObject aChest = (GameObject)Instantiate(Chest, pos, Quaternion.identity);
            aChest.GetComponent<treasureScript>().ID = i;
            aChest.GetComponent<treasureScript>().Init();
        }
    }

    private void MakeSingleRoom(int startRow, int startCol, int width, int depth)
    {
        if (startRow <= 0 || startCol <= 0)
        {
            Debug.LogError("Invalid starting column or row in MakeSingleRoom()");
        }

        //open room tiles
        for (int i = startRow; i < startRow+width; i++)
        {
            for (int j = startCol; j < startCol + depth; j++)
            {   
                DungeonTiles[i, j].Data.TileType = TileDataTypes.Room;
            }
        }
        //place perimeters
        for (int i = startRow - 1; i < startRow + width + 1; i++)
        {
            if (DungeonTiles[i, startCol - 1].Data.TileType == TileDataTypes.Nothing)
                DungeonTiles[i, startCol - 1].Data.TileType = TileDataTypes.Perimeter;
            if (DungeonTiles[i, startCol + depth].Data.TileType == TileDataTypes.Nothing)
                DungeonTiles[i, startCol + depth].Data.TileType = TileDataTypes.Perimeter;
        }
        for (int i = startCol - 1; i < startCol + depth + 1; i++)
        {
            if (DungeonTiles[startRow - 1, i].Data.TileType == TileDataTypes.Nothing)
                DungeonTiles[startRow - 1, i].Data.TileType = TileDataTypes.Perimeter;
            if (DungeonTiles[startRow + width, i].Data.TileType == TileDataTypes.Nothing)
                DungeonTiles[startRow + width, i].Data.TileType = TileDataTypes.Perimeter;
        }
    }

    private void MakeSingleOverlapRoom(int startRow, int startCol, int width, int depth)
    {
        if (startRow <= 0 || startCol <= 0)
        {
            Debug.LogError("Invalid starting column or row in MakeSingleRoom()");
        }

        //open room tiles
        for (int i = startRow; i < startRow + width; i++)
        {
            for (int j = startCol; j < startCol + depth; j++)
            {
                DungeonTiles[i, j].Data.TileType = TileDataTypes.Room;
            }
        }
        //place perimeters
        for (int i = startRow - 1; i < startRow + width + 1; i++)
        {
            DungeonTiles[i, startCol - 1].Data.TileType = TileDataTypes.Perimeter;
            DungeonTiles[i, startCol + depth].Data.TileType = TileDataTypes.Perimeter;
        }
        for (int i = startCol - 1; i < startCol + depth + 1; i++)
        {
            DungeonTiles[startRow - 1, i].Data.TileType = TileDataTypes.Perimeter;
            DungeonTiles[startRow + width, i].Data.TileType = TileDataTypes.Perimeter;
        }
    }

    #region Halls
    private void MakeStartHall(int rowcol, string dir)
    {
        MakeHall(rowcol, dir);
        if (dir == "horizontal")
        {
            DungeonTiles[5, rowcol].Data.TileType = TileDataTypes.Spawn;
        }
        if (dir == "vertical")
        {
            DungeonTiles[rowcol, 5].Data.TileType = TileDataTypes.Spawn;
        }
    }

    private void MakeGoalHall(int rowcol, string dir)
    {
        MakeHall(rowcol, dir);
        if (dir == "horizontal")
        {
            DungeonTiles[1, rowcol].Data.TileType = TileDataTypes.Trap;
            DungeonTiles[5, rowcol].Data.TileType = TileDataTypes.Stair;
        }
        if (dir == "vertical")
        {
            DungeonTiles[rowcol, 1].Data.TileType = TileDataTypes.Trap;
            DungeonTiles[rowcol, 5].Data.TileType = TileDataTypes.Stair;
        }
    }

    private void MakeHall(int rowcol, string dir)
    {
        Debug.Log("Making " + dir + " hall on:" + rowcol);
        //lay down the floor tiles
        int i;
        int j;
        # region horizontal
        if (dir == "horizontal")
        {
            //set tiles on the hall row
            for (i = 0; i < DungeonWidth; i++)
            {
                //check the ends of the hall row and place perimeters on the extreme ends
                if (i == 0 || i == DungeonWidth-1)
                {
                    DungeonTiles[i, rowcol].Data.TileType = TileDataTypes.Perimeter;
                }
                //fill the rest with room tiles
                else
                {
                    DungeonTiles[i, rowcol].Data.TileType = TileDataTypes.Room;
                }
            }
            //fill all tiles above the hall row with perimeter tiles
            for (i = 0; i < DungeonWidth; i++)
            {
                if (DungeonTiles[i, rowcol - 1].Data.TileType == TileDataTypes.Nothing)
                {
                    DungeonTiles[i, rowcol - 1].Data.TileType = TileDataTypes.Perimeter;
                }
            }
            //fill all tiles below the hall row with perimeter tiles
            for (i = 0; i < DungeonWidth; i++)
            {
                if (DungeonTiles[i, rowcol + 1].Data.TileType == TileDataTypes.Nothing)
                {
                    DungeonTiles[i, rowcol + 1].Data.TileType = TileDataTypes.Perimeter;
                }
            }
        }
        #endregion
        #region vertical
        if (dir == "vertical")
        {
            //set tiles on the hall row
            for (j = 0; j < DungeonDepth; j++)
            {
                //check the ends of the hall row and place perimeters on the extreme ends
                if (j == 0 || j == DungeonDepth - 1)
                {
                    DungeonTiles[rowcol, j].Data.TileType = TileDataTypes.Perimeter;
                }
                //fill the rest with room tiles
                else
                {
                    DungeonTiles[rowcol, j].Data.TileType = TileDataTypes.Room;
                }
            }
            //fill all tiles above the hall row with perimeter tiles
            for (j = 0; j < DungeonDepth; j++)
            {
                if (DungeonTiles[rowcol - 1, j].Data.TileType == TileDataTypes.Nothing)
                {
                    DungeonTiles[rowcol - 1, j].Data.TileType = TileDataTypes.Perimeter;
                }
            }
            //fill all tiles below the hall row with perimeter tiles
            for (j = 0; j < DungeonDepth; j++)
            {
                if (DungeonTiles[rowcol + 1, j].Data.TileType == TileDataTypes.Nothing)
                {
                    DungeonTiles[rowcol + 1, j].Data.TileType = TileDataTypes.Perimeter;
                }
            }
        }
        #endregion

    }
    #endregion

    //-----------------------
    //Helper Methods
    //-----------------------
    private void SerializeDungeon()
    {
        sTilesX = new int[DungeonArea];
        sTilesZ = new int[DungeonArea];
        sTileNames = new string[DungeonArea];
        sDirection = new string[DungeonArea];

        int counter = 0;

        for (int i = 0; i < DungeonWidth; i++)
        {
            for (int j = 0; j < DungeonDepth; j++)
            {
                sTilesX[counter] = (int)DungeonTiles[i, j].Data.DungeonPosition.x;
                sTilesZ[counter] = (int)DungeonTiles[i, j].Data.DungeonPosition.z;
                sTileNames[counter] = DungeonTiles[i, j].Data.TileType.ToString();
                sDirection[counter] = DungeonTiles[i, j].Data.TileDir.ToString();

                //Debug.Log("(" + i + ", " + j + ")" + counter + "(" + sTilesX[counter] + ", " + sTilesZ[counter] + ") Type: " + sTileNames[counter] + " Direction: " + sDirection[counter]); 

                counter++;
            }
        }
    }

    private void DungeonComplete()
    {
        SerializeDungeon();
        DatabaseCommunicator.SendDungeon(sTilesX, sTilesZ, sTileNames, sDirection);
        GM.SetupGame();
    }

    private Quaternion CalculateDungeonTileRotation(Tile t)
    {
        Quaternion rot = Quaternion.Euler(0, (float)t.Data.TileDir, 0);
        return rot;
    }

    private Vector3 CalulateDungeonTilePosition(int x, int y, int z)
    {
        Vector3 vec = new Vector3(DungeonStartingPosition.x + (x * Tile.TILE_SIZE)
                                , DungeonStartingPosition.y + (y * Tile.TILE_HEIGHT)
                                , DungeonStartingPosition.z + (z *Tile.TILE_SIZE)
                                );
        return vec;
    }
}