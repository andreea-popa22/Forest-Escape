using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

[Serializable]
public class LeaderboardEntry
{
    public string name;
    public int score;

    public LeaderboardEntry()
    {
        this.name = "-";
        this.score = 0;
    }

    public LeaderboardEntry(string name, int score)
    {
        this.name = name;
        this.score = score;
    }
}

public static class Leaderboard
{
    public static string SAVE_FOLDER = Application.persistentDataPath + "/";

    public static void Save()
    {
        LeaderboardEntry leaderboardEntry = new LeaderboardEntry();
        
        File.WriteAllText(SAVE_FOLDER + "leaderboard.json", JsonUtility.ToJson(leaderboardEntry, true));
    }
}
