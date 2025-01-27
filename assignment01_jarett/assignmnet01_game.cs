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

    private Rectangle _currentSourceRectangle; // Manages the source rectangle for rendering
    private int _celWidth = 37;
    private int _celHeight = 45;

    private float _celTime = 0.1f; // Frame duration
    private float _celTimeElapsed = 0.0f;

    private int _celIndex = 0; // Current frame index
    private int _currentRow = 0; // Current animation row
    private int _celColumnCount = 6; // Number of frames per row (columns)


    KeyboardState keyboardState = Keyboard.GetState();

    private Vector2 _playerPosition;
    private Vector2 _playerVelocity;
    private float _gravity = 500f;       // Pixels per second²
    private float _jumpForce = -300f;    // Initial velocity when jumping
    private bool _isJumping = false;     // Track if the character is jumping
    private bool _isOnGround = false;    // Check if the character is grounded
    private bool _facingRight = true;

    //fish spin
    private CelAnimationSequence _fishSpinning;
    private CelAnimationPlayer _animationFish;

    
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
        _playerPosition = new Vector2(300, _WindowHeight - 100); // Initial position
        base.Initialize();
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

        _spriteBatch = new SpriteBatch(GraphicsDevice);
        Texture2D spriteSheet = Content.Load<Texture2D>("fish-spin");
        _fishSpinning = new CelAnimationSequence(spriteSheet, 258, 1 / 9f);
        _animationFish = new CelAnimationPlayer();
        //start animating!
        _animationFish.Play(_fishSpinning);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        KeyboardState state = Keyboard.GetState();

        // Determine which animation row to use based on input
        if (state.IsKeyDown(Keys.LeftShift)) // Punching animation (row 2)
        {
            _currentRow = 2;

        }
        else if (state.IsKeyDown(Keys.D)) // Walking Right (row 1)
        {
            _currentRow = 1;
            _playerVelocity.X = 200f;  // Move right
        }
        else if (state.IsKeyDown(Keys.A))  // Walking Right (row 1)
        {
            _currentRow = 1;
            _playerVelocity.X = -200f;  // Move right
        }
        else // Idle animation (row 0)
        {
            _currentRow = 0;
            _playerVelocity.X = 0f;
            
        }
        if (_isOnGround && state.IsKeyDown(Keys.Space))
        {
            _isJumping = true;
            _isOnGround = false;
            _playerVelocity.Y = _jumpForce; // Apply jump force
        }
        if (_playerVelocity.X > 0) // Moving right
        {
            _facingRight = true;
        }
        else if (_playerVelocity.X < 0) // Moving left
        {
            _facingRight = false;
        }
        // Update animation timing
        _celTimeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
        // Apply gravity
        if (!_isOnGround)
            _playerVelocity.Y += _gravity * deltaTime;

        // Update player position
        _playerPosition += _playerVelocity * deltaTime;

        // Simulate ground collision
        float groundY = _WindowHeight - 100; // Ground level
        if (_playerPosition.Y >= groundY)
        {
            _playerPosition.Y = groundY;
            _isOnGround = true;
            _isJumping = false;
            _playerVelocity.Y = 0;
        }
        if (_celTimeElapsed >= _celTime)
        {
            _celTimeElapsed -= _celTime;

            // Advance to the next frame in the current row
            _celIndex = (_celIndex + 1) % _celColumnCount;

            // Update the source rectangle for the current frame and row
            _currentSourceRectangle.X = _celIndex * _celWidth;
            _currentSourceRectangle.Y = _currentRow * _celHeight;
        }
        {
        _animationFish.Update(gameTime);

        base.Update(gameTime);
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
        _spriteBatch.Draw(
     _playerSpriteSheet,
     _playerPosition,
     _currentSourceRectangle,
     Color.White,
     0f,
     Vector2.Zero,
     1f,
     _facingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
     0f
 );

        _animationFish.Draw(_spriteBatch, Vector2.Zero, SpriteEffects.None);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
