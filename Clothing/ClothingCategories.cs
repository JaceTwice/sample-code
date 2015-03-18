using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ClothingCategory
{
    Hair,
    Hat,
    Earring,
    Glasses,
    Neckwear,
    Undershirt,
    Outerwear,
    LeftArmwear,
    RightArmwear,
    Pants,
    Skirt,
    Dress,
    LeftSock,
    RightSock,
    LeftShoe,
    RightShoe,
    Accessory,
    Belt
}

public class ClothingDatabase : MonoBehaviour
{
    static private List<ItemNamePair> clothingItems;

    static private bool isDatabaseLoaded = false;

    static public int Size
    {
        get
        {
            ValidateDatabase();
            return clothingItems.Count; 
        }
    }

    static private void ValidateDatabase()
    {
        if (clothingItems == null) clothingItems = new List<ItemNamePair>();
        if (!isDatabaseLoaded) LoadDatabase();
    }

    static public void LoadDatabase()
    {
        if (isDatabaseLoaded) return;
        isDatabaseLoaded = true;
        ForceLoadDatabase();
    }

    static public void ForceLoadDatabase()
    {
        ValidateDatabase();
        GameObject[] resources = Resources.LoadAll<GameObject>(@"Clothing");
        foreach (GameObject item in resources)
        {
            ItemNamePair pair = new ItemNamePair();
            pair.Item = item;
            pair.Script = item.GetComponent<ClothingItem>();
            pair.id = pair.Script.name;
            pair.Type = pair.Script.ClothingType;
            clothingItems.Add(pair);
        }
    }

    static public void ClearDatabase()
    {
        isDatabaseLoaded = false;
        clothingItems.Clear();
    }

    static public GameObject GetItem(string name)
    {
        ValidateDatabase();
        foreach (ItemNamePair item in clothingItems)
        {
            if (item.id == name)
            {
                return item.Item;
            }
        }
        return null;
    }

    static public GameObject GetItem(int index)
    {
        ValidateDatabase();
        return clothingItems[index].Item;
    }

    static public string GetItemName(int index)
    {
        ValidateDatabase();
        return clothingItems[index].id;
    }

    static public List<GameObject> GetItems(ClothingCategory type)
    {
        List<GameObject> objectsOfType = new List<GameObject>();

        foreach (ItemNamePair item in clothingItems)
        {
            if (item.Type == type)
            {
                objectsOfType.Add(item.Item);
            }
        }
        return objectsOfType;
    }

    public struct ItemNamePair
    {
        public GameObject Item;
        public string id;
        public ClothingItem Script;
        public ClothingCategory Type;
    }
}

