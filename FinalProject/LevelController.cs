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
    class LevelController
    {
        InputHandler input;
        public bool isSpacePressed;

        public LevelController(Game game)
        {
            input = (InputHandler)game.Services.GetService(typeof(IInputHandler));
            isSpacePressed = false;
        }

        public void HandleInput(GameTime gametime)
        {
            if (input.KeyboardState.IsKeyDown(Keys.Space))
            {
                isSpacePressed = true;
            }
            else
            {
                isSpacePressed = false;
            }

        }
    }
}
