using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoleType
{
    Plus,
    Minus
}

public class ZiplineTower : MonoBehaviour
{
    public PoleType poleType = PoleType.Plus;
    public Vector3 pointDistance = new Vector3(0, 1);
}

