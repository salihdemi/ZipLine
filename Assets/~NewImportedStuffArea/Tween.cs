using UnityEngine;
using System.Collections;

public static class Tween
{
    public static void MoveTo(this Transform t, Vector3 target, float duration, MonoBehaviour context)
    {
        context.StartCoroutine(Move(t, target, duration));
    }

    private static IEnumerator Move(Transform t, Vector3 target, float duration)
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
