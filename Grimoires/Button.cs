using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using PersonalPunchClock.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace PersonalPunchClock.Grimoires
{
    public class AddButton
    {

        private SpriteBatch spritebatch;
        private GameTime gametime;

        private Rectangle ClickZone;
        private ButtonState LastMouseState;
        private double LastGameTime;

        private Random Rand;

        private Texture2D Texture;

        public string ID { get; set; }
        public Game1 Parent;

        public AddButton(Game1 parent, GameTime gt, SpriteBatch sb) {
            Parent = parent;

            Rand = Parent.Random;

            spritebatch = sb;
            gametime = gt;

            Texture = Parent.Content.Load<Texture2D>("Add Button");
        }
        public void Draw()
        {
            ClickZone = new Rectangle(15, 25, 100, 100);

            spritebatch.Begin();

            spritebatch.Draw(Texture, ClickZone, Color.White);

            spritebatch.End();
        }

        public void Update()
        {
            MouseState mouse = Mouse.GetState();
            if (ClickZone.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed && LastMouseState == ButtonState.Released && Parent.IsActive)
            {
                int NewID = 1;
                
                
                while (Parent.TimersSet.Where(x => x.ID == "PunchTimer" + NewID.ToString()).ToList<PunchTimer>().Count() >= 1)
                {
                    NewID++;
                }

                int R = Rand.Next(1, 25) * 10;
                int G = Rand.Next(1, 25) * 10;
                int B = Rand.Next(1, 25) * 10;

                Parent.TimersSet.Add(new PunchTimer(Parent, "PunchTimer" + NewID.ToString(), gametime, spritebatch) { BaseColor = new Color(R, G, B), PunchColor = new Color(G, B, R) });
                Parent.TimersSet.Last().Initialize();
                Parent.CalculateClockGrid();
                LastGameTime = gametime.TotalGameTime.TotalSeconds;
            }
            LastMouseState = mouse.LeftButton;
        }

        
    }

    public class ResetButton
    {

        private SpriteBatch spritebatch;
        private GameTime gametime;

        private Rectangle ClickZone;
        private ButtonState LastMouseState;
        private double LastGameTime;

        private Random Rand = new Random();

        private Texture2D Texture;

        public string ID { get; set; }
        public Game1 Parent;

        public ResetButton(Game1 parent, GameTime gt, SpriteBatch sb)
        {
            Parent = parent;

            spritebatch = sb;
            gametime = gt;

            Texture = Parent.Content.Load<Texture2D>("Reset Button");
        }
        public void Draw()
        {
            ClickZone = new Rectangle(15, 150, 100, 100);

            spritebatch.Begin();

            spritebatch.Draw(Texture, ClickZone, Color.White);

            spritebatch.End();
        }

        public void Update()
        {
            MouseState mouse = Mouse.GetState();
            if (ClickZone.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed && LastMouseState == ButtonState.Released && Parent.IsActive)
            {
                Parent.ResetEvents.RaiseButtonEvent(new EventArgs());
            }
            LastMouseState = mouse.LeftButton;
        }


    }

}
