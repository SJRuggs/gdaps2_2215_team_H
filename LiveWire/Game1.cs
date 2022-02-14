using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;

namespace LiveWire
{
    /// <summary>
    /// Level File Format:
    /// 
    /// rows,cols,tileWidth,tileHeight
    /// (grid of ints separated by commas)
    /// 
    /// Level File Legend:
    /// 0 - empty space
    /// 1 - physical tile
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
        TestLevel,
        TestLevel2,
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
        private Tile[,] board;
        private int rows;
        private int cols;
        private int tileWidth;
        private int tileHeight;



        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // screen settings
            screenWidth = 1920;
            screenHeight = 1080;
            _graphics.PreferredBackBufferWidth = screenWidth;
            _graphics.PreferredBackBufferHeight = screenHeight;
            //_graphics.IsFullScreen = true;
            _graphics.ApplyChanges();

            // objects and states
            currentState = GameState.MainMenu;
            currentLevel = Level.TestLevel;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            basicFont = Content.Load<SpriteFont>("BaseText");
            tileSpriteSheet = Content.Load<Texture2D>("LiveWireTiles");
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
                    if (SingleKeyPress(Keys.Enter, kbState, prevKbState))
                    {
                        currentState = GameState.LevelSelect;
                    }
                    break;



                case GameState.LevelSelect:
                    // TEMPORARY transition
                    if (mState.LeftButton == ButtonState.Pressed && prevMState.LeftButton == ButtonState.Released)
                    {
                        currentState = GameState.PlayLevel;
                        NewLevel(currentLevel);
                    }
                    break;



                case GameState.PlayLevel:
                    // TEMPORARY transition
                    if (SingleKeyPress(Keys.Enter, kbState, prevKbState))
                    {
                        if (currentLevel == Level.EndLevel) { currentState = GameState.MainMenu;  currentLevel = Level.TestLevel; }
                        else { NewLevel(currentLevel++); }
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
                    _spriteBatch.DrawString(
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
                        Color.Black);
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

                    DrawLevel(currentLevel);
                    break;



                case GameState.PlayLevel:
                    // display level
                    DrawLevel(currentLevel);

                    // TEMPORARY display
                    _spriteBatch.DrawString(
                        basicFont,
                        "Play Level Template",
                        new Vector2(
                                screenWidth / 2 - (int)basicFont.MeasureString("Play Level Template").X / 2,
                                screenHeight / 2 - (int)basicFont.MeasureString("Play Level Template").Y),
                        Color.White);

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

                // create board
                board = new Tile[rows, cols];
                for (int r = 0; r < rows; r++)
                {
                    newLine = reader.ReadLine();
                    for (int c = 0; c < cols; c++)
                    {
                        // TODO: must be changed when interactable objects are implemented
                        temporaryBool = newLine[c] != '0';
                        board[r, c] = new Tile(c * tileWidth, r * tileHeight, tileWidth, tileHeight, tileSpriteSheet, temporaryBool);
                    }
                }

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
                    board[rows - 1, c].AnimState[5] = board[rows - 2, c + 1].AnimState[0] && board[rows - 2, c].AnimState[0] || board[rows - 1, c].AnimState[5];
                    board[rows - 1, c].AnimState[6] = board[rows - 2, c - 1].AnimState[0] && board[rows - 2, c].AnimState[0] || board[rows - 1, c].AnimState[6];
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
                foreach (Tile tile in board)
                {
                    tile.AnimState[9]  = tile.AnimState[5] && tile.AnimState[8];
                    tile.AnimState[10] = tile.AnimState[6] && tile.AnimState[7];
                    tile.AnimState[11] = tile.AnimState[5] && tile.AnimState[6];
                    tile.AnimState[12] = tile.AnimState[7] && tile.AnimState[8];
                    tile.AnimState[13] = tile.AnimState[9] && tile.AnimState[10] && tile.AnimState[11] && tile.AnimState[12];
                }
            }
            catch (Exception e) { Console.WriteLine(e); }
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
    }
}
