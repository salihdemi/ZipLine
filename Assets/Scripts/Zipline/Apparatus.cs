using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoleType { Plus, Minus }

public class Apparatus : MonoBehaviour
{
    public PoleType currentDirection = PoleType.Plus;
    public float length = 0.5f;

    //public int chargeCapacity;
    public int currentCharge;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        { ToggleDirection(); }
    }

    // Yönü deðiþtiren fonksiyon
    public void ToggleDirection()
    {
        currentDirection = currentDirection == PoleType.Plus ? PoleType.Minus : PoleType.Plus;
    }
}
