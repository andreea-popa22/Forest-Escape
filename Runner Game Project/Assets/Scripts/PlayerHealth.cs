using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float healthPoints = 100f;
    public GameObject healthBar;
    private float fullHealthLevel;
    private SpriteRenderer sprite;
    private GameObject healthSprite;
    private Vector2 screenBounds;
    void Start()
    {
        healthBar = GameObject.Find("Bar");
        fullHealthLevel = healthBar.transform.localScale.x;
        
        // fixed size - relative to screen resolution
        //healthSprite = GameObject.Find("Health Bar");
        //healthSprite.transform.localScale = new Vector3(Screen.width/1366f, Screen.height/768f,  1);
        
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    // Update is called once per frame
    void Update()
    {
        sprite = GameObject.Find("Bar Sprite").GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        //healthSprite = GameObject.Find("Health Bar");
        //healthSprite.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.1f, 0.8f, 18f));
        
    }

    public void TakeDamage(float damage)
    {
        healthPoints -= damage;
        LoseHealth();
        CollisionHandler ch = gameObject.GetComponent<CollisionHandler>();
        Debug.Log(healthPoints);
        if (healthPoints <= 0)
        {
            Debug.Log("You lost!");
            gameObject.GetComponent<PlayerController>().enabled = false;
            ch.Invoke("ReloadLevel", 1f);
        }
    }

    public void LoseHealth()
    {
        Vector3 healthBarScale = healthBar.transform.localScale;
        float healthLevel = healthBarScale.x;
        healthLevel -= (0.25f * fullHealthLevel);
        ColorHealthBar(healthLevel);
        healthBar.transform.localScale = new Vector3(healthLevel, healthBarScale.y, healthBarScale.z);
    }

    public void ColorHealthBar(float healthLevel)
    {
        if (healthLevel == fullHealthLevel * 0.75f)
        {
            sprite.color = Color.yellow;
        }
        else if (healthLevel == fullHealthLevel * 0.5f)
        {
            sprite.color = new Color32(250,106,3, 255);
        }
        else if (healthLevel == fullHealthLevel * 0.25f)
        {
            sprite.color = Color.red;
        }
    }
}
