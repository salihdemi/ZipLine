using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ZiplineTower : MonoBehaviour
{
    public virtual PoleType poleType { get; }
    public ZiplineTower partner;
    public Vector3 pointDistance = new Vector3(0, 1);
}

