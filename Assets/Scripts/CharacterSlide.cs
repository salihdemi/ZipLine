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
    [HideInInspector]
    public bool isSliding;


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
            // Telde deðil
            onWire = false;
        }
    }
    // Tek baþvurumluk
    private void OnCollideWire(Wire wire)
    {
        if (apparatus.currentCharge > 0)
        {
            // Telde
            onWire = true;

            // Kutbu belirle
            PoleType deviceDirection = apparatus.currentDirection;

            // Kayýþý baþlat
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
        isSliding = true;



        // Baþlangýç hýzý ve ivme
        float speed = slideBaseSpeed;

        // Yön
        Vector3 direction = wire.transform.right * wire.GetDirection(poleType);//Tel yönü

        // Zaman tanýmla
        float elapsedTime = 0f;


        // Dört durum

        // Kayma iþlemi
        while (onWire)
        {
            // Yerçekimini kapat
            rb.gravityScale = 0;

            // Zaman hesaplama
            elapsedTime += Time.deltaTime;

            // Ivme
            if (speed < limitSpeed)
            {
                speed += acceleration * Time.deltaTime * 100;
            }
            // Max ivme
            else
            {
                speed = limitSpeed;
            }

            // Ýlerle
            rb.velocity = direction * speed * 0.1f;

            yield return null;
        }

        isFlying = true;

        rb.gravityScale = gravityScale;

        isSliding = false;

        //Þarz doldur
        if (wire.GetHeight(poleType) < 0)
        {
            apparatus.currentCharge++;
        }
        // Þarz tüket
        else
        {
            apparatus.currentCharge--;
        }

        yield return null;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("a");
    }
}
