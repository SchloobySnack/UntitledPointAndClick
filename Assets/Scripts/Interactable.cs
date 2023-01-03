using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class Interactable : MonoBehaviour {
    public string type;
    public GameManager gameManager;

    void Update()
    {
        if (gameManager == null)
        {
            gameManager = GameManager.instance.GetComponent<GameManager>();
        }
        
    }

    public GameObject findNearestInteractionZone()
    {
        return gameManager.GetNearestToPlayer(FindChildsWithTag("InteractionZone"));
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

    public void Interact(RaycastHit hit)
    {    
        GameObject interactable = hit.transform.gameObject;
        gameManager.navToTarget(interactable.transform, findNearestInteractionZone().transform.position);
        gameManager.task = Trigger(interactable);
        StartCoroutine(gameManager.task);      
    }

    IEnumerator Trigger(GameObject interactable)
    {
        while(!(GameManager.instance.task == null))
        {
            NavMeshAgent mNavMeshAgent = gameManager.playerNavMeshAgent;
            if (!mNavMeshAgent.pathPending)
            {
                if (mNavMeshAgent.remainingDistance <= mNavMeshAgent.stoppingDistance)
                {
                    if (!mNavMeshAgent.hasPath || mNavMeshAgent.velocity.sqrMagnitude == 0f)
                    {
                        interactable.SendMessage("trigger");
                        gameManager.task = null;
                        yield break;
                    }
                }
            }
            yield return null;
        }

    }

}