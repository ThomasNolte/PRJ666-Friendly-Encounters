using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropArea : MonoBehaviour, IDropHandler {

    // Use this for initialization
    //public DragCard.Slot typeOfCard = DragCard.Slot.MOVEMENT;
    public void OnDrop(PointerEventData data)
    {
        DragCard card = data.pointerDrag.GetComponent<DragCard>();
        if (card != null)
        {
           // if(typeOfCard == card.typeOfCard)
            //{
                card.returnPos = this.transform;
           // }
            
        }
    }
}
