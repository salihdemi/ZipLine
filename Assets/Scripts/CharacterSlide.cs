using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSlide : MonoBehaviour
{
    CustomCharacterController controller;

    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;

    private float slideSpeed = 1f;

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
        rb.isKinematic = true;
        controller.isSliding = true;



        // Ba�lang�� h�z� ve ivme
        float baseSpeed = slideSpeed;
        float acceleration = 2f;
        rb.velocity = Vector3.zero;


        Transform holdPoint = HoldPoint(wire);//Tutunma noktas�

        Vector3 direction = wire.transform.right * wire.GetDirection(poleType);//Tel y�n�

        float elapsedTime = 0f;

        Vector2 endPos = poleType == PoleType.Plus ? wire.minusPoint : wire.plusPoint;

        // Kayma i�lemi
        while (Vector3.Distance(endPos, holdPoint.position) > 0.5f)// �art� de�i�tirmek gerek
        {
            elapsedTime += Time.deltaTime;

            
            // Ivme
            baseSpeed += acceleration;

            // �lerle
            holdPoint.Translate(direction * baseSpeed * Time.deltaTime * 0.1f);
            /* sallanma
            if (Vector3.Distance(holdPoint.position, transform.position) < apparatus.length)//yak�nsa
            {
                transform.position = Vector3.Lerp(transform.position, holdPoint.position, 0.05f);//uzakta takip etme aral���

                transform.Rotate(Vector3.forward * -wire.GetDirection(poleType) * Time.deltaTime * 100);// d�nme
            }
            else//uzaksa
            {
                transform.SetParent(holdPoint);
            }
            */
            transform.position = holdPoint.position;
            yield return null;
        }

        transform.rotation = Quaternion.identity;//Rotasyon
        transform.SetParent(null);//Parent
        Destroy(holdPoint.gameObject);//Parent



        controller.isSliding = false;
        rb.isKinematic = false;
        //F�rlat
        /*
        isFlying = true;
        while (isFlying)
        {
            Debug.Log(isFlying);
            isFlying = controller.IsGrounded();
            Vector2 launchForce = direction * baseSpeed * Time.deltaTime * 0.1f;
            rb.velocity = launchForce + new Vector2(0,rb.velocity.y);
            yield return null;

        }

        rb.velocity = Vector2.zero;
        */
    }
    private Transform HoldPoint(Wire wire)
    {
        Transform holdPoint = new GameObject().transform;
        holdPoint.position = transform.localPosition;
        //holdPoint.rotation = wire.transform.localRotation;
        holdPoint.gameObject.name = "Hold Point";

        //SpriteRenderer a = holdPoint.AddComponent<SpriteRenderer>(); a.sprite = spriteRenderer.sprite;


        return holdPoint;
    }
}
