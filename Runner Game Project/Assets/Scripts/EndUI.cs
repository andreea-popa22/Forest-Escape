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
    
    // Start is called before the first frame update
    void Start()
    {
        ui = GameObject.Find("UI");
        endMenu = ui.transform.Find("End Canvas").GetComponent<Canvas>();

        newGameButton = endMenu.transform.Find("Start new game").GetComponent<Button>();
        newGameButton.onClick.AddListener(StartNewGame);
        
        pc = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartNewGame()
    {
        Debug.Log("new game");
        SceneManager.LoadScene(0);
        endMenu.gameObject.SetActive(false);
        ui.transform.Find("Start Canvas").GetComponent<Canvas>().gameObject.SetActive(false);
        pc.enabled = true;
    }
}
