using UnityEngine;
using System.IO;

public static class SaveSystem
{
    private static readonly string SavePath = Application.persistentDataPath + "/game.json";

    public static void SaveGame(GameData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);
    }

    public static GameData LoadGame()
    {
        if (!File.Exists(SavePath))
        {
            Debug.LogError("Save file not found in " + SavePath);
            return null;
        }

        string json = File.ReadAllText(SavePath);
        return JsonUtility.FromJson<GameData>(json);
    }

    public static bool SaveExists() => File.Exists(SavePath);
}