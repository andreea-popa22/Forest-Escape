using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Collections;
using UnityEngine.UI;

public class LeaderboardUI : MonoBehaviour
{
    [NonSerialized] GameObject ui;
    [NonSerialized] Canvas leaderboardMenu;
    [NonSerialized] Canvas startMenu;
    [NonSerialized] Canvas endMenu;
    [NonSerialized] Button backButton;
    [NonSerialized] private string prevCanvasKey = "PreviousCanvas";
    
    // Start is called before the first frame update
    void Start()
    {
        ui = GameObject.Find("UI");
        leaderboardMenu = ui.transform.Find("Leaderboard Canvas").GetComponent<Canvas>();
        startMenu = ui.transform.Find("Start Canvas").GetComponent<Canvas>();
        endMenu = ui.transform.Find("End Canvas").GetComponent<Canvas>();
        
        backButton = leaderboardMenu.transform.Find("Back").GetComponent<Button>();
        backButton.onClick.AddListener(BackToMainUI);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void BackToMainUI()
    {
        string prev = PlayerPrefs.GetString(prevCanvasKey);
        Debug.Log(prev);
        if (prev == "Start")
        {
            startMenu.gameObject.SetActive(true);
        }
        else
        {
            endMenu.gameObject.SetActive(true);
        }
        leaderboardMenu.gameObject.SetActive(false);
    }
}
