using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializedSaveGame
{
    public int playerHP;
    public int playerMaxScore;
    public int playerScore;
    public int enemyCount;
    public int maxEnemyCount;
    public Vector3 playerPosition;
    public Vector3 lastCheckpointPosition;
    public Vector3[] CheckpointPosition;
    public Vector3[] collectiblesPositions;
    public Vector3[] enemyPositions;
    public GameObject[] enemyObjects;
    public GameObject[] collectiblesObjects;
    public float volume;
}