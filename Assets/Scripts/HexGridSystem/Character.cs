using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Character : MonoBehaviour
{
    [SerializeField]
    private int movementPoints = 100;
    public int MovementPoints { get => movementPoints; }
    [SerializeField]
    private float movementDuration = 1, rotationDuration = 0.3f;

    private GlowHighlight glowHighlight;
    private Queue<Vector3> pathPositions  = new Queue<Vector3>();

    public event System.Action<Character> MovementFinished;

    private void Awake()
    {
        glowHighlight= GetComponent<GlowHighlight>();
    }

    public void Select()
    {
        glowHighlight.ToggleGlow(true);
    }
    public void Deselect()
    {
        glowHighlight.ToggleGlow(false);
    }

    public void MoveThroughPath(List<Vector3> path)
    {
        pathPositions = new Queue<Vector3>(path);
        Vector3 firstTarget = pathPositions.Dequeue();
        StartCoroutine(RotationCoroutine(firstTarget));
    }

    private IEnumerator RotationCoroutine(Vector3 endPosition)
    {
        Quaternion startRotation = transform.rotation;
        endPosition.y = transform.position.y;
        Vector3 direction = endPosition - transform.position;
        Quaternion endRotation = Quaternion.LookRotation(direction, Vector3.up);
        
        if (Mathf.Approximately(Mathf.Abs(Quaternion.Dot(startRotation, endRotation)), 1.0f) == false) //unless already facing direction...
        {
            float timeElapsed = 0;
            while (timeElapsed < rotationDuration)
            {
                timeElapsed += Time.deltaTime;
                float lerpStep = timeElapsed / rotationDuration; //range is 0->1
                transform.rotation = Quaternion.Lerp(startRotation, endRotation, lerpStep);
                yield return null;
            }
            transform.rotation = endRotation; //snap to exact rotation goal
        }
        StartCoroutine(MovementCoroutine(endPosition)); //start movement now that rotation is done
    }

    private IEnumerator MovementCoroutine(Vector3 endPosition)
    {
        Vector3 startPosition = transform.position;
        endPosition.y = startPosition.y;
        float elapsedTime = 0;

        while (elapsedTime < movementDuration) 
        { 
            elapsedTime += Time.deltaTime;
            float lerpStep = elapsedTime / movementDuration;
            transform.position = Vector3.Lerp(startPosition, endPosition, lerpStep);
            yield return null;
        }
        transform.position = endPosition;

        if(pathPositions.Count > 0)
        {
            Debug.Log("Selecting next position !");
            StartCoroutine(RotationCoroutine(pathPositions.Dequeue()));
        }
        else
        {
            Debug.Log("Movement ended !");
            MovementFinished?.Invoke(this);
        }
    }
}
