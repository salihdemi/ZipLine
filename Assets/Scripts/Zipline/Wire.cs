using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Wire : MonoBehaviour
{
    public float thicness = 0.05f;

    [SerializeField]
    private PlusTower plusTower;
    [SerializeField]
    private MinusTower minusTower;

    private Vector3 plusPoint;
    private Vector3 minusPoint;

    private void Awake()
    {
        AssignPoles();
        PlaceWire();
    }
    private void PlaceWire()
    {
        FixPosition();// Konumland�r
        FixRotation();// D�nd�r
        FixScale();   // Boyutland�r
    }
    private void FixPosition()
    {
        transform.localPosition = (plusPoint + minusPoint) / 2;
    }
    private void FixRotation()
    {
        Vector3 direction = plusPoint - minusPoint;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (angle > 90f) { angle -= 180f; }
        else if (angle < -90f) { angle += 180f; }
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    private void FixScale()
    {
        float distance = Vector2.Distance(plusPoint, minusPoint);
        Vector2 newScale = new Vector2(distance, thicness);
        transform.localScale = newScale;
        Debug.Log("a");
    }
    private void AssignPoles()
    {
        plusPoint = plusTower.transform.localPosition + plusTower.pointDistance;
        minusPoint = minusTower.transform.localPosition + minusTower.pointDistance;
    }

    private Vector3 GetDistanceVector(PoleType poleType)
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
    public bool IsDirectionRight(PoleType poleType)
    {
        float x = GetDistanceVector(poleType).x;
        if (x < 0) return false;
        else if (x > 0) return true;
        else
        {
            Debug.LogError("Kuleler �st �ste");
            return false;
        }

    }
    public float GetHeight(PoleType poleType)
    {
        return GetDistanceVector(poleType).y;
    }//private yap�lacak!
    public int GetDirection(PoleType poleType)
    {
        float x = GetDistanceVector(poleType).x;
        if (x > 0)
        {
            return 1;
        }
        else if (x == 0)
        {
            Debug.LogError("Kuleler �st �ste");
            return 0;
        }
        else
        {
            return -1;
        }
    }
    public void GetState(PoleType poleType)
    {
        bool rising = GetHeight(poleType) > 0;
        bool linear = false;
        // �ok h�zlan
        if (!rising && linear)
        {

        }
        // H�zlanma, �arj et
        else if (!rising && !linear)
        {

        }
        // H�zlanma-Az h�zlan �arz t�ket
        else if (rising && linear)
        {

        }
        // Geri at- �zel durum yok
        else
        {

        }
    }









}
