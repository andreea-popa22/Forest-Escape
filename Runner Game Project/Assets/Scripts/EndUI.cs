using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndUI : MonoBehaviour
{
    public GameObject ui;
    public Canvas endMenu;
    public Button newGameButton;
    public PlayerController pc;
    [NonSerialized] private Button leaderboardButton;
    [NonSerialized] private Text content;
    [NonSerialized] private Canvas leaderboardMenu;
    
    // Start is called before the first frame update
    void Start()
    {
        ui = GameObject.Find("UI");
        endMenu = ui.transform.Find("End Canvas").GetComponent<Canvas>();
        leaderboardMenu = ui.transform.Find("Leaderboard Canvas").GetComponent<Canvas>();

        newGameButton = endMenu.transform.Find("Start new game").GetComponent<Button>();
        newGameButton.onClick.AddListener(StartNewGame);
        
        leaderboardButton = endMenu.transform.Find("Leaderboard").GetComponent<Button>();
        leaderboardButton.onClick.AddListener(WriteLeaderboardInUI);
        
        pc = GameObject.Find("Player").GetComponent<PlayerController>();

        content = leaderboardMenu.transform.Find("Content").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartNewGame()
    {
        SceneManager.LoadScene(0);
        endMenu.gameObject.SetActive(false);
        ui.transform.Find("Start Canvas").GetComponent<Canvas>().gameObject.SetActive(false);
        pc.enabled = true;
    }

    void WriteLeaderboardInUI()
    {
        endMenu.gameObject.SetActive(false);
        leaderboardMenu.gameObject.SetActive(true);
        string text = SaveSystem.WriteLeaderboard();
        Debug.Log("dc nu scrie");
        content.text = text;
    }
}
