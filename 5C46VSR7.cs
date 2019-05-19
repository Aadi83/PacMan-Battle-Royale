using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacmanMovementScript : MonoBehaviour
{

    //----------
    //  FLOATS
    //----------

    [Header("How fast Pacman goes")]
    [SerializeField]
    public float Speed = 0f;

    [Header("How much Pacman's speed changes when he eats a pellet")]
    [SerializeField]
    public float SpeedChange = 0f;

    [Header("How long pacman stays powered up (seconds)")]
    [SerializeField]
    public float PoweredUpTime = 0f;

    //-------------
    //  KEYCODES
    //-------------

    [Header("Key that makes Pacman go left")]
    [SerializeField]
    public KeyCode LeftKey = KeyCode.A;

    [Header("Key that makes Pacman go right")]
    [SerializeField]
    public KeyCode RightKey = KeyCode.D;

    [Header("Key that makes Pacman go up")]
    [SerializeField]
    public KeyCode UpKey = KeyCode.W;

    [Header("Key that makes Pacman go down")]
    [SerializeField]
    public KeyCode DownKey = KeyCode.S;

    //------------------
    // PRIVATE VALUES
    //------------------

    /// <summary>
    /// Points Pacman has. More points = more speed
    /// </summary>
    public int points = 0;

    /// <summary>
    /// How much should the x position value change?
    /// </summary>
    private float xChange = 0f;
    /// <summary>
    /// How much should the y position value change
    /// </summary>
    private float yChange = 0f;

    /// <summary>
    /// Is this gameObject powered up by the power pellet?
    /// </summary>
    public bool isPowered = false;

    //-----------
    // METHODS
    //-----------

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //This code determines how pacman moves based on what keys are pressed.
        if (Input.GetKeyDown(LeftKey) && transform.position.x > -1.55f)
        {
            //Set the xChange value to the negative of the public speed var
            xChange = -Speed;
            //Reset yChange value
            yChange = 0f;

            //Rotate pacman so that he is facing the left
            transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        }
        else if (Input.GetKeyDown(RightKey) && transform.position.x < 1.55f)
        {
            //Set the xChange value to the public speed var
            xChange = Speed;
            //Reset yChange value
            yChange = 0f;

            //Rotate pacman so that he is facing the right
            transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
        }
        else if (Input.GetKeyDown(DownKey) && transform.position.y > -0.7)
        {
            //Set the yChange value to the negative of the speed value. 
            yChange = -Speed;
            //Reset xChange value
            xChange = 0f;

            //Rotate pacman so that he is facing downards
            transform.eulerAngles = Vector3.forward * 90f;
        }
        else if (Input.GetKeyDown(UpKey) && transform.position.y < 0.7)
        {
            //Set the yChange value to the speed value
            yChange = Speed;
            //Reset xChange value
            xChange = 0f;

            //Rotate pacman so that he is facing upwards
            transform.eulerAngles = Vector3.forward * 270f;
        }

        float xPos = transform.position.x + xChange > -1.55 && transform.position.x + xChange < 1.55f ? transform.position.x + xChange : transform.position.x;
        float yPos = transform.position.y + yChange > -0.7 && transform.position.y + yChange < 0.7f ? transform.position.y + yChange : transform.position.y;

        //Apply the change values to the actual positon
        transform.position = new Vector2(xPos, yPos);

        print(isPowered);
    }

    /// <summary>
    /// This method activates when something collides with our game object
    /// </summary>
    /// <param name="collision">The collider that collided with our game object</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If the gameobject is a pellet...
        if (collision.gameObject.name.Contains("Pellet"))
        {
            //Add 1 point
            points++;

            //Destroy the pellet
            Destroy(collision.gameObject);

            //Increase our speed by a bit.
            Speed += SpeedChange;

            xChange = xChange > 0 ? Speed : xChange < 0 ? -Speed : 0;
            yChange = yChange > 0 ? Speed : yChange < 0 ? -Speed : 0;
        }
        //If the gameobject is a player AND the collsion gameObject is powered up AND this gameObject is not powered up...
        else if (collision.gameObject.name.Contains("Player") && collision.gameObject.GetComponent<PacmanMovementScript>().isPowered && !isPowered)
        {
            //Destory this gameObject!
            Destroy(gameObject);
        }
        //If the gameobject is a power pellet.
        else if (collision.gameObject.name.Contains("PowerUp"))
        {
            //Delete the power pellet
            Destroy(collision.gameObject);

            //Active the DeactivePoweredUp method, which will enable in 3 seconds
            Invoke("DeactivatePoweredUp", PoweredUpTime);

            //Set isPowered to true
            isPowered = true;

            //Multiply speed by 1.25
            Speed *= 1.25f;

            xChange = xChange > 0 ? Speed : xChange < 0 ? -Speed : 0;
            yChange = yChange > 0 ? Speed : yChange < 0 ? -Speed : 0;
        }

    
    }

    /// <summary>
    /// This method reverts pacman back to normal speed.
    /// </summary>
    /// <param name="previousSpeed">Speed pacman was before powering up</param>
    /// <param name="delayTime">How many seconds pacman is powered up (seconds)</param>
    /// <returns></returns>
    public void DeactivatePoweredUp()
    {
        //Set isPowered to false
        isPowered = false;

        //Set the speed to the pevious speed
        Speed /= 1.25f;

        xChange = xChange > 0 ? Speed : xChange < 0 ? -Speed : 0;
        yChange = yChange > 0 ? Speed : yChange < 0 ? -Speed : 0;
    }
}