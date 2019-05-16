using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    class StartBlock : WallBlock
    {
        public StartBlock(Game game)
            : base(game)
        {

        }

        protected override void LoadContent()
        {
            base.LoadContent();
            this.spriteTexture = this.Game.Content.Load<Texture2D>("StartBlock");
            this.isSolid = false;
        }
    }
}
