using UnityEngine;

public class ButtonPresser : MonoBehaviour
{
    [Header("UI")]

    public bool pressed = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void buttonPressed()
    {
        if (!pressed)
        {
            pressed = true;
            
        }

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
