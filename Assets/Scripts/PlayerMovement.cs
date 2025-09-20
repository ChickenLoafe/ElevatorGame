using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Unity.Mathematics;

public class PlayerMovement : MonoBehaviour
{
    [Header("Settings")]
    
    public InputAction sprintAction; //part of the InputAction package
    private Rigidbody rb; //responsible for the physics
    private Vector2 input; //stores x + y inputs
    private Vector3 movement; //stores the information between fixed and not fixed updates

    [Header("Stats")]

    public float moveSpeed;  //the default move speed
    public float stamina; //the player's stamina
    private float maxStamina;
    public float staminaDrain;
    public float staminaRegen;
    public float sprintMultiplier;


    [Header("UI")]

    public TextMeshProUGUI staminaDisplay;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sprintAction = InputSystem.actions.FindAction("Sprint");

        rb = GetComponent<Rigidbody>(); //gets the rigidbody in the object 

        rb.freezeRotation = true; //stops the rigid from rotating
                               //should probably make the multiplier a variable too

        maxStamina = stamina;

    }

    // Update is called once per frame
    void Update()
    {
        input.x = Input.GetAxis("Horizontal"); //gets the 'a' and 'd' keys
        input.y = Input.GetAxis("Vertical"); //gets the 'w' and 's' keys
                                             //these are both defaults found in the input manager in settings

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        // flatten xz by forcing the y plane to always be 0
        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();
        //Normalize caps the length at 1

        /*
        Result:  When the player holds shift, they will sprint.  
        
        Details:
        Sprinting will multiply the movespeed by an arbitrary number
        Sprinting will lower "Stamina" 
        The player can only sprint if they have enough stamina
        The player will stop sprinting if they run out of stamina while sprinting

        Phase 1:
        Let the player sprint when holding shift

        Process:  Make a conditional so that the player will sprint when holding shift.

        DONE!
            No real issues, had to look at the documentation for the new InputAction stuff

        Phase 2:
        Make a stamina stat that drains when sprinting

        Process:
        Make a new variable
        have it so that when you are sprinting, stamina goes down
        If you have no stamina (stamina < 0), then don't sprint

        DONE!
            PROBLEM: The stamina drain is tied to the framerate which is bad, need to put it somewhere else
        */
        Vector3 move;
        move = (right * input.x + forward * input.y) * moveSpeed;
        if (sprintAction.IsPressed())
        {
            if (stamina > 0)
            {
                move *= sprintMultiplier;
                stamina = StaminaDrain(stamina);
            }
            else
            {
                // Debug.Log("OUT OF STAMINA");
            }
        }
        else
        {
            stamina = StaminaRegen(stamina);
        } //need to round stamina

        movement = new Vector3(move.x, rb.linearVelocity.y, move.z);

        staminaDisplay.text = "Stamina: " + Mathf.Round(stamina);
    }

    //constant update time regardless of framerate
    private void FixedUpdate()
    {
        rb.linearVelocity = movement; //sets the player's speed to whatever speed was calculated in the Update Function

        //this is probably where the stamina drain should happen


    }

    public float StaminaDrain(float stam)
    {
        stam -= staminaDrain * Time.deltaTime;
        // Debug.Log("Stamina is " + stam);
        return Mathf.Clamp(stam, 0, maxStamina);

    }
        public float StaminaRegen(float stam)
    {
        stam += staminaRegen * Time.deltaTime;
        // Debug.Log("Stamina is " + stam);
        return Mathf.Clamp(stam, 0, maxStamina);
    }
}