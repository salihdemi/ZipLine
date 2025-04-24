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


    private bool isOnWire;//onwire i�ini ortadan kald�r�p full isSlidingle mi yapsam acaba?!
    private bool isSliding;
    private bool isFlying;//isgrounded ile ayn� m�?!


    private void Awake()
    {
        InitializeComponents();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Wire":
                OnCollideWire(collision.GetComponent<Wire>());
                break;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Wire":
                isOnWire = false;
                break;
        }
    }
    private void InitializeComponents()
    {
        if (controller == null)
            controller = GetComponent<CustomCharacterController>();
        if (apparatus == null)
            apparatus = GetComponent<Apparatus>();
    }
    private void OnCollideWire(Wire wire)
    {
        if (apparatus.currentCharge > 0)
        {
            isOnWire = true;
            //isSliding = true;

            PoleType direction = apparatus.currentDirection;// Kutbu belirle

            StartSlide(wire, direction);// Kay��� ba�lat

        }
    }
    public void StartSlide(Wire wire, PoleType direction)
    {
        isSliding = true;
        TurnCharacterToWireDirection(wire, direction);
        StartCoroutine(SlideRoutine(wire, direction));
    }// �a��r�ld��� yere direkt yazsam?!
    private IEnumerator SlideRoutine(Wire wire, PoleType poleType)
    {
        float speed = slideBaseSpeed;// Ba�lang�� h�z�
        Vector3 direction = CalculateAndGetDirection(wire, poleType);//Tel y�n�
        // D�rt durum
        controller.DeActivateGravity();// Yer�ekimini kapat

        while (isOnWire)// Kayma i�lemi
        {
            speed = Accelerate(speed);
            Slide(speed, direction);//1 kez �a��r?!
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
        //�arz doldur
        if (wire.GetHeight(poleType) < 0)
        {
            apparatus.currentCharge++;
        }
        // �arz t�ket
        else
        {
            apparatus.currentCharge--;
        }
    }//Ge�ici!
    private void TurnCharacterToWireDirection(Wire wire, PoleType direction)
    {
        controller.spriteRenderer.flipX = !wire.IsDirectionRight(direction);
    }
    private Vector3 CalculateAndGetDirection(Wire wire, PoleType poleType)
    {
        Vector3 right = wire.transform.transform.right;
        int direction = wire.GetDirection(poleType);
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
        }
        return speed;
    }
    private void Slide(float speed, Vector3 direction)
    {
        controller.rb.velocity = direction * speed;
    }
    public bool NoFlyNoSlide()
    {
        bool noFly = !isFlying;
        bool noSlide = !isSliding;
        return noFly && noSlide;
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
