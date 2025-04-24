using UnityEngine;
using System.Collections;
using Unity.VisualScripting;


public enum State { Normal, Sliding }
public class CustomCharacterController : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 800f;
    public float jumpingPower = 16f;
    public float gravityScale = 3;

    //private
    private float horizontal;
    private bool isGrounded;

    #region Classlar
    private CharacterSlide characterSlide;
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;
    #endregion



    private void Awake()
    {
        InitializeComponents();
        ActivateGravity();
    }
    void Update()
    {
        CheckJump();
        Flip();
        Move();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Ground":
                CollideGround();
            break;

        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Ground":
                UnCollideGround();
                break;

        }
    }
    #region Temel Kontroller
    private void Move()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        if (NoFlyNoSlide())
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
    }
    private void CheckJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded && NoFlyNoSlide())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            //JumpAnimation
        }
        else if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            //FallingAnimation
        }
    }
    #endregion

    //private
    private void Flip()
    {
        bool x = spriteRenderer.flipX;
        if (!characterSlide.GetIsSliding() && (!x && horizontal < 0f || x && horizontal > 0f))
        {
            x = !x;
        }
        spriteRenderer.flipX = x;
    }
    private void CollideGround()
    {
        isGrounded = true;
        characterSlide.StopFly();
        Debug.Log("collide");
    }
    private void UnCollideGround()
    {
        isGrounded = false;
    }
    private bool NoFlyNoSlide()
    {
        bool noFly = !characterSlide.GetIsFlying();
        bool noSlide = !characterSlide.GetIsSliding();
        return noFly && noSlide;
    }

    //public
    public void ActivateGravity()
    {
        rb.gravityScale = gravityScale;
    }
    public void DeActivateGravity()
    {
        rb.gravityScale = 0;
    }
    //FasaFiso
    private void InitializeComponents()
    {
        if(rb == null)
            rb = GetComponent<Rigidbody2D>();

        if(spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if(characterSlide == null)
            characterSlide = GetComponent<CharacterSlide>();
    }


}