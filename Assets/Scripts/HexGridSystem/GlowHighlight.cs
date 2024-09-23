using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowHighlight : MonoBehaviour
{
    Dictionary<Renderer, Material[]> glowMaterialDict= new Dictionary<Renderer, Material[]>();
    Dictionary<Renderer, Material[]> originalMaterialDict = new Dictionary<Renderer, Material[]>();
    Dictionary<Color, Material> cachedGlowMaterials = new Dictionary<Color, Material>();

    public Material glowMaterial;

    private bool isGlowing = false;

    private Color validPathColor = Color.green;
    private Color originalGlowColor;

    private void Awake()
    {
        PrepareMaterialDictionaries();
        originalGlowColor = glowMaterial.GetColor("_GlowColor");
    }

    private void PrepareMaterialDictionaries()
    {
        foreach(Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            Material[] originalMaterials = renderer.materials;
            originalMaterialDict.Add(renderer, originalMaterials);

            Material[] newMaterials = new Material[renderer.materials.Length];
            for (int i=0; i < originalMaterials.Length; i++)
            {
                Material mat = null;
                if (cachedGlowMaterials.TryGetValue(originalMaterials[i].color, out mat) == false ) //in case doesn't exist
                {
                    mat = new Material(glowMaterial);
                    //By default, a color with property name "_Color" is the main color.
                    mat.color = originalMaterials[i].color;
                    cachedGlowMaterials[mat.color] = mat;
                }
                newMaterials[i] = mat; 
            }
            glowMaterialDict.Add(renderer, newMaterials);
        }
    }

    public void ToggleGlow()
    {

        if (isGlowing) 
        {
            foreach (Renderer renderer in originalMaterialDict.Keys)
            {
                renderer.materials = originalMaterialDict[renderer];
            }
        }
        else
        {
            ResetGlowHighlight();
            foreach (Renderer renderer in originalMaterialDict.Keys)
            {
                renderer.materials = glowMaterialDict[renderer];
            }
        }
        isGlowing = !isGlowing;
    }

    public void ToggleGlow(bool glowState)
    {
        if (isGlowing == glowState) return;
        isGlowing = !glowState; //glowState is the objective so we need to reverse its state before toggling
       ToggleGlow();
    }

    internal void HighLightValidPath()
    {
        if (isGlowing == false) return;

        foreach (Renderer renderer in glowMaterialDict.Keys)
        {
            foreach (Material item in glowMaterialDict[renderer])
            {
                item.SetColor("_GlowColor", validPathColor);
            }
        }
    }

    internal void ResetGlowHighlight()
    {
        foreach (Renderer renderer in glowMaterialDict.Keys)
        {
            foreach (Material item in glowMaterialDict[renderer]) 
            {
                item.SetColor("_GlowColor", originalGlowColor);
            }
        }
    }
}
