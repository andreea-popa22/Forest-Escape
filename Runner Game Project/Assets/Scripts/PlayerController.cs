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
    private const float laneDistance = 2.0f;
    private CharacterController controller;
    private int desiredLane = 1; // 0 Left, 1 Middle, 2 Right
    private Vector3 targetPosition = Vector3.zero;
    
    [SerializeField] float playerSpeed = 1f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        //controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) || (Input.GetKeyDown(KeyCode.LeftArrow)))
        {
            MoveLane(false);
        }
        if (Input.GetKeyDown(KeyCode.D) || (Input.GetKeyDown(KeyCode.RightArrow)))
        {
            MoveLane(true);
        }

        targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;
        if (desiredLane == 0)
        {
            targetPosition += Vector3.left * laneDistance;
        }
        else if (desiredLane == 2)
        {
            targetPosition += Vector3.right * laneDistance;
        }
        
        
        transform.position = targetPosition;
    }
    
    private void FixedUpdate()
     {
         MovingForward();

         if (targetPosition != Vector3.zero)
         {
             transform.position = Vector3.Lerp(transform.position, targetPosition, laneDistance * Time.fixedDeltaTime);
         }
         
     }

    public void MovingForward()
    {
        // Moving forward continuously 
        rb.AddRelativeForce(Vector3.forward * playerSpeed);
    }

    private void MoveLane(bool goingRight)
    {
        if (!goingRight)
        {
            desiredLane--;
            if (desiredLane == -1)
            {
                desiredLane = 0;
            }
        }
        else
        {
            desiredLane++;
            if (desiredLane == 3)
            {
                desiredLane = 2;
            }
        }
    }
}    