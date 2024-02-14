using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum Language : short
{
    Korean = 0
}

[Serializable]
public class GameData
{
    public float _reading;
    public float _SFX;
    public float _BGM;
    public float _sensitivity;
    public int _language;
    public int _stage;
    public int _resolution;
    public bool _fullScreen;
}
public class JsonManager : SingleTon<JsonManager>
{
    public GameData gameData;
    string fileName;

    public float Reading { get { return gameData._reading; } set { gameData._reading = value; SaveData(); } }
    public float SFX { get { return gameData._SFX; } set { gameData._SFX = value; SaveData(); } }
    public float BGM { get { return gameData._BGM; } set { gameData._BGM = value; SaveData(); } }
    public float Sensitivity { get { return gameData._sensitivity; } set { gameData._sensitivity = value; SaveData(); } }
    public int Language { get { return gameData._language; } set { gameData._language = value; SaveData(); } }
    public int Stage { get { return gameData._stage; } set { gameData._stage = value; SaveData(); } }
    public int Resolution { get { return gameData._resolution; } set { gameData._resolution = value; SaveData(); } }
    public bool FullScreen { get { return gameData._fullScreen; } set { gameData._fullScreen = value; SaveData(); } }

    private void Awake()
    {
        fileName = Path.Combine(Application.persistentDataPath + "/ PlayData.json");
        if (File.Exists(fileName))
        {
            LoadData();
        }
        else
        {
            gameData = new GameData { };
            SaveData();
        }
    }
    public void SaveData()
    {
        if (File.Exists(fileName))
            File.Delete(fileName);

        string json = JsonUtility.ToJson(gameData);

        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json);
        string encodedJson = Convert.ToBase64String(bytes);

        File.WriteAllText(fileName, encodedJson);
    }


    public void LoadData()
    {
        if (File.Exists(fileName))
        {
            string jsonFromFile = File.ReadAllText(fileName);

            byte[] bytes = Convert.FromBase64String(jsonFromFile);
            string decodedJson = System.Text.Encoding.UTF8.GetString(bytes);

            gameData = JsonUtility.FromJson<GameData>(decodedJson);

        }
    }
}
