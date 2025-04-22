using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoleType { Plus, Minus }

public class Apparatus : MonoBehaviour
{
    public PoleType currentDirection = PoleType.Plus;
    public float length = 0.5f;

    public bool IsDirectionCompatible(PoleType suggested, PoleType device)
    {
        // �imdilik y�n e�le�mesine bak�yoruz
        return suggested == device;
    }
    // Y�n� de�i�tiren fonksiyon
    public void ToggleDirection()
    {
        currentDirection = currentDirection == PoleType.Plus ? PoleType.Minus : PoleType.Plus;
    }
}
