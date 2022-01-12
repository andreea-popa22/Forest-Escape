using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    public Vector3 crashPosition;
    public bool isTransitioning = false;
    private bool collisionDisabled = false;
    private bool arrivedAtFinish = false;

    public void Start()
    {
    }

    private void handleObject(GameObject gameObject)
    {
        //if (isTransitioning || collisionDisabled) { return; }

        switch (gameObject.tag)
        {
            case "Pickable":
                PlayerScore playerScore = GameObject.Find("Player").GetComponent<PlayerScore>();
                playerScore.itemsPicked += 1;
                gameObject.SetActive(false);
                AddMana();
                break;
            case "Fin":
                arrivedAtFinish = true;
                LoadNextLevel();
                break;
            case "Friendly":
                GameObject.Find("Player").GetComponent<PlayerController>().inAir=false;
                break;
            default:
                StartCrashSequence();
                break;
        }
    }
    
    private void OnCollisionEnter(Collision other)
    {
        handleObject(other.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        handleObject(other.gameObject);
    }
    

    public void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    
    void StartCrashSequence()
    {
        isTransitioning = true;
        crashPosition = transform.position;
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
