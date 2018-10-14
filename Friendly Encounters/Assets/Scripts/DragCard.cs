using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    public Transform returnPos = null;
    //public enum Slot { MOVEMENT, INTERACTION};

    //public Slot typeOfCard = Slot.MOVEMENT;

	public void OnBeginDrag (PointerEventData data) {
        returnPos = this.transform.parent;
        this.transform.SetParent(this.transform.parent.parent);

        GetComponent<CanvasGroup>().blocksRaycasts = false;
	}
    public void OnDrag(PointerEventData data)
    {
        this.transform.position = data.position;

    }
    public void OnEndDrag(PointerEventData data)
    {
        this.transform.SetParent(returnPos);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }



}
