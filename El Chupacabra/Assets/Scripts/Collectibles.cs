using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectibles : MonoBehaviour
{
    [SerializeField] NewPlayerController _playerController;
    [SerializeField] GameObject collectEffect;
    [SerializeField] float rotationSpeed;

    // Use this for initialization
    void Start()
    {
        _playerController.MaxScore ++;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Collect();
        }
    }

    public void Collect()
    {
        if (collectEffect)
            Instantiate(collectEffect, transform.position, Quaternion.identity);
        _playerController.Score ++;
        _playerController.UpdateScore();
        Destroy(gameObject);
    }
}
