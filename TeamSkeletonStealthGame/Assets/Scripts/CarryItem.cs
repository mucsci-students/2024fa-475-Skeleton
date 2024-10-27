using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


    public class CarryItem : MonoBehaviour
    {
        public string item_type;
        public bool rotate_item;
        public float carry_size = 1f;
        public Vector2 carry_offset = Vector2.zero;
        public UnityAction<GameObject> OnTake;
        public UnityAction<GameObject> OnDrop;

        private HoldItem user;
        private Vector3 initial_pos;
        private Vector3 start_size;
        private Quaternion start_rot;
        private bool trigger_at_start;
        private Vector3 last_motion = Vector3.right;
        
        private SpriteRenderer sprite_render;
        private Collider2D collide;
        private AudioSource audioclip;
        private float over_obstacle_count = 0f;
        private bool throwing = false;
        private float destroy_timer = 0f;
        private float take_timer = 0f;
        private float flipX = 1f;
        private bool destroyed = false;
        private Vector3 target_pos;
        private Quaternion target_rotation;
    private float carry_angle_deg;

    private void Awake()
        {

            collide = GetComponent<Collider2D>();
            sprite_render = GetComponentInChildren<SpriteRenderer>();
            //audioclip = GetComponent<AudioSource>();
            initial_pos = transform.position;
            trigger_at_start = collide.isTrigger;
            start_size = transform.localScale;
            start_rot = transform.rotation;
        }


        void Start()
        {

        }

        void Update()
        {
            take_timer += Time.deltaTime;

            if (over_obstacle_count > 0f)
                over_obstacle_count -= Time.deltaTime;

            if (!user && !throwing)
            {
                UpdateCarryItem();
            }

            //Destroyed
            if (!sprite_render.enabled)
            {
                destroy_timer += Time.deltaTime;
                if (destroy_timer > 3f)
                {
                    Reset();
                }
            }
            
        }
        
        public void UpdateCarryItem()
        {
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            target_pos = transform.position;
            float target_angle = 0f;
            target_rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, target_angle);

            if (user)
            {
                Vector3 motion = user.transform.position;
                if (user)
                {
                    if (motion.magnitude > 0.1f)
                    {
                        last_motion = motion;
                    }
                }
                
                //Update position of item
                //flipX = user.GetPlayer();
                GameObject hand = user.hand.gameObject;
                target_pos = hand.transform.position + hand.transform.up * carry_offset.y + hand.transform.right * carry_offset.x * flipX;
                Vector3 rot_vector_forw = Quaternion.Euler(0f, 0f, carry_angle_deg * flipX) * hand.transform.forward;
                Vector3 rot_vector_up = Quaternion.Euler(0f, 0f, carry_angle_deg * flipX) * hand.transform.up;
                target_rotation = Quaternion.LookRotation(rot_vector_forw, rot_vector_up);
            }

            //Move object
            transform.position = target_pos;
            transform.rotation = target_rotation;

            //Flip
            transform.localScale = user || throwing ? start_size * carry_size : start_size;
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * flipX, transform.localScale.y, transform.localScale.z);

        }


        public bool CanTake(GameObject taker)
        {
            HoldItem player = taker.GetComponent<HoldItem>();
            CarryItem current_item = player.GetHeldItem();
            
            if (current_item != null && item_type == current_item.item_type)
                return false;

            return false;
        }

        public void Take(HoldItem user)
        {
            this.user = user;
            collide.isTrigger = true;

            sprite_render.sortingLayerID = user.GetComponent<SpriteRenderer>().sortingLayerID;
            transform.localScale = start_size * carry_size;
            
            if (OnTake != null)
            {
                OnTake.Invoke(user.gameObject);
            }

            UpdateCarryItem();
        }

        public void Drop()
        {
            this.user = null;
            collide.isTrigger = throwing ? false : trigger_at_start;
            take_timer = -0.01f;

            //Reset sorting order/layer
            
            if (user && !throwing)
            {
                //Reset straight floor position
                transform.position = new Vector3(user.transform.position.x, user.transform.position.y, initial_pos.z);
                transform.localScale = start_size;
                transform.rotation = start_rot;
                flipX = 1f;
            }

            if (OnDrop != null)
            {
                OnDrop.Invoke(user.gameObject);
            }
        }
        
        public bool IsThrowing()
        {
            return throwing;
        }

        public bool IsDestroyed()
        {
            return destroyed;
        }

        public void UseItem()
        {
            if (user)
            {
                //item.action()
            }
        }
        
        void PlayAudio()
        {
            if (audioclip)
                audioclip.Play();
        }

        public void Destroy()
        {
            if (user && user.GetComponent<HoldItem>())
            {
                user.GetComponent<HoldItem>().DropItem();
            }
            destroyed = true;
            collide.enabled = false;
            sprite_render.enabled = false;
            destroy_timer = 0f;
        }

        public void SetStartingPos(Vector3 start_pos)
        {
            this.initial_pos = start_pos;
        }

        public HoldItem GetUser()
        {
            return this.user;
        }

        public bool HasUser()
        {
            return (this.user != null);
        }

        public Vector3 GetOrientation()
        {
            return last_motion;
        }

        public float GetFlipX()
        {
            return flipX;
        }

        public bool IsOverObstacle()
        {
            return (over_obstacle_count > 0.01f);
        }

        public void Reset()
        {
            
                destroyed = false;
                collide.enabled = true;
                sprite_render.enabled = true;
                over_obstacle_count = 0f;
                transform.position = initial_pos;
            
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Wall"
                || collision.gameObject.tag == "Door")
            {
                over_obstacle_count = 0.2f;
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Wall"
                || collision.gameObject.tag == "Door")
            {
                over_obstacle_count = 0.2f;
            }
            
        }
        
    }

