using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public int melonCounter = 0;
    public HashSet<int> unlockedItems = new HashSet<int>();
    public int glassesLeft = 0;
    public int shoesLeft = 0;
    public int checkpointRespawnLeft = 0;
    [SerializeField] private UnityEngine.UI.Text melonCounterText;
    [SerializeField] private GameObject glassesCounterDisplay;
    [SerializeField] private GameObject glassesItemObject;
    [SerializeField] private GameObject shoesCounterDisplay;
    [SerializeField] private GameObject checkpointCounterDisplay;
    [SerializeField] private PlayerMovement playerMovement;

    [SerializeField] private Transform [] checkpointTransforms; 

    private void Awake()
    {
        unlockItem(0);
        unlockItem(1);
    }

    private void Update()
    {
        updateItemUI();
    } 
    public void updateItemUI()
    {
        melonCounterText.text = "Melon Counter: " + melonCounter;

        if(glassesLeft > 0)
        {
            glassesCounterDisplay.GetComponent<UnityEngine.UI.Text>().text = "x "+glassesLeft.ToString();
            glassesCounterDisplay.SetActive(true);
            glassesItemObject.SetActive(true);
        }
        else
        {
            glassesCounterDisplay.SetActive(false);
            glassesItemObject.SetActive(false);
        }

        if(shoesLeft > 0)
        {
            shoesCounterDisplay.GetComponent<UnityEngine.UI.Text>().text = "x "+shoesLeft.ToString();
            shoesCounterDisplay.SetActive(true);
        }
        else
        {
            shoesCounterDisplay.SetActive(false);
        }

        if(checkpointRespawnLeft > 0)
        {
            checkpointCounterDisplay.GetComponent<UnityEngine.UI.Text>().text = "x "+checkpointRespawnLeft.ToString();
            checkpointCounterDisplay.SetActive(true);
        }
        else
        {
            checkpointCounterDisplay.SetActive(false);
            playerMovement.respawnPoint.transform.position = checkpointTransforms[0].position;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MelonTag"))
        {
            melonCounter++;
            FindObjectOfType<AudioManager>().Play("Pop");
            collision.gameObject.GetComponent<Animator>().SetTrigger("melonCollected");
        }

        updateItemUI();
    }

    public void buyGlasses()
    {
        if(useMelons(50) == true)
        {
            glassesLeft+=5;
        }
    }

    public void buyShoes()
    {
        if(useMelons(75) == true)
        {
            shoesLeft+=5;
        }
    }

    public void buyCheckpointRespawn(int checkPointNumber)
    {
        switch(checkPointNumber)
        {
            case 1:
                if(useMelons(10) == true)
                {
                    if(playerMovement.respawnPoint.transform.position == checkpointTransforms[checkPointNumber].position)
                    {
                        checkpointRespawnLeft += 5;
                    }
                    else
                    {
                        checkpointRespawnLeft = 5;
                        playerMovement.respawnPoint.transform.position = checkpointTransforms[checkPointNumber].position;
                    }
                }
                break;
            case 2:
                if(useMelons(25) == true)
                {
                    if(playerMovement.respawnPoint.transform.position == checkpointTransforms[checkPointNumber].position)
                    {
                        checkpointRespawnLeft += 5;
                    }
                    else
                    {
                        checkpointRespawnLeft = 5;
                        playerMovement.respawnPoint.transform.position = checkpointTransforms[checkPointNumber].position;
                    }
                }
                break;
            case 3:
                if(useMelons(50) == true)
                {
                    if(playerMovement.respawnPoint.transform.position == checkpointTransforms[checkPointNumber].position)
                    {
                        checkpointRespawnLeft += 5;
                    }
                    else
                    {
                        checkpointRespawnLeft = 5;
                        playerMovement.respawnPoint.transform.position = checkpointTransforms[checkPointNumber].position;
                    }
                }
                break;
            case 4:
                if(useMelons(75) == true)
                {
                    if(playerMovement.respawnPoint.transform.position == checkpointTransforms[checkPointNumber].position)
                    {
                        checkpointRespawnLeft += 5;
                    }
                    else
                    {
                        checkpointRespawnLeft = 5;
                        playerMovement.respawnPoint.transform.position = checkpointTransforms[checkPointNumber].position;
                    }
                }
                break;
        }
    }


    public bool useMelons(int itemCost)
    {
        if(this.melonCounter >= itemCost)
        {
            this.melonCounter -= itemCost;
            FindObjectOfType<AudioManager>().Play("Money");
            //Debug.Log("Buying Stuff");
            return true;
        }
        else
        {
            FindObjectOfType<AudioManager>().Play("Error");
            return false;
        }
    }

    public void addMelons()
    {
        melonCounter += 50;
    }

    public void unlockItem(int itemIndex)
    {
        unlockedItems.Add(itemIndex);
    }

    public void deductItems()
    {
        if(glassesLeft > 0)
        {
            glassesLeft--;
        }
        if(shoesLeft > 0)
        {
            shoesLeft--;
        }
        if(checkpointRespawnLeft > 0)
        {
            checkpointRespawnLeft--;
        }
    }

}
