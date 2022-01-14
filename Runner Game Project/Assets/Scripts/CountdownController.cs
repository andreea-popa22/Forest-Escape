using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownController : MonoBehaviour
{
    public GameObject ui;
    public Canvas menu;
    public int countdownTime = 3;
    public Text countdownDisplay;
    public PauseUI pauseScript;
    public PlayerController pc;
    
    
    // Start is called before the first frame update
    void Start()
    {
        pc = GameObject.Find("Player").GetComponent<PlayerController>();
        
        pauseScript = gameObject.GetComponent<PauseUI>();
        ui = GameObject.Find("UI");
        menu = ui.transform.Find("Pause Canvas").GetComponent<Canvas>();
        countdownDisplay = menu.transform.Find("Countdown Text").GetComponent<Text>();
    }

    public IEnumerator CountdownToStart()
    {
        countdownDisplay.gameObject.SetActive(true);
        
        while (countdownTime > 0)
        {
            countdownDisplay.text = "  " + countdownTime;

            yield return new WaitForSecondsRealtime(1);

            countdownTime--;
        }

        countdownDisplay.text = "GO!";

        yield return new WaitForSecondsRealtime(1);

        countdownDisplay.gameObject.SetActive(false);
        
        pauseScript.inCountdown = false;

        countdownTime = 3;

        pc.ResumeGame();
    }
    
    
}
