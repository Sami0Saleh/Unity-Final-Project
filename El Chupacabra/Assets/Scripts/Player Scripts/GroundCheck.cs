using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{

    [SerializeField] NewPlayerController controller;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            Debug.Log("Is Grounded");
            controller.IsGrounded = true;
            controller.IsFalling = false;
        }
    }
        
      
        
    
}
