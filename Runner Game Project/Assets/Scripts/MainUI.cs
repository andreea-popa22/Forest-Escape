using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MainUI : MonoBehaviour
{
    public GameObject ui;
    public Canvas menu;
    public Button startButton;
    public Button resumeButton;
    public PlayerController pc;
    private InputField inputName;
    [NonSerialized] private Button leaderboardButton;
    [NonSerialized] private Text content;
    [NonSerialized] private Canvas leaderboardMenu;
    [NonSerialized] private string prevScoreKey = "PreviousScore";
    [NonSerialized] private string playerNameKey = "PlayerName";

    void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        ui = GameObject.Find("UI");
        menu = ui.transform.Find("Start Canvas").GetComponent<Canvas>();
        leaderboardMenu = ui.transform.Find("Leaderboard Canvas").GetComponent<Canvas>();

        startButton = menu.transform.Find("Start").GetComponent<Button>();
        startButton.onClick.AddListener(StartGame);
        
        resumeButton = menu.transform.Find("Resume").GetComponent<Button>();
        resumeButton.onClick.AddListener(ResumeGameProgress);
        
        leaderboardButton = menu.transform.Find("Leaderboard").GetComponent<Button>();
        leaderboardButton.onClick.AddListener(WriteLeaderboardInUI);
        
        pc = GameObject.Find("Player").GetComponent<PlayerController>();
        
        //ps = GameObject.Find("Player").GetComponent<PlayerScore>();
        
        inputName = menu.transform.Find("Name Textbox").GetComponent<InputField>();
        
        content = leaderboardMenu.transform.Find("Content").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Return)) { SubmitName(inputName.text); }
        
    }

    void SubmitName(string name)
    {
        PlayerPrefs.SetString(playerNameKey, name);
        PlayerPrefs.Save();
    }
    
    void StartGame()
    {
        // check previous score
        int prev = PlayerPrefs.GetInt(prevScoreKey, 0);
        SaveSystem.CheckScore(prev);
        
        // initialize score 0
        PlayerPrefs.SetInt(prevScoreKey, 0);
        PlayerPrefs.Save();
        
        menu.gameObject.SetActive(false);
        pc.enabled = true;
    }

    void ResumeGameProgress()
    {
        menu.gameObject.SetActive(!menu.gameObject.activeSelf);
        pc.enabled = true;
        SaveSystem.Load();
    }
    
    void WriteLeaderboardInUI()
    {
        menu.gameObject.SetActive(false);
        leaderboardMenu.gameObject.SetActive(true);
        string text = SaveSystem.WriteLeaderboard();
        Debug.Log("dc nu scrie");
        content.text = text;
    }
}
