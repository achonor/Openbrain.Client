using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class EventTrigger : MonoBehaviour, IPointerClickHandler
    , IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    , IPointerUpHandler, ISelectHandler, IUpdateSelectedHandler

{
    public delegate void VoidDelegate(GameObject go);
    public VoidDelegate onClick;
    public VoidDelegate onDown;
    public VoidDelegate onEnter;
    public VoidDelegate onExit;
    public VoidDelegate onUp;
    public VoidDelegate onSelect;
    public VoidDelegate onUpdateSelect;

    public static EventTrigger Get(GameObject go)
    {
        EventTrigger listener = go.GetComponent<EventTrigger>();
        if (listener == null)
            listener = go.AddComponent<EventTrigger>();
        return listener;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (onClick != null) onClick(gameObject);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (onDown != null) onDown(gameObject);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (onEnter != null) onEnter(gameObject);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (onExit != null) onExit(gameObject);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (onUp != null) onUp(gameObject);
    }
    public void OnSelect(BaseEventData eventData)
    {
        if (onSelect != null) onSelect(gameObject);
    }
    public void OnUpdateSelected(BaseEventData eventData)
    {
        if (onUpdateSelect != null) onUpdateSelect(gameObject);
    }

}
