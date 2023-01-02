using UnityEngine;
using System.Collections.Generic;
public class Door : Interactable
{
    
    public Animator animator;
    //private variables
    private bool isOpen;
    public GameObject targetLocation;

    // private functions

    private void OpenDoor()
    {
        isOpen = true;
    }

    private void CloseDoor()
    {
        isOpen = false;
    }
    

    //public functions
    public void Interact()
    {
        isOpen = !isOpen;
        
        if(isOpen)
        {
            animator.SetBool("isOpen", isOpen);
        }
        else 
        {
            animator.SetBool("isOpen", isOpen);
        }
        targetLocation = findNearestInteractionZone();
        GameManager.instance.SendMessage("navToTarget", targetLocation.transform);

    }
}