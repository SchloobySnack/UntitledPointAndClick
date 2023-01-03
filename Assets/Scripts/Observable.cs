using System.Diagnostics;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Observable : Interactable
{
    public Animation animation;
    private bool isPlaying;
    public void trigger()
    {
        string thought = "Hmm, It Appears to be a box.";
        StartCoroutine(displayThought(thought));
    }        

    IEnumerator displayThought(string thought)
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