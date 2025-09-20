using System.Linq;
using TMPro;
using UnityEngine;

public class ButtonCounter : MonoBehaviour
{
    
    int buttonsPressed = 0;  //total number of buttons pressed
    public ButtonPresser[] buttons; //An array of buttonPresser classes, effectively a list of every button in the game
    public TextMeshProUGUI counterDisplay; //the UI to show the player the button progress
    public bool isLevelComplete = false;  //bool that checks if the level is complete
    public GameObject exit; //the exit door
    public LayerMask playerLayer = 6;  //Layermasks let you filter raycasts, this one is set to the player layer

    public void Start()
    {
        
    }

    public void Update()
    {
        if (isLevelComplete) return;  //stops the script from updating if the level is done
        if (buttonsPressed == buttons.Length)  //if the total buttons pressed equals the number of buttons in the level...
        {  //...finish the level
            counterDisplay.text = "Level complete! :3!!!";
            isLevelComplete = true;
            BoxCollider exitCollision = exit.GetComponent<BoxCollider>();
            Color c = exit.GetComponent<MeshRenderer>().material.color;
            c.a = 0.2f;
            // Debug.Log(c.a);
            exit.GetComponent<MeshRenderer>().material.color = c;
                exitCollision.excludeLayers = playerLayer;
                return;
            }
            
        buttonsPressed = 0;
        foreach (ButtonPresser button in buttons)
        {
            if (button.pressed)
            {
                buttonsPressed++;
            }
        }
        counterDisplay.text = "Buttons pressed: " + buttonsPressed.ToString();  //text displayed after update
    }
}
