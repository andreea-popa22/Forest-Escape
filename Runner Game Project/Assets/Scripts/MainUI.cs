using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    public GameObject menu;
    public Button startButton;
    public PlayerController pc;
    private InputField inputName;
    //private string input;

    void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        menu = GameObject.Find("Start Canvas");
        
        startButton = GameObject.Find("Start").GetComponent<Button>();
        startButton.onClick.AddListener(StartGame);
        
        pc = GameObject.Find("Player").GetComponent<PlayerController>();
        
        inputName = GameObject.Find("Name Textbox").GetComponent<InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Return)) { SubmitName(inputName.text); }

        if (Input.GetKeyUp(KeyCode.P)) { PauseGame();};
    }

    void SubmitName(string arg0)
    {
        Debug.Log(arg0);
    }
    
    void StartGame()
    {
        Debug.Log("yes");
        menu.gameObject.SetActive(!menu.gameObject.activeSelf);
        pc.enabled = !pc.enabled;
    }

    void PauseGame()
    {
        
    }
}
