using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using GlobalEnums;
namespace Shuriken
{
    class ShurikenBehaviour: MonoBehaviour
    {
        NewDamage damage;
        public float speed = 25;
        private Rigidbody2D body;
        float time = 0;
        Collider2D collider;
        private List<Collider2D> collidersTouched= new List<Collider2D>();

        public int damageHover = PlayerData.instance.nailDamage-1;
        public states currentState = states.Foward;
        private states prevState;

        public float fowardTime =0.5f;
        private bool canTeleport
        {
            get
            {
                foreach (Collider2D col in collidersTouched)
                {
                    if (col.bounds.Contains(collider.bounds.center)) return false;
                }
            return true;
            }
        }


        public Vector2 direction;

        public enum states
        {
            Foward = 0,
            Hang = 1,
            Back = 2
        }

        private void Start()
        {
            body = GetComponent<Rigidbody2D>();
            time = 0;
            prevState = currentState;
            //Physics2D.IgnoreLayerCollision(((int)PhysLayers.DEFAULT), ((int)PhysLayers.ENEMY_ATTACK),true);
            //Physics2D.IgnoreLayerCollision(((int)PhysLayers.DEFAULT), ((int)PhysLayers.ENEMIES),true);
            collider = GetComponent<Collider2D>();
            collider.isTrigger = true;
        }

        private void StateChange()
        {

            prevState = currentState;
        }

        void OnTriggerEnter2D(Collider2D col)
        {

            if(col.gameObject.layer ==((int)PhysLayers.TERRAIN) || col.gameObject.layer==((int)PhysLayers.SOFT_TERRAIN))
            {
                
                collidersTouched.Add(col);


            }

            if (currentState!= states.Foward && col.gameObject.name == "Knight")
            {
                Destroy(this.gameObject);

            }

        }
        void OnTriggerExit2D(Collider2D col)
        {
            if(collidersTouched!=null && collidersTouched.Contains(col) )
            {
                collidersTouched.Remove(col);

            } 
        }

        void Update()
        {

            if (ShurikenMod.settings.keybinds.ShurikenKey.IsPressed && ShurikenMod.settings.keybinds.ShurikenKey.HasChanged
                && ShurikenControl.canShuriken)
            {

                if (currentState != states.Hang)
                {
                    currentState = states.Hang;
                    damage = GetComponent<NewDamage>();
                    damage.damageDealt = damageHover;
                    //damage.ignoreInvuln = true;
                   
                    
;
                }

                else
                {
                    RaycastHit2D rayDown = Physics2D.Raycast(transform.position, -Vector2.up);
                    RaycastHit2D rayUp = Physics2D.Raycast(transform.position, Vector2.up);
                    if (canTeleport&&InputHandler.Instance.inputActions.up.IsPressed && !collider.IsTouchingLayers()&&(rayDown.collider!=null) &&
                        (rayDown.collider.Distance(this.collider).distance<100) && (rayUp.collider != null) &&
                        ((rayUp.collider.Distance(this.collider).distance < 100) || GameManager.instance.sceneName.Contains("Town")))
                    {
                        HeroController.instance.transform.position = this.transform.position;
                        Destroy(this.gameObject);
                    }


                    currentState = states.Back;
                    damage.damageDealt= ShurikenControl.damage;
                    damage.ignoreInvuln = false;

                }
            }

        }


        private void FixedUpdate()
        {
            if (currentState != prevState) StateChange();
            switch (currentState)
            {
                case states.Foward:
                    body.velocity = direction.normalized * speed;
                    time += Time.fixedDeltaTime;
                    if (time >= fowardTime) { time = 0; currentState = states.Back; }
                    this.transform.Rotate(0, 0, -18);
                    break;

                case states.Hang:
                    body.velocity = new Vector2(0,0);
                    this.transform.Rotate(0, 0, -10);
                    break;

                case states.Back:
                    this.transform.Rotate(0, 0, -18);
                    speed = Math.Abs(speed);
                    body.velocity = (body.transform.position - HeroController.instance.transform.position).normalized * -speed;
                    break;
            }

        }

        void OnCollisionEnter2D(Collision2D col)
        {



        }
    }
}
