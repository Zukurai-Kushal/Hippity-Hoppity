using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    // Update is called once per frame
    [SerializeField] private float speed = 2f;
    [SerializeField] private bool backAndForthMode = false;
    [SerializeField] private float rotationTime = 1f;
    public int rotateDirection = 1; 
    private float originalRotationTime;

    private void Start()
    {
        originalRotationTime = rotationTime;
    }
    private void Update()
    {
        if(backAndForthMode)
        {
            if(rotationTime <= 0)
            {
                rotateDirection *= -1;
                rotationTime = originalRotationTime;
            }
            else
            {
                rotationTime -= Time.deltaTime;
            }

            transform.Rotate(0f, 0f, Time.deltaTime * speed * rotateDirection);
        }
        else{
            transform.Rotate(0f, 0f, 360 * Time.deltaTime * speed);
        }
    }
}
