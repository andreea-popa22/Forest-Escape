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
        
        content = leaderboardMenu.transform.Find("Content").GetComponent<Text>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartNewGame()
    {
        SceneManager.LoadScene(0);
    }

    void WriteLeaderboardInUI()
    {
        endMenu.gameObject.SetActive(false);
        leaderboardMenu.gameObject.SetActive(true);
        string text = SaveSystem.WriteLeaderboard();
        content.text = text;
    }
}
