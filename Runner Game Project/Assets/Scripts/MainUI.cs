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
    [NonSerialized] private string prevScoreKey = "PreviousScore";

    void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        ui = GameObject.Find("UI");
        menu = ui.transform.Find("Start Canvas").GetComponent<Canvas>();
        
        startButton = menu.transform.Find("Start").GetComponent<Button>();
        startButton.onClick.AddListener(StartGame);
        
        resumeButton = menu.transform.Find("Resume").GetComponent<Button>();
        resumeButton.onClick.AddListener(ResumeGameProgress);
        
        pc = GameObject.Find("Player").GetComponent<PlayerController>();
        
        inputName = menu.transform.Find("Name Textbox").GetComponent<InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Return)) { SubmitName(inputName.text); }
        
    }

    void SubmitName(string name)
    {
        Debug.Log(name);
    }
    
    void StartGame()
    {
        PlayerPrefs.SetInt(prevScoreKey, 0);
        PlayerPrefs.Save();
        menu.gameObject.SetActive(!menu.gameObject.activeSelf);
        pc.enabled = true;
    }

    void ResumeGameProgress()
    {
        menu.gameObject.SetActive(!menu.gameObject.activeSelf);
        pc.enabled = true;
        SaveSystem.Load();
    }
}
