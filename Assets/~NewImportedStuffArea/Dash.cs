using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Dash : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Dash")]
    public float dashForce = 10f;
    public float dashCooldown = 1f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool canDash = true;
    private bool isDashing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f; // E�er yer�ekimi istemiyorsan
        rb.freezeRotation = true;
    }

    void Update()
    {
        // Hareket Girdisi
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        // Dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(PerformDash());
        }
    }

    void FixedUpdate()
    {
        if(!isDashing)
        rb.velocity = moveInput * moveSpeed;
    }

    System.Collections.IEnumerator PerformDash()
    {
        canDash = false;
        isDashing = true;

        // Dash y�n�n� input geldi�i anda sabitle
        Vector2 dashDirection = moveInput;

        // E�er hi� y�n verilmemi�se dash atma (ya da default y�n verebilirsin)
        if (dashDirection == Vector2.zero)
        {
            dashDirection = Vector2.right; // �rne�in sa�a default dash
        }

        // Normal hareketi ge�ici olarak durdur
        rb.velocity = Vector2.zero;

        // Ani kuvvet uygula
        rb.AddForce(dashDirection * dashForce, ForceMode2D.Impulse);

        // Dash s�resi (k�sa gecikme)
        yield return new WaitForSeconds(0.1f);

        // Cooldown
        yield return new WaitForSeconds(dashCooldown);
        isDashing = false;
        canDash = true;
    }
}
