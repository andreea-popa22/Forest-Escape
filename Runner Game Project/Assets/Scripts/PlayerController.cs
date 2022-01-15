using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Xml.Schema;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private CharacterController controller;
    [SerializeField] private float currentLane = Lane.Middle;
    private Vector3 targetPosition = Vector3.zero;
    [NonSerialized] public bool pausedGame = false;
    [NonSerialized] public GameObject ui;
    [NonSerialized] public Canvas pauseUI;
    [NonSerialized] public Canvas saveStateUI;

    [SerializeField] public float playerSpeed = 5f;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float gravity;

    public float boostCooldown = 5f;
    public float boostDuration = 2f;
    private float speedBoost = 2f;

    public bool hasInvincibility = false;
    private bool hasBoost = false;
    private bool hasBoostCooldown;
    public bool inAir = false;
    [NonSerialized] public Animator anim;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        GameObject.Find("Virtual Follow Camera").GetComponent<CinemachineVirtualCamera>().Follow = this.transform;
        //controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        ui = GameObject.Find("UI");
        playerSpeed = SceneManager.GetActiveScene().name switch
        {
            "TotalScene" => 5,
            "TotalScene1" => 10,
            "TotalScene2" => 15,
            _ => 5
        };
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !hasBoostCooldown)
        {
            // activate the cooldown and start the deactivation method for the boost
            StartCoroutine(ActivateCooldown());
        }

        if (!hasInvincibility && Input.GetKeyDown(KeyCode.F))
        {
            //hasInvincibility = true;
            // activate invincibility if mana is full
            StartCoroutine(ActivateInvincibility(GetMana()));
        }

        if (Input.GetKeyDown(KeyCode.R)) SaveSystem.Save();
        if (Input.GetKeyDown(KeyCode.Y)) SaveSystem.Load();

        //jump force
        if (Input.GetKeyDown(KeyCode.Space) && rb.velocity.y == 0 && inAir==false) //velocity>0 means player is on the ground
        {
            anim.SetBool("inAir1", true);
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.VelocityChange);
            inAir = true;
        }


        if (Input.GetKeyDown(KeyCode.P) && pausedGame == false)
            PauseGame();

        if (currentLane != Lane.Left && Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) // go left
            currentLane -= Lane.Distance;

        if (currentLane != Lane.Right && (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))) // go right
            currentLane += Lane.Distance;

        targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;

        if (currentLane == Lane.Left) targetPosition += Vector3.left * Lane.Distance;
        else if (currentLane == Lane.Right) targetPosition += Vector3.right * Lane.Distance;

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
        SetMana(0);
        Debug.Log("boost ready");
    }

    IEnumerator ActivateInvincibility(float duration)
    {
        Debug.Log("Player now invincible!");
        hasInvincibility = true;
        yield return new WaitForSeconds(duration / 25);
        hasInvincibility = false;
        Debug.Log("Player invincible no more!");
        SetMana(0);
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
        
        saveStateUI = ui.transform.Find("Save State").GetComponent<Canvas>();

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
        pauseUI.gameObject.SetActive(false);
        saveStateUI.gameObject.SetActive(true);
    }


}