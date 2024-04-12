using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkpointScript : MonoBehaviour
{
    [SerializeField] int itemIndex = 0;
    private Animator checkPointAnimator;
    private ItemManager itemManager;
    private void Start()
    {
        itemManager = FindObjectOfType<PlayerMovement>().GetComponent<ItemManager>();  
        this.checkPointAnimator = GetComponent<Animator>();
        if(itemManager.unlockedItems.Contains(itemIndex))
        {
            checkPointAnimator.SetTrigger("activateCheckpoint");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && itemManager.unlockedItems.Contains(itemIndex) == false)
        {
            FindObjectOfType<AudioManager>().Play("Trumpet");
            checkPointAnimator.SetTrigger("activateCheckpoint");
            itemManager.unlockItem(itemIndex);
            //collision.gameObject.GetComponent<PlayerMovement>().respawnPoint.position = this.transform.position;
        }
    }
}
