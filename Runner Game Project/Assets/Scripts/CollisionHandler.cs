using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    private bool isTransitioning = false;
    private bool collisionDisabled = false;
    private bool arrivedAtFinish = false;

    public void Start()
    {
    }
    
    private void OnCollisionEnter(Collision other)
    {
        //if (isTransitioning || collisionDisabled) { return; }

        switch (other.gameObject.tag)
        {
            case "Pickable":
                other.gameObject.SetActive(false);
                AddMana();
                break;
            case "Fin":
                arrivedAtFinish = true;
                LoadNextLevel();
                break;
            case "Friendly":
                break;
            default:
                StartCrashSequence();
                break;
        }
    }
    
    public void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    
    void StartCrashSequence()
    {
        isTransitioning = true;

        // Decrease health
        gameObject.GetComponent<PlayerHealth>().TakeDamage(25f);

        // Move player back
        transform.position += Vector3.back * 3;
    }

    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }

    
    public void AddMana()
    {
        gameObject.GetComponent<PlayerMana>().AddMana(25f);
    }

}
