using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCoordinates : MonoBehaviour
{

    public static float xOffset = 2, yOffset = 1, zOffset = 1.75f;

    [Header("Offset Coordinates")]
    [SerializeField] private Vector3Int offsetCoordinates;


    public Vector3Int GetHexCoords()
    {
        return offsetCoordinates;
    }

    private void Awake()
    {
        offsetCoordinates = ConvertPositionToOffset(transform.position);
    }

    public static Vector3Int ConvertPositionToOffset(Vector3 position)
    {
        int x = Mathf.CeilToInt(position.x/xOffset);
        int y = Mathf.CeilToInt(position.y/yOffset);
        int z = Mathf.CeilToInt(position.z/zOffset);
        return new Vector3Int(x, y, z);
    }
}
