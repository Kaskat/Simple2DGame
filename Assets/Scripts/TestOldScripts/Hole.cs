using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace OldSript
{
    public class Hole : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("Nope");
            if (collision.tag == "Player")
            {
                Debug.Log("Dead_In_Hole");
                Application.LoadLevel("MarioTestSprite");
            }
        }

    }
}