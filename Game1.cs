using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace pong_csharp
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Paddle paddleLeft;
        Paddle paddleRight;

        Ball ball;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 858;
            _graphics.PreferredBackBufferHeight = 525;
            _graphics.ApplyChanges();
            float centerHeight = _graphics.PreferredBackBufferHeight / 2;
            float centerWidth = _graphics.PreferredBackBufferWidth / 2;
            paddleLeft = new Paddle(this, "paddle", new Vector2(16, centerHeight - 64));
            paddleRight = new Paddle(this, "paddle", new Vector2(_graphics.PreferredBackBufferWidth - 32, centerHeight - 64));

            ball = new Ball(this, "ball", new Vector2(centerWidth - 8, centerHeight));
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        // Move the given paddle and prevent that exceed the screen limits
        public void movePaddle(GameTime gameTime, KeyboardState keys, Keys key1, Keys key2, Paddle paddle) {
            if (keys.IsKeyDown(key1) && paddle.position.Y + 64 > 0)
                paddle.position.Y -= paddle.speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (keys.IsKeyDown(key2) && paddle.position.Y + 64 < _graphics.PreferredBackBufferHeight)
                paddle.position.Y += paddle.speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var keys = Keyboard.GetState();
            movePaddle(gameTime, keys, Keys.W, Keys.S, paddleLeft);
            movePaddle(gameTime, keys, Keys.Up, Keys.Down, paddleRight);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            paddleLeft.Draw(_spriteBatch);
            paddleRight.Draw(_spriteBatch);
            ball.Draw(_spriteBatch);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
