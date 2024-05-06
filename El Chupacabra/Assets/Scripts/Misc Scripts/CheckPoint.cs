using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] MeshRenderer _redSign;
    [SerializeField] MeshRenderer _blueSign;
    [SerializeField] SaveNLoadJson _saveRef;
    private Vector3 _respawnPosition;
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
            _redSign.enabled = false;
            _blueSign.enabled = true;
            UpdateRespawnPosition(other.transform);
            
        }
    }
    
}
