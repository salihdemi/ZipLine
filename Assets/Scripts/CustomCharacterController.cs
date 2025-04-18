using UnityEngine;
using System.Collections;

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
    private bool isFacingRight = true;
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
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            //spriteRenderer.flipX = isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
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
            if (wire == null) return;

            // Kayma yönünü otomatik tahmin et
            PoleType suggestedDirection = wire.GetSuggestedDirection(transform.position);
            PoleType deviceDirection = apparatus.currentDirection;

            // Aparat yönü alýnýr (Apparatus bileþeni üzerinden)
            if (apparatus == null) return;


            // Aparat yönü ile tel yönü uyumlu mu kontrol et
            if (!IsDirectionCompatible(suggestedDirection, deviceDirection))
            {
                Debug.Log("Aparat yönü bu kayýþ yönüyle uyumlu deðil!");
                return;
            }

            // Kayýþý baþlat
            StartSlide(wire, suggestedDirection);
        }
    }
    private bool IsDirectionCompatible(PoleType suggested, PoleType device)
    {
        // Þimdilik yön eþleþmesine bakýyoruz
        return suggested == device;
    }



    private void StartSlide(Wire wire, PoleType poleType)
    {
        isSliding = true;
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;

        PoleType direction = GetComponent<Apparatus>().currentDirection;

        StartCoroutine(SlideRoutine(wire, direction));
    }

    private IEnumerator SlideRoutine(Wire wire, PoleType direction)
    {
        float elapsed = 0f;
        float duration = wire.duration;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            transform.position = wire.GetSlidePoint(t, direction);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = wire.GetSlidePoint(1f, direction);

        rb.isKinematic = false;
        isSliding = false;
    }




}