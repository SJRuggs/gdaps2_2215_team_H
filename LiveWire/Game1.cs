using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace LiveWire
{
    /// <summary>
    /// TODO: Add Summary
    /// </summary>

    // --- FINITE STATE MACHINE : GAMESTATE ---
    enum GameState
    {
        MainMenu,
        LevelSelect,
        PlayLevel,
    }

    public class Game1 : Game
    {

        // --- VARIABLE DECLARATIONS ---
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private int screenWidth;
        private int screenHeight;
        private SpriteFont basicFont;

        private StreamReader reader;
        private StreamWriter writer;

        private KeyboardState kbState;
        private KeyboardState prevKbState;
        private MouseState mState;
        private MouseState prevMState;

        private Player player;
        private int playerWidth;
        private int playerHeight;



        GameState currentState;
        Tile[,] board; // TODO: Implement reader / writer logic

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
            _graphics.ApplyChanges();

            // objects and states
            currentState = GameState.MainMenu;


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            basicFont = Content.Load<SpriteFont>("BaseText");

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            // update mnk
            kbState = Keyboard.GetState();
            mState = Mouse.GetState();

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
                    }
                    break;



                case GameState.PlayLevel:
                    // TEMPORARY transition
                    if (SingleKeyPress(Keys.Enter, kbState, prevKbState))
                    {
                        currentState = GameState.MainMenu;
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
                    break;



                case GameState.PlayLevel:
                    // TEMPORARY display
                    _spriteBatch.DrawString(
                        basicFont,
                        "Play Level Template",
                        new Vector2(
                                screenWidth / 2 - (int)basicFont.MeasureString("Play Level Template").X / 2,
                                screenHeight / 2 - (int)basicFont.MeasureString("Play Level Template").Y),
                        Color.Black);

                    _spriteBatch.DrawString(
                        basicFont,
                        "Press Enter To Advance",
                        new Vector2(
                                screenWidth / 2 - (int)basicFont.MeasureString("Press enter To Advance").X / 2,
                                screenHeight / 2),
                        Color.Black);
                    break;
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        // --- HELPER METHODS ---

        // detects a single key press, returns a boolean value
        private bool SingleKeyPress(Keys key, KeyboardState kbState, KeyboardState prevKbState)
        {
            if (prevKbState != null)
                return kbState.IsKeyDown(key) && !prevKbState.IsKeyDown(key);
            return true;
        }
    }
}
