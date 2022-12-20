using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class GameManager : MonoBehaviour 
{
    public NavMeshAgent playerNavMeshAgent;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 targetLocation = hit.point;

                // Set the target position for the nav mesh agent
                playerNavMeshAgent.SetDestination(targetLocation);
            }
        }
    }

}

