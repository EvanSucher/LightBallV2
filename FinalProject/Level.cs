using Microsoft.Xna.Framework;
using MonoGameLibrary.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{

    enum GameState { Start, Running, Win, Lose };  //GameStates help to stop the game when you die/win

    class Level : GameComponent
    {

        GameConsole console;
        public Player player; //ball that player controls
        public BlockManager blockm; //manages all the blocks 
        public BallManager ballm; //manages all enemies and player
        public LevelController lController; //only really used to advance after level
        public int currentLevel; //keeps track of current level

        public GameState gameState;
        public GameState State
        {
            get { return gameState; }
            protected set
            {
                if (this.gameState != value)       //Change state if it is differnt than previous state                
                {
                    this.gameState = value;
                }
            }
        }

        public Level(Game game) : base(game)
        {
            lController = new LevelController(game); //Adds a LevelController

            console = (GameConsole)this.Game.Services.GetService(typeof(IGameConsole));
            if (console == null) //ohh no no console
            {
                console = new GameConsole(this.Game);
                this.Game.Components.Add(console);  //add a new game console to Game

            }
        }

        public override void Initialize()
        {
            gameState = GameState.Running;
            currentLevel = 1;
            player = new Player(this.Game);

            blockm = new BlockManager(this.Game, player);
            blockm.Initialize();
            this.Game.Components.Add(blockm);

            ballm = new BallManager(this.Game, player, blockm, this);
            ballm.Initialize();
            this.Game.Components.Add(ballm);
            base.Initialize();

            LoadLevel();

            player.Initialize();
            //player.Location = new Vector2(120, 200);
            this.Game.Components.Add(player);
        }

        public override void Update(GameTime gameTime) 
        {
            lController.HandleInput(gameTime);
            switch (gameState)
            {
                case GameState.Win:
                    checkInputWhileWaiting();
                    break;
                case GameState.Lose:
                    checkInputWhileWaiting();
                    break;

            }


            base.Update(gameTime);
        }

        public List<string> LevelSelector() //passes a sring based on currentLevel
        {
            List<string> stringList = new List<string>();
            if (currentLevel == 1)
            {
                stringList.Add("XXXXXXXXXXXXXXXXXXXX");
                stringList.Add("X..................X");
                stringList.Add("X..................X");
                stringList.Add("X..................X");
                stringList.Add("X.............e....X");
                stringList.Add("X..................X");
                stringList.Add("X..................X");
                stringList.Add("X..................X");
                stringList.Add("X..................X");
                stringList.Add("X..................X");
                stringList.Add("X..................X");
                stringList.Add("X..................X");
                stringList.Add("X..................X");
                stringList.Add("X....s.........f...X");
                stringList.Add("X..................X");
                stringList.Add("XXXXXXXXXXXXXXXXXXXX");
            }
            else if (currentLevel == 2)
            {
                stringList.Add("XXXXXXXXXXXXXXXXXXXX");
                stringList.Add("X..................X");
                stringList.Add("X..................X");
                stringList.Add("X..................X");
                stringList.Add("X.............e....X");
                stringList.Add("X..................X");
                stringList.Add("X..................X");
                stringList.Add("X.....XXXXXXXX.....X");
                stringList.Add("X.....XXXXXXXX.....X");
                stringList.Add("X.....X......X.....X");
                stringList.Add("X..................X");
                stringList.Add("X..................X");
                stringList.Add("X..................X");
                stringList.Add("X....s.........f...X");
                stringList.Add("X..................X");
                stringList.Add("XXXXXXXXXXXXXXXXXXXX");
            }
            else if (currentLevel == 3)
            {
                stringList.Add("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
                stringList.Add("X...............x...........xxxxxxx....X");
                stringList.Add("X...............x........e..xxxxxx.....X");
                stringList.Add("X...............xxxx........xxxxxx.....X");
                stringList.Add("X...s...........xxx.......xxxxxx.......X");
                stringList.Add("X.........................xxxxxx.......X");
                stringList.Add("Xxxxxxxxx.............xxxxxx...........X");
                stringList.Add("X.....................xxxxxx.....e.....X");
                stringList.Add("X...e......x...x..........xx.......xxxxX");
                stringList.Add("X..........x...x...................xxxxX");
                stringList.Add("X..........x...x...................xxxxX");
                stringList.Add("X...xxxxxxxx...xxxxx...................X");
                stringList.Add("X.......................xxxx...........X");
                stringList.Add("X..........e..........xxxxxxxx.........X");
                stringList.Add("X.......................xxxx.......x...X");
                stringList.Add("X...xxxxxxxx....xxxx...................X");
                stringList.Add("X..........................e...........X");
                stringList.Add("X......................x...........x...X");
                stringList.Add("X........xxxxxx...............x........X");
                stringList.Add("X.........xxxx........x................X");
                stringList.Add("X..........xx..............x...........X");
                stringList.Add("X....e.....xx.....x....................X");
                stringList.Add("X........................xxxx..........X");
                stringList.Add("X.....xx........x....xxxxxxxxxxxx......X");
                stringList.Add("X.....xx........x....x....xx....x...e..X");
                stringList.Add("X...xxxxxx...........x..........x......X");
                stringList.Add("X...xxxxxx.......e.....................X");
                stringList.Add("X...xxxxxx.............................X");
                stringList.Add("X.....xx........x........xxxx..........X");
                stringList.Add("X.....xx........x........xxxx....xxxxxxX");
                stringList.Add("X...........xxxx.........xxxx....xxxxxxX");
                stringList.Add("X...........x..........................X");
                stringList.Add("X....xxx....x..........................X");
                stringList.Add("X....xxx....x..e....xxxxxxxx...........X");
                stringList.Add("X...xxx.............xxxxxxxx...........X");
                stringList.Add("X...xxx...................xx.......f...X");
                stringList.Add("X.........................xxxx.........X");
                stringList.Add("X.....e........xx.........xxxx.........X");
                stringList.Add("X............xxxxxx.......xxxx.........X");
                stringList.Add("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
            }
            else if (currentLevel == 4)
            {
                stringList.Add("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
                stringList.Add("X...................................x...........X");
                stringList.Add("X...................................x...........X");
                stringList.Add("X...................................x...........X");
                stringList.Add("X...................................x...........X");
                stringList.Add("X...................................x...........X");
                stringList.Add("X.....x.....xxxxxxxxxxxxxxxxxxx.....x.....x.....X");
                stringList.Add("X.....x.....x...........x...........x.....x.....X");
                stringList.Add("X.....x.....x...........x...........x.....x.....X");
                stringList.Add("X.....x.....x.......e...x......e....x.....x.....X");
                stringList.Add("X.....x.....x...........x...........x.....x.....X");
                stringList.Add("X.....x.....x...........x.........xxx.....x.....X");
                stringList.Add("X...........x.....xxxxxxx.........xx......x.....X");
                stringList.Add("X...........x.....x...............xx......x.....X");
                stringList.Add("X...........x.....x...............xx......x.....X");
                stringList.Add("X........e..x.....x..f.........xxxxx.....xx.....X");
                stringList.Add("X...........x.....x............x.........xx.....X");
                stringList.Add("X.....xxxxxxx.....x............x.........xx.....X");
                stringList.Add("X.....x...........xxxxxxxxxxxxxx................X");
                stringList.Add("X.....x...........x............x................X");
                stringList.Add("X.....x...........x............x..e.............X");
                stringList.Add("X.....x...........x.........s..x................X");
                stringList.Add("X.....x...........x............x................X");
                stringList.Add("X.....x...........x............x................X");
                stringList.Add("X.....x.....xxxxxxx.....xx.....xxxxxxxxxxxx.....X");
                stringList.Add("X.....x.....xxxxxxx.....xx.....xxxxxxxxxxxx.....X");
                stringList.Add("X.....x...................................x.....X");
                stringList.Add("X.....x...................................x.....X");
                stringList.Add("X.....x.................................e.x.....X");
                stringList.Add("X.....x...................................x.....X");
                stringList.Add("X.....x...................................x.....X");
                stringList.Add("X.....xxxxxxxxxxx.....xxxxxx.....xxxxxxxxxx.....X");
                stringList.Add("X.....xxxxxxxxxxx.....xxxxxx.....xxx............X");
                stringList.Add("X.....xxxxxxxxxxx.....xxxxxx.....xxx..e.........X");
                stringList.Add("X.....xxxxxxxxxxx.....xxxxxx.....xxx............X");
                stringList.Add("X.....x.................xxxx.....xxx............X");
                stringList.Add("X.....x.................xxxx.....xxx............X");
                stringList.Add("X.....x.................xxxx.....xxx............X");
                stringList.Add("X.....x.e...............xxxx.............xx.....X");
                stringList.Add("X.....x..................................xx.....X");
                stringList.Add("X.....x..................................xx.....X");
                stringList.Add("X.....xxxxxxxxxxx........................xx.....X");
                stringList.Add("X.....xxxxxxxxxxx........................xx.....X");
                stringList.Add("X.....xxxxxxxxxxx................xxxxxxxxxx.....X");
                stringList.Add("X.....xxxxxxxxxxx.....xxxxxx.....xxxxxxxxxx.....X");
                stringList.Add("X.....................xxxxxx....................X");
                stringList.Add("X.....................xxxxxx....................X");
                stringList.Add("X...e.................xxxxxx.................e..X");
                stringList.Add("X.....................xxxxxx....................X");
                stringList.Add("X.....................xxxxxx....................X");
                stringList.Add("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
            }
                return stringList;
        }

        public void LoadLevel()
        {
            player.Visible = true;
            float xPos = 0;
            float yPos = 0;
            List<string> stringList = LevelSelector();
            foreach (string rowString in stringList)
            {
                foreach (char c in rowString)
                {
                    AddToLevel(c,xPos,yPos);
                    xPos += 50;
                }
                xPos = 0;
                yPos += 50;
            }
            ballm.UpdateNearbyNavNodes();
            ballm.UpdateEnemyNavMesh();
            player.Direction.X = 0;
            player.Direction.Y = 0;
            gameState = GameState.Running;
        }

        public void UnloadLevel()
        {
            blockm.RemoveAllBlocks();
            ballm.RemoveAllEnemies();
            ballm.ClearNavMesh();
            player.resetEndTouch();
            player.Visible = false;
        }

        public void AddToLevel(char c, float x, float y)// adds object to level based on character in string
        {
            if (c == 'X' || c == 'x')
            {
                blockm.AddBlock(x, y);
            }
            else if(c == '.')
            {
                ballm.AddNavMeshNode(x,y);
            }
            else if(c == 's')
            {
                blockm.AddStartBlock(x, y);
                player.Location = new Vector2(x, y);
                ballm.AddNavMeshNode(x, y);
            }
            else if (c == 'f')
            {
                blockm.AddEndBlock(x, y);
                ballm.AddNavMeshNode(x, y);
            }
            else if (c == 'e')
            {
                ballm.AddEnemy(x, y);
                ballm.AddNavMeshNode(x, y);
            }
        }

        public void Win() //unloads level and then waits for input 
        {
            gameState = GameState.Win;
            UnloadLevel();
            currentLevel++;
        }

        public void Lose() //unloads level and then waits for input
        {
            UnloadLevel();
            gameState = GameState.Lose;
        }

        public void checkInputWhileWaiting() //Checks input between rounds
        {
            //console.GameConsoleWrite(""+lController.isSpacePressed);
            if (lController.isSpacePressed)
            {
                console.GameConsoleWrite("NEW GAME");
                LoadLevel();
                gameState = GameState.Running;
            }
        }

    }
}
