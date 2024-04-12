using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    float cameraSize;
    void Start()
    {
        cameraSize = this.GetComponent<Camera>().orthographicSize;
        this.GetComponentInChildren<SpriteRenderer>().enabled = false;
        //print("cameraSize:"+cameraSize);
    }
    // Update is called once per frame
    void Update()
    {
        if(playerTransform.position.y > this.transform.position.y + cameraSize)
        {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 2*cameraSize, this.transform.position.z);
        }
        else if(playerTransform.position.y < this.transform.position.y - cameraSize)
        {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 2*cameraSize, this.transform.position.z);
        }
    }
}
