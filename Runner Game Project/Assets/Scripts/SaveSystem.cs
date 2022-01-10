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
    public string obstacleController; // is ignored if not used by game object
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

    //[SerializeField] public PlayerController controller;
    //public PlayerHealth health;
    //public PlayerMana mana;
    //public PlayerScore score;
}

public static class SaveSystem
{
    public static string SAVE_FOLDER = Application.persistentDataPath + "/";

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
            GameObject child = obstacles.transform.GetChild(i).gameObject;
            gameData.objects.Add(new ObjectData()
            {
                obstacleController = JsonUtility.ToJson(child.GetComponent<ObstaclesController>()),
                position = child.transform.position,
                prefab = "",
            });
        }
        File.WriteAllText(SAVE_FOLDER + "save.json", JsonUtility.ToJson(gameData, true));
    }

    public static List<ObjectData> Load()
    {
        if (!File.Exists(SAVE_FOLDER + "save.json"))
            return null;

        string dataJson = File.ReadAllText(SAVE_FOLDER + "save.json");

        var player = GameObject.Find("Player");
        GameData gameData = JsonUtility.FromJson<GameData>(dataJson);
        JsonUtility.FromJsonOverwrite(gameData.controller, player.GetComponent<PlayerController>());
        JsonUtility.FromJsonOverwrite(gameData.health, player.GetComponent<PlayerHealth>());
        JsonUtility.FromJsonOverwrite(gameData.mana, player.GetComponent<PlayerMana>());
        JsonUtility.FromJsonOverwrite(gameData.score, player.GetComponent<PlayerScore>());
        player.GetComponent<Transform>().position = gameData.position;

        player.GetComponent<PlayerHealth>().TransformHealthBar();
        player.GetComponent<PlayerMana>().TransformManaBar();

        return null;

        var obsParent = GameObject.Find("Obstacles");
        foreach (var data in gameData.objects)
        {
            var child = GameObject.Instantiate(null, obsParent.transform);
            //child.transform.position += data.position;
        }

        return gameData.objects;

        //string data = JsonUtility.ToJson(playerData);
        //BinaryFormatter fm = new BinaryFormatter();
        //FileStream stream = new FileStream(path: "", FileMode.Create);
        //fm.Serialize(stream, data);
        //Debug.Log(SAVE_FOLDER);

        //playerData.controller = player.GetComponent<PlayerController>();
        //playerData.health = player.GetComponent<PlayerHealth>();
        //playerData.mana = player.GetComponent<PlayerMana>();
        //playerData.score = player.GetComponent<PlayerScore>();

        //playerData.rigidbody = JsonUtility.ToJson(player.GetComponent<Rigidbody>());
        //playerData.transform = JsonUtility.ToJson(player.GetComponent<Transform>());

        //int saveNum = 1;
        //while (File.Exists(SAVE_FOLDER + "save_" + saveNum + ".json"))
        //    saveNum += 1;
        //File.WriteAllText();

        /* load */
        //DirectoryInfo info = new DirectoryInfo(SAVE_FOLDER);
        //FileInfo[] saveFiles = info.GetFiles();
        //FileInfo recent = null;
        //foreach (var file in saveFiles)
        //    if (file.LastWriteTime > recent.LastWriteTime)
        //        recent = file;

        //if (File.Exists(SAVE_FOLDER + "save.json"))
        //    return File.ReadAllText(SAVE_FOLDER + "save.json");
        //else
        //    return null;

        //JsonUtility.FromJsonOverwrite("", null);
        //File.ReadAllText();
    }
}
