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
    [SerializeField] NewPlayerController _playerController;
    [SerializeField] Transform _playerTransform;
    
    public void SaveGame()
    {
        _serializedSaveGame = new SerializedSaveGame();
        
        _serializedSaveGame.playerHP = _playerController.CurrentHp;
        _serializedSaveGame.playerMaxScore = _playerController.MaxScore;
        _serializedSaveGame.playerScore = _playerController.Score;
        _serializedSaveGame.maxEnemyCount = _playerController.MaxEnemyCount;
        _serializedSaveGame.enemyCount = _playerController.EnemyCount;
        _serializedSaveGame.playerPosition = _playerTransform.position;
        //_serializedSaveGame.playerCheckpoint = GetCurrentPlayerCheckpoint();

        // Gather enemy data
        _serializedSaveGame.enemyPositions = GetAllEnemyPositions();

        // Gather collectibles data
        _serializedSaveGame.collectiblesPositions = GetAllCollectiblesPositions();

        // Gather volume data
        //_serializedSaveGame.volume = GetVolumeLevel();
        SaveToJson();
    }
    public void LoadGame()
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
    public Vector3[] GetAllCheckPointsPositions()
    {

        GameObject[] checkPointObjects = GameObject.FindGameObjectsWithTag("checkPoint");

        // Create an array to store enemy positions
        Vector3[] checkPointPositions = new Vector3[checkPointObjects.Length];

        // Iterate through each enemy object and store its position
        for (int i = 0; i < checkPointObjects.Length; i++)
        {
            checkPointPositions[i] = checkPointObjects[i].transform.position;
        }

        return checkPointPositions;
    }
    public Vector3[] GetAllCollectiblesPositions()
    {

        GameObject[] collectibleObjects = GameObject.FindGameObjectsWithTag("collectible");

        // Create an array to store enemy positions
        Vector3[] collectiblePositions = new Vector3[collectibleObjects.Length];

        // Iterate through each enemy object and store its position
        for (int i = 0; i < collectibleObjects.Length; i++)
        {
            collectiblePositions[i] = collectibleObjects[i].transform.position;
        }

        return collectiblePositions;
    }
    public Vector3[] GetAllEnemyPositions()
    {
        // Get all enemy game objects in the scene
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("enemy");

        // Create an array to store enemy positions
        Vector3[] enemyPositions = new Vector3[enemyObjects.Length];

        // Iterate through each enemy object and store its position
        for (int i = 0; i < enemyObjects.Length; i++)
        {
            enemyPositions[i] = enemyObjects[i].transform.position;
        }

        return enemyPositions;
    }
}
