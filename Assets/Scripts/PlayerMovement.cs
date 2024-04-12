using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{

    // Sound Variable
    // [SerializeField] private AudioSource jump_s; // jump sound
    private float jumpCharge = 0f;
    //Charge Bar Vars
    public GameObject CharbarObj;
    public GameObject CharBarHolder;
    [SerializeField] private SpriteRenderer leftWallIndicator;
    [SerializeField] private SpriteRenderer rightWallIndicator;
    [SerializeField] private SpriteRenderer groundIndicator;
    private float jumpPower;
    [SerializeField] private float maxJumpHoldTime = 1.0f;
    private string[] groundTypes = {"Ground", "SlipperyGround", "SlimWallGround"};
    private string belowPlayer;
    private string rightOfPlayer;
    private string leftOfPlayer;
    public Transform respawnPoint;
    public Rigidbody2D playerRb;
    private BoxCollider2D playerCollider;
    private SpriteRenderer playerSprite;
    private Animator playerAnimaiton;
    [SerializeField] private float minJumpVelocity = 5;
    [SerializeField] private float maxJumpVelocity = 18;
    [SerializeField] private float maxFallVelocity = 35;
    public enum MovementStates {idle, squat, jump, fall, wallJump};
    public MovementStates movementState = MovementStates.idle;
    public int deathCounter = 0;
    [SerializeField] private UnityEngine.UI.Text deathCounterText;
    private float originalGravityScaleValue;
    private ItemManager itemManager;
    private bool isFacingRight = true;
    private bool isFacingUp = false;
    // Start is called before the first frame update
    private void Awake()
    {
        this.playerRb = GetComponent<Rigidbody2D>();
        this.playerSprite = GetComponent<SpriteRenderer>();
        this.playerAnimaiton = GetComponent<Animator>();
        this.playerCollider = GetComponent<BoxCollider2D>();
        this.itemManager = GetComponent<ItemManager>();
        this.originalGravityScaleValue = this.playerRb.gravityScale;

        loadData();
    }

    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        checkPlayerSurrounding();
        handlePlayerInputs();
        updateMovementAnimation();
        updateJumpCharge();
        //AddFullFriction();
        //LimitFallingSpeed();
        updateIndicatorsForSurrounding();
        if(itemManager.shoesLeft > 0)
        {
            AddFullFriction();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) //player dies if it collides with any DeathTag
    {
        if(collision.gameObject.CompareTag("DeathTag"))
        {
            die();
        }
    }

    private void checkPlayerSurrounding()
    {
        belowPlayer = checkGroundBelow();
        rightOfPlayer = checkRightWall();
        leftOfPlayer = checkLeftWall();
    }

    private void handlePlayerInputs()
    {
        if(PauseMenuScript.gameIsPaused == true)
        {
            return;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if(leftOfPlayer != null)
            {
                switch(leftOfPlayer)
                {
                    case "Ground":
                        this.playerRb.gravityScale = 0.0f;
                        goto case "rightJumpSetup";

                    case "SlipperyGround":
                        if(itemManager.shoesLeft > 0)
                        {
                            goto case "Ground";
                        }
                        this.playerRb.gravityScale = 0.5f;
                        goto case "rightJumpSetup";

                    case "SlimWallGround":
                        break;

                    case "rightJumpSetup":
                        this.playerRb.velocity = new Vector2(0,0);
                        float nearestWallPosX = Mathf.RoundToInt(this.transform.position.x - 0.5f) + 0.5f;
                        this.transform.position = new Vector3(nearestWallPosX, this.transform.position.y, this.transform.position.z);
                        //this.playerRb.bodyType = RigidbodyType2D.Static;
                        //this.playerSprite.flipX = false;
                        lookRight();
                        FindObjectOfType<AudioManager>().Play("Latch");
                        movementState = MovementStates.wallJump;
                        break;

                    default:
                        print("Error at handlePlayerInputs(), Unaccounted groundType at leftOfPlayer: " + leftOfPlayer);
                        break;
                }
            }
            else if(rightOfPlayer != null)
            {
                switch(rightOfPlayer)
                {
                    case "Ground":
                        this.playerRb.gravityScale = 0.0f;
                        goto case "leftJumpSetup";

                    case "SlipperyGround":
                        if(itemManager.shoesLeft > 0)
                        {
                            goto case "Ground";
                        }
                        this.playerRb.gravityScale = 0.5f;
                        goto case "leftJumpSetup";

                    case "SlimWallGround":
                        break;
                    
                    case "leftJumpSetup":
                        this.playerRb.velocity = new Vector2(0,0);
                        float nearestWallPosX = Mathf.RoundToInt(this.transform.position.x - 0.5f) + 0.5f;
                        this.transform.position = new Vector3(nearestWallPosX, this.transform.position.y, this.transform.position.z);
                        //this.playerRb.bodyType = RigidbodyType2D.Static;
                        //this.playerSprite.flipX = true;
                        lookLeft();
                        FindObjectOfType<AudioManager>().Play("Latch");
                        movementState = MovementStates.wallJump;
                        break;

                    default:
                        print("Error at handlePlayerInputs(), Unaccounted groundType at rightOfPlayer: " + rightOfPlayer);
                        break;
                }
            }
            else if(belowPlayer != null)
            {
                switch(belowPlayer)
                {
                    case "SlimWallGround":
                        goto case "Ground";

                    case "Ground":
                        this.playerRb.velocity = new Vector2(0,0);
                        this.playerRb.gravityScale = 0.0f;
                        goto case "groundJumpSetup";

                    case "SlipperyGround":
                        //this.playerRb.velocity = new Vector2(this.playerRb.velocity.x, 0);
                        //this.playerRb.gravityScale = 0.0f;
                        if(itemManager.shoesLeft > 0)
                        {
                            goto case "Ground";
                        }
                        goto case "groundJumpSetup";
                    
                    case "groundJumpSetup":
                        if(movementState == MovementStates.fall)
                        {
                            FindObjectOfType<AudioManager>().Play("Landing");
                        }
                        movementState = MovementStates.squat; 
                        break;

                    default:
                        print("Error at handlePlayerInputs(), Unaccounted groundType at belowPlayer: " + belowPlayer);
                        break;
                }
            }
        }
        else if(Input.GetKeyDown(KeyCode.Q) && (movementState == MovementStates.squat || movementState == MovementStates.wallJump))
        {
            this.playerRb.bodyType = RigidbodyType2D.Dynamic;
            this.playerRb.gravityScale = originalGravityScaleValue;
            movementState = MovementStates.idle;
        }
        else if(Input.GetButtonUp("Jump"))
        {
            if (movementState == MovementStates.squat || movementState == MovementStates.wallJump)
            {
                this.Jump();
            }
        }
        else if(Input.GetKeyDown(KeyCode.A) && movementState != MovementStates.wallJump)
        {
            //this.playerSprite.flipX = true;
            lookLeft();
        }
        else if(Input.GetKeyDown(KeyCode.D) && movementState != MovementStates.wallJump)
        {
            //this.playerSprite.flipX = false;
            lookRight();
        }
        else if(Input.GetKeyDown(KeyCode.R))
        {
            die();
        }

        if(Input.GetKey(KeyCode.W) && movementState != MovementStates.wallJump)
        {
            isFacingUp = true;
        }
        else
        {
            isFacingUp = false;
        }
    }

    private void updateMovementAnimation()
    {

        if (this.playerRb.velocity.y > .1f)
        {
            movementState = MovementStates.jump;
        }
        else if(this.playerRb.velocity.y < -.1f)
        {
            if(movementState != MovementStates.wallJump)
            {
                movementState = MovementStates.fall;
                this.playerRb.gravityScale = originalGravityScaleValue;
            }
            else if(movementState == MovementStates.wallJump && checkRightWall() == null && checkLeftWall() == null)
            {
                movementState = MovementStates.fall;
                this.playerRb.gravityScale = originalGravityScaleValue;
            } 
        }
        else if(movementState == MovementStates.fall)
        {
            FindObjectOfType<AudioManager>().Play("Landing");
            movementState = MovementStates.idle;
        }

        this.playerAnimaiton.SetInteger("state", (int)this.movementState);
    }
    
    private void updateJumpCharge()
    {
        if(movementState == MovementStates.squat || movementState == MovementStates.wallJump)
        {
            jumpCharge = (jumpCharge += Time.deltaTime)>maxJumpHoldTime? maxJumpHoldTime : jumpCharge += Time.deltaTime;
        }
        else
        {
            jumpCharge = 0.0f;
        }
        CharbarObj.transform.localScale = new Vector3(jumpCharge/maxJumpHoldTime, 1, 1);
    }

    private void Jump()
    {
        // jump_s.Play();
        this.playerRb.bodyType = RigidbodyType2D.Dynamic;
        this.playerRb.gravityScale = originalGravityScaleValue;
        FindObjectOfType<AudioManager>().Play("Jump");
        float verticalVelocity = Mathf.Max(minJumpVelocity, jumpCharge*maxJumpVelocity / maxJumpHoldTime);
        float horizontalVelocity = Mathf.Max(minJumpVelocity/2, jumpCharge*maxJumpVelocity/(2* maxJumpHoldTime));
        
        //UnityEngine.Debug.Log("Spacebar held for" + this.jumpCharge + " and Jump Power:" + verticalVelocity);
        if(isFacingUp == true)
        {
            this.playerRb.velocity = new Vector2(0,verticalVelocity);
        }
        else if(isFacingRight == false)
        {
            //print("Jumping Left, gameObject.transform.localScale.x: "+ gameObject.transform.localScale.x);
            //horizontalVelocity = (this.playerRb.velocity.x < 0)? this.playerRb.velocity.x - horizontalVelocity : - horizontalVelocity;
            this.playerRb.velocity = new Vector2(-horizontalVelocity,verticalVelocity);
        }
        else if(isFacingRight == true)
        {
            //print("Jumping Right, gameObject.transform.localScale.x: "+ gameObject.transform.localScale.x);
            //horizontalVelocity = (this.playerRb.velocity.x > 0)? this.playerRb.velocity.x + horizontalVelocity : horizontalVelocity;
            this.playerRb.velocity = new Vector2(horizontalVelocity,verticalVelocity);
        }
    }

    public void AddFullFriction()
    {
        if(checkGroundBelow()!=null && movementState == MovementStates.fall)
        {
            this.playerRb.velocity = new Vector2(0, playerRb.velocity.y);
        }
    }

    private string checkGroundBelow()   //returns what type of ground is under the player, returns null if no ground
    {
        foreach (string groundType in groundTypes)
        {
            if(Physics2D.BoxCast(playerCollider.bounds.center, new Vector2 (playerCollider.bounds.size.x, playerCollider.bounds.size.y - 0.5f), 0f, Vector2.down, .41f, LayerMask.GetMask(groundType))){
                //print("Checking groundType: "+groundType+" Is Present");
                return groundType;
            }
            //print("Checking groundType: "+groundType+" Not Present");
        }
        return null;
    }

    private string checkRightWall()   //returns what type of ground is on the right of the player, returns null if no ground
    {
        foreach (string groundType in groundTypes)
        {
            if(Physics2D.BoxCast(playerCollider.bounds.center, new Vector2 (playerCollider.bounds.size.x - .5f, playerCollider.bounds.size.y - 0.5f), 0f, Vector2.right, .65f, LayerMask.GetMask(groundType))){
                return groundType;
            }
        }
        return null;
    }

    private string checkLeftWall()
    {
        foreach (string groundType in groundTypes)
        {
            if(Physics2D.BoxCast(playerCollider.bounds.center, new Vector2 (playerCollider.bounds.size.x - .5f, playerCollider.bounds.size.y -0.5f), 0f, Vector2.left, .65f, LayerMask.GetMask(groundType))){
                return groundType;
            }
        }
        return null;
    }

    private void LimitFallingSpeed()
    {
        if (playerRb.velocity.y < -maxFallVelocity)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x,-maxFallVelocity);
        }
    }

    private void die()
    {
        FindObjectOfType<AudioManager>().Play("Death");
        deathCounter++;
        playerRb.bodyType = RigidbodyType2D.Static;
        playerAnimaiton.SetTrigger("death");
    }

    private void loadData()
    {
        //load data from file
        gameData data = SaveSystem.loadGame();
        if(data != null)
        {
            this.respawnPoint.position = new Vector3(data.respawnPoint[0],data.respawnPoint[1],data.respawnPoint[2]);
            this.transform.position = new Vector3(data.playerPosition[0],data.playerPosition[1],data.playerPosition[2]);
            this.deathCounter = data.deathCounter;
            itemManager.melonCounter = data.melonCounter;
            itemManager.unlockedItems = data.unlockedItems;
            itemManager.glassesLeft = data.glassesLeft;
            itemManager.shoesLeft = data.shoesLeft;
            itemManager.checkpointRespawnLeft = data.checkpointRespawnLeft;
        }
        
        //use loaded data
        deathCounterText.text = "Death Counter: " + deathCounter;
    }

    public void saveData()
    {
        //save data to file
        SaveSystem.saveGame(this, itemManager);
    }

    private void restartLevel()
    {
        this.transform.position = respawnPoint.position;
        itemManager.deductItems();
        saveData();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void updateIndicatorsForSurrounding()
    {
        leftWallIndicator.color = getIndicatorColor(leftOfPlayer);
        rightWallIndicator.color = getIndicatorColor(rightOfPlayer);
        groundIndicator.color = getIndicatorColor(belowPlayer);
    }

    private Color getIndicatorColor(string groundType)
    {
        if(groundType == null)
        {
            Color transparentWhite = Color.white;
            transparentWhite.a = 0.2f;
            return transparentWhite;
        }
        else if(groundType == "Ground")
        {
            return Color.green;
        }
        else if(groundType == "SlipperyGround")
        {
            return Color.cyan;
        }
        else if(groundType == "SlimWallGround")
        {
            return Color.yellow;
        }
        else
        {
            print("Unaccounted Groung Type: "+groundType);
            return Color.red;
        }
    }

    private void lookLeft()
    {
        isFacingRight = false;
        gameObject.transform.localScale = new Vector3(-1, 1, 1); //Edited
        CharBarHolder.gameObject.transform.localScale = new Vector3(-1, 1, 1); //Charge bar scale correction
        leftWallIndicator.transform.localPosition = new Vector3(1.5f, 0.5f, 0);
        rightWallIndicator.transform.localPosition = new Vector3(-1.5f, 0.5f, 0);
        //print("Turning Left, gameObject.transform.localScale.x: "+ gameObject.transform.localScale.x);
    }

    private void lookRight()
    {
        isFacingRight = true;
        gameObject.transform.localScale = new Vector3(1, 1, 1); //Edited
        CharBarHolder.gameObject.transform.localScale = new Vector3(1, 1, 1); //Charge bar scale correction
        leftWallIndicator.transform.localPosition = new Vector3(-1.5f, 0.5f, 0);
        rightWallIndicator.transform.localPosition = new Vector3(1.5f, 0.5f, 0);
        //print("Turning Right, gameObject.transform.localScale.x: "+ gameObject.transform.localScale.x);
    }

}
