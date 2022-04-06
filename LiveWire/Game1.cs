using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using System.Collections.Generic;

namespace LiveWire
{
    /// <summary>
    /// Level File Format:
    /// 
    /// rows,cols,tileWidth,tileHeight
    /// (grid of ints separated by commas)
    /// 
    /// Level File Legend:
    /// '-' - empty space
    /// '0' - basic tile
    /// TODO: and more to come!
    /// </summary>

    // --- FINITE STATE MACHINE : GAMESTATE ---
    enum GameState
    {
        MainMenu,
        LevelSelect,
        PlayLevel,
    }

    // --- FINITE STATE MACHINE: LEVEL ---
    // one state for each level, used only to progress through the levels
    enum Level
    {
        MainMenu,
        Level1,
        Level2,
        Level3,
        Level4,
        Level5,
        Level6,
        EndLevel
    }

    public class Game1 : Game
    {

        // --- VARIABLE DECLARATIONS ---
        
        // graphics handlers
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private int screenWidth;
        private int screenHeight;
        private SpriteFont basicFont;
        private Texture2D tileSpriteSheet;
        private Texture2D playerSprite;

        // stream handlers
        private StreamReader reader;
        private StreamWriter writer;

        // input handlers
        private KeyboardState kbState;
        private KeyboardState prevKbState;
        private MouseState mState;
        private MouseState prevMState;

        // player handlers
        private Player player;
        private int playerWidth;
        private int playerHeight;

        // FSM handlers
        private GameState currentState;
        private Level currentLevel;

        // board handlers
        private TileParent[,] board;
        private int rows;
        private int cols;
        private int tileWidth;
        private int tileHeight;
        // Machines are stored in a list separate from tiles
        private List<Machine> machines;

        // TEST WIRE
        private Wire wire;

        // MAIN MENU
        private List<Button> menuButtons;
        private List<Button> levelButtons;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // screen settings

            // NOTE FROM OWEN: making the screen this large means I
            // can't fit the entire window on my monitor for some reason,
            // and I can't resize the window.
            screenWidth = 1920;
            screenHeight = 1080;
            _graphics.PreferredBackBufferWidth = screenWidth;
            _graphics.PreferredBackBufferHeight = screenHeight;
            // Make sure the line below is commented out before committing
            //_graphics.IsFullScreen = true;
            _graphics.ApplyChanges();

            // objects and states
            currentState = GameState.MainMenu;
            currentLevel = Level.MainMenu;

            menuButtons = new List<Button>();
            levelButtons = new List<Button>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            basicFont = Content.Load<SpriteFont>("BaseText");
            tileSpriteSheet = Content.Load<Texture2D>("LiveWireTiles");
            playerSprite = Content.Load<Texture2D>("Robot");
            NewLevel(Level.MainMenu);

            menuButtons.Add(new Button(
                _graphics.GraphicsDevice,
                new Rectangle(screenWidth/2 - 80, screenHeight/2 -40, 160, 80),
                "Start Game",
                basicFont,
                Color.FromNonPremultiplied(86, 91, 143, 255)));
            menuButtons.Add(new Button(
                _graphics.GraphicsDevice,
                new Rectangle(screenWidth / 2 - 80, screenHeight / 2 + 80, 160, 80),
                "Level Select",
                basicFont,
                Color.FromNonPremultiplied(86, 91, 143, 255)));

            levelButtons.Add(new Button(
                _graphics.GraphicsDevice,
                new Rectangle(screenWidth / 3 - 80, screenHeight / 3 -40, 160, 80),
                "Level 1",
                basicFont,
                Color.FromNonPremultiplied(86, 91, 143, 255)));

            levelButtons.Add(new Button(
                _graphics.GraphicsDevice,
                new Rectangle(screenWidth / 3, screenHeight / 3 - 40, 160, 80),
                "Level 2",
                basicFont,
                Color.FromNonPremultiplied(86, 91, 143, 255)));

            levelButtons.Add(new Button(
                _graphics.GraphicsDevice,
                new Rectangle(screenWidth / 3 + 80, screenHeight / 3 -40, 160, 80),
                "Level 3",
                basicFont,
                Color.FromNonPremultiplied(86, 91, 143, 255)));

            levelButtons.Add(new Button(
                _graphics.GraphicsDevice,
                new Rectangle(screenWidth / 3 -80, screenHeight / 3 + 90, 160, 80),
                "Level 4",
                basicFont,
                Color.FromNonPremultiplied(86, 91, 143, 255)));

            levelButtons.Add(new Button(
                _graphics.GraphicsDevice,
                new Rectangle(screenWidth / 3, screenHeight / 3 + 90, 160, 80),
                "Level 5",
                basicFont,
                Color.FromNonPremultiplied(86, 91, 143, 255)));

