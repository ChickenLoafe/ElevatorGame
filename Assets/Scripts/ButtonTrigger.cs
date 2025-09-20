using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    [Header("Trigger Settings")]
    public string triggeringTag = "Player"; 
    public bool checkForButton = false;
    public KeyCode keyCodeInteract;

    [Header("Events")]
    public UnityEvent onEnter;
    public UnityEvent onExit;
    
    public UnityEvent onStay;
    
    

    
    bool inside;

    void OnTriggerEnter(Collider other){
        if(other.CompareTag(triggeringTag)){
            if(!checkForButton){
                onEnter.Invoke();
            }
        }
    }

    void OnTriggerStay(Collider other){
        if(other.CompareTag(triggeringTag)){
            if(checkForButton){
                inside = true;
            }else{
                onStay.Invoke();
            }
        }
    }

    void OnTriggerExit(Collider other){
        if(other.CompareTag(triggeringTag)){
            if(checkForButton){
                inside = false;
            }
            else
            {
                onExit.Invoke();
            }
        }
    }


    


    private void Update() {
        if(Input.GetKeyDown(keyCodeInteract) && inside){
            onStay.Invoke();
        }
    }

}