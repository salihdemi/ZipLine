using UnityEngine;

public class CustomCharacterController : MonoBehaviour
{
    public float speed = 8f;
    public float jumpingPower = 16f;


    private bool isFacingRight = true;
    private float horizontal;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;




    private SpriteRenderer spriteRenderer;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");


/*
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.AddForce(Vector2.up * jumpingPower, ForceMode2D.Impulse);
        }
*/
        
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
        

        Flip();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    private bool IsGrounded()
    {
        //Physics2D.OverlapCircle(groundCheck.position, 0.7f, groundLayer);
        return Physics2D.OverlapArea(transform.position + new Vector3(-0.75f, -0.51f), transform.position + new Vector3(0.75f, -0.51f), groundLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}