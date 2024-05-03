using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadUI : MonoBehaviour
{
    [SerializeField] GameObject _mainMenu;



    public void ReturnButton()
    {
        gameObject.SetActive(false);
        _mainMenu.SetActive(true);
    }
}
