using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleGroundScript : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    private UnityEngine.Tilemaps.TilemapRenderer tilemapRenderer;

    void Start()
    {
        tilemapRenderer =  this.GetComponent<UnityEngine.Tilemaps.TilemapRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        if(playerMovement.movementState == PlayerMovement.MovementStates.squat || playerMovement.movementState == PlayerMovement.MovementStates.wallJump)
        {
            tilemapRenderer.enabled = false;
        }
        else
        {
            tilemapRenderer.enabled = true;
        }
    }
}
