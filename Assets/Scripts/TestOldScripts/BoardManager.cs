using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace OldSript
{
    public class BoardManager : MonoBehaviour
    {


        public GameObject groundTile;


        int rows = 2;
        int colums = 8;

        void Start()
        {

            SetupScene();

        }

        void Update()
        {
        }

        public void SetupScene()
        {
            for (int i = 0; i < colums; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    Instantiate(groundTile, new Vector3(-4 + i, j, 0f), Quaternion.identity);
                }
            }
        }
    }
}