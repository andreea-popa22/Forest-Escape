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
    public bool pausedGame = false;
    public GameObject ui;
    public Canvas pauseUI;
    
    [SerializeField] float playerSpeed = 1f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        //controller = GetComponent<CharacterController>();
        
        ui = GameObject.Find("UI");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            SaveSystem.Save();
        if (Input.GetKeyDown(KeyCode.Y))
            SaveSystem.Load();

        if (Input.GetKeyDown(KeyCode.P) && pausedGame == false)
            PauseGame();
        // else if (Input.GetKeyDown(KeyCode.P) && pausedGame)
        //     ResumeGame();

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

    public void PauseGame()
    {
        Debug.Log("paused");
        Time.timeScale = 0;
        pausedGame = true;
        pauseUI = ui.transform.Find("Pause Canvas").GetComponent<Canvas>();
        // activate Pause Canvas
        pauseUI.gameObject.SetActive(true); 
        // activate Resume Button
        pauseUI.transform.Find("Resume").GetComponent<Button>().gameObject.SetActive(true);
        // activate Quit Button
        pauseUI.transform.Find("Quit").GetComponent<Button>().gameObject.SetActive(true);
    }
    
    public void ResumeGame()
    {
        Debug.Log("resumed");
        Time.timeScale = 1;
        pausedGame = false;
        pauseUI.gameObject.SetActive(false); 
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}