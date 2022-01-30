using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Shuriken
{
    public class ShurikenDamage: DamageEnemies
    {

        private void OnCollisionEnter2D(Collision2D collision)
        {
            ShurikenMod.Instance.Log("oi");
        }



    }
}
