using UnityEngine;
using System.Collections;
using System;

public class ColorableObject : MonoBehaviour {



    //The SkinnedMeshRenderer to use
    public SkinnedMeshRenderer Renderer;
    //The the types of materials that exist in the SkinnedMeshRenderer
    public MaterialType[] EditableMaterials;
    //The names of each portion of the colorable object
    public String[] SectionLabel;


    private bool ifColorsInit = false;
    public bool IfColorsInit
    {
        get { return ifColorsInit; }
    }

    private Color[] colors;
    public Color[] Colors
    {
        get { return colors; }
    }

    void Awake()
    {
        SetupColors();
    }

    private void SetupColors()
    {
        //Set the length of the colors array
        int numOfColors = 0;
        for (int i = 0; i < EditableMaterials.Length; i++)
        {
            switch (EditableMaterials[i])
            {
                case MaterialType.OneColor:
                    numOfColors++;
                    break;
                case MaterialType.ThreeColor:
                    numOfColors += 3;
                    break;
                case MaterialType.SixColor:
                    numOfColors += 6;
                    break;
            }
        }
        colors = new Color[numOfColors];

        //Populate the default colors array
        for (int i = 0; i < numOfColors; i++)
        {
            colors[i] = GetColor(i);
        }
        ifColorsInit = true;
    }

    
    public Color GetColor(int index)
    {
        Vector2 matIndex = GetMaterialIndex(index);
        return Renderer.materials[(int)matIndex.x].GetColor("_BaseColor" + (matIndex.y + 1));
    }

    public void SetColor(Color color, int index)
    {
        if (!ifColorsInit)
        {
            SetupColors();
        }

        colors[index] = color;
        Vector2 matIndex = GetMaterialIndex(index);
        Renderer.materials[(int)matIndex.x].SetColor("_BaseColor" + (matIndex.y+1), color);
    }

    public void SetAllColors(Color[] newColors)
    {
        if (!ifColorsInit)
        {
            SetupColors();
        }

        if (newColors.Length == colors.Length)
        {
            for (int i = 0; i < colors.Length; i++)
            {
                SetColor(newColors[i], i);
            }
        }
    }

    //return Material Index(x) and _BaseColor Append(y)
    private Vector2 GetMaterialIndex(int index)
    {
        int indexCount = 0;
        for (int i = 0; i < EditableMaterials.Length; i++)
        {
            switch (EditableMaterials[i])
            {
                case MaterialType.OneColor:
                    if (indexCount == index) return new Vector2(i, 0);
                    indexCount++;
                    break;
                case MaterialType.ThreeColor:
                    for (int c = 0; c < 3; c++)
                    {
                        if (indexCount == index) return new Vector2(i, c);
                        indexCount++;
                    }
                    break;
                case MaterialType.SixColor:
                    for (int c = 0; c < 6; c++)
                    {
                        if (indexCount == index) return new Vector2(i, c);
                        indexCount++;
                    }
                    break;
            }
        }
        return Vector2.zero;
    }
}