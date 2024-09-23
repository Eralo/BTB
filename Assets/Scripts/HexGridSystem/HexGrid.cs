using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{

    Dictionary<Vector3Int, Hex> hexTileDict = new Dictionary<Vector3Int, Hex>();
    Dictionary<Vector3Int, List<Vector3Int>> hexTileNeighboursDict = new Dictionary<Vector3Int, List<Vector3Int>>(); //cached calculated neighbours.
                                                                                                                     //Stored clockwise. See GetNeighboursFor()

    void Start()
    {
        foreach (Hex hex in FindObjectsOfType<Hex>()) //fill Dict with all child Hexes
        {
            hexTileDict[hex.HexCoords] = hex;
        }
    }

    public Hex GetTileAt(Vector3Int hexCoordinates)
    {
        Hex result = null;
        hexTileDict.TryGetValue(hexCoordinates, out result);
        return result;
    }

    public List<Vector3Int> GetNeighboursFor(Vector3Int hexCoordinates)
    {
        if (!hexTileDict.ContainsKey(hexCoordinates)) //throw
            return new List<Vector3Int>();

        if (hexTileNeighboursDict.ContainsKey(hexCoordinates)) //if already calculated return
            return hexTileNeighboursDict[hexCoordinates];

        hexTileNeighboursDict.Add(hexCoordinates, new List<Vector3Int>()); //otherwise make list

        foreach (Vector3Int direction in Direction.GetDirectionList(hexCoordinates.z))
        {
            if (hexTileDict.ContainsKey(hexCoordinates + direction))
            {
                hexTileNeighboursDict[hexCoordinates].Add(hexCoordinates + direction);
            }
        }
        return hexTileNeighboursDict[hexCoordinates];
    }

    public Vector3Int GetClosestHex(Vector3 worldPosition)
    {
        worldPosition.y = 0;
        return HexCoordinates.ConvertPositionToOffset(worldPosition);
    }
}

public static class Direction
{
    public static List<Vector3Int> directionOffsetOdd = new List<Vector3Int>
    {
        new Vector3Int(-1, 0, 1),  //N1
        new Vector3Int( 0, 0, 1),  //N2
        new Vector3Int( 1, 0, 0),  //E
        new Vector3Int( 0, 0,-1),  //S2
        new Vector3Int(-1, 0,-1),  //S1
        new Vector3Int(-1, 0, 0)   //W
    };
    public static List<Vector3Int> directionOffsetEven = new List<Vector3Int>
    {
        new Vector3Int( 0, 0, 1),  //N1
        new Vector3Int( 1, 0, 1),  //N2
        new Vector3Int( 1, 0, 0),  //E
        new Vector3Int( 1, 0,-1),  //S2
        new Vector3Int(-0, 0,-1),  //S1
        new Vector3Int(-1, 0, 0)   //W
    };

    public static List<Vector3Int> GetDirectionList(int z) 
        => z%2 == 0 ? directionOffsetEven : directionOffsetOdd;
} 


// checkout https://www.youtube.com/watch?v=htZijEO7ZmE&t=625s