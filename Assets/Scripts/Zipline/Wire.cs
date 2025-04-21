using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : MonoBehaviour
{
    public float thicness = 0.05f;


    public PlusTower plusTower;
    public MinusTower minusTower;
    public Vector3 plusPoint, minusPoint;

    public float duration = 1;

    void Start()
    {
        AssignPoles();
        plusPoint =  plusTower.transform.position + plusTower.pointDistance;
        minusPoint = minusTower.transform.position + plusTower.pointDistance;
        AssignWire();
    }

    void Update()
    {

    }

    private void AssignWire()
    {
        //konumlandir
        transform.localPosition = (plusPoint + minusPoint) / 2;

        //boyutlandir
        float distance = Vector2.Distance(plusPoint, minusPoint);
        Vector2 newScale = new Vector2(distance, thicness);
        transform.localScale = newScale;

        //dondur
        Vector3 direction = plusPoint - minusPoint;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }


    void AssignPoles()
    {
        plusPoint = plusTower.transform.position + plusTower.pointDistance;
        minusPoint = minusTower.transform.position + minusTower.pointDistance;
    }
    public Vector2 GetSlidePoint(float slidePoint, PoleType direction)//!
    {
        if (direction == PoleType.Plus)
        {
            return Vector2.Lerp(plusPoint, minusPoint, slidePoint);
        }
        else
        {
            return Vector2.Lerp(minusPoint, plusPoint, slidePoint);
        }
    }
    public float GetNormalizedT(Vector2 position, PoleType direction)
    {
        Vector2 start;
        Vector2 end;

        if (direction == PoleType.Plus)
        {
            start = plusPoint;
            end = minusPoint;
        }
        else
        {
            start = minusPoint;
            end = plusPoint;
        }

        Vector2 fullVector = end - start;
        Vector2 hitVector = position - start;

        float projected = Vector2.Dot(hitVector, fullVector.normalized);
        float t = projected / fullVector.magnitude;

        return Mathf.Clamp01(t);
    }


    public bool IsDirectionRight(PoleType direction)
    {
        Vector2 moveVector;

        if (direction == PoleType.Plus)
            moveVector = minusPoint - plusPoint;
        else
            moveVector = plusPoint - minusPoint;

        return moveVector.x > 0;
    }










}
