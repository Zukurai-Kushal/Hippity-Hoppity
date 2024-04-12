using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinMenuScript : MonoBehaviour
{
    [SerializeField] private GameObject winMedal;
    [SerializeField] private Sprite [] medals;

    [SerializeField] private PlayerMovement playerMovement;

    [SerializeField] private UnityEngine.UI.Text scoreText;

    private int score;

    // Update is called once per frame
    void Update()
    {
        score = playerMovement.deathCounter;
        scoreText.text = "Score: "+score.ToString();
        if(score == 0)
        {
            winMedal.GetComponent<UnityEngine.UI.Image>().sprite = medals[0];
        }
        else if(score <= 50)
        {
            winMedal.GetComponent<UnityEngine.UI.Image>().sprite = medals[1];
        }
        else if(score <= 100)
        {
            winMedal.GetComponent<UnityEngine.UI.Image>().sprite = medals[2];
        }
        else if(score <= 300)
        {
            winMedal.GetComponent<UnityEngine.UI.Image>().sprite = medals[3];
        }
        else
        {
            winMedal.GetComponent<UnityEngine.UI.Image>().sprite = medals[4];
        }
    }
}
