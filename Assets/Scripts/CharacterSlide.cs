using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSlide : MonoBehaviour
{
    private CustomCharacterController controller;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    public float slideBaseSpeed = 1f;
    public float limitSpeed = 500f;



    [HideInInspector]
    public bool isFlying = false;

    private void Awake()
    {
        //Application.targetFrameRate = 15;


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



        // Baþlangýç hýzý ve ivme
        float baseSpeed = slideBaseSpeed;
        float acceleration = 2f;
        rb.velocity = Vector3.zero;



        Vector3 direction = wire.transform.right * wire.GetDirection(poleType);//Tel yönü

        float elapsedTime = 0f;

        Vector3 endPos = poleType == PoleType.Plus ? wire.minusPoint : wire.plusPoint;

        rb.gravityScale = 0;
        // Kayma iþlemi
        while (Vector3.Dot(endPos - transform.position, direction) > 0f)// þartý deðiþtirmek gerek
        {
            elapsedTime += Time.deltaTime;

            
            // Ivme
            if(baseSpeed < limitSpeed)
            {
                baseSpeed += acceleration * Time.deltaTime * 100;//çarpan koy!
            }
            else
            {
                baseSpeed = limitSpeed;
            }

            // Ýlerle
            rb.velocity = direction * baseSpeed * 0.1f;
            yield return null;
        }
        controller.isSliding = false;

        rb.gravityScale = 3;
        isFlying = true;

        while (isFlying)
        {
            isFlying = !controller.IsGrounded();
            yield return null;
        }


        isFlying = false;



        //Fýrlat
        /*
        isFlying = true;
        while (isFlying)
        {
            Vector2 launchForce = direction * baseSpeed * 0.1f;
            rb.velocity = launchForce;
            Debug.Log(launchForce);

            isFlying = !controller.IsGrounded();
            yield return null;
        }
        */

    }
}
