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
    enum EnemyState { Chase, Wander }; //when enemy can see the player they chase, otherwise wander
    class Enemy : Ball
    {
        public Texture2D alertSprite;
        public Texture2D wanderSprite;
        GameConsole console;
        public float lastInputUpdate;
        public float lastTargetUpdate;
        public bool toBeDestroyed;
        Vector2 currentTarget; //usually location of node target
        Vector2 inputDirection;
        public NavMesh navMesh;
        public NavNode nodeTarget; //destination node
        public NavNode currentNode; //node closest to current position
        public Player player;


        public EnemyState eState;
        public EnemyState State
        {
            get { return eState; }
            protected set
            {
                if (this.eState != value)       //Change state if it is differnt than previous state                
                {
                    this.eState = value;
                }
            }
        }


        public Enemy(Game game, Player p) : base(game)
        {
            console = (GameConsole)this.Game.Services.GetService(typeof(IGameConsole));
            if (console == null) //if there is no console yet it adds one
            {
                console = new GameConsole(this.Game);
                this.Game.Components.Add(console);  //add a new game console to Game
            }
            lastInputUpdate = 0;
            lastTargetUpdate = 0;
            inputDirection = new Vector2(0, 0);
            toBeDestroyed = false;
            player = p;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            this.alertSprite = this.Game.Content.Load<Texture2D>("EnemyBallAlert");
            this.wanderSprite = this.Game.Content.Load<Texture2D>("EnemyBall");
            this.maxSpeed = 3;
            this.acceleration = 0.2f;
            this.decceleration = 0.95f;
            this.spriteTexture = wanderSprite;
            this.isPlayer = false;
            this.eState = EnemyState.Wander;
        }

        public override void Update(GameTime gameTime)
        {
            currentNode = navMesh.nodeClosestToPoint(this.Location.X, this.Location.Y);//updates currentNode

            UpdateTarget(gameTime);
            UpdateInput(gameTime);
            currentTarget = nodeTarget.Location;// sets current target to the nodeTarget

            this.Direction += (inputDirection) * acceleration; //updates direction based on input

            base.Update(gameTime);
        }

        public override void Collide(List<WallBlock> Blocks) //collides with wall. Destroys itself if its lit
        {
            foreach(WallBlock b in Blocks)
            {
                if (b.isLit())
                {
                    toBeDestroyed = true;
                }
            }
            base.Collide(Blocks);
        }

        public void UpdateTarget(GameTime gameTime)//updates the target node
        {
            lastTargetUpdate += gameTime.ElapsedGameTime.Milliseconds;//last time the enemy updated the target

            switch (eState)
            {
                case EnemyState.Wander: //if enemy is wandering
                    if(nodeTarget == null) //creates node target in current location
                    {
                        this.nodeTarget = currentNode;
                    }
                    if (lastTargetUpdate>100)
                    {
                        this.nodeTarget = WanderTarget(); //sets node target based off of wander function
                        lastTargetUpdate = 0;
                    }
                    break;
                case EnemyState.Chase: //if enemy is chasing player
                    float xDif = player.Location.X - this.Location.X;
                    float yDif = player.Location.Y - this.Location.Y;
                    float dist = (float)Math.Sqrt((xDif * xDif) + (yDif * yDif));
                    if (dist < 125) //if close enough to player it will seek out nodes near player, even if near walls
                    {
                        nodeTarget = navMesh.nodeClosestToPoint(player.Location.X, player.Location.Y);
                    }
                    else //if far enough from the player but still sees him, select node close current node not by a wall
                    {
                        nodeTarget = navMesh.nodeClosestToPointNextToNode(player.Location.X, player.Location.Y, currentNode);
                    }
                    break;
            }
        }

        public void UpdateInput(GameTime gameTime) //updates the input based on the target node
        {
            lastInputUpdate += gameTime.ElapsedGameTime.Milliseconds;
                switch (eState)
                {
                    case EnemyState.Wander: //if wandering
                    if (lastInputUpdate < 100) //only inputs about 1/4 of the time
                    {
                        SeekToTargetLocation();
                    }
                    else if (lastInputUpdate < 400) // 3/4 of the time the input is 0,0
                    {
                        this.inputDirection = new Vector2(0, 0);
                        lastInputUpdate = 0;
                    }

                    break;
                    case EnemyState.Chase: // if chasing enemy
                    if (lastInputUpdate < 100) //only inputs about 1/4 of the time
                    {
                        float xDif = player.Location.X - this.Location.X;
                        float yDif = player.Location.Y - this.Location.Y;
                        float dist = (float)Math.Sqrt((xDif * xDif) + (yDif * yDif));
                        if (dist < 200) //if close enough to player it wrecklessly dashes at it
                        {
                            DashToTargetLocation();
                            console.GameConsoleWrite("I'm real close");
                        }
                        else
                        {
                            SeekToTargetLocation(); // if a decent distance from player it carefully seeks it
                            console.GameConsoleWrite("kinda close");
                        }
                    }
                    else if (lastInputUpdate < 400) // 3/4 of the time the input is 0,0
                    {
                        lastInputUpdate = 0;
                        this.inputDirection = new Vector2(0, 0);
                    }
                    break;
                }
        }

        public void DashToTargetLocation() //changes inputDirection to directly towards target
        {
            float x = currentTarget.X - this.Location.X;
            float y = currentTarget.Y - this.Location.Y;
            inputDirection = new Vector2(x, y);
            inputDirection.Normalize();
        }

        public void SeekToTargetLocation() //changes inputDirection to counteract current direction to get desired vector
        {
            float x = currentTarget.X - this.Location.X;
            float y = currentTarget.Y - this.Location.Y;
            inputDirection = new Vector2(x, y);

            inputDirection.Normalize();
            inputDirection = inputDirection*maxSpeed - this.Direction;
            inputDirection.Normalize();
            //console.GameConsoleWrite("" + inputDirection);
        }

        public NavNode WanderTarget() //returns a random NavNode that is next to currentNode. Prefers nodes in direction enemy is heading
        {
            
            System.Random r = new System.Random();
            float x;
            float y;
            x = currentNode.Location.X + (float)(-100 + r.NextDouble() * (200));
            r = new System.Random();
            y = currentNode.Location.Y + (float)(-100 + r.NextDouble() * (200));
            x += this.Direction.X * 75;
            y += this.Direction.Y * 75;
            NavNode passNode = navMesh.nodeClosestToPointNextToNode(x, y, currentNode);
            return passNode;
        }

        public void SetToChase() // Sets enemy state to chase
        {
            eState = EnemyState.Chase;
            this.spriteTexture = alertSprite;
        }

        public void SetToWander() // sets enemy state to wander
        {
            eState = EnemyState.Wander;
            this.spriteTexture = wanderSprite;
        }

    }
}
