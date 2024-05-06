using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float _speed;
    void Update()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            NewPlayerController newPlayerController = other.GetComponent<NewPlayerController>();
            if (newPlayerController != null)
            {
                newPlayerController.TakeDamage();
            }
        }
        Destroy(gameObject);
    }
}
