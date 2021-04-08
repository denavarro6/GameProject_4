using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using GameProject_4.Collisions;

namespace GameProject_4
{

    /// <summary>
    /// An example game demonstrating the use of particle systems
    /// </summary>
    public class Game1 : Game, IParticleEmitter
    {
        #region Variables
        private ContentManager _content;
        private double _waitTimer;
        private SpriteFont _gameFont;
        private SoundEffect _hitBed;
        private SoundEffect _hitBurger;
        // private int height = ScreenManager.GraphicsDevice.Viewport.Hieght;
        private Vector2 _playerPosition = new Vector2(100, 500);
        private Vector2 _enemyPosition1 = new Vector2(2820, 450);
        private Vector2 _enemyPosition2 = new Vector2(450, 450);
        private Vector2 _enemyPosition3 = new Vector2(1000, 450);
        private Vector2 _enemyPosition4 = new Vector2(1600, 450);
        private bool _enemy1Land = true;
        private bool _enemy2Land = true;
        private bool _enemy3Land = true;
        private bool _enemy4Land = true;
        private int _gameState = 1;
        private BoundingRectangle _enemy1Bounds = new BoundingRectangle(new Vector2(2830, 450), 45, 45);
        private BoundingRectangle _enemy2Bounds = new BoundingRectangle(new Vector2(460, 450), 45, 45);
        private BoundingRectangle _enemy3Bounds = new BoundingRectangle(new Vector2(1010, 450), 45, 45);
        private BoundingRectangle _enemy4Bounds = new BoundingRectangle(new Vector2(1610, 450), 45, 45);
        //private BoundingRectangle[] _enemyBounds = new BoundingRectangle[] { new BoundingRectangle(new Vector2(140, 450),) }

        private Texture2D _bed;
        private Texture2D _player1;
        private Texture2D _enemy;
        private SpriteBatch spriteBatch;
        private double animationTimer;
        private bool flipped;
        private BoundingRectangle _playerbounds = new BoundingRectangle(new Vector2(100, 500), 40, 50);

        private BoundingRectangle _bedBounds = new BoundingRectangle(new Vector2(3760, 480 - 30), (float)56.5, (float)26.75);

        private short animationFrame = 0;

        private readonly Random _random = new Random();

        Texture2D _background;
        Texture2D _hitbox;
        Song _backgroundMusic;

        bool _isPlaying = false;

        #endregion
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        MouseState _priorMouse;
        ExplosionParticleSystem _explosions;
        FireworkParticleSystem _fireworks;

        public Vector2 Position { get; set; }

        public Vector2 Velocity { get; set; }

        /// <summary>
        /// Constructs an instance of the game
        /// </summary>
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        /// <summary>
        /// Initializes the game
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            RainParticleSystem rain = new RainParticleSystem(this, new Rectangle(15, -20, 3100, 10));
            Components.Add(rain);

            _explosions = new ExplosionParticleSystem(this, 20);
            Components.Add(_explosions);

            _fireworks = new FireworkParticleSystem(this, 20);
            Components.Add(_fireworks);

            if (_content == null)
                _content = new ContentManager(Services, "Content");
            _gameFont = _content.Load<SpriteFont>("gamefont");
            _background = _content.Load<Texture2D>("Background-3");
            _backgroundMusic = _content.Load<Song>("Ending");
            _bed = _content.Load<Texture2D>("bed");
            _hitBed = _content.Load<SoundEffect>("HitBed");
            _hitBurger = _content.Load<SoundEffect>("HitBurger");
            _hitbox = _content.Load<Texture2D>("redeclipse_bk");

            _player1 = _content.Load<Texture2D>("player-spritemap");
            _enemy = _content.Load<Texture2D>("hamburger");
            spriteBatch = new SpriteBatch(GraphicsDevice);


            // A real game would probably have more content than this sample, so
            // it would take longer to load. We simulate that by delaying for a
            // while, giving you a chance to admire the beautiful loading screen.
            Thread.Sleep(1000);

