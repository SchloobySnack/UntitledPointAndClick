using UnityEngine;
using UnityEngine.AI;
using System.Collections;
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

    public void Interact(GameObject interactable)
    {
        if (!(GameManager.instance.task == null))
        {
            StopCoroutine(GameManager.instance.task);
            GameManager.instance.task = null;
        }
        if (GameManager.instance.task == null)
        {
            GameManager.instance.task = Trigger(interactable);
            GameObject targetLocation = findNearestInteractionZone();
            GameManager.instance.SendMessage("navToTarget", targetLocation.transform);
            StartCoroutine(GameManager.instance.task);
        }
        
    }

    IEnumerator Trigger(GameObject interactable)
    {
        while(!(GameManager.instance.task == null))
        {
            NavMeshAgent mNavMeshAgent = GameManager.instance.playerNavMeshAgent;
            if (!mNavMeshAgent.pathPending)
            {
                if (mNavMeshAgent.remainingDistance <= mNavMeshAgent.stoppingDistance)
                {
                    if (!mNavMeshAgent.hasPath || mNavMeshAgent.velocity.sqrMagnitude == 0f)
                    {
                        interactable.SendMessage("trigger");
                        GameManager.instance.task = null;
                        yield break;
                    }
                }
            }
            yield return null;
        }

    }

}