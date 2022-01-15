using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CollisionHandler : MonoBehaviour
{
    public Vector3 crashPosition;
    private bool collisionDisabled = false;
    private bool arrivedAtFinish = false;

    private float manaPerPickable = 25f;

    [NonSerialized] private PlayerScore ps;
    [NonSerialized] public GameObject ui;
    [NonSerialized] public Canvas endMenu;
    [NonSerialized] public Text congratulations;
    [NonSerialized] private Text finalScore;

    public void Start()
    {
        ps = GameObject.Find("Player").GetComponent<PlayerScore>();
        
        ui = GameObject.Find("UI");
        endMenu = ui.transform.Find("End Canvas").GetComponent<Canvas>();
        finalScore = endMenu.transform.Find("Final Score").GetComponent<Text>();

        congratulations = endMenu.transform.Find("Congratulations").GetComponent<Text>();
    }

    private void handleObject(GameObject gameObject)
    {
        switch (gameObject.tag)
        {
            case "Pickable":
                PlayerScore playerScore = GameObject.Find("Player").GetComponent<PlayerScore>();
                playerScore.itemsPicked += 1;
                gameObject.SetActive(false);
                SetMana(GetMana() + manaPerPickable);
                break;
            case "Fin":
                CheckFinishGame();
                ps.SaveScoreToPrefs();
                arrivedAtFinish = true;
                LoadNextLevel();
                UnloadPreviousLevel();
                break;
            case "Friendly":
                GameObject.Find("Player").GetComponent<PlayerController>().inAir = false;
                GameObject.Find("Player").GetComponent<Animator>().SetBool("inAir1",false);
                break;
            case "Decor":
                break;
            case "Untagged":
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
    

    //public void ReloadLevel()
    //{
    //    int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    //    SceneManager.LoadScene(currentSceneIndex);
    //}
    
    void StartCrashSequence()
    {
        float damage = IsInvincibile() ? 0 : 25f;
        crashPosition = transform.position;
        gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);      // Decrease health
        transform.position += Vector3.back * 6;                          // Move player back
    }

    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        
        if (nextSceneIndex == 3)
        {
            nextSceneIndex = 0;
        }
        
        // if (nextSceneIndex == 0)
        // {
        //     SceneManager.LoadScene("TotalScene");
        // }
        if (nextSceneIndex == 1)
        {
            SceneManager.LoadScene("TotalScene1");
        }
        if (nextSceneIndex == 2)
        {
            SceneManager.LoadScene("TotalScene2");
        }
    }

    public static void UnloadPreviousLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    
        if (currentSceneIndex == 1)
            {
                if(SceneManager.GetSceneByName("TotalScene").isLoaded)
                {
                    SceneManager.UnloadSceneAsync("TotalScene");
                }
            }

        if (currentSceneIndex == 2)
        {
            if(SceneManager.GetSceneByName("TotalScene1").isLoaded)
            {
                SceneManager.UnloadSceneAsync("TotalScene1");
            }

        }
        
        if (currentSceneIndex == 0)
        {
            if(SceneManager.GetSceneByName("TotalScene2").isLoaded)
            {
                SceneManager.UnloadSceneAsync("TotalScene2");
            }

        }
        
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

    void CheckFinishGame()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex == 2)
        {
            PlayerController pc = GameObject.Find("Player").GetComponent<PlayerController>();
            pc.enabled = false;
            int score = ps.SaveScoreToPrefs();
            finalScore.text = "Final score: " + score;
            SaveSystem.SaveHighScore();
            endMenu.gameObject.SetActive(true);
            congratulations.gameObject.SetActive(true);
        }
    }
}
