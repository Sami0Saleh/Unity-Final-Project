using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsUI : MonoBehaviour
{
    [SerializeField] GameObject _mainMenu;



    public void ReturnButton()
    {
        gameObject.SetActive(false);
        _mainMenu.SetActive(true);
    }
}
