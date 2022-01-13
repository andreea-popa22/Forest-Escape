using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Xml.Schema;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private CharacterController controller;
    [SerializeField] private float currentLane = Lane.Middle;
    private Vector3 targetPosition = Vector3.zero;
    [SerializeField] float playerSpeed = 5f;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float gravity;
    public bool inAir = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        //controller = GetComponent<CharacterController>();
    }

    private void Update()

    {
        if (!isCrouching && Input.GetKeyDown(KeyCode.LeftShift))
        {
            isSprinting = true;
            isCrouching = false;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isSprinting = false;
        }

        if (!isSprinting && Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouching = true;
            isSprinting = false;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            isCrouching = false;
        }

        if (Input.GetKeyDown(KeyCode.R))
            SaveSystem.Save();
        if (Input.GetKeyDown(KeyCode.Y))
            SaveSystem.Load();


        //jump force
        if (Input.GetKeyDown(KeyCode.Space) && rb.velocity.y == 0 && inAir==false) //velocity>0 means player is on the ground
        {
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.VelocityChange);
            inAir = true;
        }


        if (Input.GetKeyDown(KeyCode.A) || (Input.GetKeyDown(KeyCode.LeftArrow))) // go left
            if (currentLane != Lane.Left)
                currentLane -= Lane.Distance;

        if (Input.GetKeyDown(KeyCode.D) || (Input.GetKeyDown(KeyCode.RightArrow))) // go right
            if (currentLane != Lane.Right)
                currentLane += Lane.Distance;

        targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;

        if (currentLane == Lane.Left)
            targetPosition += Vector3.left * Lane.Distance;
        else if (currentLane == Lane.Right)
            targetPosition += Vector3.right * Lane.Distance;

        transform.position = targetPosition;
    }


    private void FixedUpdate()
    {
        rb.AddForce(Physics.gravity * (gravity - 1) * rb.mass); //add gravity for make jump faster
        MovingForward();
        if (targetPosition != Vector3.zero)
        {
            transform.position =
                Vector3.Lerp(transform.position, targetPosition, Lane.Distance * Time.fixedDeltaTime);
        }
    }


    public void MovingForward()
    {
        var currentSpead = playerSpeed;
        if (isSprinting) currentSpead = playerSpeed * sprintMultiplier;       // * 2
        else if (isCrouching) currentSpead = playerSpeed * crouchMultiplier;  // * .71

        Debug.Log(currentSpead);


        transform.Translate(Vector3.forward * Time.fixedDeltaTime * playerSpeed); //move player forward
    }
}