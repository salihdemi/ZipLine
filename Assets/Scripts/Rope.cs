using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    public float thicness = 0.05f;


    public ZiplineTower plus, minus;
    public Vector2 pointP, pointM;


    private bool isOn;

    void Start()
    {

        pointP =  plus.transform.position + plus.pointDistance;
        pointM = minus.transform.position + plus.pointDistance;

        //konumlandir
        transform.localPosition = (pointP + pointM) / 2;
        
        //boyutlandir
        float distance = Vector2.Distance(pointP, pointM);
        Vector2 newScale = new Vector2 (distance, thicness);
        transform.localScale = newScale;

        //dondur
        Vector3 direction = pointP - pointM;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Update()
    {
        if(isOn)
        {
            //Slide();
        }
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
