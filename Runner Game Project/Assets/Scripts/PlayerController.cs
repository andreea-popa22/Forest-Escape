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
    [NonSerialized] public GameObject ui;
    [NonSerialized] public Canvas pauseUI;

    [SerializeField] float playerSpeed = 5f;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float gravity;

    public float boostCooldown = 5f;
    public float boostDuration = 2f;
    private float speedBoost = 2f;
    private float invincibilityDuration = 2f;
    private bool hasInvincibility = false;
    private bool hasBoost = false;
    private bool hasBoostCooldown;
    public bool inAir = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        //controller = GetComponent<CharacterController>();
        
        ui = GameObject.Find("UI");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !hasBoostCooldown)
        {
            // activate the cooldown and start the deactivation method for the boost
            StartCoroutine(ActivateCooldown());
        }
        if (Input.GetKeyDown(KeyCode.F) && GetMana() == GetMaxMana())
        {
            // activate invincibility if mana is full
            StartCoroutine(ActivateInvincibility(invincibilityDuration));
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


        if (Input.GetKeyDown(KeyCode.P) && pausedGame == false)
            PauseGame();

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
        rb.AddForce(Physics.gravity * (gravity - 1) * rb.mass); // add gravity for make jump faster
        MovingForward();
        if (targetPosition != Vector3.zero)
        {
            transform.position =
                Vector3.Lerp(transform.position, targetPosition, Lane.Distance * Time.fixedDeltaTime);
        }
    }

    IEnumerator ActivateCooldown()
    {
        hasBoostCooldown = true;
        hasBoost = true;
        yield return new WaitForSeconds(boostDuration);     // wait until the boost is ready again
        hasBoost = false;
        yield return new WaitForSeconds(boostCooldown);
        hasBoostCooldown = false;
        Debug.Log("boost ready");
    }

    IEnumerator ActivateInvincibility(float duration)
    {
        SetMana(0);
        Debug.Log("Player now invincible!");
        hasInvincibility = true;
        yield return new WaitForSeconds(duration);
        hasInvincibility = false;
        Debug.Log("Player invincible no more!");
    }

    public void MovingForward()
    {
        var currentSpead = playerSpeed;
        if (hasBoost) currentSpead = playerSpeed * speedBoost;  // * 2

        transform.Translate(Vector3.forward * Time.fixedDeltaTime * currentSpead); //move player forward
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

    public float GetMana()
    {
        return gameObject.GetComponent<PlayerMana>().GetMana();
    }

    public float GetMaxMana()
    {
        return gameObject.GetComponent<PlayerMana>().GetMaxMana();
    }

    public bool isInvincibile()
    {
        return hasInvincibility;
    }

    public void SetMana(float mana)
    {
        gameObject.GetComponent<PlayerMana>().SetMana(mana);
    }

    public void QuitGame()
    {
        Application.Quit();
    }


}