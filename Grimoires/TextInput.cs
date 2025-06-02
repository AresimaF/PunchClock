using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Input;
using MonoGame.Extended.Input.InputListeners;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace PersonalPunchClock.Grimoires
{
    public class TextInput
    {
        private Game1 Parent;
        private SpriteBatch spriteBatch;
        private GameTime gameTime;
        private MouseListener mouseListener = new MouseListener();
        private KeyboardListener keyboardListener = new KeyboardListener();
        
        public SpriteFont Font { get; set; }
        public Color ForegroundColor { get; set; } = Color.White;
        public string Value { get; set; } = "";
        public Rectangle Position { get; set; } = Rectangle.Empty;
        public bool Active { get; set; } = false;

        private ButtonState LastMouseState;

        //Text Cursor
        private double LastCursorUpdate;
        private bool CursorVisible;
        private int CursorLocation;

        //Storage
        private string AllowableCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz_-1234567890 ";


        public TextInput(SpriteBatch spritebatch, GameTime gametime, Game1 parent)
        {
            this.spriteBatch = spritebatch;
            this.gameTime = gametime;
            this.Parent = parent;

            mouseListener.MouseDown += ClickCheck;
            keyboardListener.KeyPressed += KeyboardHandle;
        }

        public void Update(GameTime gt)
        {
            mouseListener.Update(gameTime);
            keyboardListener.Update(gameTime);

            gameTime = gt;
            
            CursorCheck();
            
        }

        public void Draw(Rectangle rectangle)
        {
            Position = rectangle;
            

            //Finding Text Pos + size
            Vector2 position = new Vector2(Position.X, Position.Y);
            Vector2 scale = new Vector2((float)Position.Width / 600, (float)Position.Height / 150);

            //Cursor
            float CharacterWidth;

            if (CursorLocation < Value.Length)
            {
                CharacterWidth = Font.MeasureString((Value.Remove(CursorLocation, Value.Length - CursorLocation))).X * scale.X; // -Calculate this
            }
            else
            {
                { CharacterWidth = Font.MeasureString(Value).X * scale.X; }
            }
            Vector2 CursorPos1 = new Vector2(position.X + (CharacterWidth), position.Y);
            Vector2 CursorPos2 = new Vector2(CursorPos1.X, CursorPos1.Y + (150 * scale.Y));



            //Background
            Color bg = Color.Black;
            if (Active)
            {
                bg = Color.Red;
            }
            //spriteBatch.DrawRectangle(rectangle, Color.Black, 2, 0);

            //Text
            spriteBatch.DrawString(Font, Value, position, ForegroundColor, 0, new Vector2( 0, 0), scale, SpriteEffects.None, 0);

            //Cursor
            if (CursorVisible)
            {
                spriteBatch.DrawLine(CursorPos1, CursorPos2, Color.Black, 3);
            }
            else
            {
                spriteBatch.DrawLine(CursorPos1, CursorPos2, Color.Black * 0, 3);

            }
            


        }

        private void ClickCheck(object sender, MouseEventArgs args)
        {
            if (Parent.IsActive)
            {
                Vector2 scale = new Vector2((float)Position.Width / 600, (float)Position.Height / 150);

                if (Position.Contains(args.Position))
                {
                    Active = true;
                    //place cursor at closest click spot
                    float TextWidth = Font.MeasureString(Value).X * scale.X;
                    float CharacterWidth = TextWidth / Value.Length;

                    float MouseRelativePosition = args.Position.X - Position.X;
                    CursorLocation = (int)Math.Round(MouseRelativePosition / CharacterWidth);

                    if (CursorLocation > Value.Length)
                    {
                        CursorLocation = Value.Length;
                    }
                    else if (CursorLocation < 0)
                    {
                        CursorLocation = 0;
                    }

                }
                else
                {
                    Active = false;
                }
            }
        }

        private void CursorCheck()
        {
            if (Active)
            {
                if (gameTime.TotalGameTime.TotalSeconds - LastCursorUpdate > 0.5)
                {
                    if (CursorVisible) { CursorVisible = false; }
                    else {  CursorVisible = true; }
                        LastCursorUpdate = gameTime.TotalGameTime.TotalSeconds;                    
                }
            }
            else
            {
                CursorVisible = false;
            }
        }

        private void KeyboardHandle(object sender, KeyboardEventArgs args)
        {
            Vector2 scale = new Vector2((float)Position.Width / 600, (float)Position.Height / 150);

            if (Active && Parent.IsActive)
            {
                if (args.Character.HasValue && AllowableCharacters.Contains(args.Character.Value) && Font.MeasureString(Value).X * scale.X <= Position.Width - (50 * scale.X))
                {
                    Value = Value.Insert(CursorLocation, args.Character.ToString());
                    CursorLocation++;
                }
                else if (args.Key == Keys.Back && CursorLocation != 0)
                {
                    CursorLocation--;
                    Value = Value.Remove(CursorLocation, 1);

                }
                else if (args.Key == Keys.Left && CursorLocation > 0)
                {
                    CursorLocation--;
                }
                else if (args.Key == Keys.Right && CursorLocation <= Value.Length - 1)
                {
                    CursorLocation++;
                }
                else if (args.Key == Keys.Up)
                {
                    CursorLocation = 0;
                }
                else if (args.Key == Keys.Down)
                {
                    CursorLocation = Value.Length;
                }
                else if (args.Key == Keys.Enter)
                {
                    Active = false;
                }
            }
        }
    }
}
