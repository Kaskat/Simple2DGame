using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace OldSript
{
    public class Reload : MonoBehaviour
    {

        // Use this for initialization
        public void ReloadWorld()
        {
            Debug.Log("Nope");
            Debug.Log("Reload");
            Application.LoadLevel("MarioTestSprite");
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
