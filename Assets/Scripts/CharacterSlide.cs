using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSlide : MonoBehaviour
{
    #region Classlar
    private CustomCharacterController controller;
    private Apparatus apparatus;
    #endregion

    [Header("SlideSpeed")]
    public float slideBaseSpeed = 1f;
    public float limitSpeed = 500;
    public float acceleration = 2f;


    [HideInInspector]
    public bool onWire;

    private bool isFlying;
    public bool GetIsFlying() { return isFlying; }
    private bool isSliding;
    public bool GetIsSliding(){ return isSliding; }


    private void Awake()
    {
        controller = GetComponent<CustomCharacterController>();
        apparatus = GetComponent<Apparatus>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wire"))//switch Case  eg eçir!
        {
            Wire wire = collision.GetComponent<Wire>();
            OnCollideWire(wire);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Wire"))
        {
            // Telde deðil
            onWire = false;
        }
    }
    private void OnCollideWire(Wire wire)
    {
        if (apparatus.currentCharge > 0)
        {
            // Telde
            onWire = true;

            // Kutbu belirle
            PoleType deviceDirection = apparatus.currentDirection;

            // Kayýþý baþlat
            StartSlide(wire, deviceDirection);
        }
    }
    public void StartSlide(Wire wire, PoleType direction)
    {
        TurnCharacterToWireDirection(wire, direction);
        StartCoroutine(SlideRoutine(wire, direction));
        isSliding = true;
    }
    private IEnumerator SlideRoutine(Wire wire, PoleType poleType)
    {
        float speed = slideBaseSpeed;// Baþlangýç hýzý
        Vector3 direction = CalculateAndGetDirection(wire, poleType);//Tel yönü
        // Dört durum
        controller.DeActivateGravity();// Yerçekimini kapat

        while (onWire)// Kayma iþlemi
        {
            speed = Accelerate(speed);
            Slide(speed, direction);
            yield return null;
        }
        EndSlide();

        controller.ActivateGravity();

        Charge(wire, poleType);

    }
    private void EndSlide()
    {
        isSliding = false;
        isFlying = true; 
    }
    public void StopFly()
    {
        if (isFlying)
        {
            isFlying = false;
        }
    }
    private void Charge(Wire wire, PoleType poleType)
    {
        //Þarz doldur
        if (wire.GetHeight(poleType) < 0)
        {
            apparatus.currentCharge++;
        }
        // Þarz tüket
        else
        {
            apparatus.currentCharge--;
        }
    }
    private void TurnCharacterToWireDirection(Wire wire, PoleType direction)
    {
        controller.spriteRenderer.flipX = !wire.IsDirectionRight(direction);
    }
    private Vector3 CalculateAndGetDirection(Wire wire, PoleType poleType)
    {
        Vector3 right = wire.transform.transform.right;
        float direction = wire.GetDirection(poleType);
        return right * direction;
    }
    private float Accelerate(float speed)
    {
        // Ivme
        if (speed < limitSpeed)
        {
            speed += acceleration * Time.deltaTime;
        }
        // Max ivme
        else
        {
            speed = limitSpeed;
            Debug.Log("Limit");
        }
        return speed;
    }
    private void Slide(float speed, Vector3 direction)
    {
        controller.rb.velocity = direction * speed;
    }
    /*private Transform HoldPoint(Wire wire)
    {
        Transform holdPoint = new GameObject().transform;
        holdPoint.position = transform.localPosition;
        holdPoint.rotation = wire.transform.localRotation;
        holdPoint.gameObject.name = "Hold Point";

        //SpriteRenderer a = holdPoint.AddComponent<SpriteRenderer>(); a.sprite = spriteRenderer.sprite;


        return holdPoint;
    }*/

}