            base.Initialize();
        }

        /// <summary>
        /// Loads the game content
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// Updates the game.  Called every frame of the game loop.
        /// </summary>
        /// <param name="gameTime">The time in the game</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            MouseState currentMouse = Mouse.GetState();
            Vector2 mousePosition = new Vector2(currentMouse.X, currentMouse.Y);

            Velocity = mousePosition - Position;
            Position = mousePosition;

            if (_enemyPosition1.Y < 500 && _enemyPosition1.Y > 100 && _enemy1Land)
            {
                _enemyPosition1.Y = 0;
                _enemy1Bounds.Y = 0;
                _enemy1Land = false;
            }
            else if (_enemyPosition1.Y >= 480)
            {
                _enemy1Land = true;
            }
            else
            {
                _enemyPosition1.Y += 8;
                _enemy1Bounds.Y = _enemyPosition1.Y + 8;
            }


            if (_enemyPosition2.Y < 500 && _enemyPosition2.Y > 100 && _enemy2Land)
            {
                _enemyPosition2.Y = 0;
                _enemy2Bounds.Y = 0;
                _enemy2Land = false;
            }
            else if (_enemyPosition2.Y >= 480)
            {
                _enemy2Land = true;
            }
            else
            {
                _enemyPosition2.Y += 4;
                _enemy2Bounds.Y = _enemyPosition2.Y + 8;

            }

            if (_enemyPosition3.Y < 500 && _enemyPosition3.Y > 100 && _enemy3Land)
            {
                _enemyPosition3.Y = 0;

                _enemy3Bounds.Y = 0;
                _enemy3Land = false;
            }
            else if (_enemyPosition3.Y >= 480)
            {
                _enemy3Land = true;
            }
            else
            {
                _enemyPosition3.Y += 2;
                _enemy3Bounds.Y = _enemyPosition3.Y + 8;

            }

            if (_enemyPosition4.Y < 500 && _enemyPosition4.Y > 100 && _enemy4Land)
            {
                _enemyPosition4.Y = 0;
                _enemy4Bounds.Y = 0;
                _enemy4Land = false;
            }
            else if (_enemyPosition4.Y >= 480)
            {
                _enemy4Land = true;
            }
            else
            {
                _enemyPosition4.Y += 6;
                _enemy4Bounds.Y = _enemyPosition4.Y + 8;
            }


            var keyboardState = Keyboard.GetState();

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!

