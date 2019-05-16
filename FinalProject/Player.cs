using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    class Player : Ball
    {
        BallController controller;
        GameConsole console;
        public bool isTouchingEnd;

        public Player(Game game) : base(game)
        {
            controller = new BallController(game); //Adds a BallController

            console = (GameConsole)this.Game.Services.GetService(typeof(IGameConsole));
            if (console == null) //if there is no console yet it adds one
            {
                console = new GameConsole(this.Game);
                this.Game.Components.Add(console);  //add a new game console to Game

            }
            this.ShowMarkers = false; //rectangular hitbox markers
        }

        protected override void LoadContent()
        {
            this.spriteTexture = this.Game.Content.Load<Texture2D>("PlayerBall");
            //== Movement Parameters ==
            this.decceleration = 0.88f;
            this.acceleration = 0.3f;
            this.maxSpeed = 3.5f;
            this.Speed = 200;
            this.offsetX = 0;
            this.offsetY = 0;
            this.isPlayer = true;
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            //Movement from controller
            controller.HandleInput(gameTime);

            AccelerateWithController();

            offsetX = 475 - (int)this.Location.X; //changes offset so the player is in the middle of the screen
            offsetY = 350 - (int)this.Location.Y;
            base.Update(gameTime);
        }

        public void AccelerateWithController() //Adds Controller direction vector to the ball direction vector
        {
            this.Direction += (controller.Direction) * acceleration;
        }

        public void resetEndTouch()//resets bool that keeps track of if it's touching the end
        {
            isTouchingEnd = false;
        }

    }
}
