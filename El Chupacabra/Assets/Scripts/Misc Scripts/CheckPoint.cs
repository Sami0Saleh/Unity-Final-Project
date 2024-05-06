using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] GameObject _redSign;
    [SerializeField] GameObject _blueSign;
    [SerializeField] SaveNLoadJson _saveRef;
    private Vector3 _respawnPosition;

    private void Start()
    {
        Debug.Log("It's Alive");
    }
    private void UpdateRespawnPosition(Transform checkpoint)
    {
        _saveRef.CheckPointPos = checkpoint.transform.position;
        _saveRef.SaveGame();
    }

    public void RespawnPlayer()
    {
        // Respawn the player at the last checkpoint's position
        transform.position = new Vector3(_respawnPosition.x, _respawnPosition.y, _respawnPosition.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("CheckPoint");
            UpdateRespawnPosition(other.transform);
            _redSign.SetActive(true);
            _blueSign.SetActive(false);
        }
    }
    
}
