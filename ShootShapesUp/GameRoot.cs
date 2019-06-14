using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Linq;

namespace ShootShapesUp
{
    public class GameRoot : Game
    {
        // some helpful static properties
        public static GameRoot Instance { get; private set; }
        public static Viewport Viewport { get { return Instance.GraphicsDevice.Viewport; } }
        public static Vector2 ScreenSize { get { return new Vector2(Viewport.Width, Viewport.Height); } }
        public static GameTime GameTime { get; private set; }

        public static Texture2D Player { get; private set; }
        public static Texture2D Seeker { get; private set; }
        public static Texture2D Bullet { get; private set; }
        public static Texture2D Pointer { get; private set; }
        public static Texture2D Background { get; private set; }
        public static Texture2D Background2 { get; private set; }
        public static Texture2D Background3 { get; private set; }
        public static Texture2D Killer { get; private set; }    

        public static SpriteFont Font { get; private set; }

        public static Song Music { get; private set; }

        private static readonly Random rand = new Random();

        private static SoundEffect[] explosions;
        // return a random explosion sound
        public static SoundEffect Explosion { get { return explosions[rand.Next(explosions.Length)]; } }

        private static SoundEffect[] shots;
        public static SoundEffect Shot { get { return shots[rand.Next(shots.Length)]; } }

        private static SoundEffect[] spawns;
        public static SoundEffect Spawn { get { return spawns[rand.Next(spawns.Length)]; } }

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        GameState _state = GameState.MainMenu;
        enum GameState {MainMenu, LevelOne, LevelTwo,  FinalLevel, EndGame, GameCompl};
        
  

        public GameRoot()
        {
            Instance = this;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = @"Content";

            //graphics.PreferredBackBufferWidth = 800;
           // graphics.PreferredBackBufferHeight = 600;
           graphics.IsFullScreen = true;
           graphics.ApplyChanges();
        }
        //function for main menu
        public void UpdateMainMenu(GameTime gameTime)
        {
           

            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                
                _state = GameState.LevelOne;

            }

            base.Update(gameTime);
            EntityManager.Update();
            EnemySpawner.Update();
            Input.Update();
        }

        //function for level 1
        public void UpdateLevelOne(GameTime gameTime)
        {
            
            if (PlayerStatus.Score > 75)
            {
                _state = GameState.LevelTwo;
            }
            if (PlayerStatus.Lives <= 0)

            {
                _state = GameState.EndGame;
                PlayerStatus.Reset();

            }

            Input.Update();
            EntityManager.Update();
            EnemySpawner.Update();
            base.Update(gameTime);
        }
        //function for level 2
        void UpdateLevelTwo(GameTime gameTime)
        {
            
            if (PlayerStatus.Score > 250)
            {
                _state = GameState.FinalLevel;
            }
            if (PlayerStatus.Lives <= 0)

            {
                _state = GameState.EndGame;
                PlayerStatus.Reset();

            }

            Input.Update();
            EntityManager.Update();
            EnemySpawner.Update2();
            base.Update(gameTime);
        }

        //function for final boss fight 
        public void UpdateFinalLevel(GameTime gameTime)
            
        {

            if (PlayerStatus.Score > 750)
            {
                _state = GameState.GameCompl;
            }
            if (PlayerStatus.Lives <= 0)

            {
                _state = GameState.EndGame;
                PlayerStatus.Reset();

            }
            Input.Update();
            EntityManager.Update();
            EnemySpawner.Update();
            EnemySpawner.Update2();
            base.Update(gameTime);

        }



        //function for gameover
        void UpdateEndGame(GameTime gameTime)
        {
            if (Input.WasButtonPressed(Buttons.Back) || Input.WasKeyPressed(Keys.Escape))
                this.Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {

                _state = GameState.MainMenu;
                PlayerStatus.Reset();

            }
            
            base.Update(gameTime);
            EntityManager.Update();
            EnemySpawner.Update();
            Input.Update();

        }

        //function for completed game
        void UpdateGameCompl(GameTime gameTime)
        {
            if (Input.WasButtonPressed(Buttons.Back) || Input.WasKeyPressed(Keys.Escape))
                this.Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {

                _state = GameState.MainMenu;
                PlayerStatus.Reset();
            }

            base.Update(gameTime);
            EntityManager.Update();
            EnemySpawner.Update();
            Input.Update();

        }


