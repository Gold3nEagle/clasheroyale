using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    bool toggleMenu = false;
    AudioSource audioSource;


    [SerializeField]
    GameObject menuPanel;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void LoadScene(int sceneId)
    {
        SceneManager.LoadScene(sceneId);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ToggleMenu()
    {
        toggleMenu = !toggleMenu;
        menuPanel.SetActive(toggleMenu);

        if (toggleMenu == true)
        {
            audioSource.Pause();
            Time.timeScale = 0f;
        }
        else
        {
            audioSource.Play();
            Time.timeScale = 1f;
        }
    }
}
