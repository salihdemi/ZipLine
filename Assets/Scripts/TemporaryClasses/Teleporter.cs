using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Vector2 respawnPoint = new Vector2(-7, 3);
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.transform.position = respawnPoint;
    }
}