        //displays main menu text before game starts
        void DrawMainMenu(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            spriteBatch.DrawString(Font, "Planet EARTH, now a no mans land where humans", new Vector2(20,50), Color.White);
            spriteBatch.DrawString(Font, "are roaring against the enemies aliens that took over", new Vector2(30, 100), Color.White);
            spriteBatch.DrawString(Font, "their planet", new Vector2(300, 150), Color.White);
            spriteBatch.DrawString(Font, "Press Enter to start the game", new Vector2(200,250), Color.White);
            spriteBatch.End();

        }
        // displays level one sprites
        void DrawLevelOne(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Opaque);
            spriteBatch.Draw(Background, new Rectangle(0, 0, 800, 480), Color.Plum);
            spriteBatch.End();
            // Draw entities. Sort by texture for better batching.
            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive);
            spriteBatch.DrawString(Font, "The revolution", new Vector2(300,0), Color.White);
            spriteBatch.DrawString(Font, "Level One", new Vector2(650,50), Color.White);
            EntityManager.Draw(spriteBatch);
            
            spriteBatch.Draw(GameRoot.Pointer, Input.MousePosition, Color.White);
            spriteBatch.DrawString(Font, string.Format("Lives: {0}", PlayerStatus.Lives), new Vector2(5), Color.White);
            DrawRightAlignedString(string.Format("Score: {0}", PlayerStatus.Score), 5);


            spriteBatch.End();
            spriteBatch.End();
            
            // Draw user interface
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);

            spriteBatch.End();

            base.Draw(gameTime);
        }
        
        void DrawLevelTwo(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Opaque);
            spriteBatch.Draw(Background3, new Rectangle(0, 0, 800, 480), Color.LightCoral);
            spriteBatch.End();
            // Draw entities. Sort by texture for better batching.
            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive);
            spriteBatch.DrawString(Font, "The revolution", new Vector2(300, 0), Color.White);
            spriteBatch.DrawString(Font, "Level Two", new Vector2(650, 50), Color.White);
            EntityManager.Draw(spriteBatch);
            
            spriteBatch.Draw(GameRoot.Pointer, Input.MousePosition, Color.White);
            spriteBatch.DrawString(Font, string.Format("Lives: {0}", PlayerStatus.Lives), new Vector2(5), Color.White);
            DrawRightAlignedString(string.Format("Score: {0}", PlayerStatus.Score), 5);


            spriteBatch.End();
            spriteBatch.End();
            // Draw user interface
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);

            spriteBatch.End();

            base.Draw(gameTime);

        }

        //help display final level design
        void DrawFinalLevel(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Opaque);
            spriteBatch.Draw(Background2, new Rectangle(0, 0, 800, 480), Color.Blue);
            spriteBatch.End();
            // Draw entities. Sort by texture for better batching.
            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive);
            spriteBatch.DrawString(Font, "The revolution", new Vector2(300, 0), Color.White);
            spriteBatch.DrawString(Font, "Final Level", new Vector2(650, 50), Color.White);
            EntityManager.Draw(spriteBatch);

            spriteBatch.Draw(GameRoot.Pointer, Input.MousePosition, Color.White);
            spriteBatch.DrawString(Font, string.Format("Lives: {0}", PlayerStatus.Lives), new Vector2(5), Color.White);
            DrawRightAlignedString(string.Format("Score: {0}", PlayerStatus.Score), 5);


            spriteBatch.End();
            spriteBatch.End();
            // Draw user interface
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);

            spriteBatch.End();

            base.Draw(gameTime);

        }
        //displays game over page
        void DrawEndGame(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            spriteBatch.DrawString(Font, "Game Over, press Space to restart the game", new Vector2(80,150), Color.White);
            spriteBatch.End();
        }

        //displays completed game page
        void DrawGameCompl(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            spriteBatch.DrawString(Font, "You have saved Earth", new Vector2(275, 150), Color.White);
            spriteBatch.DrawString(Font, "press Space to restart the game", new Vector2(200, 200), Color.White);
            spriteBatch.End();
        }

        

        protected override void Initialize()
        {
            base.Initialize();

            EntityManager.Add(PlayerShip.Instance);

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(GameRoot.Music);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Player = Content.Load<Texture2D>("Art/Player");
            Seeker = Content.Load<Texture2D>("Art/Seeker");
            Killer = Content.Load<Texture2D>("Art/Killer");
            Bullet = Content.Load<Texture2D>("Art/Bullet");
            Pointer = Content.Load<Texture2D>("Art/Pointer");
            Background = Content.Load<Texture2D>("Art/p2");
            Background2 = Content.Load<Texture2D>("Art/p3");
            Background3 = Content.Load<Texture2D>("Art/p1");

            Font = Content.Load<SpriteFont>("Font");

            Music = Content.Load<Song>("Sound/Music");

            // These linq expressions are just a fancy way loading all sounds of each category into an array.
            explosions = Enumerable.Range(1, 8).Select(x => Content.Load<SoundEffect>("Sound/explosion-0" + x)).ToArray();
            shots = Enumerable.Range(1, 4).Select(x => Content.Load<SoundEffect>("Sound/shoot-0" + x)).ToArray();
            spawns = Enumerable.Range(1, 8).Select(x => Content.Load<SoundEffect>("Sound/spawn-0" + x)).ToArray();
        }
        //usd to help run all the sprites used and give it functions.
        protected override void Update(GameTime gameTime)
        {
            GameTime = gameTime;
           

            // Allows the game to exit
            if (Input.WasButtonPressed(Buttons.Back) || Input.WasKeyPressed(Keys.Escape))
                this.Exit();
            

            base.Update(gameTime);
            switch (_state)
            {
                case GameState.MainMenu:
                    UpdateMainMenu(gameTime);
                    break;
                case GameState.LevelOne:
                   UpdateLevelOne(gameTime);
                    break;
                case GameState.LevelTwo:
                  UpdateLevelTwo(gameTime);
                    break;
                case GameState.FinalLevel:
                    UpdateFinalLevel(gameTime);
                    break;
                case GameState.EndGame:
                   UpdateEndGame(gameTime);
                    break;
                case GameState.GameCompl:
                    UpdateGameCompl(gameTime);
                    break;


            }
        }


        protected override void Draw(GameTime gameTime)
        {
            base.Update(gameTime);
            switch (_state)
            {

                case GameState.MainMenu:
                    DrawMainMenu(gameTime);
                    break;
                case GameState.LevelOne:
                    DrawLevelOne(gameTime);
                    break;
                case GameState.LevelTwo:
                    DrawLevelTwo(gameTime);
                    break;
                case GameState.FinalLevel:
                    DrawFinalLevel(gameTime);
                    break;
                case GameState.EndGame:
                    DrawEndGame(gameTime);
                    break;
                case GameState.GameCompl:
                    DrawGameCompl(gameTime);
                    break;

                    
            }

            }

        private void DrawRightAlignedString(string text, float y)
        {
            var textWidth = GameRoot.Font.MeasureString(text).X;
            spriteBatch.DrawString(GameRoot.Font, text, new Vector2(ScreenSize.X - textWidth - 5, y), Color.White);
        }
    }
}
