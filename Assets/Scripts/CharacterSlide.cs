using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSlide : MonoBehaviour
{
    CustomCharacterController controller;

    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;

    private float slideBaseSpeed = 1f;
    private float limitSpeed = 500f;

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



        // Baþlangýç hýzý ve ivme
        float baseSpeed = slideBaseSpeed;
        float acceleration = 2f;
        rb.velocity = Vector3.zero;


        Transform holdPoint = HoldPoint(wire);//Tutunma noktasý

        Vector3 direction = wire.transform.right * wire.GetDirection(poleType);//Tel yönü

        float elapsedTime = 0f;

        Vector2 endPos = poleType == PoleType.Plus ? wire.minusPoint : wire.plusPoint;

        // Kayma iþlemi
        while (Vector3.Distance(endPos, holdPoint.position) > 0.5f)// þartý deðiþtirmek gerek
        {
            elapsedTime += Time.deltaTime;

            
            // Ivme
            if(baseSpeed < limitSpeed)
            {
                baseSpeed += acceleration;
            }
            else
            {
                baseSpeed = limitSpeed;
            }

            // Ýlerle
            holdPoint.Translate(direction * baseSpeed * Time.deltaTime * 0.1f);//translate?
            /* sallanma
            if (Vector3.Distance(holdPoint.position, transform.position) < apparatus.length)//yakýnsa
            {
                transform.position = Vector3.Lerp(transform.position, holdPoint.position, 0.05f);//uzakta takip etme aralýðý

                transform.Rotate(Vector3.forward * -wire.GetDirection(poleType) * Time.deltaTime * 100);// dönme
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
        //Fýrlat
        
        isFlying = true;
        while (isFlying)
        {
            isFlying = controller.IsGrounded();
            Vector2 launchForce = direction * baseSpeed * Time.deltaTime * 0.1f;
            rb.velocity = launchForce + new Vector2(0,rb.velocity.y);
            yield return null;

        }

        rb.velocity = Vector2.zero;
        
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
