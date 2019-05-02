using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OldSript
{
    public class EnemySundewAI : MonoBehaviour
    {

        public float delay = 2f;

        public float startTime = 0;
        public float speed;
        public float MoveHeight = 0.5f;

        bool ready = true;
        Vector2 originalPosition;
        public float timeStay = 0;
        public float timeBeforeStay = 3f;

        void Start()
        {
            originalPosition = transform.localPosition;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                Debug.Log("Dead_In_Hole");
                Application.LoadLevel("MarioTestSprite");
            }
        }

        // Update is called once per frame
        void Update()
        {
            startTime += Time.deltaTime;
            timeStay += Time.deltaTime;
            if (startTime >= delay)
            {
                ready = true;
                startTime = 0;
                StartCoroutine(MoveDownSundew());

            }


            if (timeStay >= timeBeforeStay & ready)
            {
                ready = false;
                timeStay = 0;
                StartCoroutine(MoveUpSundew());
            }
        }

        IEnumerator MoveUpSundew()
        {
            while (true)
            {
                transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y + speed * Time.deltaTime);

                if (transform.localPosition.y >= originalPosition.y + MoveHeight + 1)
                    break;

                yield return null;
            }
        }

        IEnumerator MoveDownSundew()
        {
            while (true)
            {
                transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y - speed * Time.deltaTime);

                if (transform.localPosition.y <= originalPosition.y)
                    break;

                yield return null;
            }
        }
    }
}