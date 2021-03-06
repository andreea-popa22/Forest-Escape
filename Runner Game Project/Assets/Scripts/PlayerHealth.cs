using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float healthPoints = 100f;
    [NonSerialized] public GameObject healthBar;
    private float fullHealthLevel;
    private SpriteRenderer sprite;
    private GameObject healthSprite;
    [NonSerialized] private PlayerScore ps;
    [NonSerialized] public GameObject ui;
    [NonSerialized] public Canvas endMenu;
    [NonSerialized] public Text gameOver;
    [NonSerialized] private GameObject obstacles;
    [NonSerialized] private Text finalScore;
    //[NonSerialized] private string prevScoreKey = "PreviousScore";

    void Start()
    {
        healthBar = GameObject.Find("Bar");
        fullHealthLevel = healthBar.transform.localScale.x;
        
        ui = GameObject.Find("UI");
        endMenu = ui.transform.Find("End Canvas").GetComponent<Canvas>();
        finalScore = endMenu.transform.Find("Final Score").GetComponent<Text>();

        gameOver = endMenu.transform.Find("Game over").GetComponent<Text>();

        ps = GameObject.Find("Player").GetComponent<PlayerScore>();
        
        obstacles = GameObject.Find("Obstacles-Manual");

    }

    // Update is called once per frame
    void Update()
    {
        sprite = GameObject.Find("Bar Sprite").GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        
        
    }

    public void TakeDamage(float damage)
    {
        if (healthPoints != 0)
        {
            
            healthPoints -= damage;
            TransformHealthBar();
            CollisionHandler ch = gameObject.GetComponent<CollisionHandler>();

        }
        else if (healthPoints <= 0)
        {
            // activate end UI
            endMenu.gameObject.SetActive(true);
            gameOver.gameObject.SetActive(true);
            int score = ps.SaveScoreToPrefs();
            finalScore.text = "Final score: " + score;
            obstacles.gameObject.SetActive(false);
            gameObject.GetComponent<PlayerController>().enabled = false;
            SaveSystem.SaveHighScore();
            //ch.Invoke("ReloadLevel", 1f);
        }
    }

    public void TransformHealthBar()
    {
        Vector3 healthBarScale = healthBar.transform.localScale;
        float healthLevel = fullHealthLevel * (healthPoints / 100);
        ColorHealthBar(healthLevel);
        healthBar.transform.localScale = new Vector3(healthLevel, healthBarScale.y, healthBarScale.z);
    }

    public void ColorHealthBar(float healthLevel)
    {
        if (healthLevel >= fullHealthLevel * 0.75f)
        {
            sprite.color = Color.yellow;
        }
        else if (healthLevel >= fullHealthLevel * 0.5f)
        {
            sprite.color = new Color32(250,106,3, 255);
        }
        else if (healthLevel >= fullHealthLevel * 0.25f)
        {
            sprite.color = Color.red;
        }
    }
}
