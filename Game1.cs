using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JcGame;

public class Game1 : Game
{
    Player player;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D playerTexture;
    

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        playerTexture = Content.Load<Texture2D>("player");
        
        player = new Player(new Vector2(350, 400), playerTexture, 100, 10, 20, 10);
    }
    protected override void Update(GameTime gameTime)
    {
        var playerPosition = player.Position;
        var keyboardState = Keyboard.GetState();
        
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        if (keyboardState.IsKeyDown(Keys.Left))
        {
            playerPosition.X -= player.Speed;
        }

        if (keyboardState.IsKeyDown(Keys.Right))
        {
            playerPosition.X += player.Speed;
        }

        if (keyboardState.IsKeyDown(Keys.Down))
        {
            playerPosition.Y += player.Speed;
        }

        if (keyboardState.IsKeyDown(Keys.Up))
        {
            playerPosition.Y -= player.Speed;
        }
        player.Position = playerPosition;    
        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
       
        player.DrawPlayer(_spriteBatch);
        
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
