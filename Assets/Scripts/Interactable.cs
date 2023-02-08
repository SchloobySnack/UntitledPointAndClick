using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
namespace HeyAlexi
{
    public abstract class Interactable : MonoBehaviour {
        public string type;
        public GameManager gameManager;

        void Update()
        {
            if (gameManager == null)
            {
                gameManager = GameManager.instance.GetComponent<GameManager>();
            }
            
        }

        public GameObject FindNearestInteractionZone()
        {
            return gameManager.GetNearestToPlayer(FindChildsWithTag("InteractionZone"));
        }

        public List<GameObject> FindChildsWithTag(string tag)
        {
            List<GameObject> children = new();
            foreach (Transform t in transform.parent.transform)
            {
                if (t.CompareTag(tag))
                {
                    children.Add(t.gameObject);
                }
                
                
            }

            return children;
        }

        public abstract void Trigger();

        // public void Interact(RaycastHit hit)
        // {    
        //     GameObject interactable = hit.transform.gameObject;
        //     gameManager.NavToTarget(interactable.transform, FindNearestInteractionZone().transform.position);
        //     gameManager.task = Trigger(interactable);
        //     StartCoroutine(gameManager.task);      
        // }

        // IEnumerator Trigger(GameObject interactable)
        // {
        //     while(!(GameManager.instance.task == null))
        //     {
        //         NavMeshAgent mNavMeshAgent = gameManager.playerNavMeshAgent;
        //         if (!mNavMeshAgent.pathPending)
        //         {
        //             if (mNavMeshAgent.remainingDistance <= mNavMeshAgent.stoppingDistance)
        //             {
        //                 if (!mNavMeshAgent.hasPath || mNavMeshAgent.velocity.sqrMagnitude == 0f)
        //                 {
        //                     interactable.SendMessage("Trigger");
        //                     gameManager.task = null;
        //                     yield break;
        //                 }
        //             }
        //         }
        //         yield return null;
        //     }

        // }

    }
}