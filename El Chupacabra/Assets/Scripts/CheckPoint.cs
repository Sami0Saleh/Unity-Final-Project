using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] GameObject RedSign;
    [SerializeField] GameObject BlueSign;
    [SerializeField] SaveNLoadJson SaveRef;

    private Vector3 respawnPosition;

    private void UpdateRespawnPosition(Transform checkpoint)
    {
        
        SaveRef.CheckPointPos = checkpoint.transform.position;
        SaveRef.SaveGame();
    }

    public void RespawnPlayer()
    {
        // Respawn the player at the last checkpoint's position
        transform.position = new Vector3(respawnPosition.x, respawnPosition.y, respawnPosition.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("CheckPoint");
            UpdateRespawnPosition(other.transform);
            RedSign.SetActive(true);
            BlueSign.SetActive(false);
        }
    }
}
