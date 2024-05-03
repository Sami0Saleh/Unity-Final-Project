using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] GameObject _load;
    [SerializeField] GameObject _options;
    [SerializeField] GameObject _credits;
    public void NewGameButton()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(1);
    }
    public void LoadButton()
    {
        gameObject.SetActive(false);
        _load.SetActive(true);
    }
    public void OptionsButton()
    {
        gameObject.SetActive(false);
        _options.SetActive(true);
    }
    public void CreditsButton()
    {
        gameObject.SetActive(false);
        _credits.SetActive(true);
    }
    public void QuitGameButton()
    {
        Application.Quit();
    }
}
