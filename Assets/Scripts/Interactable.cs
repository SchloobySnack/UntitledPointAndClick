using UnityEngine;
using System.Collections.Generic;

public class Interactable : MonoBehaviour {
    public string type;

    public GameObject findNearestInteractionZone()
    {
        return GameManager.instance.GetNearestToPlayer(FindChildsWithTag("InteractionZone"));
    }

    public List<GameObject> FindChildsWithTag(string tag)
    {
        List<GameObject> children = new List<GameObject>();
        foreach (Transform t in transform.parent.transform)
        {
            if (t.tag == tag)
            {
                children.Add(t.gameObject);
            }
            
            
        }

        return children;
    }

}