            levelButtons.Add(new Button(
                _graphics.GraphicsDevice,
                new Rectangle(screenWidth / 3 + 80, screenHeight / 3 + 90, 160, 80),
                "Level 6",
                basicFont,
                Color.FromNonPremultiplied(86, 91, 143, 255)));

            levelButtons.Add(new Button(
                _graphics.GraphicsDevice,
                new Rectangle(screenWidth / 3 + 80, screenHeight / 3 + 180, 160, 80),
                "Final Level",
                basicFont,
                Color.FromNonPremultiplied(86, 91, 143, 255)));


            menuButtons[0].OnButtonClick += this.StartGame;
            menuButtons[1].OnButtonClick += this.SelectLevel;

            levelButtons[0].OnButtonClick += this.Level1;
            levelButtons[1].OnButtonClick += this.Level2;
            levelButtons[2].OnButtonClick += this.Level3;
            levelButtons[3].OnButtonClick += this.Level4;
            levelButtons[4].OnButtonClick += this.Level5;
            levelButtons[5].OnButtonClick += this.Level6;
            levelButtons[6].OnButtonClick += this.LastLevel;
            player = new Player(new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2), playerSprite);

            // TEST WIRE
            wire = new Wire(player);
            wire.AddSegment(new Segment(new Vector2(750, 1000), new Vector2(700, 800)));
        }

        protected override void Update(GameTime gameTime)
        {
            // update mnk
            kbState = Keyboard.GetState();
            mState = Mouse.GetState();

            if (kbState.IsKeyDown(Keys.Escape)) { this.Exit(); }

            switch (currentState)
            {
                case GameState.MainMenu:
                    // transition
                    foreach(Button b in menuButtons)
                    {
                        b.Update();
                    }
                    break;
                    
                case GameState.LevelSelect:
                    // TEMPORARY transition
                    foreach(Button b in levelButtons)
                    {
                        b.Update();
                    }
                    break;

                case GameState.PlayLevel:
                    player.PlayerMovement(kbState, prevKbState, board);
                    wire.Wires[wire.Wires.Count - 1].Node2 = player.Position;
                    wire.Update(board);
                    // TEMPORARY transition
                    if (SingleKeyPress(Keys.Enter, kbState, prevKbState))
                    {
                        if (currentLevel == Level.EndLevel)
                        {
                            currentState = GameState.MainMenu;
                            currentLevel = Level.Level1;
                        }

                        else
                        {
                            NewLevel(currentLevel++);
                        }
                        
                        if (currentLevel == Level.EndLevel) 
                        { 
                            currentState = GameState.MainMenu;  
                            currentLevel = Level.Level1; 
                        }

                        else 
                        { 
                            NewLevel(currentLevel++); 
                        }
                    }

                    break;
            }

            // update mnk
            prevKbState = kbState;
            prevMState = mState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Beige);
            _spriteBatch.Begin();

            switch (currentState)
            {
                case GameState.MainMenu:
                    // TEMPORARY display
                    /*_spriteBatch.DrawString(
                        basicFont,
                        "Main Menu Template",
                        new Vector2(
                                screenWidth / 2 - (int)basicFont.MeasureString("Main Menu Template").X / 2,
                                screenHeight / 2 - (int)basicFont.MeasureString("Main Menu Template").Y),
                        Color.Black);

                    _spriteBatch.DrawString(
                        basicFont,
                        "Press Enter To Advance",
                        new Vector2(
                                screenWidth / 2 - (int)basicFont.MeasureString("Press Enter To Advance").X / 2,
                                screenHeight / 2),
                        Color.Black);*/
                    DrawLevel(currentLevel);

                    foreach(Button b in menuButtons)
                    {
                        b.Draw(_spriteBatch);
                    }
                    break;

                case GameState.LevelSelect:
                    // TEMPORARY display
                    _spriteBatch.DrawString(
                        basicFont,
                        "Level Select Template",
                        new Vector2(
                                screenWidth / 2 - (int)basicFont.MeasureString("Level Select Template").X / 2,
                                screenHeight / 2 - (int)basicFont.MeasureString("Level Select Template").Y),
                        Color.Black);

                    _spriteBatch.DrawString(
                        basicFont,
                        "Press LMB To Advance",
                        new Vector2(
                                screenWidth / 2 - (int)basicFont.MeasureString("Press LMB To Advance").X / 2,
                                screenHeight / 2),
                        Color.Black);

                    foreach(Button b in levelButtons)
                    {
                        b.Draw(_spriteBatch);
                    }

                    DrawLevel(currentLevel);
                    break;


                case GameState.PlayLevel:
                    // display level
                    DrawLevel(currentLevel);

                    // TEST WIRE
                    wire.Draw(_spriteBatch, GraphicsDevice);

                    // display player
                    player.Draw(_spriteBatch);

                    // TEMPORARY display
                    _spriteBatch.DrawString(
                        basicFont,
                        "Play Level Template",
                        new Vector2(
                                screenWidth / 2 - (int)basicFont.MeasureString("Play Level Template").X / 2,
                                screenHeight / 2 - (int)basicFont.MeasureString("Play Level Template").Y),
                        Color.White);

                    // TODO: Implement code for drawing list of Machines
                    /*
                    foreach (Machine machine in machines)
                    {
                        machine.Draw(_spriteBatch);
                    }
                    */

                    _spriteBatch.DrawString(
                        basicFont,
                        "Press Enter To Advance",
                        new Vector2(
                                screenWidth / 2 - (int)basicFont.MeasureString("Press enter To Advance").X / 2,
                                screenHeight / 2),
                        Color.White);
                    break;
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }



        // --- HELPER METHODS ---
        private bool SingleKeyPress(Keys key, KeyboardState kbState, KeyboardState prevKbState)
        {
            if (prevKbState != null)
                return kbState.IsKeyDown(key) && !prevKbState.IsKeyDown(key);
            return true;
        }

        private void NewLevel(Level level)
        {
            try 
            {
                // read info
                string[] line;
                string newLine;
                reader = new StreamReader("../../../Levels/" + currentLevel + ".txt");
                line = reader.ReadLine().Split(',');
                rows = int.Parse(line[0]);
                cols = int.Parse(line[1]);
                tileWidth = int.Parse(line[2]);
                tileHeight = int.Parse(line[3]);
                bool temporaryBool;
                // Variables for loading Machines
                string machinesNextLine;
                int machineCount;

                // create board
                board = new TileParent[rows, cols];
                for (int r = 0; r < rows; r++)
                {
                    newLine = reader.ReadLine();
                    for (int c = 0; c < cols; c++)
                    {
                        // Note from Owen: I deleted the if block here that checked for whether a tile
                        // is a machine, since I'll be storing machines outside the level grid in
                        // the files.
                        temporaryBool = newLine[c] != '-';
                        board[r, c] = new Tile(c * tileWidth, r * tileHeight, tileWidth, tileHeight, tileSpriteSheet, temporaryBool);
                    }
                }

                // TODO: Read Machines from Machine list at the end of the level file

                /*
                // If there are more lines after the level, try to load them as machines
                if ((machinesNextLine = reader.ReadLine().Trim()) != null)
                {
                    // Treat the first line as the number of machines in the level
                    if (int.TryParse(machinesNextLine, out machineCount))
                    {
                        // Loop through each expected line 
                        for (int i = 0; i < machineCount; i++)
                        {
                            // Splits the Machine info into an array
                            line = reader.ReadLine().Trim().Split(',');

                            // Parses the first value of each line as the type of Machine it is,
                            // and constructs the Machine accordingly
                            // The second and third values on each split line will always be
                            // the Machine's position, and values after that represent special
                            // data unique for each Machine Type.
                            switch (Enum.Parse<MachineType>(line[0]))
                            {
                                case MachineType.WireSource:
                                    break;
                                case MachineType.PlugDoorController:
                                    break;
                                case MachineType.DoorSegment:
                                    break;
                            }
                        }
                    }
                }
                */

                // --- ANIM STATES ---

                // corner tiles
                board[rows - 1, cols - 1].AnimState[5] = board[rows - 2, cols - 2].AnimState[0];
                board[rows - 1, cols - 1].AnimState[6] = true;
                board[rows - 1, cols - 1].AnimState[7] = true;
                board[rows - 1, cols - 1].AnimState[8] = true;
                board[rows - 1, 0].AnimState[5] = true;
                board[rows - 1, 0].AnimState[6] = board[rows - 2, 1].AnimState[0];
                board[rows - 1, 0].AnimState[7] = true;
                board[rows - 1, 0].AnimState[8] = true;
                board[0, 0].AnimState[5] = true;
                board[0, 0].AnimState[6] = true;
                board[0, 0].AnimState[7] = board[1, cols - 2].AnimState[0];
                board[0, 0].AnimState[8] = true;
                board[0, cols - 1].AnimState[5] = true;
                board[0, cols - 1].AnimState[6] = true;
                board[0, cols - 1].AnimState[7] = true;
                board[0, cols - 1].AnimState[8] = board[1, 1].AnimState[0];

                // edge tiles
                for (int r = 1; r < rows - 1; r++)
                {
                    board[r, 0].AnimState[2] = !board[r, 1].AnimState[0];
                    board[r, 0].AnimState[5] = true;
                    board[r, 0].AnimState[6] = board[r - 1, 1].AnimState[0] && board[r, 1].AnimState[0] || board[r, 0].AnimState[6];
                    board[r, 0].AnimState[7] = board[r + 1, 1].AnimState[0] && board[r, 1].AnimState[0] || board[r, 0].AnimState[7];
                    board[r, 0].AnimState[8] = true;
                    board[r, cols - 1].AnimState[4] = !board[r, cols - 2].AnimState[0];
                    board[r, cols - 1].AnimState[5] = board[r - 1, cols - 2].AnimState[0] && board[r, cols - 2].AnimState[0];
                    board[r, cols - 1].AnimState[6] = true;
                    board[r, cols - 1].AnimState[7] = true;
                    board[r, cols - 1].AnimState[8] = board[r + 1, cols - 2].AnimState[0] && board[r, cols - 2].AnimState[0];

                }
                for (int c = 1; c < cols - 1; c++)
                {
                    board[0, c].AnimState[3] = !board[1, c].AnimState[0];
                    board[0, c].AnimState[5] = true;
                    board[0, c].AnimState[6] = true;
                    board[0, c].AnimState[7] = board[1, c + 1].AnimState[0] && board[1, c].AnimState[0] || board[0, c].AnimState[7];
                    board[0, c].AnimState[8] = board[1, c - 1].AnimState[0] && board[1, c].AnimState[0] || board[0, c].AnimState[8];
                    board[rows - 1, c].AnimState[1] = !board[rows - 2, c].AnimState[0];
                    board[rows - 1, c].AnimState[5] = board[rows - 2, c - 1].AnimState[0] && board[rows - 2, c].AnimState[0] || board[rows - 1, c].AnimState[5];
                    board[rows - 1, c].AnimState[6] = board[rows - 2, c + 1].AnimState[0] && board[rows - 2, c].AnimState[0] || board[rows - 1, c].AnimState[6];
                    board[rows - 1, c].AnimState[7] = true;
                    board[rows - 1, c].AnimState[8] = true;
                }

                // inside tiles
                for (int r = 1; r < rows - 1; r++)
                {
                    for (int c = 1; c < cols - 1; c++)
                    {
                        if (board[r, c].AnimState[0])
                        {
                            board[r, c].AnimState[1] = !board[r - 1, c].AnimState[0];
                            board[r, c].AnimState[2] = !board[r, c + 1].AnimState[0]; 
                            board[r, c].AnimState[3] = !board[r + 1, c].AnimState[0]; 
                            board[r, c].AnimState[4] = !board[r, c - 1].AnimState[0];
                            board[r, c].AnimState[5] = board[r - 1, c - 1].AnimState[0] && board[r, c - 1].AnimState[0] && board[r - 1, c].AnimState[0];
                            board[r, c].AnimState[6] = board[r - 1, c + 1].AnimState[0] && board[r, c + 1].AnimState[0] && board[r - 1, c].AnimState[0];
                            board[r, c].AnimState[7] = board[r + 1, c + 1].AnimState[0] && board[r, c + 1].AnimState[0] && board[r + 1, c].AnimState[0];
                            board[r, c].AnimState[8] = board[r + 1, c - 1].AnimState[0] && board[r, c - 1].AnimState[0] && board[r + 1, c].AnimState[0];
                        }
                    }
                }

                // all tile bridges
                foreach (TileParent tile in board)
                {
                    tile.AnimState[9]  = tile.AnimState[5] && tile.AnimState[8];
                    tile.AnimState[10] = tile.AnimState[6] && tile.AnimState[7];
                    tile.AnimState[11] = tile.AnimState[5] && tile.AnimState[6];
                    tile.AnimState[12] = tile.AnimState[7] && tile.AnimState[8];
                    tile.AnimState[13] = tile.AnimState[9] && tile.AnimState[10] && tile.AnimState[11] && tile.AnimState[12];
                }
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
            if (reader != null) { reader.Close(); }
        }

        private void DrawLevel(Level level)
        {
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    board[r, c].Draw(_spriteBatch);
                }
            }
        }

        public void StartGame()
        {
            currentState = GameState.PlayLevel;
            currentLevel = Level.Level1;
        }

        public void SelectLevel()
        {
            currentState = GameState.LevelSelect;
        }

        public void Level1()
        {
            currentState = GameState.PlayLevel;
            currentLevel = Level.Level1;
        }
        public void Level2()
        {
            currentState = GameState.PlayLevel;
            currentLevel = Level.Level2;
        }
        public void Level3()
        {
            currentState = GameState.PlayLevel;
            currentLevel = Level.Level3;
        }
        public void Level4()
        {
            currentState = GameState.PlayLevel;
            currentLevel = Level.Level4;
        }
        public void Level5()
        {
            currentState = GameState.PlayLevel;
            currentLevel = Level.Level5;
        }
        public void Level6()
        {
            currentState = GameState.PlayLevel;
            currentLevel = Level.Level6;
        }
        public void LastLevel()
        {
            currentState = GameState.PlayLevel;
            currentLevel = Level.EndLevel;
        }
    }
}
