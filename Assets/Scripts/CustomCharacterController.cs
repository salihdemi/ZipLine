using UnityEngine;
using System.Collections;
using Unity.VisualScripting;


public enum State { Normal, Sliding }
public class CustomCharacterController : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 8f;
    public float jumpingPower = 16f;




    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;


    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Apparatus apparatus;

    private float horizontal;
    bool isSliding = false;
    public float slideSpeed = 1f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        apparatus = GetComponent<Apparatus>();
    }
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        if(Input.GetKeyDown(KeyCode.E))
        { apparatus.ToggleDirection(); }

        CheckJump();
        Flip();
    }

    private void FixedUpdate()
    {
        if (isSliding) return;
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }
    #region temel hareketler
    private void CheckJump()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            //JumpAnimation
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            //FallingAnimation
        }
    }
    private void Flip()
    {
        bool x = spriteRenderer.flipX;
        if (!isSliding && (!x && horizontal < 0f || x && horizontal > 0f))
        {
            x = !x;
        }
        spriteRenderer.flipX = x;
    }
    private bool IsGrounded()
    {
        //Physics2D.OverlapCircle(groundCheck.position, 0.7f, groundLayer);
        return Physics2D.OverlapArea(
            transform.position + new Vector3(-0.75f, -0.5f),
            transform.position + new Vector3(0.75f, -0.5f),
            groundLayer);
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wire"))
        {
            Wire wire = collision.GetComponent<Wire>();
            OnCollideWire(wire);
        }
    }
    //tek baþvurumluk
    private void OnCollideWire(Wire wire)
    {
        if (wire == null) return;

        PoleType deviceDirection = apparatus.currentDirection;

        //float startT = wire.GetNormalizedT(transform.position, deviceDirection);

        // Kayýþý baþlat
        StartSlide2(wire, deviceDirection);
    }

    /*
    private void StartSlide(Wire wire, PoleType direction, float startT)
    {

        spriteRenderer.flipX = !wire.IsDirectionRight(direction);

        StartCoroutine(SlideRoutine(wire, direction, startT));
    }
    */
    private void StartSlide2(Wire wire, PoleType direction)
    {

        spriteRenderer.flipX = !wire.IsDirectionRight(direction);

        StartCoroutine(SlideRoutine2(wire, direction));
    }

    private IEnumerator SlideRoutine2(Wire wire, PoleType poleType)
    {
        rb.isKinematic = true;
        isSliding = true ;



        // Baþlangýç hýzý ve ivme
        float baseSpeed = slideSpeed;
        float acceleration = 2f;
        rb.velocity = Vector3.zero;


        Transform holdPoint = HoldPoint(wire);//Tutunma noktasý
        Vector3 direction = transform.right * wire.GetDirection(poleType);//Tel yönü



        float elapsedTime = 0f;

        Vector2 endPos = poleType == PoleType.Plus ? wire.minusPoint : wire.plusPoint;

        // Kayma iþlemi
        while (Vector3.Distance(endPos, holdPoint.position) > 0.5f)// þartý deðiþtirmek gerek
        {
            Debug.Log(Vector3.Distance(endPos, holdPoint.position));


            elapsedTime += Time.deltaTime;

            
            // Ivme
            baseSpeed += acceleration;

            // Ýlerle
            holdPoint.Translate(direction * baseSpeed * Time.deltaTime * 0.1f);

            if (Vector3.Distance(holdPoint.position, transform.position) < apparatus.length)//yakýnsa
            {
                transform.position = Vector3.Lerp(transform.position, holdPoint.position, 0.05f);//uzakta takip etme aralýðý

                transform.Rotate(Vector3.forward * -wire.GetDirection(poleType) * Time.deltaTime * 100);// dönme
            }
            else//uzaksa
            {
                transform.SetParent(holdPoint);
            }

            yield return null;
        }

        transform.rotation = Quaternion.identity;
        transform.SetParent(null);
        Destroy(holdPoint.gameObject);



        rb.isKinematic = false;
        isSliding = false;
        
    }
    private Transform HoldPoint(Wire wire)
    {
        Transform holdPoint = new GameObject().transform;
        holdPoint.position = transform.localPosition;
        holdPoint.rotation = wire.transform.localRotation;
        holdPoint.gameObject.name = "Hold Point";

        //SpriteRenderer a = holdPoint.AddComponent<SpriteRenderer>(); a.sprite = spriteRenderer.sprite;


        return holdPoint;
    }
    /*
    private IEnumerator SlideRoutine(Wire wire, PoleType direction, float startT)
    {

        isSliding = true;
        rb.isKinematic = true;

        Vector2 startPos = wire.GetSlidePoint(startT, direction);
        Vector2 endPos = direction == PoleType.Plus ? wire.minusPoint : wire.plusPoint;
        Vector2 slideDirection = (endPos - startPos).normalized;

        float totalDistance = Vector2.Distance(startPos, endPos); // Toplam mesafe
        float traveled = 0f;

        // Baþlangýç hýzý ve ivme
        float baseSpeed = 5f;
        float acceleration = 20f;

        // Telin uzunluðu, mesafeye göre hýz artýþý saðlamak için
        float distanceFactor = 1f + (totalDistance / 10f);
        float speed = baseSpeed * distanceFactor; // Hýzý mesafeye göre ayarlýyoruz

        float elapsedTime = 0f;

        // Kayma iþlemi
        //while (traveled < totalDistance)
        while (Vector3.Distance(endPos, transform.position) > 0.1f)
        {
            elapsedTime += Time.deltaTime;

            // Ivme arttýkça hýz artacak
            speed += acceleration * Time.deltaTime;
            // Ýlerle
            traveled += speed * Time.deltaTime;
            float t = Mathf.Clamp01(traveled / totalDistance);

            // Kayýþýn yeni pozisyonu
            Vector2 newPos = Vector2.Lerp(startPos, endPos, t);
            transform.position = newPos;

            yield return null;
        }

        // Bitince
        rb.isKinematic = false;
        isSliding = false;
    }
    */



}