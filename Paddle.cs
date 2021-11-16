using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Paddle {
	public Texture2D texture;
	public Vector2 position;
	
	public float speed = 250f;

	public Paddle(Game game, string pathToTexture, Vector2 position) {
		this.texture = game.Content.Load<Texture2D>(pathToTexture);
		this.position = position;
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

	public Vector4 getRect() {
		return new Vector4(this.position.X, this.position.Y, this.texture.Width, this.texture.Height);
	}
}