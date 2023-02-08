using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HeyAlexi
{
    public class ItemMenu : Interactable
    {
        public override void Trigger()
        {
            gameManager.OpenItemMenu();
        }        

    }
}