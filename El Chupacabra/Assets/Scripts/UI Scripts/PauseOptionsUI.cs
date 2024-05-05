using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseOptionsUI : MonoBehaviour
{
    [SerializeField] GameObject _pause;



    public void ReturnButton()
    {
        gameObject.SetActive(false);
        _pause.SetActive(true);
    }
}
