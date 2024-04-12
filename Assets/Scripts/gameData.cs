using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class gameData
{
    public float [] respawnPoint;
    public float [] playerPosition;
    public int melonCounter;
    public int deathCounter;
    public int glassesLeft;
    public int shoesLeft;
    public int checkpointRespawnLeft;
    public HashSet<int> unlockedItems = new HashSet<int>(); 
    public gameData(PlayerMovement playerMovement, ItemManager itemManager)
    {
        respawnPoint = new float[3] {playerMovement.respawnPoint.position.x, playerMovement.respawnPoint.position.y, playerMovement.respawnPoint.position.z};
        playerPosition = new float[3] {playerMovement.transform.position.x,playerMovement.transform.position.y,playerMovement.transform.position.z};
        melonCounter = itemManager.melonCounter;
        deathCounter = playerMovement.deathCounter;
        unlockedItems = itemManager.unlockedItems;
        glassesLeft = itemManager.glassesLeft;
        shoesLeft = itemManager.shoesLeft;
        checkpointRespawnLeft = itemManager.checkpointRespawnLeft;
    }
}
