using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
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
    public List<ObjectData> objects = new List<ObjectData>();
}

// [Serializable]
// public class LeaderboardEntry
// {
//     public string name;
//     public int score;
// }

public static class SaveSystem
{
    public static readonly string SAVE_FOLDER = Application.persistentDataPath + "/";
    public static readonly string SAVE_FILE = SAVE_FOLDER + "save.json";
    public static readonly string LEADERBOARD_FILE = SAVE_FOLDER + "leaderboard.json";

    public static void AddToLeaderBoard(string name, int score)
    {
        File.Create(LEADERBOARD_FILE);
        string data = File.ReadAllText(LEADERBOARD_FILE);
        var list = JsonUtility.FromJson<List<LeaderboardEntry>>(data);
        list.Add(new LeaderboardEntry() { name = name, score = score });
        File.WriteAllText(LEADERBOARD_FILE, JsonUtility.ToJson(list, prettyPrint: true));
    }

    public static List<LeaderboardEntry> LoadLeaderboards()
    {
        if (File.Exists(LEADERBOARD_FILE))
            return JsonUtility.FromJson<List<LeaderboardEntry>>(File.ReadAllText(LEADERBOARD_FILE));
        else
            return new List<LeaderboardEntry>();
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

    public static void Load(bool loadGeneratedObjects = false)
    {
        if (!File.Exists(SAVE_FILE))
            return;

        string dataJson = File.ReadAllText(SAVE_FILE);

        var player = GameObject.Find("Player");
        GameData gameData = JsonUtility.FromJson<GameData>(dataJson);
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
    }
}
