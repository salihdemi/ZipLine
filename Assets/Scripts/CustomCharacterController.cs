using UnityEngine;
using System.Collections;
using Unity.VisualScripting;


public enum State { Normal, Sliding }
public class CustomCharacterController : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 800f;
    public float jumpingPower = 16f;




    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    #region Classlar
    private CharacterSlide characterSlide;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    #endregion

    public float horizontal;


     float gravityScale;



    [SerializeField]
    int targetFPS;


    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFPS;

        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        characterSlide = GetComponent<CharacterSlide>();
    }
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        CheckJump();
        Flip();

        if (!characterSlide.isSliding && !characterSlide.isFlying)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
    }
  
    private void CheckJump()
    {
        if (Input.GetButtonDown("Jump") && !characterSlide.isFlying && !characterSlide.isSliding)
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
    private void Flip()
    {
        bool x = spriteRenderer.flipX;
        if (!characterSlide.isSliding && (!x && horizontal < 0f || x && horizontal > 0f))
        {
            x = !x;
        }
        spriteRenderer.flipX = x;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Eðer sliding modundaysa ve yatay çarpýþma varsa
        if (characterSlide.isFlying && collision.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            // Kontrolü býrak
            characterSlide.isFlying = false;
            // isFlying = false; // Bu satýrý buraya alma! Aþaðýda iþle
        }
    }


    



}