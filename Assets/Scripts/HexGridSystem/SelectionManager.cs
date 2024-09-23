using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static GraphSearch;

public class SelectionManager : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;
    public LayerMask selectionmask;

    public UnityEvent<GameObject> CharSelected;
    public UnityEvent<GameObject> TerrainSelected;


    private void Awake()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    public void HandleClick(Vector3 mousePos)
    {
        GameObject result;
        if (FindTarget(mousePos, out result))
        {
            if(IsCharacterSelected(result))
            {
                CharSelected?.Invoke(result);
            }
            else
                TerrainSelected?.Invoke(result);
        }
    }

    private bool IsCharacterSelected(GameObject result)
    {
        return result.GetComponent<Character>() != null;
    }

    private bool FindTarget(Vector3 mousePos, out GameObject result)
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out hit, selectionmask)) 
        {
            result = hit.collider.gameObject;
            return true;
        }
        result = null;
        return false;
    }
}
