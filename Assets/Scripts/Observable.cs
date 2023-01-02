using UnityEngine;
using System.Collections.Generic;
public class Observable : Interactable
{
    //public functions
    public void Interact()
    {
        Debug.Log("Eventually this message should be thrown to the UI");
        GameObject targetLocation = findNearestInteractionZone();
        GameManager.instance.SendMessage("navToTarget", targetLocation.transform);

    }
}