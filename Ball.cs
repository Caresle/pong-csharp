using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Ball {
	public Texture2D texture;
	public Vector2 position;
	
	private Vector2 originalPosition;
	
	public static float speed = 250f;
	public float speedX = speed;
	public float speedY = speed;

	public Ball(Game game, string pathToTexture, Vector2 position) {
		this.texture = game.Content.Load<Texture2D>(pathToTexture);
		this.position = position;
		this.originalPosition = position;
	}
	
	public void Draw(SpriteBatch _spriteBatch) {
		_spriteBatch.Draw(
			this.texture,
			this.position,
			null,
			Color.White,
			0f,
			Vector2.Zero,
			Vector2.One,
			SpriteEffects.None,
			0f
		);
	}

	public void changeDirection(Vector2 direction) {
		this.speedX = speed * direction.X;
		this.speedY = speed * direction.Y; 
	}

	public void Move(GameTime gameTime) {
		this.position.Y += this.speedY * (float)gameTime.ElapsedGameTime.TotalSeconds;
		this.position.X += this.speedX * (float)gameTime.ElapsedGameTime.TotalSeconds;
	}

	public void resetBall() {
		this.position = this.originalPosition;
	}

	public Vector4 getRect() {
		return new Vector4(this.position.X, this.position.Y, this.texture.Width, this.texture.Height);
	}
}