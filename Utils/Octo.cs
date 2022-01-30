using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Shuriken.Utils
{
    public static class Octo
    {
        public static Vector2 InputVector()
        {
            Vector2 direction = new Vector2();

            if (GameManager.instance.inputHandler.inputActions.up.IsPressed)
            {
                direction.y = 1;
            }
            else if (GameManager.instance.inputHandler.inputActions.down.IsPressed)
            {
                direction.y = -1;
            }
            else { direction.y = 0; }

            if (GameManager.instance.inputHandler.inputActions.right.IsPressed)
            {
                direction.x = 1;
            }
            else if (GameManager.instance.inputHandler.inputActions.left.IsPressed)
            {
                direction.x = -1;
            }

            if (direction == new Vector2(0, 0)) { direction = new Vector2(HeroController.instance.cState.facingRight ? 1 : -1, 0); }

            return direction;
        }
        }

    }

