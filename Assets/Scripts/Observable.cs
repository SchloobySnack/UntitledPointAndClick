using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HeyAlexi
{
    public class Observable : Interactable
    {
        public new Animation animation;
        public string thought;
        private bool isPlaying;
        public override void Trigger()
        {
            StartCoroutine(DisplayThought(thought));
        }        

        IEnumerator DisplayThought(string thought)
        {
            gameManager.innerThought.text = thought;
            isPlaying = true;
            animation.Play();
        
            while(isPlaying)
            {
                if (gameManager.innerThought.text == null)
                {
                    yield break;
                }
                yield return null;   
            }
            gameManager.innerThought.text = null;   

        }
        void AnimationCompleted()
        {
        isPlaying = false;
        }
    }
}