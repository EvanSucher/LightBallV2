using Microsoft.Xna.Framework;
using MonoGameLibrary.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    class BallManager : GameComponent
    {
        GameConsole console;
        Player player;
        BlockManager blockM;
        List<Enemy> Enemies;
        List<Enemy> EnemiesToDestroy;
        NavMesh navMesh;
        Level level;
        public int offsetX;
        public int offsetY;

        public BallManager(Game game, Player p, BlockManager bm, Level l) : base(game)
        {
            console = (GameConsole)this.Game.Services.GetService(typeof(IGameConsole));
            if (console == null) //ohh no no console
            {
                console = new GameConsole(this.Game);
                this.Game.Components.Add(console);  //add a new game console to Game
            }
            this.level = l;
            this.Enemies = new List<Enemy>();
            this.navMesh = new NavMesh();
            this.player = p;
            this.blockM = bm;

        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            Collision();
            UpdateBallOffset();
            EnemyPlayerCollision();
            UpdateEnemySight();
            CheckPlayerWin();
            base.Update(gameTime);
        }

        public void AddEnemy(float x, float y)
        {
            Enemy e;
            e = new Enemy(this.Game, player); //Creates new wall
            e.Initialize();
            e.Location = new Vector2(x, y); //places the block based off i and j
            Enemies.Add(e); //adds the block to the Blocks List
            this.Game.Components.Add(e); //Adds the blick to the game componenet. Couldn't draw otherwise
        }

        public void UpdateBallOffset()//Updates screen offset for all enemies and player
        {
            offsetX = player.offsetX;
            offsetY = player.offsetY;
            //console.GameConsoleWrite("" + offsetX + "," + offsetY);
            foreach (Enemy e in Enemies) //iterates through all the balls in the list
            {
                e.offsetX = offsetX;
                e.offsetY = offsetY;
            }
        }

        public void Collision()//gets the collision blocks from block manager and using that collides each ball
        {
            List<WallBlock> Blocks;
            Blocks = blockM.GetCollisionBlocks(player);
            if (Blocks.Count != 0)
            {
                player.Collide(Blocks);
                if (player.isTouchingEnd)
                {
                    level.Win();
                }
            }

            EnemiesToDestroy = new List<Enemy>();
            foreach (Enemy e in Enemies)
            {
                Blocks = blockM.GetCollisionBlocks(e);
                if (Blocks.Count != 0)
                {
                    e.Collide(Blocks);
                }
                if (e.toBeDestroyed)
                {
                    EnemiesToDestroy.Add(e);
                }
            }

            DestroyEnemies();
        }

        public void DestroyEnemies()//Destroys enemies in the EnemiesToDestroy List
        {
            foreach (Enemy e in EnemiesToDestroy)
            {
                Enemies.Remove(e);
                this.Game.Components.Remove(e);
            }
        }

        public void EnemyPlayerCollision()//detects collision between enemy and player
        {
            foreach (Enemy e in Enemies)
            {
                if (e.Intersects(player))
                {
                    level.Lose();
                }
            }

        }

        public void CheckPlayerWin() //checks player collision with end block
        {
            if (player.isTouchingEnd)
            {
                level.Win();
            }
        }

        public void RemoveAllEnemies()
        {
            foreach (Enemy e in Enemies)
            {
                this.Game.Components.Remove(e);
            }
            Enemies = new List<Enemy>();
        }

        public void AddNavMeshNode(float x, float y)//adds a nod to the navigation mesh
        {
            navMesh.AddNode(x,y);
        }

        public void ClearNavMesh()
        {
            navMesh.Clear();
        }

        public void UpdateNearbyNavNodes() //updates each node and what its nearby nodes are
        {
            navMesh.UpdateNearbyNodes();
        }

        public void UpdateEnemySight()//updates the state of enemies based on line of sight
        {
            foreach (Enemy e in Enemies)
            {
                if (blockM.BlockingEnemyVision(e))
                {
                    //console.GameConsoleWrite("Huh?");
                    if (e.eState == EnemyState.Chase)
                    {
                        e.SetToWander();
                        //e.lastTargetUpdate = 0;
                    }
                }
                else
                {
                    //console.GameConsoleWrite("I SEE YOU");
                    if (e.eState == EnemyState.Wander)
                    {
                        e.SetToChase();
                        //e.lastTargetUpdate = 0;
                    }
                }

            }
        }

        public void UpdateEnemyNavMesh() //changes enemy navmesh to match current one
        {
            foreach (Enemy e in Enemies)
            {
                e.navMesh = navMesh;
            }
        }
    }
}
