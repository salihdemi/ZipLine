using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    public Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play(0);
    }
}
