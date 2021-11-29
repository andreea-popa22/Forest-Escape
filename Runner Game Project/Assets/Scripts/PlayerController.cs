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
    private float currentLane = Lane.Middle;
    private Vector3 targetPosition = Vector3.zero;
    
    [SerializeField] float playerSpeed = 1f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        //controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) || (Input.GetKeyDown(KeyCode.LeftArrow))) // go left
            if (currentLane != Lane.Left)
                currentLane -= Lane.Distance;
        if (Input.GetKeyDown(KeyCode.D) || (Input.GetKeyDown(KeyCode.RightArrow))) // go right
            if(currentLane != Lane.Right)
                currentLane += Lane.Distance;

        targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;
        if(currentLane == Lane.Left)
            targetPosition += Vector3.left * Lane.Distance;
        else if (currentLane == Lane.Right)
            targetPosition += Vector3.right * Lane.Distance;
        
        transform.position = targetPosition;
    }

    private void FixedUpdate()
    {
        MovingForward();
        if (targetPosition != Vector3.zero)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, Lane.Distance * Time.fixedDeltaTime);
        }
    }

    public void MovingForward()
    {
        // Moving forward continuously 
        rb.AddRelativeForce(Vector3.forward * playerSpeed);
    }

}