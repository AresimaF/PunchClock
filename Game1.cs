using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input.InputListeners;
using PersonalPunchClock.Events;
using PersonalPunchClock.Grimoires;
using System;
using System.Collections.Generic;


namespace PersonalPunchClock
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D AddButtonTexture;
        private Texture2D SubtractButtonTexture;

        private MouseListener mouseListener = new MouseListener();


        public List<PunchTimer> TimersSet { get; } = new List<PunchTimer>();
        private AddButton AddButton;
        private ScrollBar ScrollBar;
        public ClockEvents ClockEvents { get; } = new ClockEvents();
        public ButtonEvents ButtonEvents { get; } = new ButtonEvents();
        public KillEvents KillEvents { get; } = new KillEvents();
        public List<string>TimersToKill { get; set; } = new List<string>();
        public float ScrollLocation { get; set; } = 0f;
        public float MaximumScroll { get; set; } = 0f;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Window.Title = "Personal Punch Clock 1.2 - by Aresima";

            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += OnResize;
            mouseListener.MouseWheelMoved += MouseHandle;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here           

            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.ApplyChanges();


            

            KillEvents.KillEvent += KillTimer;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            AddButtonTexture = Content.Load<Texture2D>("Add Button");
            SubtractButtonTexture = Content.Load<Texture2D>("Subtract Button");

            AddButton = new AddButton(this, new GameTime(), _spriteBatch);
            ScrollBar = new ScrollBar(_spriteBatch, this);

            TimersSet.Add(new PunchTimer(this, "PunchTimer1", new GameTime(), _spriteBatch));

            CalculateClockGrid();

            if (TimersSet.Count > 0)
            {
                foreach (PunchTimer timer in TimersSet)
                {
                    timer.Initialize();
                }
            }

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            AddButton.Update();

            mouseListener.Update(gameTime);

            if (TimersSet.Count > 0)
            {
                foreach (PunchTimer timer in TimersSet)
                {
                    timer.Update(gameTime);
                }
                foreach (string timerID in TimersToKill)
                {
                    if (TimersSet.Count > 1)
                    {
                        TimersSet.RemoveAll(x => x.ID == timerID);
                    }
                }
                TimersToKill.Clear();
                CalculateClockGrid();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Lavender);

            // TODO: Add your drawing code here

            if (TimersSet.Count > 0)
            {
                for (int i = 0; i < TimersSet.Count; i++)
                {
                    PunchTimer timer = TimersSet[i];
                    timer.Draw();
                }
            }

            AddButton.Draw();
            ScrollBar.Draw();

            base.Draw(gameTime);
        }

        public void OnResize(Object sender, EventArgs e)
        {

            CalculateClockGrid();

           
        }

        public void CalculateClockGrid()
        {
            int Width = _graphics.GraphicsDevice.Viewport.Width;
            int Height = _graphics.GraphicsDevice.Viewport.Height -100;
            int RequiredCells = TimersSet.Count;

            float scale = 1f;

            if (RequiredCells == 1)
            {
                TimersSet[0].Reposition(new Vector2(200, 200), Width / 800);
            }
            else if (RequiredCells == 2)
            {
                scale = ((float)Width - 200f) / 800f / 2f;
                int x = (int)(800 * scale);
                int y = 100;

                for (int i = 0; i < TimersSet.Count; i++)
                {                    
                    TimersSet[i].Reposition(new Vector2(x * i + 100, y), scale);
                }
            }
            else
            {
                scale = ((float)Width - 200f) / 800f / 3f;
                int x = (int)(800 * scale);
                int y = 100;

                for (int i = 0; i < TimersSet.Count; i++)
                {
                    int xmult = 1;

                    if (i + 1 == 1) { xmult = 1; }
                    else { xmult = ((i + 4) % 3); }

                    if (xmult == 0) { xmult = 3; }

                    //if (i >= 4) { xmult += 1; }

                        TimersSet[i].Reposition(new Vector2(100 + x * (xmult - 1), y + (int)((800 * scale) * ((i) / 3))), scale);
                }
            }

            MaximumScroll = (int)(TimersSet.Count / 3) * (800 * scale);

        }

        private void KillTimer(object? sender, KillEventArgs e)
        {
            TimersToKill.Add(e.ID);   
        }

        private void MouseHandle(object? sender,  MouseEventArgs e)
        {
            if (this.IsActive)
            {
                if (e.ScrollWheelDelta != 0)
                {
                    int ScrollWheelDelta = e.ScrollWheelDelta * -1;
                    
                    if (ScrollWheelDelta > 0)
                    {
                        if (ScrollLocation + ScrollWheelDelta > MaximumScroll)
                        {
                            ScrollLocation = MaximumScroll;
                        }
                        else
                        {
                            ScrollLocation += ScrollWheelDelta;
                        }
                    }
                    else if (ScrollWheelDelta < 0)
                    {
                        if (ScrollLocation + ScrollWheelDelta < 0)
                        {
                            ScrollLocation = 0;
                        }
                        else
                        {
                            ScrollLocation += ScrollWheelDelta;
                        }
                    }
                }
            }
        }

        public Vector2 GetScreenSize()
        {
            return new Vector2(_graphics.GraphicsDevice.Viewport.Width, _graphics.GraphicsDevice.Viewport.Height);
        }

    }
}
