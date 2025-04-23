using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSlide : MonoBehaviour
{
    #region Classlar
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private CustomCharacterController controller;
    private Apparatus apparatus;
    #endregion

    [Header("SlideSpeed")]
    public float slideBaseSpeed = 1f;
    public float limitSpeed = 500;
    public float acceleration = 2f;

    public float gravityScale = 3;

    [HideInInspector]
    public bool isFlying = false;

    public bool onWire;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
     
        controller = GetComponent<CustomCharacterController>();
        apparatus = GetComponent<Apparatus>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wire"))
        {
            Wire wire = collision.GetComponent<Wire>();
            OnCollideWire(wire);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Wire"))
        {
            // Telde de�il
            onWire = false;
        }
    }
    // Tek ba�vurumluk
    private void OnCollideWire(Wire wire)
    {
        if(apparatus.currentCharge > 0)
        {
            // Telde
            onWire = true;

            // Kutbu belirle
            PoleType deviceDirection = apparatus.currentDirection;

            // Kay��� ba�lat
            StartSlide(wire, deviceDirection);
        }
    }
    public void StartSlide(Wire wire, PoleType direction)
    {

        spriteRenderer.flipX = !wire.IsDirectionRight(direction);

        StartCoroutine(SlideRoutine(wire, direction));
    }
    private IEnumerator SlideRoutine(Wire wire, PoleType poleType)
    {
        controller.isSliding = true;



        // Ba�lang�� h�z� ve ivme
        float speed = slideBaseSpeed;

        // Y�n
        Vector3 direction = wire.transform.right * wire.GetDirection(poleType);//Tel y�n�

        // Zaman tan�mla
        float elapsedTime = 0f;

        // Yer�ekimini kapat
        rb.gravityScale = 0;

        // D�rt durum

        // Kayma i�lemi
        while (onWire)
        {
            // Zaman hesaplama
            elapsedTime += Time.deltaTime;

            // Ivme
            if(speed < limitSpeed)
            {
                speed += acceleration * Time.deltaTime * 100;
            }
            // Max ivme
            else
            {
                speed = limitSpeed;
            }

            // �lerle
            rb.velocity = direction * speed * 0.1f;

            yield return null;
        }
        //�arz doldur
        if (wire.GetHeight(poleType) < 0)
        {
            apparatus.currentCharge++;
        }
        // �arz t�ket
        else
        {
            apparatus.currentCharge--;
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
    /*private Transform HoldPoint(Wire wire)
    {
        Transform holdPoint = new GameObject().transform;
        holdPoint.position = transform.localPosition;
        holdPoint.rotation = wire.transform.localRotation;
        holdPoint.gameObject.name = "Hold Point";

        //SpriteRenderer a = holdPoint.AddComponent<SpriteRenderer>(); a.sprite = spriteRenderer.sprite;


        return holdPoint;
    }*/
}
