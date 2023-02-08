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
    }
}