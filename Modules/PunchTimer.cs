using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using PersonalPunchClock.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Vector3 = Microsoft.Xna.Framework.Vector3;


namespace PersonalPunchClock.Modules
{
    public class PunchTimer
    {
        private Game1 Parent;
        private SpriteBatch _spriteBatch;
        private GameTime _gameTime;

        public Color BaseColor { get; set; } = new Color(160, 190, 255);
        public Color PunchColor { get; set; } = new Color(255, 195, 95);
        public int SecondsPassed { get; set; } = 0;
        private TimeSpan Time { get { return TimeSpan.FromSeconds(SecondsPassed); } }
        private double LastGameTime;
        public bool Active { get; set; } = false;
        public string ID { get; set; }
        private float Scale { get; set; }
        private Vector2 Position { get; set; }

        private Rectangle ClickZone;
        private Rectangle RemoveClickZone;
        private ButtonState LastMouseState;

        private Texture2D BaseOutlineTex;
        private Texture2D BaseTex;
        private Texture2D PlungerOutlineTex;
        private Texture2D PlungerTex;
        private Texture2D FaceTex;
        private Texture2D RemoveButton;

        private SpriteFont FaceFont;
        private SpriteFont LabelFont;

        public TextInput Label;

        private Song ClickSound;


        public PunchTimer(Game1 parent, string id, GameTime gameTime, SpriteBatch spriteBatch)
        {
            Parent = parent;
            _gameTime = gameTime;
            _spriteBatch = spriteBatch;

            ID = id;

            Parent.ClockEvents.ClockEvent += Trigger;
        }

        public void Initialize()
        {
            BaseOutlineTex = Parent.Content.Load<Texture2D>("Punch Timer Base Outline");
            PlungerOutlineTex = Parent.Content.Load<Texture2D>("Punch Timer Plunger Outline");
            BaseTex = Parent.Content.Load<Texture2D>("Punch Timer Base");
            PlungerTex = Parent.Content.Load<Texture2D>("Punch Timer Plunger");
            FaceTex = Parent.Content.Load<Texture2D>("Punch Timer Clock Face");
            RemoveButton = Parent.Content.Load<Texture2D>("Subtract Button");

            FaceFont = Parent.Content.Load<SpriteFont>("Digital Readout");
            LabelFont = Parent.Content.Load<SpriteFont>("Labels");

            ClickSound = Parent.Content.Load<Song>("Switch Click");

            Vector3 TextColor = BaseColor.ToVector3();

            Label = new TextInput(_spriteBatch, _gameTime, Parent) { Font = LabelFont , ForegroundColor = new Color((int)(255 - (TextColor.X * 255)), (int)(255 - (TextColor.Y * 255)), (int)(255 - (TextColor.Z * 255))), Value = "Timer1"};

        }

        public void Update(GameTime gt)
        {
            

            if (gt.TotalGameTime.TotalSeconds > LastGameTime && (gt.TotalGameTime.TotalSeconds - LastGameTime) > 1 && Active)
            {
                SecondsPassed += Convert.ToInt32(Math.Truncate(gt.TotalGameTime.TotalSeconds - LastGameTime));
                LastGameTime = gt.TotalGameTime.TotalSeconds;                
            }

            MouseState mouse = Mouse.GetState();
            if (ClickZone.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed && LastMouseState == ButtonState.Released  && Parent.IsActive)
            {
                Parent.ClockEvents.RaiseClockEvent(new ClockEventArgs() { Activate = !Active, ID = this.ID });
                LastGameTime = gt.TotalGameTime.TotalSeconds;
            }
            if (RemoveClickZone.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed && LastMouseState == ButtonState.Released && Parent.IsActive)
            {
                Parent.KillEvents.RaiseKillEvent(new KillEventArgs() { ID = this.ID });
            }
            LastMouseState = mouse.LeftButton;

            Label.Update(gt);

        }

        public void Draw()
        {


            ClickZone = new Rectangle((int)Position.X + 50, (int)Position.Y, (int)(Scale * 800) - 50, (int)(Scale * 300));
            RemoveClickZone = new Rectangle((int)Position.X + (int)(625 * Scale), (int)Position.Y + (int)(685 * Scale), (int)(Scale * 60), (int)(Scale * 60));

            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            _spriteBatch.Draw(BaseOutlineTex, new Rectangle((int)Position.X, (int)Position.Y, (int)(Scale * 800), (int)(Scale * 800)), BaseColor);
            _spriteBatch.Draw(PlungerOutlineTex, new Rectangle((int)Position.X, (int)Position.Y + (int)((Scale * 100) * Convert.ToInt32(Active)), (int)(Scale * 800), (int)(Scale * 800)), PunchColor);
            _spriteBatch.Draw(PlungerTex, new Rectangle((int)Position.X, (int)Position.Y + (int)((Scale * 100) * Convert.ToInt32(Active)), (int)(Scale * 800), (int)(Scale * 800)), PunchColor);
            _spriteBatch.Draw(BaseTex, new Rectangle((int)Position.X, (int)Position.Y, (int)(Scale * 800), (int)(Scale * 800)), BaseColor);
            _spriteBatch.Draw(FaceTex, new Rectangle((int)Position.X, (int)Position.Y, (int)(Scale * 800), (int)(Scale * 800)), new Color(new Microsoft.Xna.Framework.Vector3(255, 255, 255)));
            _spriteBatch.Draw(RemoveButton, new Rectangle((int)Position.X + (int)(625 * Scale), (int)Position.Y + (int)(685 * Scale), (int)(Scale * 60), (int)(Scale * 60)), Color.White * 0.6f);

            _spriteBatch.DrawString(FaceFont, Time.ToString(@"hh\:mm\:ss"), new Vector2((int)Position.X + (int)(Scale * 190), (int)Position.Y + (int)(Scale * 505)), new Color(50, 200, 50), 0f, new Vector2(0,0), (float)Scale, SpriteEffects.None, 0);

            Label.Draw(new Rectangle((int)Position.X + (int)(150 * Scale), (int)Position.Y + (int)(350 * Scale), (int)(Scale * 500), (int)(Scale * 125)));

            _spriteBatch.End();
        }

        private void Trigger(object? sender, ClockEventArgs e)
        {
            if (e.ID == ID)
            {
                Active = e.Activate;
                MediaPlayer.Play(ClickSound);
            }
        }

        public void Reposition (Vector2 position, float scale = 0)
        {
            Position = position;

            if (scale != 0)
            {
                Scale = scale;
            }
        }
    }
}
