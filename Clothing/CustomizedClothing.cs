using UnityEngine;
using System.Collections;

public class CustomizedClothing{

    private static uint globalNextID = 0;
    public static uint GlobalNextID
    {
        get
        { 
            globalNextID++;
            return globalNextID;
        }
    }
    public uint id;
    public GameObject Item;
    public ClothingItem ItemScr;
    public ClothingCategory Type;
    public uint idMirror = 0;

    public Color[] Colors;

    public bool equipped = false;

    public CustomizedClothing(GameObject item)
    {
        id = CustomizedClothing.GlobalNextID;
        if (ValidateNewItem(item))
        {
            Item = item;
            ItemScr = item.GetComponent<ClothingItem>();
            Type = ItemScr.ClothingType;
            Colors = ItemScr.Colors;
        }
    }

    private bool ValidateNewItem(GameObject item)
    {
        if(item.GetComponent<ClothingItem>())
            return true;
        return false;
    }
}
