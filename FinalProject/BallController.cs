using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    class BallController //Input Conroller for Ball
    {
        InputHandler input;
        public Vector2 Direction { get; private set; }

        public BallController(Game game)
        {
            input = (InputHandler)game.Services.GetService(typeof(IInputHandler));
            this.Direction = Vector2.Zero;
        }

        public void HandleInput(GameTime gametime)
        {
            this.Direction = Vector2.Zero;  //Start with no direction on each new update
            int verDir = 0;
            int horDir = 0;

            //Gets the sum of the horizontal input and sum of vertical input
            if (input.KeyboardState.IsKeyDown(Keys.Left))
            {
                horDir += -1;
            }
            if (input.KeyboardState.IsKeyDown(Keys.Right))
            {
                horDir += 1;
            }
            if (input.KeyboardState.IsKeyDown(Keys.Up))
            {
                verDir += -1;
            }
            if (input.KeyboardState.IsKeyDown(Keys.Down))
            {
                verDir += 1;
            }

            Vector2 preNorm = new Vector2(horDir, verDir);
            if (verDir != 0 || horDir !=0)              //Normalizes the vector unless it's (0,0)
            {                                           //because you just get NaN if you try to 
                preNorm.Normalize();                    //normalize a (0,0) vector, which is no good
            }
            this.Direction = preNorm;


        }


    }
}
