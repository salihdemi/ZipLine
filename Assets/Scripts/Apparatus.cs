using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apparatus : MonoBehaviour
{
    public PoleType currentDirection = PoleType.Plus;

    public bool IsDirectionCompatible(PoleType suggested, PoleType device)
    {
        // Þimdilik yön eþleþmesine bakýyoruz
        return suggested == device;
    }
}
