using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OldSript
{
    public class Castle : MonoBehaviour
    {

        public GameObject canvas;

        private void OnTriggerEnter2D(Collider2D collision)
        {

            if (collision.tag == "Player")
            {
                Debug.Log(collision.tag);
                canvas.SetActive(true);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag == "Player")
                canvas.SetActive(false);
        }
    }
}