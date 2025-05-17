using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Input.InputListeners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalPunchClock.Grimoires
{
    public class ScrollBar
    {

        private Game1 Parent;
        private SpriteBatch spriteBatch;

        private Texture2D ScrollBarEnd;
        private Texture2D ScrollBarMiddle;
        private Texture2D ScrollBarCursor;

        public ScrollBar(SpriteBatch spritebatch, Game1 parent)
        {
            this.spriteBatch = spritebatch;
            this.Parent = parent;

            ScrollBarEnd = Parent.Content.Load<Texture2D>("Scroll Bar End");
            ScrollBarMiddle = Parent.Content.Load<Texture2D>("Scroll Bar Middle");
            ScrollBarCursor = Parent.Content.Load<Texture2D>("Scroll Bar Cursor");

        }

        public void Update(GameTime gt)
        {

        }

        public void Draw()
        {
            Vector2 ScreenSize = Parent.GetScreenSize();

            float MovementRange = ScreenSize.Y - 150;

            float CursorPos = (Parent.ScrollLocation / Parent.MaximumScroll) * MovementRange;

            spriteBatch.Begin();

            spriteBatch.Draw(ScrollBarMiddle, new Rectangle((int)ScreenSize.X - 75, 50, 75, (int)ScreenSize.Y - 100), Color.White * 0.5f);

            spriteBatch.Draw(ScrollBarEnd, new Rectangle((int)ScreenSize.X - 75, -25, 75, 75), Color.White * 0.5f);
            spriteBatch.Draw(ScrollBarEnd, new Rectangle((int)ScreenSize.X - 75, (int)ScreenSize.Y - 50, 75, 75), null, Color.White * 0.5f, 0, new Vector2(0,0), SpriteEffects.FlipVertically, 0);

            spriteBatch.Draw(ScrollBarCursor, new Rectangle((int)ScreenSize.X - 87, 25 + (int)CursorPos, 100, 100), null, Color.White * 0.75f, 0, new Vector2(0, 0), SpriteEffects.None, 0);

            spriteBatch.End();
        }

        
    }
}
