using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Assignment01_Jarett;

public class Assignment01_Game : Game
{
    private SpriteBatch _spriteBatch;
    private Texture2D _forestBackground;
    private const int _WindowWidth = 960;
    private const int _WindowHeight = 540;
    private GraphicsDeviceManager _graphics;

    private Texture2D _ground;
    private Texture2D _playerSpriteSheet;
    private CelAnimationPlayer _playerAnimator;

    private Rectangle _currentSourceRectangle; // Manages the source rectangle for rendering
    private int _celWidth = 37;
    private int _celHeight = 45;

    private float _celTime = 0.1f; // Frame duration
    private float _celTimeElapsed = 0.0f;

    private int _celIndex = 0; // Current frame index
    private int _currentRow = 0; // Current animation row
    private int _celColumnCount = 6; // Number of frames per row (columns)
    private int _celRowCount = 3; // Number of rows in the sprite sheet

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
        _playerSpriteSheet = Content.Load<Texture2D>("clearBackgroundStickman");

        // Initialize the first frame of the idle animation (row 0)
        _currentSourceRectangle = new Rectangle(0, 0, _celWidth, _celHeight);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        KeyboardState state = Keyboard.GetState();

        // Determine which animation row to use based on input
        if (state.IsKeyDown(Keys.Right)) // Running animation (row 2)
        {
            _currentRow = 2;
            
        }
        else if (state.IsKeyDown(Keys.Up)) // Walking animation (row 1)
        {
            _currentRow = 1;
            
        }
        else // Idle animation (row 0)
        {
            _currentRow = 0;
            
        }
        
        // Update animation timing
        _celTimeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_celTimeElapsed >= _celTime)
        {
            _celTimeElapsed -= _celTime;

            // Advance to the next frame in the current row
            _celIndex = (_celIndex + 1) % _celColumnCount;

            // Update the source rectangle for the current frame and row
            _currentSourceRectangle.X = _celIndex * _celWidth;
            _currentSourceRectangle.Y = _currentRow * _celHeight;
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        // Draw the background
        _spriteBatch.Draw(_forestBackground, Vector2.Zero, Color.White);

        // Draw the ground
        float scale = 0.4f;
        int groundWidth = (int)(_ground.Width * scale);
        int groundHeight = (int)(_ground.Height * scale);

        for (int x = 0; x < GraphicsDevice.Viewport.Width; x += groundWidth)
        {
            _spriteBatch.Draw(
                _ground,
                new Vector2(x, GraphicsDevice.Viewport.Height - groundHeight),
                null,
                Color.White,
                0f,
                Vector2.Zero,
                scale,
                SpriteEffects.None,
                0f
            );
        }

        // Draw the player animation
        Vector2 playerPosition = new Vector2(200, GraphicsDevice.Viewport.Height - groundHeight - _celHeight);
        _spriteBatch.Draw(_playerSpriteSheet, playerPosition, _currentSourceRectangle, Color.White);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
