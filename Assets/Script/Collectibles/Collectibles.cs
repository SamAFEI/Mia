using Assets.Script.Manager;
using UnityEngine;

namespace Assets.Script.Collectibles
{
    public class Collectibles : MonoBehaviour
    {
        public CollectiblesData Data;
        public Rigidbody2D RB { get; private set; }
        [SerializeField] private GameObject trail;

        public int Value { get; private set; }
        public bool IsActive { get; private set; }
        public bool IsCollecting { get; private set; }
        private float delayTime = 1.5f;

        private void Awake()
        {
            Value = (int)(Data.BaseValue * Random.Range(0.8f,1.2f));
            RB = GetComponent<Rigidbody2D>();
        }
        private void Update()
        {
            
            delayTime -= Time.deltaTime;
            if (delayTime < 0)
            {
                IsActive = true;
            }
            if (IsCollecting && IsActive)
            {

                trail.SetActive(true);
                transform.position = Vector2.MoveTowards(transform.position
                                            , GameManager.Instance.Player.transform.position
                                            , 20 * Time.deltaTime);
                RB.gravityScale = 0;
            }
            else
            {
                trail.SetActive(RB.velocity.y != 0);
            }
        }
        public void SetCollecting()
        {
            IsCollecting = IsActive;
        }
    }
}
