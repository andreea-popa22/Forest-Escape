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

    private float manaPerPickable = 25f;

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
                SetMana(GetMana() + manaPerPickable);
                break;
            case "Fin":
                arrivedAtFinish = true;
                LoadNextLevel();
                break;
            case "Friendly":
                GameObject.Find("Player").GetComponent<PlayerController>().inAir = false;
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
        float damage = IsInvincibile() ? 0 : 25f;
        crashPosition = transform.position;
        gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);      // Decrease health
        transform.position += Vector3.back * 3;                          // Move player back
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

    public float GetMana()
    {
        return gameObject.GetComponent<PlayerMana>().GetMana();
    }

    public void SetMana(float mana)
    {
        gameObject.GetComponent<PlayerMana>().SetMana(mana);
    }

    public bool IsInvincibile()
    {
        return gameObject.GetComponent<PlayerController>().isInvincibile();
    }
}
