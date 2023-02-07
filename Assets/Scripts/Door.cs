using UnityEngine;
using System.Collections.Generic;

namespace HeyAlexi
{
    public class Door : Interactable
    {
        
        public Animator animator;
        //private variables
        private bool isOpen;
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
        public override void Trigger()
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

        }
    }
}