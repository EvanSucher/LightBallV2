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
    class BlockManager : GameComponent //Manages and creates blocks, also runs collisions and other functions for all blocks
    {
        public List<WallBlock> Blocks { get; private set; }
        GameConsole console;
        public Player player;
        public int offsetX;
        public int offsetY;

        public BlockManager(Game game, Player p) : base(game)
        {
            console = (GameConsole)this.Game.Services.GetService(typeof(IGameConsole));
            if (console == null) //ohh no no console
            {
                console = new GameConsole(this.Game);
                this.Game.Components.Add(console);  //add a new game console to Game

            }

            this.Blocks = new List<WallBlock>();

            this.player = p;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            LightUpBlocks();
            //CollisionTest();
            UpdateBlockOffset();


            base.Update(gameTime);
        }

        public void AddBlock(float x, float y)
        {
            WallBlock w;
            w = new WallBlock(this.Game); //Creates new wall
            w.Initialize();
            w.Location = new Vector2(x,y); //places the block based off i and j
            Blocks.Add(w); //adds the block to the Blocks List
            this.Game.Components.Add(w); //Adds the blick to the game componenet. Couldn't draw otherwise
        }

        public void AddStartBlock(float x, float y)
        {
            StartBlock s;
            s = new StartBlock(this.Game); //Creates new wall
            s.Initialize();
            s.Location = new Vector2(x, y); //places the block based off i and j
            Blocks.Add(s); //adds the block to the Blocks List
            this.Game.Components.Add(s); //Adds the blick to the game componenet. Couldn't draw otherwise
        }

        public void AddEndBlock(float x, float y)
        {
            EndBlock e;
            e = new EndBlock(this.Game); //Creates new wall
            e.Initialize();
            e.Location = new Vector2(x, y); //places the block based off i and j
            Blocks.Add(e); //adds the block to the Blocks List
            this.Game.Components.Add(e); //Adds the blick to the game componenet. Couldn't draw otherwise
        }

        public void UpdateBlockOffset()//Updates screen offset for each block
        {
            offsetX = player.offsetX;
            offsetY = player.offsetY;
            //console.GameConsoleWrite("" + offsetX + "," + offsetY);
            foreach (WallBlock b in Blocks) //iterates through all the blocks in the list
            {
                b.offsetX = offsetX;
                b.offsetY = offsetY;
            }
        }

        public void LightUpBlocks() //Lights up Blocks based on if they are a certain distances away
        {
            foreach (WallBlock b in Blocks) //iterates through all the blocks in the list
            {
                float xDif = b.Location.X - player.Location.X; //Difference between block and ball x location
                float yDif = b.Location.Y - player.Location.Y; //Difference between block and ball y location

                if (b.isSolid)
                {
                    switch (b.blockState)
                    {
                        case BlockState.Unlit:
                            if ((xDif * xDif) + (yDif * yDif) < (b.lightUpDist * b.lightUpDist)) //block is unlit but within light range
                            {
                                b.LightOn();
                            }
                            break;
                        case BlockState.Lit:
                            if ((xDif * xDif) + (yDif * yDif) >= (b.lightUpDist * b.lightUpDist)) //block is lit but out of light range
                            {
                                b.LightOff();
                            }
                            break;
                    }
                }
            }
        }

        public List<WallBlock> GetCollisionBlocks(Ball ball)//returns list of blocks colliding with ball
        {
            List<WallBlock> passList = new List<WallBlock>();
            foreach (WallBlock b in Blocks) //iterates through all the blocks in the list
            {
                if (b.Intersects(ball))
                {
                    if (b.isSolid)
                    {
                            passList.Add(b);
                    }
                    else if (b.isEnd)
                    {
                        if (ball.isPlayer)
                        {
                            player.isTouchingEnd = true;
                            //console.GameConsoleWrite("WIN");
                        }
                    }
                }
            }
            return passList;
        }

        public void RemoveAllBlocks()
        {
            foreach (WallBlock b in Blocks)
            {
                this.Game.Components.Remove(b);
            }
            Blocks = new List<WallBlock>();
        }

        public bool BlockingEnemyVision(Enemy e) //returns true if a wall is blocking the enemies vision
        {
            bool returnBoolVal = false;
            float enemyX = e.Location.X;
            float enemyY = e.Location.Y;
            float playerX = player.Location.X;
            float playerY = player.Location.Y;
            float blockX;
            float blockY;
            float e_pSlope;
            float e_bSlope;
            float e_pyDif;
            float e_pxDif;
            float e_byDif;
            float e_bxDif;


            foreach (WallBlock b in Blocks)
            {
                if (b.isSolid)
                {

                    blockX = b.Location.X;
                    blockY = b.Location.Y;
                    e_pxDif = playerX - enemyX;
                    e_pyDif = playerY - enemyY;
                    e_bxDif = blockX - enemyX;
                    e_byDif = blockY - enemyY;


                    e_pSlope = e_pyDif / e_pxDif;
                    e_bSlope = e_byDif / e_bxDif;
                    if (Math.Sqrt(e_pxDif* e_pxDif) < 400 && Math.Sqrt(e_pyDif* e_pyDif) < 400) //if the enemy is close enough to the player
                    {
                        if ((e_bxDif < 60 && e_bxDif > -60) && (e_pxDif < 60 && e_pxDif > -60)) //accounts for if angle is close to 0
                        {
                            if (e_byDif * e_byDif < e_pyDif * e_pyDif) // compares y value
                            {
                                returnBoolVal = true;
                            }
                        }
                        else if ((e_pxDif * e_bxDif > 0) && (e_pyDif * e_byDif > 0)) //if the slopes are both negative/both positive
                        {
                            if ((e_pSlope * e_bxDif > e_byDif - 50) && (e_pSlope * e_bxDif < e_byDif + 50)) //if checks if the slopes are the same
                            {
                                if (e_bxDif * e_bxDif < e_pxDif * e_pxDif) //if the player is farther from the enemy than the block
                                {
                                    returnBoolVal = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        returnBoolVal = true;
                    }
                   

                }

            }

            return returnBoolVal;
        }

   
    }
}