            if (_gameState == 1)
            {
                // Otherwise move the player position.
                var movement = Vector2.Zero;

                if (keyboardState.IsKeyDown(Keys.Left))
                {
                    if (animationTimer > .3)
                    {
                        animationFrame++;
                        if (animationFrame > 7) animationFrame = 0;
                        animationTimer -= .3;
                    }
                    movement.X--;

                    flipped = true;
                }

                if (keyboardState.IsKeyDown(Keys.Right))
                {
                    if (animationTimer > .3)
                    {
                        animationFrame++;
                        if (animationFrame > 7) animationFrame = 0;
                        animationTimer -= .3;
                    }

                    movement.X++;

                    flipped = false;
                }

                /*if (keyboardState.IsKeyDown(Keys.Up))
                {
                    if (animationTimer > .3)
                    {
                        animationFrame++;
                        if (animationFrame > 7) animationFrame = 0;
                        animationTimer -= .3;
                    }
                    movement.Y--;
                }*/

                if (keyboardState.IsKeyDown(Keys.Down))
                {
                    if (animationTimer > .3)
                    {
                        animationFrame++;
                        if (animationFrame > 7) animationFrame = 0;
                        animationTimer -= .3;
                    }
                    movement.Y++;
                }
                float t = (float)gameTime.ElapsedGameTime.TotalSeconds;
                Vector2 acceleration = new Vector2(0, 2);
                /*if (keyboardState.IsKeyDown(Keys.Space))
                {
                    acceleration = new Vector2(0, -7);
                    //jump.Play(.25f,0,0);
                }*/
                _playerPosition += acceleration;
                if (_playerPosition.Y < 50) _playerPosition.Y = 50;
                if (_playerPosition.Y > GraphicsDevice.Viewport.Height) _playerPosition.Y = GraphicsDevice.Viewport.Height;
                if (_playerPosition.X < 64) _playerPosition.X = 64;
                if (_playerPosition.X > 3810) _playerPosition.X = 3810;
                _playerbounds.X = _playerPosition.X;
                _playerbounds.Y = _playerPosition.Y - 64;

                if (_playerPosition.X > 3000 && _playerPosition.X < 3200)
                {
                    _fireworks.PlaceFirework(new Vector2(600, GraphicsDevice.Viewport.Height - 64));
                }
                if (_playerPosition.X > 3200 && _playerPosition.X < 3400)
                {
                    _fireworks.PlaceFirework(new Vector2(700, GraphicsDevice.Viewport.Height - 64));
                }
                if (_playerPosition.X > 3400)
                {
                    _fireworks.PlaceFirework(new Vector2(800, GraphicsDevice.Viewport.Height - 64));
                }
                if (_playerbounds.CollidesWith(_bedBounds))
                {
                    MediaPlayer.Stop();
                    _hitBed.Play();
                    _gameState = 0;
                    if(Keyboard.GetState().IsKeyDown(Keys.Enter))
                        Exit();
                    //ExitScreen();
                }
                if (_playerbounds.CollidesWith(_enemy2Bounds) || _playerbounds.CollidesWith(_enemy1Bounds) || _playerbounds.CollidesWith(_enemy3Bounds) || _playerbounds.CollidesWith(_enemy4Bounds))
                {
                    _hitBurger.Play();


                    var waitTime = new TimeSpan(0, 0, 5);
                    var waitUntil = DateTime.Now + waitTime;
                    _explosions.PlaceExplosion(new Vector2(400, _playerPosition.Y-40));
                    _playerPosition.X = _playerPosition.X - 50;
                }


                if (movement.Length() > 1)
                    movement.Normalize();

                _playerPosition += movement * 4f;
            }
            if (!_isPlaying)
            {
                MediaPlayer.Play(_backgroundMusic);
                MediaPlayer.IsRepeating = true;
                _isPlaying = true;
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the game.  Called every frame of the game loop.
        /// </summary>
        /// <param name="gameTime">The time in the game</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);

            float playerX = MathHelper.Clamp(_playerPosition.X, 300, 3340);
            float offsetX = 300 - playerX;

            Matrix transform = Matrix.CreateTranslation(offsetX, 0, 0);

            spriteBatch.Begin(transformMatrix: transform);
            spriteBatch.Draw(_background, Vector2.Zero, Color.White);
            //spriteBatch.DrawString(_gameFont, "// TODO", _playerPosition, Color.Green);
            //spriteBatch.DrawString(_gameFont, "Insert Gameplay Here",
            // _enemyPosition, Color.DarkRed);
            //spriteBatch.Draw(_enemy, _enemyPosition, source, Color.White, 0, new Vector2(64, 64), 1, spriteEffects, 0);
            // ScreenManager.SpriteBatch.
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            //Update animation frme

            SpriteEffects spriteEffects = (flipped) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            spriteBatch.Draw(_enemy, _enemyPosition1, null, Color.White, 0, new Vector2(0, 0), .25f, spriteEffects, 1);
            var rect = new Rectangle((int)(_enemy1Bounds.X),
                                            (int)(_enemy1Bounds.Y),
                                            (int)(_enemy1Bounds.Width), (int)(_enemy1Bounds.Height));
            //spriteBatch.Draw(_hitbox, rect, Color.White);
            var rect2 = new Rectangle((int)(_enemy2Bounds.X),
                                           (int)(_enemy2Bounds.Y),
                                           (int)(_enemy2Bounds.Width), (int)(_enemy2Bounds.Height));
            //spriteBatch.Draw(_hitbox, rect2, Color.White);
            spriteBatch.Draw(_enemy, _enemyPosition2, null, Color.White, 0, new Vector2(0, 0), .25f, spriteEffects, 1);


            spriteBatch.Draw(_enemy, _enemyPosition3, null, Color.White, 0, new Vector2(0, 0), .25f, spriteEffects, 1);
            var rect3 = new Rectangle((int)(_enemy3Bounds.X),
                                           (int)(_enemy3Bounds.Y),
                                           (int)(_enemy3Bounds.Width), (int)(_enemy3Bounds.Height));
            //spriteBatch.Draw(_hitbox, rect3, Color.White);

            spriteBatch.Draw(_enemy, _enemyPosition4, null, Color.White, 0, new Vector2(0, 0), .25f, spriteEffects, 1);
            var rect4 = new Rectangle((int)(_enemy4Bounds.X),
                                           (int)(_enemy4Bounds.Y),
                                           (int)(_enemy4Bounds.Width), (int)(_enemy4Bounds.Height));
            //spriteBatch.Draw(_hitbox, rect4, Color.White);


            var source = new Rectangle(animationFrame * 46, 150, 46, 50);
            spriteBatch.Draw(_player1, _playerPosition, source, Color.White, 0, new Vector2(0, 64), 1, spriteEffects, 1);
            var rect7 = new Rectangle((int)(_playerbounds.X),
                                           (int)(_playerbounds.Y),
                                           (int)(40), (int)(50));
            //spriteBatch.Draw(_hitbox, rect7, Color.White);
            spriteBatch.DrawString(_gameFont, "Insomnia", new Vector2(200, 64), Color.White, 0, new Vector2(64, 64), .75f, SpriteEffects.None, 0);
            spriteBatch.DrawString(_gameFont, "How  did  we  get  here?\nAnyways,  you  really  need  to  sleep.", new Vector2(GraphicsDevice.Viewport.Width - 300, 64), Color.CornflowerBlue, 0, new Vector2(64, 64), .5f, SpriteEffects.None, 0);
            spriteBatch.DrawString(_gameFont, "Watch  Out!", new Vector2(500, 140), Color.CornflowerBlue, 0, new Vector2(64, 64), .5f, SpriteEffects.None, 0);
            spriteBatch.DrawString(_gameFont, "Whew!  It's   Over.", new Vector2(1900, 140), Color.CornflowerBlue, 0, new Vector2(64, 64), .5f, SpriteEffects.None, 0);
            spriteBatch.DrawString(_gameFont, "Oops  I  Lied.", new Vector2(3000, 140), Color.CornflowerBlue, 0, new Vector2(64, 64), .5f, SpriteEffects.None, 0);
            spriteBatch.DrawString(_gameFont, "Almost  There!", new Vector2(3100, 64), Color.CornflowerBlue, 0, new Vector2(64, 64), .5f, SpriteEffects.None, 0);
            if (_gameState == 0)
            {
                spriteBatch.DrawString(_gameFont, "You  Win!\n Press Esc to Exit\nAnd Sleep", new Vector2(3500, 72), Color.LimeGreen, 0, new Vector2(64, 64), 1.25f, SpriteEffects.None, 0);
            }
            spriteBatch.Draw(_bed, new Vector2(3774, GraphicsDevice.Viewport.Height - 12), null, Color.White, 0, new Vector2(64, 64), .25f, SpriteEffects.None, 1.0f);
            var rect8 = new Rectangle((int)(_bedBounds.X),
                                           (int)(_bedBounds.Y),
                                           (int)(_bedBounds.Width), (int)(_bedBounds.Height));
            //spriteBatch.Draw(_hitbox, rect8, Color.White);
            //spriteBatch.Draw(_player1, _playerPosition, Color.White);


            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
