using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SaveStateUI : MonoBehaviour
{
    [NonSerialized] public GameObject ui;
    [NonSerialized] public Canvas saveStateUI;
    [NonSerialized] public Button yes;
    [NonSerialized] public Button no;
    
    // Start is called before the first frame update
    void Start()
    {
        ui = GameObject.Find("UI");
        saveStateUI = ui.transform.Find("Save State").GetComponent<Canvas>();

        yes = saveStateUI.transform.Find("Yes").GetComponent<Button>();
        yes.onClick.AddListener(SaveState);
        
        no = saveStateUI.transform.Find("No").GetComponent<Button>();
        no.onClick.AddListener(NoSave);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SaveState()
    {
        SaveSystem.Save();
        saveStateUI.gameObject.SetActive(false);
        Application.Quit();
    }

    void NoSave()
    {
        saveStateUI.gameObject.SetActive(false);
        Application.Quit();
    }
}
