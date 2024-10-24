using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JcGame;

public class Game1 : Game
{
    Player player;
    SmallEnemy smallEnemy;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D playerTexture;
    private Texture2D eyelanderTexture;
    

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
        eyelanderTexture = Content.Load<Texture2D>("eyelander");
        
        player = new Player(new Vector2(350, 400), playerTexture, 100, 10, 20, 5);
        smallEnemy = new SmallEnemy(new Vector2(380, 20), eyelanderTexture);
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
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
       
        player.DrawPlayer(_spriteBatch);
        smallEnemy.DrawSmallEnemy(_spriteBatch);
        
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
