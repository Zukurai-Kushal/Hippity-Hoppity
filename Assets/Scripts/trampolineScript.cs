using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trampolineScript : MonoBehaviour
{
    private Animator trampolineAnimation;
    // Start is called before the first frame update
    void Start()
    {
        trampolineAnimation = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            trampolineAnimation.SetTrigger("jumping");
            FindObjectOfType<AudioManager>().Play("Boing");
        }
    }
}
