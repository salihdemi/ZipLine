using UnityEngine;
using System.Collections;

public static class Tween
{
    public static void MoveTo(this Transform t, Vector3 target, float duration, MonoBehaviour context)
    {
        context.StartCoroutine(MoveTransform(t, target, duration));
    }

    private static IEnumerator MoveFloat(float t, float target, float duration)
    {
        float start = t;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            t = Mathf.Lerp(start, target, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        t = target;
    }
    private static IEnumerator MoveTransform(Transform t, Vector3 target, float duration)
    {
        Vector3 start = t.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            t.position = Vector3.Lerp(start, target, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        t.position = target;
    }
}
