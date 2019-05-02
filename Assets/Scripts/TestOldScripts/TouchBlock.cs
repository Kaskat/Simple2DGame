using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OldSript
{
    public class TouchBlock : MonoBehaviour
    {

        public GameObject coin;

        AudioSource audio;
        GameObject player;
        PlayerController plc;

        public float bounceHeight = 0.5f;
        public float bounceSpeed = 4f;

        public float coinMoveSpeed = 0f;
        public float coinMoveHeight = 3f;
        public float coinFallDistance = 2f;

        public Sprite emptySprite;

        private Vector2 originalPosition;
        private bool canBounce = true;

        // Use this for initialization
        void Start()
        {
            originalPosition = transform.localPosition;

            audio = GetComponent<AudioSource>();
            player = GameObject.FindGameObjectWithTag("Player");
            plc = player.GetComponent<PlayerController>();
        }

        void ChangeSprite()
        {
            GetComponent<Animator>().enabled = false;

            GetComponent<SpriteRenderer>().sprite = emptySprite;
        }

        public void BlockBounce()
        {
            if (canBounce)
            {
                canBounce = false;

                StartCoroutine(Bounce());
            }
        }

        public void PresentCoin()
        {
            GameObject spiningCoin = Instantiate(coin);

            spiningCoin.transform.SetParent(this.transform.parent);

            spiningCoin.transform.localPosition = new Vector2(originalPosition.x, originalPosition.y + 1);

            StartCoroutine(MoveCoin(spiningCoin));
        }

        IEnumerator Bounce()
        {

            ChangeSprite();

            PresentCoin();

            audio.enabled = true;

            plc.CoinUp();

            while (true)
            {
                transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y + bounceSpeed * Time.deltaTime);

                if (transform.localPosition.y >= originalPosition.y + bounceHeight)
                    break;

                yield return null;
            }

            while (true)
            {
                transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y - bounceSpeed * Time.deltaTime);

                if (transform.localPosition.y <= originalPosition.y + bounceHeight)
                {
                    transform.localPosition = originalPosition;
                    break;
                }

                yield return null;
            }
        }

        IEnumerator MoveCoin(GameObject coin)
        {
            while (true)
            {
                coin.transform.localPosition = new Vector2(coin.transform.localPosition.x, coin.transform.localPosition.y + coinMoveSpeed * Time.deltaTime);

                if (coin.transform.localPosition.y >= originalPosition.y + coinMoveHeight + 1)
                    break;

                yield return null;
            }

            while (true)
            {
                coin.transform.localPosition = new Vector2(coin.transform.localPosition.x, coin.transform.localPosition.y - coinMoveSpeed * Time.deltaTime);

                if (coin.transform.localPosition.y <= originalPosition.y + coinFallDistance + 1)
                {
                    Destroy(coin.gameObject);
                    break;
                }
                yield return null;
            }

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}