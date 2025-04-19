using UnityEngine;
using System.Collections;
using System.ComponentModel.Design;

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
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        apparatus = GetComponent<Apparatus>();
    }
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");


        CheckJump();
        

        Flip();
    }

    private void FixedUpdate()
    {
        if (isSliding) return;
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }
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


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wire"))
        {
            Wire wire = collision.GetComponent<Wire>();
            CollideWire(wire);
        }
    }
    //tek baþvurumluk
    private void CollideWire(Wire wire)
    {
        if (wire == null) return;

        // Kayma yönünü otomatik tahmin et
        //PoleType suggestedDirection = wire.GetSuggestedDirection(transform.position);
        PoleType deviceDirection = apparatus.currentDirection;

        float startT = wire.GetNormalizedT(transform.position, deviceDirection);

        // Kayýþý baþlat
        StartSlide(wire, deviceDirection, startT);
    }


    private void StartSlide(Wire wire, PoleType direction, float startT)
    {
        isSliding = true;
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;

        spriteRenderer.flipX = !wire.IsDirectionRight(direction);

        StartCoroutine(SlideRoutine(wire, direction, startT));
    }

    private IEnumerator SlideRoutine(Wire wire, PoleType direction, float startT)
    {
        float elapsed = 0f;
        float duration = wire.duration * (1f - startT); // kalan mesafeye göre süre;

        while (elapsed < duration)
        {
            float t = Mathf.Lerp(startT, 1f, elapsed / duration);
            Vector2 newPos = wire.GetSlidePoint(t, direction);
            transform.position = newPos;
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = wire.GetSlidePoint(1f, direction);

        rb.isKinematic = false;
        isSliding = false;
    }




}