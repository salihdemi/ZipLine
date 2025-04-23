using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSlide : MonoBehaviour
{
    #region Classlar
    private CustomCharacterController controller;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    #endregion

    [Header("SlideSpeed")]
    public float slideBaseSpeed = 1f;
    public float limitSpeed = 500f;
    public float acceleration = 2f;

    public float gravityScale = 3;

    [HideInInspector]
    public bool isFlying = false;

    private void Awake()
    {
        controller = GetComponent<CustomCharacterController>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }
    public void StartSlide(Wire wire, PoleType direction)
    {

        spriteRenderer.flipX = !wire.IsDirectionRight(direction);

        StartCoroutine(SlideRoutine(wire, direction));
    }

    private IEnumerator SlideRoutine(Wire wire, PoleType poleType)
    {
        //rb.isKinematic = true;
        controller.isSliding = true;



        // Ba�lang�� h�z� ve ivme
        float speed = slideBaseSpeed;



        Vector3 direction = wire.transform.right * wire.GetDirection(poleType);//Tel y�n�


        Vector3 endPos = poleType == PoleType.Plus ? wire.minusPoint : wire.plusPoint;

        float elapsedTime = 0f;
        rb.gravityScale = 0;
        // Kayma i�lemi
        while (Vector3.Dot(endPos - transform.position, direction) > 0f)
        {
            elapsedTime += Time.deltaTime;

            
            // Ivme
            if(speed < limitSpeed)
            {
                speed += acceleration * Time.deltaTime * 100;
            }
            else
            {
                speed = limitSpeed;
            }

            // �lerle
            rb.velocity = direction * speed * 0.1f;
            yield return null;
        }
        controller.isSliding = false;

        rb.gravityScale = gravityScale;

        isFlying = true;
        while (isFlying)
        {
            isFlying = !controller.IsGrounded();
            yield return null;
        }
        isFlying = false;
    }
}
