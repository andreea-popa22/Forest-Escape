using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadPlayerOnNewScene : MonoBehaviour
{
    private void Update()
    {
        // vezi daca avem date de incarcat si ne aflam in scena noua
        if (SaveSystem.s_gameData != null && SaveSystem.s_gameData.sceneIndex == SceneManager.GetActiveScene().buildIndex)
        {
            SaveSystem.LoadFromGameData(SaveSystem.s_gameData);
            SaveSystem.s_gameData = null;
        }
    }
}