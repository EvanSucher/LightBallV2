using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    class EndBlock : NonSolidBlock
    {
        public EndBlock(Game game)
            : base(game)
        {

        }

        protected override void LoadContent()
        {
            base.LoadContent();
            this.spriteTexture = this.Game.Content.Load<Texture2D>("EndBlock");
            this.isSolid = false;
            this.isEnd = true;
        }
    }
}
