using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] private HexGrid hexGrid;
    [SerializeField] private MovementSystem movementSystem;

    public bool PlayersTurn { get; private set; } = true;

    [SerializeField] private Character selectedCharacter;
    private Hex previouslySelectedHex;

    public void HandleUnitSelected(GameObject character)
    {
        if (!PlayersTurn) 
            return;
        Character characterRef = character.GetComponent<Character>();
        if (IsSameCharacterSelected(characterRef))
            return;
        PrepareCharacterForMovement(characterRef);
    }

    private bool IsSameCharacterSelected(Character characterRef)
    {
        if (selectedCharacter == characterRef)
        {
            ClearOldSelection();
            return true;
        }
        return false;
    }

    private void PrepareCharacterForMovement(Character characterRef)
    {
        if (this.selectedCharacter != null) 
            ClearOldSelection();

        selectedCharacter= characterRef;
        selectedCharacter.Select();
        movementSystem.ShowRange(this.selectedCharacter, hexGrid);
    }

    private void ClearOldSelection()
    {
        previouslySelectedHex= null;
        selectedCharacter.Deselect();
        movementSystem.HideRange(hexGrid);
        selectedCharacter = null;
    }

    public void HandleTerrainSelected(GameObject hex)
    {
        if (selectedCharacter == null || PlayersTurn == false)
        {
            return;
        }

        Hex selectedHex = hex.GetComponent<Hex>();

        if (HandleHexOutOfRange(selectedHex.HexCoords) || HandleSelectedHexIsCharacterHex(selectedHex.HexCoords))
            return;

        HandleTargetHexSelected(selectedHex);
    }

    private void HandleTargetHexSelected(Hex selectedHex)
    {
        if (previouslySelectedHex == null || previouslySelectedHex != selectedHex)
        {
            previouslySelectedHex = selectedHex;
            movementSystem.ShowPath(selectedHex.HexCoords, hexGrid);
        }
        else
        {
            movementSystem.MoveUnit(selectedCharacter, hexGrid);
            PlayersTurn = false;
            selectedCharacter.MovementFinished += ResetTurn;
            ClearOldSelection();
        }
    }

    private void ResetTurn(Character selectedCharacter)
    {
        selectedCharacter.MovementFinished -= ResetTurn;
        PlayersTurn = true;
    }

    private bool HandleSelectedHexIsCharacterHex(Vector3Int hexPosition)
    {
        if(hexPosition == hexGrid.GetClosestHex(selectedCharacter.transform.position))
        {
            selectedCharacter.Deselect();
            ClearOldSelection();
            return true;
        }
        return false;
    }

    private bool HandleHexOutOfRange(Vector3Int hexPosition)
    {
        if (movementSystem.IsHexInRange(hexPosition) == false)
        {
            Debug.Log("Hex out of range! ");
            return true;
        }
        return false;
    }
}
