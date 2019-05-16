using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Sprite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    enum BlockState { Unlit, Lit }; //WallBlock states are lit and unlit

    class WallBlock : DrawableSprite //WallBlocks are blocks that light up when the player is near by. enemies dy when hit lit block
    {
        public Texture2D litSprite;
        public Texture2D unlitSprite;
        public int lightUpDist;
        public int offsetX;
        public int offsetY;
        public bool isSolid;
        public bool isEnd;

        public BlockState blockState;            
        public BlockState State
        {
            get { return blockState; }
            protected set
            {
                if (this.blockState != value)       //Change state if it is differnt than previous state                
                {
                    this.blockState = value;
                }
            }
        }

        public WallBlock(Game game)
            : base(game)
        {

        }

        protected override void LoadContent()
        {
            this.litSprite = this.Game.Content.Load<Texture2D>("WallBlockLit"); //Lit Wall Sprite
            this.unlitSprite = this.Game.Content.Load<Texture2D>("WallBlock"); //Unlit Wall Sprite
            offsetX = 0;
            offsetY = 0;
            this.spriteTexture = unlitSprite;
            this.blockState = BlockState.Unlit;
            this.lightUpDist = 300;
            this.isSolid = true;
            this.isEnd = false;
            base.LoadContent();
        }

        public void LightOn() //changes state and sprite
        {
            this.spriteTexture = litSprite;
            this.blockState = BlockState.Lit;
        }

        public void LightOff() //changes state and sprite
        {
            this.spriteTexture = unlitSprite;
            this.blockState = BlockState.Unlit;
        }



        public bool isLit() //returns true if block is lit
        {
            switch (this.blockState)
            {
                case BlockState.Lit:
                    return true;
                case BlockState.Unlit:
                    return false;
            }
            return false;
        }

        public override void Draw(GameTime gameTime)
        {
            rectangle.X = (int)Location.X + offsetX;
            rectangle.Y = (int)Location.Y + offsetY;
            

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
