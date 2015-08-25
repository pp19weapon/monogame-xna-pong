using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace Pong
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D tPaddle1, tPaddle2, tBall;

        Paddle paddle1;
        Paddle paddle2;
        Ball ball;

        SpriteFont font;

        int pointsOfPlayer1, pointsOfPlayer2 = 0;
        Vector2 textSize;

        Random rand = new Random();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        void ResetGame()
        {
            paddle1 = new Paddle(10, 200);
            paddle2 = new Paddle(770, 200);
            ball = new Ball(385, 285);
        }

        protected override void Initialize()
        {

            tPaddle1 = new Texture2D(this.GraphicsDevice, 20, 100);
            tPaddle2 = new Texture2D(this.GraphicsDevice, 20, 100);

            Color[] colorData = new Color[20 * 100];
            for (int i = 0; i < 2000; i++) {
                colorData[i] = Color.White;
            }

            tPaddle1.SetData<Color>(colorData);
            tPaddle2.SetData<Color>(colorData);

            ResetGame();

            base.Initialize();


        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            tBall = Content.Load<Texture2D>("ball");

            font = Content.Load<SpriteFont>("font");
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            font = null;
        }

        protected override void Update(GameTime gameTime)
        {
            UpdatePaddles();
            UpdateBall();
            CheckCollisions();
            textSize = font.MeasureString(pointsOfPlayer1 + " : " + pointsOfPlayer2);
            Debug.WriteLine(ball.vSpeed + " : " + ball.hSpeed);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            spriteBatch.Draw(tPaddle1, new Rectangle(paddle1.pos.X, paddle1.pos.Y, tPaddle1.Width, tPaddle1.Height), Color.White);
            spriteBatch.Draw(tPaddle2, new Rectangle(paddle2.pos.X, paddle2.pos.Y, tPaddle2.Width, tPaddle2.Height), Color.White);
            spriteBatch.Draw(tBall, new Rectangle(ball.pos.X, ball.pos.Y, tBall.Width, tBall.Height), Color.White);
            spriteBatch.DrawString(font, pointsOfPlayer1 + " : " + pointsOfPlayer2, new Vector2(Window.ClientBounds.Width / 2 - textSize.X, Window.ClientBounds.Height / 2 - textSize.Y - 100), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        void UpdatePaddles()
        {
            //Get input
            if (Keyboard.GetState().IsKeyDown(Keys.Up) || (Keyboard.GetState().IsKeyDown(Keys.W)) )
                paddle1.pos.Y -= paddle1.speed;

            if (Keyboard.GetState().IsKeyDown(Keys.Down) || (Keyboard.GetState().IsKeyDown(Keys.S)) )
                paddle1.pos.Y += paddle1.speed;

            //Enemy paddle
            if (paddle2.pos.Y + (tPaddle2.Height / 2) > ball.pos.Y)
                paddle2.pos.Y -= paddle2.speed;
            else if (paddle2.pos.Y + (tPaddle2.Height / 2) < ball.pos.Y)
                paddle2.pos.Y += paddle2.speed;

            if (paddle1.pos.Y + (tPaddle1.Height / 2) > ball.pos.Y)
                paddle1.pos.Y -= paddle1.speed;
            else if (paddle1.pos.Y + (tPaddle1.Height / 2) < ball.pos.Y)
                paddle1.pos.Y += paddle1.speed;

            //Check boundaries
            if (paddle1.pos.Y <= 10)
                paddle1.pos.Y = 10;
            if (paddle2.pos.Y <= 10)
                paddle2.pos.Y = 10;

            if (paddle1.pos.Y + tPaddle1.Height >= Window.ClientBounds.Height - 10)
                paddle1.pos.Y = Window.ClientBounds.Height - tPaddle1.Height - 10;
            if (paddle2.pos.Y + tPaddle2.Height >= Window.ClientBounds.Height - 10)
                paddle2.pos.Y = Window.ClientBounds.Height - tPaddle2.Height - 10;
        }

        void UpdateBall()
        {
            //Update position
            ball.pos.X += ball.hSpeed;
            ball.pos.Y += ball.vSpeed;

            //Check for boundaries
            //Bottom
            if (ball.pos.Y > (Window.ClientBounds.Height - 10 - tBall.Height))
                ball.vSpeed *= -1;

            //Top
            if (ball.pos.Y < 10)
                ball.vSpeed *= -1;
        }

        void CheckCollisions()
        {
            //check paddle1 if ball moving left, else check paddle2
            if (ball.hSpeed < 0)
            {
                //check if ball has surpassed paddle
                if (ball.pos.X < paddle1.pos.X + 20)
                {
                    //check if ball has hit paddle or went through
                    if ((ball.pos.Y + tBall.Height < paddle1.pos.Y) || (ball.pos.Y > paddle1.pos.Y + tPaddle1.Height))
                    {
                        //LOST!
                        ResetGame();
                        pointsOfPlayer2++;
                    }
                    else
                    {
                        Random rand = new Random(Guid.NewGuid().GetHashCode());
                        //ball hit - calculate new speeds
                        //speed of ball changes randomly - 3 to 6
                        if (ball.hSpeed < 0)
                        {
                            ball.hSpeed = rand.Next(3, 7);
                        }
                        else
                        {
                            ball.hSpeed = rand.Next(-6, -2);
                        }

                        if (ball.vSpeed < 0)
                            ball.vSpeed = rand.Next(3, 7);
                        else ball.vSpeed = rand.Next(-6, -2);

                        ball.vSpeed *= -1;
                    }
                }
            }
            else
            {
                //check if ball has surpassed paddle
                if (ball.pos.X + tBall.Width > paddle2.pos.X)
                {
                    //check if ball has hit paddle or went through
                    if ((ball.pos.Y + tBall.Height < paddle2.pos.Y) || (ball.pos.Y > paddle2.pos.Y + tPaddle2.Height))
                    {
                        //LOST!
                        ResetGame();
                        pointsOfPlayer1++;
                    }
                    else
                    {
                        //ball hit - calculate new speeds
                        //speed of ball changes randomly - 3 to 6
                        if (ball.hSpeed < 0)
                            ball.hSpeed = rand.Next(3, 7);
                        else ball.hSpeed = rand.Next(-6, -2);

                        if (ball.vSpeed < 0)
                            ball.vSpeed = rand.Next(3, 7);
                        else ball.vSpeed = rand.Next(-6, -2);

                        ball.vSpeed *= -1;
                    }
                }
            }
        }
    }
}
