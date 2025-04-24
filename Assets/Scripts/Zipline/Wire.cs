using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : MonoBehaviour
{
    public float thicness = 0.05f;


    public PlusTower plusTower;
    public MinusTower minusTower;
    public Vector3 plusPoint, minusPoint;
    public float chargeCost;

    void Start()
    {
        AssignPoles();
        AssignWire();
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

        if (angle > 90f) { angle -= 180f; }
        else if (angle < -90f) { angle += 180f; }

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    private void AssignPoles()
    {
        plusPoint = plusTower.transform.localPosition + plusTower.pointDistance;
        minusPoint = minusTower.transform.localPosition + minusTower.pointDistance;
    }

    private Vector3 Distance(PoleType poleType)
    {
        if (poleType == PoleType.Plus)
        {
            return minusPoint - plusPoint;
        }
        else
        {
            return plusPoint - minusPoint;
        }
    }

    //Saga verilen kutup saga dogru götürecekse true
    public bool IsDirectionRight(PoleType poleType)
    {
        float x = Distance(poleType).x;
        if (x < 0) return false;
        else if (x > 0) return true;
        else
        {
            Debug.LogError("Kuleler üst üste");
            return false;
        }

    }
    public float GetHeight(PoleType poleType)
    {
        return Distance(poleType).y;
    }
    public int GetDirection(PoleType poleType)
    {
        if (Distance(poleType).x > 0)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }
    public void GetState(PoleType poleType)
    {
        bool isRising = GetHeight(poleType) > 0;
        bool isLinear = false;
        // Hýzlan
        if (!isRising && isLinear)
        {

        }
        // Hýzlanma, þarj et
        else if (!isRising && !isLinear)
        {

        }
    }









}
