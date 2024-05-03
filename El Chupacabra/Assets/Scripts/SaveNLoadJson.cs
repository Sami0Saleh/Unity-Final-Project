using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using UnityEngine;

public class SaveNLoadJson : MonoBehaviour
{
    private const string SAVE_FILE_NAME = "/UnityAdvancedSave.dat";
    private SerializedSaveGame _serializedSaveGame;

    public void SaveGame() // need to be added to a button
    {
        _serializedSaveGame = new SerializedSaveGame();
        SaveToJson();
    }

    public void LoadGame() // need to be added to a button
    {
        LoadFromJson();
    }


    public void SaveToJson()
    {
        string jsonString = JsonUtility.ToJson(_serializedSaveGame, true);
        File.WriteAllText(Application.persistentDataPath + SAVE_FILE_NAME, jsonString);

    }

    public void LoadFromJson()
    {
        string jsonString = File.ReadAllText(Application.persistentDataPath + SAVE_FILE_NAME);
        _serializedSaveGame = JsonUtility.FromJson<SerializedSaveGame>(jsonString);
    }

}
