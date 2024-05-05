using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMainMenuUI : MonoBehaviour
{
    [SerializeField] GameObject _pause;
    public void YesButton()
    {
        SceneManager.LoadScene(0);
    }
    public void NoButton()
    {
        gameObject.SetActive(false);
        _pause.SetActive(true);
    }
}
