using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;

[Serializable]
public struct ObjectData
{
    public Vector3 position;
    public string prefab;
    //public string obstacleController; // is ignored if not used by game object
}

[Serializable]
public class GameData
{
    public Vector3 position;
    public string controller;
    public string health;
    public string mana;
    public string score;
    public int sceneIndex;
    public List<ObjectData> objects = new List<ObjectData>();
}

[Serializable]
public class LeaderboardEntry
{
    public int score;
    public string name;

    public LeaderboardEntry()
    {
        this.score = 0;
        this.name = "Empty";
    }
    
    public LeaderboardEntry(string name, int score)
    {
        this.name = name;
        this.score = score;
    }
}

[Serializable]
public class ListScores
{
    public List<LeaderboardEntry> list;
}

public static class SaveSystem
{
    public static readonly string SAVE_FOLDER = Application.persistentDataPath + "/";
    public static readonly string SAVE_FILE = SAVE_FOLDER + "save.json";
    public static readonly string LEADERBOARD_FILE = SAVE_FOLDER + "leaderboard.json";
    public static GameData s_gameData;

    public static void AddToLeaderBoard(List<LeaderboardEntry> list)
    {
        File.WriteAllText(LEADERBOARD_FILE, JsonUtility.ToJson(new ListScores(){list = list}, true));
    }

    public static List<LeaderboardEntry> LoadLeaderboard()
    {
        if (File.Exists(LEADERBOARD_FILE))
            return JsonUtility.FromJson<ListScores>(File.ReadAllText(LEADERBOARD_FILE)).list;
        return EmptyLeaderboard();
    }

    public static void Save()
    {
        GameObject player = GameObject.Find("Player");
        GameData gameData = new GameData()
        {
            controller = JsonUtility.ToJson(player.GetComponent<PlayerController>()),
            health = JsonUtility.ToJson(player.GetComponent<PlayerHealth>()),
            mana = JsonUtility.ToJson(player.GetComponent<PlayerMana>()),
            score = JsonUtility.ToJson(player.GetComponent<PlayerScore>()),
            position = player.GetComponent<Transform>().position,
            sceneIndex = SceneManager.GetActiveScene().buildIndex,
        };

        GameObject obstacles = GameObject.Find("Obstacles");
        for (int i = 0; i < obstacles.transform.childCount; i++)
        {
            GameObject obs = obstacles.transform.GetChild(i).gameObject;
            if (!obs.activeSelf)
                continue;
            gameData.objects.Add(new ObjectData()
            {
                //obstacleController = JsonUtility.ToJson(obs.GetComponent<ObstaclesController>()),
                position = obs.transform.position,
                prefab = obs.tag,
            });
        }
        File.WriteAllText(SAVE_FILE, JsonUtility.ToJson(gameData, true));
    }

    public static void LoadFromGameData(GameData gameData, bool loadGeneratedObjects = false)
    {
        var player = GameObject.Find("Player");
        JsonUtility.FromJsonOverwrite(gameData.controller, player.GetComponent<PlayerController>());
        JsonUtility.FromJsonOverwrite(gameData.health, player.GetComponent<PlayerHealth>());
        JsonUtility.FromJsonOverwrite(gameData.mana, player.GetComponent<PlayerMana>());
        JsonUtility.FromJsonOverwrite(gameData.score, player.GetComponent<PlayerScore>());
        player.GetComponent<Transform>().position = gameData.position;

        player.GetComponent<PlayerHealth>().TransformHealthBar();
        player.GetComponent<PlayerMana>().TransformManaBar();

        if (loadGeneratedObjects)
        {
            TerrainGenerator.Pool.SetInactiveAll();
            foreach (var data in gameData.objects)
            {
                var ob = TerrainGenerator.Pool.GetObject(data.prefab);
                ob.transform.position = data.position;
            }
        }

        var camera = GameObject.Find("Virtual Follow Camera").GetComponent<CinemachineVirtualCamera>();
        camera.ForceCameraPosition(new Vector3(0, 2.7f, player.transform.position.z - 7), Quaternion.Euler(20.343f, 0.116f, -0.28f));
        GameObject.Find("Environment").GetComponent<DeactivateObstacles>().Check(); // spawneaza obiectele
        player.GetComponent<PlayerController>().PauseGame();
    }

    public static void Load(bool loadGeneratedObjects = false)
    {
        if (!File.Exists(SAVE_FILE))
            return;

        GameData gameData = JsonUtility.FromJson<GameData>(File.ReadAllText(SAVE_FILE));
        if(gameData.sceneIndex == SceneManager.GetActiveScene().buildIndex)
        {
            LoadFromGameData(gameData);
            return;
        }
        else
        {
            s_gameData = gameData;
            SceneManager.LoadScene(s_gameData.sceneIndex, LoadSceneMode.Single);
        }
    }

    public static void SaveHighScore()
    {
        Debug.Log("Save Score");

        int score = PlayerPrefs.GetInt(PlayerPrefsKeys.prevSceneScore);
        PlayerPrefs.SetInt(PlayerPrefsKeys.prevSceneScore, 0);

        List<LeaderboardEntry> leaderboard = LoadLeaderboard();
        if (leaderboard.Count > 0)
        {
            Debug.Log("mai mare ca 0");
            string name = PlayerPrefs.GetString(PlayerPrefsKeys.name);
            if (score > leaderboard[4].score)
            {
                leaderboard[4] = new LeaderboardEntry(name, score);
                leaderboard = leaderboard.OrderByDescending(o=>o.score).ToList();
            }
            AddToLeaderBoard(leaderboard);
        } 
        
    }
    
    public static string WriteLeaderboard()
    {
        string text = "";
        List<LeaderboardEntry> leaderboard = LoadLeaderboard();
        foreach (LeaderboardEntry entry in leaderboard)
        {
            // add place
            text += (leaderboard.IndexOf(entry) + 1).ToString() + ".  ";
            // add score
            text += entry.score + "  ";
            // add name
            text += entry.name + "\n";
        }
        // write index+1 , entry.tostring, \n
        return text;
    }

    public static List<LeaderboardEntry> EmptyLeaderboard()
    {
        List<LeaderboardEntry> leaderboard = new List<LeaderboardEntry>();
        for (int i = 0; i < 5; i++)
        {
            LeaderboardEntry entry = new LeaderboardEntry();
            leaderboard.Add(entry);
        }
        AddToLeaderBoard(leaderboard);
        return leaderboard;
    }
}
