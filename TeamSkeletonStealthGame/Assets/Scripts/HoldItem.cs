using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class HoldItem : MonoBehaviour
    {
        public Transform hand;

        private Player player;

        private CarryItem held_item = null;
        private float take_item_timer = 0f;

        void Awake()
        {
            player = GetComponent<Player>();
        }

        private void Start()
        {
            player.onDeath += DropItem;
        }

        void Update()
        {
            
            take_item_timer += Time.deltaTime;
            //if (held_item && player.GetActionDown())
               // held_item.UseItem();
        }

        private void LateUpdate()
        {
            if (held_item != null)
                held_item.UpdateCarryItem();
        }

        public void TakeItem(CarryItem item) {

            if (item == held_item || take_item_timer < 0f)
                return;

            if (held_item != null)
                DropItem();

            held_item = item;
            take_item_timer = -0.2f;
            item.Take(this);
        }

        public void DropItem()
        {
            if (held_item != null)
            {
                held_item.Drop();
                held_item = null;
            }
        }

        public Player GetPlayer()
        {
            return player;
        }

        public CarryItem GetHeldItem()
        {
            return held_item;
        }

        public Vector3 GetHandPos()
        {
            if (hand)
                return hand.transform.position;
            return transform.position;
        }
/**
        public Vector2 GetMove()
        {
            return player.GetMove();
        }

        public Vector2 GetFacing()
        {
            return player.GetFacing();
        }
**/
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<CarryItem>())
                TakeItem(collision.GetComponent<CarryItem>());
        }

        void OnCollisionStay2D(Collision2D coll)
        {
            if (coll.gameObject.GetComponent<Door>() && held_item && held_item.GetComponent<Key>())
            {
                held_item.GetComponent<Key>().TryOpenDoor(coll.gameObject);
            }
        }
    }


