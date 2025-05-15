using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalPunchClock.Modules
{
    internal class TextInput
    {
        private SpriteBatch spriteBatch;
        private GameTime gameTime;
        
        public SpriteFont Font { get; set; }
        public Color ForegroundColor { get; set; } = Color.White;
        public string Value { get; set; } = string.Empty;

        public TextInput(SpriteBatch spritebatch, GameTime gametime)
        {
            this.spriteBatch = spritebatch;
            this.gameTime = gametime;
        }

        public void Update()
        {
            //Find way to pull input events, convert to textbox
        }

        public void Draw(Rectangle rectangle)
        {
            //Finding Text Pos + size
            Vector2 position = new Vector2(rectangle.X, rectangle.Y);
            Vector2 scale = new Vector2((float)rectangle.Width, (float)rectangle.Height);

            spriteBatch.Begin();
            
            //Text
            spriteBatch.DrawString(Font, Value, position, ForegroundColor, 0, new Vector2( 0, 0), scale, SpriteEffects.None, 0);

            //Draw a line on top for a cursor, don't do it in-text

            spriteBatch.End();

        }
    }
}
