using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] RectTransform thumbStickTrans, backgroundTrans, centerTrans;

    public delegate void OnStickInputValueUpdated(Vector2 inputVal);
    public event OnStickInputValueUpdated onStickInputValueUpdated;

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 touchPos = eventData.position;
        Vector2 centerPos = backgroundTrans.position;

        Vector2 localOffset = Vector2.ClampMagnitude(touchPos - centerPos, backgroundTrans.sizeDelta.x/2);

        Vector2 inputVal = localOffset/backgroundTrans.sizeDelta/2;

        thumbStickTrans.position =centerPos + localOffset;
        onStickInputValueUpdated?.Invoke(inputVal);

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        backgroundTrans.position = eventData.position;
        thumbStickTrans.position = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        backgroundTrans.position = centerTrans.position;
        thumbStickTrans.position = backgroundTrans.position;
        onStickInputValueUpdated?.Invoke(Vector2.zero);
    }
}
