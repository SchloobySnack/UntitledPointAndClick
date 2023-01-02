using UnityEngine;
using System.Collections.Generic;
public class Observable : Interactable
{
    //public functions
    public void trigger()
    {
        Debug.Log("Eventually this message should be thrown to the UI");
    }
}