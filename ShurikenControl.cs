using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Shuriken.Utils;
using System.IO;
using GlobalEnums;
using HutongGames.PlayMaker;
using InControl;


namespace Shuriken
{
    public class ShurikenControl: MonoBehaviour
    {

        public static  GameObject shurikenW;
        public static GameObject shurikenB;
        public static GameObject shuriken
        {
            get
            {
                return ShurikenMod.settings.shurikenLevel == 2 ? shurikenB: shurikenW;


            }
        }


        public PlayerAction key = ShurikenMod.settings.keybinds.ShurikenKey;
        public static GameObject shurikenInstance = null;

        private tk2dSpriteAnimator anim;
        public static int damage
        {
            get
            {
                return ((int)((shuriken==shurikenW ? 10: 35)*(PlayerData.instance.equippedCharm_19 ? 1.3f : 1)));
            }
        }
        public static  bool canShuriken
        {
            get
            {


                return HeroController.instance.CanInput() &&(ShurikenMod.settings.shurikenLevel==1 || ShurikenMod.settings.shurikenLevel == 2);
            }
            set
            {

            }
        }
        private void Start()
        {


          //  DontDestroyOnLoad(shurikenW);
            //DontDestroyOnLoad(shurikenB);
            //DontDestroyOnLoad(shuriken);

            #region W
            NewDamage damageEnemiesW = shurikenW.gameObject.AddComponent<NewDamage>();
            damageEnemiesW.gameObject.layer = ((int)PhysLayers.DEFAULT);
            damageEnemiesW.attackType = AttackTypes.Spell;
            damageEnemiesW.damageDealt = damage;
            damageEnemiesW.ignoreInvuln = false;
            damageEnemiesW.magnitudeMult = 0.001f;
            damageEnemiesW.moveDirection = false;
            damageEnemiesW.specialType = 0;
            damageEnemiesW.circleDirection = false;
            damageEnemiesW.damageLimit = 50;
            damageEnemiesW.ratio = 3;


            //anim = GetComponent<tk2dSpriteAnimator>();

            Rigidbody2D rigidbodyW = shurikenW.AddComponent<Rigidbody2D>();
            rigidbodyW.gravityScale = 0.0001f;

            BoxCollider2D colliderW = shurikenW.AddComponent<BoxCollider2D>();
            //collider.tag= "Nail Attack";
            colliderW.isTrigger = true;
            shurikenW.AddComponent<ShurikenBehaviour>().speed=20;
            #endregion

            #region B
            NewDamage damageEnemiesB = shurikenB.gameObject.AddComponent<NewDamage>();
            damageEnemiesB.gameObject.layer = ((int)PhysLayers.DEFAULT);
            damageEnemiesB.attackType = AttackTypes.Spell;
            damageEnemiesB.damageDealt = damage;
            damageEnemiesB.ignoreInvuln = false;
            damageEnemiesB.magnitudeMult = 0.001f;
            damageEnemiesB.moveDirection = false;
            damageEnemiesB.specialType = 0;
            damageEnemiesB.circleDirection = false;
            damageEnemiesB.damageLimit=100;
            damageEnemiesB.ratio = 2;
            

            //anim = GetComponent<tk2dSpriteAnimator>();

            Rigidbody2D rigidbodyB = shurikenB.AddComponent<Rigidbody2D>();
            rigidbodyB.gravityScale = 0.0001f;

            BoxCollider2D colliderB = shurikenB.AddComponent<BoxCollider2D>();
            //collider.tag= "Nail Attack";
            colliderB.isTrigger = true;
            ShurikenBehaviour behavB = shurikenB.AddComponent<ShurikenBehaviour>();
            behavB.damageHover = 12;
            behavB.speed = 25;


            #endregion




        }

        public void Update()
        { 
            if (canShuriken && key.IsPressed && key.HasChanged && shurikenInstance==null)
            {

                shuriken.GetComponent<ShurikenBehaviour>().direction = Octo.InputVector();

                shuriken.transform.position = HeroController.instance.transform.position;
                shurikenInstance =Instantiate(shuriken);
                shurikenInstance.SetActive(true);
                //shuriken.transform.Rotate
            }
            

        }

    


    }
}
