using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System;

namespace pong_csharp
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Paddle paddleLeft;
        Paddle paddleRight;

        Ball ball;

        SoundEffect soundEffect;
        SpriteFont font;

        // Score position;
        Vector2 scoreLeftPosition;
        Vector2 scoreRightPosition;

        int scoreLeft = 0;
        int scoreRight = 0;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            //Change the screen resolution
            _graphics.PreferredBackBufferWidth = 858;
            _graphics.PreferredBackBufferHeight = 525;
            _graphics.ApplyChanges();
            
            float centerHeight = _graphics.PreferredBackBufferHeight / 2;
            float centerWidth = _graphics.PreferredBackBufferWidth / 2;

            // Paddles    
            paddleLeft = new Paddle(this, "paddle", new Vector2(16, centerHeight - 64));
            paddleRight = new Paddle(this, "paddle", new Vector2(_graphics.PreferredBackBufferWidth - 32, centerHeight - 64));

            // Score
            scoreLeftPosition = new Vector2(centerWidth - 128, 32);
            scoreRightPosition = new Vector2(centerWidth + 128, 32);

            // Ball
            ball = new Ball(this, "ball", new Vector2(centerWidth - 8, centerHeight));
            ball.changeDirection(new Vector2(1, 0));
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            soundEffect = Content.Load<SoundEffect>("hit_ball");
            font = Content.Load<SpriteFont>("fontGame");
        }

        // Move the given paddle and prevent that exceed the screen limits
        public void movePaddle(GameTime gameTime, KeyboardState keys, Keys key1, Keys key2, Paddle paddle) {
            if (keys.IsKeyDown(key1) && paddle.position.Y + 64 > 0)
                paddle.position.Y -= paddle.speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (keys.IsKeyDown(key2) && paddle.position.Y + 64 < _graphics.PreferredBackBufferHeight)
                paddle.position.Y += paddle.speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        
        public bool isColliding(Vector4 rect1, Vector4 rect2) {
            if (rect1.X > rect2.X + rect2.Z
                || rect1.X + rect2.Z < rect2.X
                || rect1.Y > rect2.Y + rect2.W
                || rect1.Y + rect1.W < rect2.Y
            )
                return false;
            return true;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var keys = Keyboard.GetState();
            movePaddle(gameTime, keys, Keys.W, Keys.S, paddleLeft);
            movePaddle(gameTime, keys, Keys.Up, Keys.Down, paddleRight);

            ball.Move(gameTime);
            // collision with screen
            if (ball.position.Y <= 0) {
                soundEffect.Play();
                ball.updateSpeed();
                ball.speedY *= -1;
            }
            if (ball.position.Y + ball.texture.Height >= _graphics.PreferredBackBufferHeight) {
                soundEffect.Play();
                ball.updateSpeed();
                ball.speedY *= -1;
            }

            // Reset ball position
            if (ball.position.X < 0) {
                scoreRight++;
                ball.resetBall();
            }
            if (ball.position.X > _graphics.PreferredBackBufferWidth) {
                scoreLeft++;
                ball.resetBall();
            }

            // Paddle right collision
            if (isColliding(ball.getRect(), paddleRight.getRect())) {
                var pos = Math.Floor(ball.position.Y - paddleRight.position.Y);
                if (pos >= 0 && pos <= 39)
                    ball.changeDirection(new Vector2(-1));
                if (pos >= 40 && pos <= 64)
                    ball.changeDirection(new Vector2(-1, 0));
                if (pos >= 65 && pos <= 128)
                    ball.changeDirection(new Vector2(-1, 1));
                soundEffect.Play();
                ball.updateSpeed();
            }

            // Paddle left collision
            if (isColliding(ball.getRect(), paddleLeft.getRect())) {
                var pos = Math.Floor(ball.position.Y - paddleLeft.position.Y);
                if (pos >= 0 && pos <= 39)
                    ball.changeDirection(new Vector2(1, -1));
                if (pos >= 40 && pos <= 64)
                    ball.changeDirection(new Vector2(1, 0));
                if (pos >= 65 && pos <= 128)
                    ball.changeDirection(new Vector2(1));
                soundEffect.Play();
                ball.updateSpeed();
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            paddleLeft.Draw(_spriteBatch);
            paddleRight.Draw(_spriteBatch);
            ball.Draw(_spriteBatch);
            // Draw Score
            _spriteBatch.DrawString(font, scoreLeft.ToString(), scoreLeftPosition, Color.White);
            _spriteBatch.DrawString(font, scoreRight.ToString(), scoreRightPosition, Color.White);
            
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
