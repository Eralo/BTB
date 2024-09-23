using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Hex : MonoBehaviour
{
    [SerializeField]
    private GlowHighlight highlight;
    private HexCoordinates hexCoordinates;
    public Vector3Int HexCoords => hexCoordinates.GetHexCoords();

    [SerializeField]
    private HexType hexType;

    public int GetCost() => hexType switch
    {
        HexType.Difficult => 100,
        HexType.Default => 50,
        HexType.Road => 20,
        _ => throw new System.Exception($"ERROR : Hex of type {hexType} not supported !")
    };

    public bool IsObstacle()
    {
        return hexType == HexType.Obstacle;
    }

    private void Awake()
    {
        hexCoordinates = GetComponent<HexCoordinates>();
        highlight = GetComponent<GlowHighlight>();
    }

    public void EnableHighlight()
    {
        highlight.ToggleGlow(true);
    }

    public void DisableHighlight()
    {
        highlight.ToggleGlow(false);
    }

    internal void ResetHighlight()
    {
        highlight.ResetGlowHighlight();
    }

    internal void HighlightPath()
    {
        highlight.HighLightValidPath();
    }

    public enum HexType
    {
        None,
        Default,
        Difficult,
        Road,
        Water,
        Obstacle,
    };
}