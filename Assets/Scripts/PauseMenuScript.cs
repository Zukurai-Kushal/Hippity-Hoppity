using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    public static bool gameIsPaused = false;
    // Start is called before the first frame update
    public GameObject pauseMenuUI;
    public GameObject inGameUI;
    public GameObject shopMenuUI;
    public GameObject WinMenuUI;
    [SerializeField] PlayerMovement playerMovement;
    void Update()
    {
        if(Input.GetButtonDown("Cancel")) //Esc Key
        {
            if(gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        inGameUI.SetActive(true);
        shopMenuUI.SetActive(false);
        WinMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
        FindObjectOfType<AudioManager>().ResumeSound();
    }
    
    void Pause()
    {
        FindObjectOfType<AudioManager>().Play("Pause");
        inGameUI.SetActive(false);
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
        //FindObjectOfType<AudioManager>().PauseSound();
    }

    public void LoadMainMenu()
    {
        playerMovement.saveData();
        Resume();
        SceneManager.LoadScene(0);
    }

    public void LoadShopMenu()
    {
        shopMenuUI.SetActive(true);
        shopMenuUI.GetComponent<ShopUIScript>().showItems();
        Debug.Log("Loading Shop Menu");
    }

    public void QuitGame()
    {
        playerMovement.saveData();
        Debug.Log("Quiting Game");
        Application.Quit();
    }

    public void CloseShopMenu()
    {
        shopMenuUI.SetActive(false);
    }

    public void OpenWinMenu()
    {
        FindObjectOfType<AudioManager>().Play("Win");
        inGameUI.SetActive(false);
        WinMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }
    public void CloseWinMenu()
    {
        playerMovement.saveData();
        Resume();
        SceneManager.LoadScene(0);
    }
}
