using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    public GameObject ui;
    public Canvas menu;
    public Button resumeButton;
    public Button quitButton;
    public PlayerController pc;
    private InputField inputName;
    private Text three;
    private Text two;
    private Text one;
    public bool inCountdown = false;

    // Start is called before the first frame update
    void Start()
    {
        ui = GameObject.Find("UI");
        menu = ui.transform.Find("Pause Canvas").GetComponent<Canvas>();
        pc = GameObject.Find("Player").GetComponent<PlayerController>();
        
        resumeButton = menu.transform.Find("Resume").GetComponent<Button>();
        resumeButton.onClick.AddListener(HideUI);
        
        quitButton = menu.transform.Find("Quit").GetComponent<Button>();
        quitButton.onClick.AddListener(pc.QuitGame);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void HideUI()
    {
        inCountdown = true;
        resumeButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        StartCoroutine(ui.gameObject.GetComponent<CountdownController>().CountdownToStart());
    }

}
