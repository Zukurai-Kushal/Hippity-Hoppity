using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trollTriger : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        this.GetComponent<SpriteRenderer>().enabled = true;
    }
}
