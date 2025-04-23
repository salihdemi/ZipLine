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

    #region Classlar
    private CharacterSlide characterSlide;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Apparatus apparatus;
    #endregion

    private float horizontal;

    public bool isSliding;

     float gravityScale;


    private void Awake()
    {
        characterSlide = GetComponent<CharacterSlide>();
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
        if(!isSliding && !characterSlide.isFlying)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
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
    public bool IsGrounded()
    {
        //Physics2D.OverlapCircle(groundCheck.position, 0.7f, groundLayer);
        bool isGrounded = Physics2D.OverlapArea(
            transform.position + new Vector3(-0.75f, -0.5f),
            transform.position + new Vector3(0.75f, -0.5f),
            groundLayer);
        return isGrounded;
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
        if (collision.CompareTag("Wire") && isSliding)
        {
            //Debug.LogError("Telden koptu");
        }
    }
    //tek baþvurumluk
    private void OnCollideWire(Wire wire)
    {
        if (wire == null) return;

        PoleType deviceDirection = apparatus.currentDirection;

        //float startT = wire.GetNormalizedT(transform.position, deviceDirection);

        // Kayýþý baþlat
        characterSlide.StartSlide(wire, deviceDirection);
    }




}