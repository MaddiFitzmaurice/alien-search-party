using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class MenuData
{
    public static bool StoryModeOn = true;
    public static int LevelSelect;
    public static int LevelStory;

    [Serializable]
    class SaveData
    {
        public int level;
    }

    public static void SaveStoryData()
    {
        SaveData dataToSave = new SaveData();
        dataToSave.level = LevelStory;

        string json = JsonUtility.ToJson(dataToSave);
        File.WriteAllText(Application.persistentDataPath + "/savedata.json", json);
    }

    public static void LoadStoryData()
    {
        string path = Application.persistentDataPath + "/savedata.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData loadedData = JsonUtility.FromJson<SaveData>(json);

            LevelStory = loadedData.level;
        }
        // If save doesn't exist
        else
        {
            LevelStory = -1;
        }
    }
}
