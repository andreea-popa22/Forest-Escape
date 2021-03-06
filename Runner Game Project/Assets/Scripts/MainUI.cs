using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    [NonSerialized] private string prevCanvas;

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
        if (SceneManager.GetActiveScene().buildIndex == 0)
            pc.enabled = false;
        else
            pc.enabled = true;
        
        //ps = GameObject.Find("Player").GetComponent<PlayerScore>();
        
        inputName = menu.transform.Find("Name Textbox").GetComponent<InputField>();
        
        content = leaderboardMenu.transform.Find("Content").GetComponent<Text>();
    }
    
    void StartGame()
    {
        // check previous score
        int prev = PlayerPrefs.GetInt(PlayerPrefsKeys.prevSceneScore, 0);

        // initialize score 0
        if(inputName.text == "")
            PlayerPrefs.SetString(PlayerPrefsKeys.name, "Foxy");
        else
            PlayerPrefs.SetString(PlayerPrefsKeys.name, inputName.text);
        PlayerPrefs.Save();
        
        menu.gameObject.SetActive(false);
        pc.enabled = true;
    }

    void ResumeGameProgress()
    {
        menu.gameObject.SetActive(!menu.gameObject.activeSelf);
        pc.enabled = true;
        SaveSystem.Load(); // incarca in scena asta
    }
    
    void WriteLeaderboardInUI()
    {
        PlayerPrefs.SetString(PlayerPrefsKeys.prevCanvasKey, "Start");
        PlayerPrefs.Save();
        menu.gameObject.SetActive(false);
        leaderboardMenu.gameObject.SetActive(true);
        string text = SaveSystem.WriteLeaderboard();
        content.text = text;
    }
}
