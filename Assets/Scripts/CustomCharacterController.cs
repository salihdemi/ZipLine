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
    #endregion

    private float horizontal;

    public bool isSliding;

     float gravityScale;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        characterSlide = GetComponent<CharacterSlide>();
    }
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

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




}