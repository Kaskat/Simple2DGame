using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
namespace OldSript
{
    public class JoyButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
    {

        [HideInInspector]
        public bool pressed;

        public void OnPointerDown(PointerEventData eventData)
        {
            pressed = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            pressed = false;
        }

        void Update()
        {

        }
    }
}