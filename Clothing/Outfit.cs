using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class Outfit
{
    public string Name = "New Outfit";

    private List<CustomizedClothing> parts = new List<CustomizedClothing>();
    public List<CustomizedClothing> Parts
    {
        get { return parts; }
    }

    public void AddItem(CustomizedClothing item)
    {
        if (ItemExists(item.Type))
        {
            ClearItem(item.Type);
        }
        Parts.Add(item);
    }

    public GameObject GetItem(ClothingCategory type)
    {
        for (int i = 0; i < parts.Count; i++)
        {
            if (parts[i].Type == type)
            {
                return parts[i].Item;
            }
        }
        return null;
    }

    public GameObject GetItem(uint id)
    {
        for (int i = 0; i < parts.Count; i++)
        {
            if (parts[i].id == id)
            {
                return parts[i].Item;
            }
        }
        return null;
    }

    public bool ItemExists(ClothingCategory type)
    {
        for (int i = 0; i < parts.Count; i++)
        {
            if (parts[i].Type == type)
            {
                return true;
            }
        }
        return false;
    }

    public bool ItemExists(uint id)
    {
        for (int i = 0; i < parts.Count; i++)
        {
            if (parts[i].id == id)
            {
                return true;
            }
        }
        return false;
    }

    public void ClearItem(ClothingCategory type)
    {
        for (int i = 0; i < parts.Count; i++)
        {
            if (parts[i].Type == type)
            {
                parts.RemoveAt(i);
                return;
            }
        }
    }

    public void ClearItem(uint id)
    {
        for (int i = 0; i < parts.Count; i++)
        {
            if (parts[i].id == id)
            {
                parts.RemoveAt(i);
                return;
            }
        }
    }
}
