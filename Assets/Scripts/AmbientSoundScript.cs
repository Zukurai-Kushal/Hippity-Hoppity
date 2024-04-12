using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AmbientSoundScript : MonoBehaviour
{
    //[SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject [] zoneRange = new GameObject [2];
    private void Update()
    {
        if(zoneRange.Length == 2)
        {
            if(PauseMenuScript.gameIsPaused == false && Camera.main.transform.position.y - Camera.main.orthographicSize < zoneRange[0].transform.position.y && Camera.main.transform.position.y + Camera.main.orthographicSize > zoneRange[1].transform.position.y)
            {
                if(this.GetComponent<AudioSource>().isPlaying == false)
                {
                    this.GetComponent<AudioSource>().Play();
                }
            }
            else
            {
                //print("mainCamera.transform.position.y");
                this.GetComponent<AudioSource>().Pause();
            }
        }
    }
}
