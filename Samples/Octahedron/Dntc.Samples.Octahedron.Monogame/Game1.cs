using Dntc.Samples.Octahedron.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dntc.Samples.Octahedron.Monogame;

public class Game1 : Game
{
    private const int Width = 800;
    private const int Height = 480;
    
    private readonly GraphicsDeviceManager _graphics;
    private readonly ushort[] _rawBuffer = new ushort[Width * Height];
    private readonly Color[] _colorBuffer = new Color[Width * Height];
    private readonly Camera _camera = Camera.Default();
    private SpriteBatch _spriteBatch = null!;
    private Texture2D _texture = null!;
 
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";

        _camera.PixelWidth = Width;
        _camera.PixelHeight = Height;
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _texture = new Texture2D(_graphics.GraphicsDevice, 800, 480);
    }

    protected override void Update(GameTime gameTime)
    {
        for (var x = 0; x < _rawBuffer.Length; x++)
        {
            _rawBuffer[x] = 0;
        }
        
        OctahedronRenderer.Render(_rawBuffer, _camera, (float) gameTime.TotalGameTime.TotalSeconds);
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        for (var x = 0; x < _rawBuffer.Length; x++)
        {
            _colorBuffer[x] = Rgb565ToColor(_rawBuffer[x]);
        }
        
        _texture.SetData(_colorBuffer);
        _spriteBatch.Begin();
        _spriteBatch.Draw(_texture, Vector2.Zero, Color.White);
        _spriteBatch.End();
    }

    private static Color Rgb565ToColor(ushort original)
    {
        var tempRed = (original >> 11) * 8;
        var tempGreen = ((original & 0x0730) >> 5) *4;
        var tempBlue = (original & 0x1F) * 8;

        tempRed = Math.Min(tempRed, 255);
        tempBlue = Math.Min(tempBlue, 255);
        tempGreen = Math.Min(tempGreen, 255);

        return new Color(tempRed, tempGreen, tempBlue);
    }
    
}