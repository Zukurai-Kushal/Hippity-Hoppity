using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuScript : MonoBehaviour
{
    public GameObject deleteGameDataButton;
    public UnityEngine.UI.Text playGameText;
    // Start is called before the first frame update
    void Start()
    {
        if(SaveSystem.loadGame() == null)
        {
            playGameText.text = "START NEW GAME";
            deleteGameDataButton.SetActive(false);
        }
        else
        {
            playGameText.text = "CONTINUE GAME";
            deleteGameDataButton.SetActive(true);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void DeletGameData()
    {
        SaveSystem.deleteGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
