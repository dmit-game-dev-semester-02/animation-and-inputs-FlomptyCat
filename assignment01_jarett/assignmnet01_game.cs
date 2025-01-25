using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Assignment01_Jarett;
 
public class Assignment01_Game : Game
{
    private const int _WindowWidth = 960;
    private const int _WindowHeight = 540;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _forestBackground;
    private Texture2D _ground;

    public Assignment01_Game()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = _WindowWidth;
        _graphics.PreferredBackBufferHeight = _WindowHeight;
        _graphics.ApplyChanges();

        base.Initialize();
    }

    protected override void LoadContent()
    {
         _spriteBatch = new SpriteBatch(GraphicsDevice);
        _forestBackground = Content.Load<Texture2D>("forest-background");
        _ground = Content.Load<Texture2D>("grass-block");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        // GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
        // _spriteBatch.Draw(_forestBackground, Vector2.Zero, Color.White);
        // _spriteBatch.Draw(_ground, Vector2.Zero, Color.White);

        
        
        // //_spriteBatch.Draw(_beetleImage, new Vector2(_x, _y), Color.White);
        

        // Draw the background
_spriteBatch.Draw(_forestBackground, Vector2.Zero, Color.White);

// Get the width of the ground texture
int groundWidth = _ground.Width;
int groundHeight = _ground.Height;

// Set the scale factor for the ground
float scale = 0.4f; // Adjust this to make the ground smaller (e.g., 0.5 means half size)

// Calculate the scaled width and height
int scaledWidth = (int)(groundWidth * scale);
int scaledHeight = (int)(groundHeight * scale);

// Loop to repeat the ground texture along the bottom
for (int x = 0; x < GraphicsDevice.Viewport.Width; x += scaledWidth)
{
    _spriteBatch.Draw(
        _ground, 
        new Vector2(x, GraphicsDevice.Viewport.Height - scaledHeight), 
        null, 
        Color.White, 
        0f, 
        Vector2.Zero, 
        scale, 
        SpriteEffects.None, 
        0f
    );
}

// Uncomment and add any other sprites as needed
// _spriteBatch.Draw(_beetleImage, new Vector2(_x, _y), Color.White);

        _spriteBatch.End();
        base.Draw(gameTime);
    }
}
