using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    private bool isTransitioning = false;
    private bool collisionDisabled = false;

    private void OnCollisionEnter(Collision other)
    {
        if (isTransitioning || collisionDisabled) { return; }

        switch (other.gameObject.tag)
        {
            case "Friendly":
                break;
            default:
                StartCrashSequence();
                break;
        }
    }
    
    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    
    void StartCrashSequence()
    {
        isTransitioning = true;
        GetComponent<PlayerController>().enabled = false;
        Debug.Log("You lost!");
        ReloadLevel();
    }
    
}
