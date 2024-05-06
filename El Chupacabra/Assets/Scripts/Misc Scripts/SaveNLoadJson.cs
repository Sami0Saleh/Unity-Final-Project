using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using UnityEngine;

public class SaveNLoadJson : MonoBehaviour
{
    private const string SAVE_FILE_NAME = "/SaveGame.dat";
    private const string CHECK_POINT_SAVE_FILE_NAME = "/CheckPointSave.dat";
    private SerializedSaveGame _serializedSaveGame;
    [SerializeField] NewPlayerController _playerController;
    [SerializeField] GameObject _playerObject;
    [SerializeField] public Vector3 CheckPointPos;
    public void SaveGame()
    {
        _serializedSaveGame = new SerializedSaveGame();
        
        _serializedSaveGame.playerHP = _playerController.CurrentHp;
        _serializedSaveGame.playerMaxScore = _playerController.MaxScore;
        _serializedSaveGame.playerScore = _playerController.Score;
        _serializedSaveGame.maxEnemyCount = _playerController.MaxEnemyCount;
        _serializedSaveGame.enemyCount = _playerController.EnemyCount;
        _serializedSaveGame.playerPosition = _playerObject.transform.position;

        // Gather enemy data
        _serializedSaveGame.enemyPositions = GetAllEnemyPositions();
        _serializedSaveGame.enemyObjects = GetAllEnemyObjects();

        // Gather collectibles data
        _serializedSaveGame.collectiblesPositions = GetAllCollectiblesPositions();
        _serializedSaveGame.collectiblesObjects = GetAllCollectiblesObjects();

        // Gather volume data
        //_serializedSaveGame.volume = GetVolumeLevel();
        SaveToJson();
    }
    public void LoadGame()
    {
        LoadFromJson();
        _playerController.CurrentHp = _serializedSaveGame.playerHP;
        _playerController.MaxScore = _serializedSaveGame.playerMaxScore;
        _playerController.Score = _serializedSaveGame.playerScore;
        _playerController.MaxEnemyCount = _serializedSaveGame.maxEnemyCount;
        _playerController.EnemyCount = _serializedSaveGame.enemyCount;
        _playerObject.transform.position = _serializedSaveGame.playerPosition;
        SetPlayer(_playerObject.transform.position);
        SetAllEnemyPositions();
        SetAllCollectiblesPositions();
    }
    public void CheckPointSaveGame()
    {
        _serializedSaveGame = new SerializedSaveGame();

        _serializedSaveGame.playerHP = _playerController.CurrentHp;
        _serializedSaveGame.playerMaxScore = _playerController.MaxScore;
        _serializedSaveGame.playerScore = _playerController.Score;
        _serializedSaveGame.maxEnemyCount = _playerController.MaxEnemyCount;
        _serializedSaveGame.enemyCount = _playerController.EnemyCount;
        _serializedSaveGame.lastCheckpointPosition = CheckPointPos;

        // Gather enemy data
        _serializedSaveGame.enemyPositions = GetAllEnemyPositions();
        _serializedSaveGame.enemyObjects = GetAllEnemyObjects();

        // Gather collectibles data
        _serializedSaveGame.collectiblesPositions = GetAllCollectiblesPositions();
        _serializedSaveGame.collectiblesObjects = GetAllCollectiblesObjects();

        // Gather volume data
        //_serializedSaveGame.volume = GetVolumeLevel();
        CheckPointSaveToJson();
    }
    public void CheckPointLoadGame()
    {
        CheckPointLoadFromJson();
        _playerController.CurrentHp = _serializedSaveGame.playerHP;
        _playerController.MaxScore = _serializedSaveGame.playerMaxScore;
        _playerController.Score = _serializedSaveGame.playerScore;
        _playerController.MaxEnemyCount = _serializedSaveGame.maxEnemyCount;
        _playerController.EnemyCount = _serializedSaveGame.enemyCount;
        _serializedSaveGame.lastCheckpointPosition = CheckPointPos;
        SetPlayer(_serializedSaveGame.lastCheckpointPosition);
        SetAllEnemyPositions();
        SetAllCollectiblesPositions();
    }
    public void SaveToJson()
    {
        string jsonString = JsonUtility.ToJson(_serializedSaveGame, true);
        File.WriteAllText(Application.persistentDataPath + SAVE_FILE_NAME, jsonString);
    }
    public void CheckPointSaveToJson()
    {
        string jsonString = JsonUtility.ToJson(_serializedSaveGame, true);
        File.WriteAllText(Application.persistentDataPath + CHECK_POINT_SAVE_FILE_NAME, jsonString);
    }
    public void LoadFromJson()
    {
        string jsonString = File.ReadAllText(Application.persistentDataPath + SAVE_FILE_NAME);
        _serializedSaveGame = JsonUtility.FromJson<SerializedSaveGame>(jsonString);
    }
    public void CheckPointLoadFromJson()
    {
        string jsonString = File.ReadAllText(Application.persistentDataPath + CHECK_POINT_SAVE_FILE_NAME);
        _serializedSaveGame = JsonUtility.FromJson<SerializedSaveGame>(jsonString);
    }
    public void SetPlayer(Vector3 position)
    {
        Instantiate(_playerObject, position, Quaternion.identity);
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
    public Vector3[] SetAllCheckPointsPositions()
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
    public GameObject[] GetAllCollectiblesObjects()
    {
        GameObject[] collectiblesObjects = new GameObject[_serializedSaveGame.collectiblesPositions.Length];

        for (int i = 0; i < _serializedSaveGame.collectiblesPositions.Length; i++)
        {
            collectiblesObjects = GameObject.FindGameObjectsWithTag("enemy");
        }

        return collectiblesObjects;

    }
    public void SetAllCollectiblesPositions()
    {
        for (int i = 0; i < _serializedSaveGame.collectiblesPositions.Length; i++)
        {
            Instantiate(_serializedSaveGame.collectiblesObjects[i], _serializedSaveGame.collectiblesPositions[i], Quaternion.identity);
        }
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
    public GameObject[] GetAllEnemyObjects()
    {
        GameObject[] enemyObjects = new GameObject[_serializedSaveGame.enemyPositions.Length];

        for (int i = 0; i < _serializedSaveGame.enemyPositions.Length; i++)
        {
            enemyObjects = GameObject.FindGameObjectsWithTag("enemy");
        }

        return enemyObjects;

    }
    public void SetAllEnemyPositions()
    {
        for (int i = 0; i < _serializedSaveGame.enemyPositions.Length; i++)
        {
            Instantiate( _serializedSaveGame.enemyObjects[i] , _serializedSaveGame.enemyPositions[i] , Quaternion.identity);
        }
    }
}
