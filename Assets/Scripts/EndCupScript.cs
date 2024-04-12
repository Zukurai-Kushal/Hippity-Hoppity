using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCupScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private PauseMenuScript pauseMenuScript;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            pauseMenuScript.OpenWinMenu();
        }
    }
}
