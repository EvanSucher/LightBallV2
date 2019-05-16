using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGameLibrary.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Util;

namespace FinalProject
{
    class Ball : DrawableSprite //Ball is currently the player, although I might make it an abstract class
    {
        GameConsole console;
        public float decceleration;
        public float acceleration;
        public float maxSpeed;
        public float bounceOff;
        public int offsetX;
        public int offsetY;
        public bool isPlayer;

        public Ball(Game game) : base(game)
        {
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
            this.decceleration = 0.96f;
            this.acceleration = 0.35f;
            this.maxSpeed = 3.8f;
            this.bounceOff = 1.5f;
            this.Speed = 200;
            this.offsetX = 0;
            this.offsetY = 0;
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            //Accelerate(); 
            Deccelerate();
            CheckSpeed();
            UpdatePosition(gameTime);

            base.Update(gameTime);
        }

        public void UpdatePosition(GameTime gameTime) //changes ball location based on direction
        {
            this.Location += this.Direction * (this.Speed * gameTime.ElapsedGameTime.Milliseconds / 1000);
        }

        public virtual void Collide(List<WallBlock> Blocks) //collides with a list of wallblocks passed by ball manager
        {
            if (Blocks.Count == 1) // if theres only 1 block it is colliding with
            {
                float xDist = Blocks[0].Location.X - this.Location.X;//Location relative to block
                float yDist = Blocks[0].Location.Y - this.Location.Y;//Location relative to block


                if (xDist * xDist > yDist * yDist) // if the xDistance is bigger than y distance (X collision)
                {
                    if (xDist * this.Direction.X > 0) //if the ball is moving towards the block x
                    {
                        this.Direction.X = -this.Direction.X;
                        if (this.Direction.X > 0) { this.Location.X += bounceOff; }
                        else { this.Location.X -= bounceOff; }
                    }
                }
                else // if the yDistance is bigger than xDistance (Y collision)
                {
                    if (yDist * this.Direction.Y > 0) //if the ball is moving towards the block y
                    {
                        this.Direction.Y = -this.Direction.Y;
                        if (this.Direction.Y > 0) { this.Location.Y += bounceOff; }
                        else { this.Location.Y -= bounceOff; }
                    }
                }
            }
            else if (Blocks.Count == 2) //2 block collision
            {
                if (Blocks[0].Location.X == Blocks[1].Location.X) //Blocks share x location
                {
                    float xDist = Blocks[0].Location.X - this.Location.X; //Location relative to block
                    if (xDist * this.Direction.X > 0) //if the ball is moving towards the block x
                    {
                        this.Direction.X = -this.Direction.X;
                        if (this.Direction.X > 0) { this.Location.X += bounceOff; }
                        else { this.Location.X -= bounceOff; }
                    }
                }
                else if (Blocks[0].Location.Y == Blocks[1].Location.Y) //blocks share y location
                {
                    float yDist = Blocks[0].Location.Y - this.Location.Y; //Location relative to block
                    if (yDist * this.Direction.Y > 0) //if the ball is moving towards the block x
                    {
                        this.Direction.Y = -this.Direction.Y;
                        if (this.Direction.Y > 0) { this.Location.Y += bounceOff; }
                        else { this.Location.Y -= bounceOff; }
                    }
                }
                else //blocks are kitty corner
                {

                }
            }
            else if (Blocks.Count == 3)
            {
                float x1 = Blocks[0].Location.X;
                float y1 = Blocks[0].Location.Y;
                float x2 = Blocks[1].Location.X;
                float y2 = Blocks[1].Location.Y;
                float x3 = Blocks[2].Location.X;
                float y3 = Blocks[2].Location.Y;

                float xDist;
                float yDist;

                //Basically it just finds out which box is the crux of the collision and runs a collision with that block
                if ((x1 == x2 || x1 == x3) && (y1 == y2 || y1 == y3)) //if the block1 is the crux of the three blocks
                {
                    xDist = Blocks[0].Location.X - this.Location.X;
                    yDist = Blocks[0].Location.Y - this.Location.Y;
                }
                else if ((x2 == x1 || x2 == x3) && (y2 == y1 || y2 == y3)) //if the block2 is the crux of the three blocks
                {
                    xDist = Blocks[1].Location.X - this.Location.X;
                    yDist = Blocks[1].Location.Y - this.Location.Y;
                }
                else //if the block3 is the crux of the three blocks
                {
                    xDist = Blocks[2].Location.X - this.Location.X;
                    yDist = Blocks[2].Location.Y - this.Location.Y;
                }

                if (xDist * this.Direction.X > 0) //if the ball is moving towards the block x
                {
                    this.Direction.X = -this.Direction.X;
                    if (this.Direction.X > 0) { this.Location.X += bounceOff; }
                    else { this.Location.X -= bounceOff; }
                }
                if (yDist * this.Direction.Y > 0) //if the ball is moving towards the block y
                {
                    this.Direction.Y = -this.Direction.Y;
                    if (this.Direction.Y > 0) { this.Location.Y += bounceOff; }
                    else { this.Location.Y -= bounceOff; }
                }

            }

        }

        public void Accelerate() //Adds Controller direction vector to the ball direction vector
        {
            //this.Direction += (controller.Direction) * acceleration;
        }

        public void Deccelerate() //Constantly multiplies vector by decceleration value
        {
            this.Direction = this.Direction * decceleration;
        }

        public void CheckSpeed() //If the x speed + y speed is greater than the max speed, then
        {                        //it normalizes the ball direction vector and then multiplies by max speed

            //check to make sure it isn't going too fast
            double absValX = this.Direction.X * this.Direction.X;
            double absValY = this.Direction.Y * this.Direction.Y;
            if (absValX + absValY > maxSpeed*maxSpeed)
            {
                this.Direction.Normalize();
                this.Direction = this.Direction * maxSpeed;
            }
            //if the ball's velocity in a direction is low enough it gets set to 0
            if (this.Direction.X < 0.1 && this.Direction.X > -0.1)
            {
                this.Direction.X = 0;
            }
            if (this.Direction.Y < 0.1 && this.Direction.Y > -0.1)
            {
                this.Direction.Y = 0;
            }
        }

        
        public override void Draw(GameTime gameTime) //Custom Draw based off screen offset
        {
            rectangle.X = (int)Location.X+offsetX;
            rectangle.Y = (int)Location.Y+offsetY;
            //console.GameConsoleWrite(""+offsetX+","+offsetY);



            spriteBatch.Begin();
            spriteBatch.Draw(spriteTexture,
                rectangle,
                null,
                Color.White,
                MathHelper.ToRadians(Rotate),
                this.Origin,
                SpriteEffects,
                0);


            DrawMarkers(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
        
    }


}
