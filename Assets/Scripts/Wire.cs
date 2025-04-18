using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : MonoBehaviour
{
    public float thicness = 0.05f;


    public ZiplineTower plusTower, minusTower;
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
        if (plusTower.poleType == PoleType.Plus && minusTower.poleType == PoleType.Minus)
        {
            plusPoint = plusTower.transform.position + plusTower.pointDistance;
            minusPoint = minusTower.transform.position + minusTower.pointDistance;
        }
        else if (plusTower.poleType == PoleType.Minus && minusTower.poleType == PoleType.Plus)
        {
            Debug.LogError("Plus ve minus kuleleri ters atanmýþ.", this);
        }
        else
        {
            Debug.LogError("Tel sadece bir PLUS bir MINUS kuleye baðlanmalýdýr.", this);
        }
    }
    public Vector2 GetSlidePoint(float t, PoleType direction)//!
    {
        return direction == PoleType.Plus
            ? Vector2.Lerp(plusPoint, minusPoint, t)
            : Vector2.Lerp(minusPoint, plusPoint, t);
    }

    public PoleType GetSuggestedDirection(Vector2 from)
    {
        float distToPlus = Vector2.Distance(from, plusPoint);
        float distToMinus = Vector2.Distance(from, minusPoint);
        return distToPlus < distToMinus ? PoleType.Plus : PoleType.Minus;
    }



















    public void Slide(Transform obj, Vector3 target, float duration)
    {
        StartCoroutine(MoveToPosition(obj, target, duration));
    }


    IEnumerator MoveToPosition(Transform obj, Vector3 target, float duration)
    {
        Vector3 start = obj.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            obj.position = Vector3.Lerp(start, target, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        obj.position = target; // Son konumu sabitle
    }







}
