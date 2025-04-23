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
        rb.gravityScale = 0f; // Eðer yerçekimi istemiyorsan
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
        float currentDashForce = dashForce;

        while (currentDashForce >= 1)
        {
            if(currentDashForce > dashForce / 2)//2. yarýda daha hýzlý yavaþlat
            {
                currentDashForce -= Time.deltaTime * 100;
            }

            currentDashForce -= Time.deltaTime * 100;
            Debug.Log(currentDashForce);
            rb.velocity = Vector3.right * currentDashForce;

            yield return null;
        }

        isDashing = false;
        canDash = true;
    }
}
