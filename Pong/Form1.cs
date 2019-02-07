/*
 * Description:     A basic PONG simulator
 * Author:          Cieran Diebolt 
 * Date:            February 5th, 2019
 */

#region libraries

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Media;

#endregion

namespace Pong
{
    public partial class Form1 : Form
    {
        #region global values

        //graphics objects for drawing
        SolidBrush drawBrush = new SolidBrush(Color.White);
        Font drawFont = new Font("Courier New", 10);

        // Sounds for game
        SoundPlayer scoreSound = new SoundPlayer(Properties.Resources.score);
        SoundPlayer collisionSound = new SoundPlayer(Properties.Resources.collision);

        //determines whether a key is being pressed or not
        Boolean wKeyDown, sKeyDown, aKeyDown, dKeyDown, upKeyDown, downKeyDown, leftKeyDown, rightKeyDown;

        // check to see if a new game can be started
        Boolean newGameOk = true;

        //ball directions, speed, and rectangle
        Boolean ballMoveRight = true;
        Boolean ballMoveDown = true;
        const int BALL_SPEED = 3;
        Rectangle ball;

        //paddle speeds and rectangles
        const int PADDLE_SPEED = 3;
        Rectangle p1, p2;

        //player and game scores
        int player1Score = 0;
        int player2Score = 0;
        int gameWinScore = 2;  // number of points needed to win game

        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        // -- YOU DO NOT NEED TO MAKE CHANGES TO THIS METHOD
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //check to see if a key is pressed and set is KeyDown value to true if it has
            switch (e.KeyCode)
            {
                case Keys.W:
                    wKeyDown = true;
                    break;
                case Keys.S:
                    sKeyDown = true;
                    break;
                case Keys.A:
                    aKeyDown = true;
                    break;
                case Keys.D:
                    dKeyDown = true;
                    break;
                case Keys.Up:
                    upKeyDown = true;
                    break;
                case Keys.Down:
                    downKeyDown = true;
                    break;
                case Keys.Left:
                    leftKeyDown = true;
                    break;
                case Keys.Right:
                    rightKeyDown = true;
                    break;
                case Keys.Y:
                case Keys.Space:
                    if (newGameOk)
                    {
                        SetParameters();
                    }
                    break;
                case Keys.N:
                    if (newGameOk)
                    {
                        Close();
                    }
                    break;
            }
        }

        // -- YOU DO NOT NEED TO MAKE CHANGES TO THIS METHOD
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            //check to see if a key has been released and set its KeyDown value to false if it has
            switch (e.KeyCode)
            {
                case Keys.W:
                    wKeyDown = false;
                    break;
                case Keys.S:
                    sKeyDown = false;
                    break;
                case Keys.A:
                    aKeyDown = false;
                    break;
                case Keys.D:
                    dKeyDown = false;
                    break;
                case Keys.Up:
                    upKeyDown = false;
                    break;
                case Keys.Down:
                    downKeyDown = false;
                    break;
                case Keys.Left:
                    leftKeyDown = false;
                    break;
                case Keys.Right:
                    rightKeyDown = false;
                    break;
            }
        }

        /// <summary>
        /// sets the ball and paddle positions for game start
        /// </summary>
        private void SetParameters()
        {
            if (newGameOk)
            {
                player1Score = player2Score = 0;
                newGameOk = false;
                startLabel.Visible = false;
                gameUpdateLoop.Start();
            }

            //set starting position for paddles on new game and point scored 
            const int PADDLE_EDGE = 20;  // buffer distance between screen edge and paddle            

            p1.Width = p2.Width = 10;    //height for both paddles set the same
            p1.Height = p2.Height = 40;  //width for both paddles set the same

            //p1 starting position
            p1.X = PADDLE_EDGE;
            p1.Y = this.Height / 2 - p1.Height / 2;

            //p2 starting position
            p2.X = this.Width - PADDLE_EDGE - p2.Width;
            p2.Y = this.Height / 2 - p2.Height / 2;

            // TODO set Width and Height of ball
            ball.Width = ball.Height = 5;
            // TODO set starting X position for ball to middle of screen, (use this.Width and ball.Width)
            ball.X = this.Width / 2 + ball.Width / 2;
            // TODO set starting Y position for ball to middle of screen, (use this.Height and ball.Height)
            ball.Y = this.Height / 2 + ball.Height / 2;
        }

        /// <summary>
        /// This method is the game engine loop that updates the position of all elements
        /// and checks for collisions.
        /// </summary>
        private void gameUpdateLoop_Tick(object sender, EventArgs e)
        {
            #region update ball position

            // TODO create code to move ball either left or right based on ballMoveRight and using BALL_SPEED
            if (ballMoveRight == true)
            {
                ball.X = ball.X + BALL_SPEED;
            }
            else if (ballMoveRight == false)
            {
                ball.X = ball.X - BALL_SPEED;
            }
            // TODO create code move ball either down or up based on ballMoveDown and using BALL_SPEED
            if (ballMoveDown == true)
            {
                ball.Y = ball.Y + BALL_SPEED;
            }
            else if (ballMoveDown == false)
            {
                ball.Y = ball.Y - BALL_SPEED;
            }
            #endregion

            #region update paddle positions

            if (wKeyDown == true && p1.Y > 0)
            {
                // TODO create code to move player 1 paddle up using p1.Y and PADDLE_SPEED
                p1.Y = p1.Y - PADDLE_SPEED;
            }

            // TODO create an if statement and code to move player 1 paddle down using p1.Y and PADDLE_SPEED
            if (sKeyDown == true && p1.Y < this.Height - p1.Height)
            {
                p1.Y = p1.Y + PADDLE_SPEED;
            }

            // TODO create an if statement and code to move player 2 paddle up using p2.Y and PADDLE_SPEED
            if (upKeyDown == true && p2.Y > 0)
            {
                p2.Y = p2.Y - PADDLE_SPEED;
            }
            // TODO create an if statement and code to move player 2 paddle down using p2.Y and PADDLE_SPEED
            if (downKeyDown == true && p2.Y < this.Height - p2.Height)
            {
                p2.Y = p2.Y + PADDLE_SPEED;
            }
            #endregion

            #region ball collision with top and bottom lines

            if (ball.Y <= 0) // if ball hits top line
            {
                // TODO use ballMoveDown boolean to change direction
                ballMoveDown = true;
                // TODO play a collision sound
                collisionSound.Play();
            }
            // TODO In an else if statement use ball.Y, this.Height, and ball.Width to check for collision with bottom line
            // If true use ballMoveDown down boolean to change direction
            else if (ball.Y >= this.Height - ball.Height)
            {
                ballMoveDown = false;
                collisionSound.Play();
            }

            #endregion

            #region ball collision with paddles

            // TODO create if statment that checks p1 collides with ball and if it does
            // --- play a "paddle hit" sound and
            // --- use ballMoveRight boolean to change direction

            // TODO create if statment that checks p2 collides with ball and if it does
            // --- play a "paddle hit" sound and
            // --- use ballMoveRight boolean to change direction

            /*  ENRICHMENT
             *  Instead of using two if statments as noted above see if you can create one
             *  if statement with multiple conditions to play a sound and change direction
             */

            if (p1.IntersectsWith(ball) || p2.IntersectsWith(ball))
            {
                ballMoveRight = !ballMoveRight;
                collisionSound.Play();
            }

            #endregion

            #region ball collision with side walls (point scored)

            if (ball.X < 0)  // ball hits left wall logic
            {
                // TODO
                // --- play score sound
                // --- update player 2 score
                scoreSound.Play();
                player2Score++;

                // TODO use if statement to check to see if player 2 has won the game. If true run 
                // GameOver method. Else change direction of ball and call SetParameters method.
                if (player2Score == 3)
                {
                    GameOver("Player 2");
                }
                else
                {
                    ballMoveRight = !ballMoveRight;
                    SetParameters();
                }
            }

            // TODO same as above but this time check for collision with the right wall
            if (ball.X > this.Width - ball.Width)
            {
                scoreSound.Play();
                player1Score++;

                if (player1Score == 3)
                {
                    GameOver("Player 1");
                }
                else
                {
                    ballMoveRight = !ballMoveRight;

                    SetParameters();
                }
            }
            #endregion

            //refresh the screen, which causes the Form1_Paint method to run
            this.Refresh();
        }

        /// <summary>
        /// Displays a message for the winner when the game is over and allows the user to either select
        /// to play again or end the program
        /// </summary>
        /// <param name="winner">The player name to be shown as the winner</param>
        private void GameOver(string winner)
        {
            newGameOk = true;

            // TODO create game over logic
            // --- stop the gameUpdateLoop
            gameUpdateLoop.Stop();
            // --- show a message on the startLabel to indicate a winner, (need to Refresh).
            startLabel.Visible = true;
            startLabel.Text = (winner + "\nWins!");
            // --- pause for two seconds 
            this.Refresh();
            Thread.Sleep(2000);
            // --- use the startLabel to ask the user if they want to play again
            startLabel.Text = "Rematch?";

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            // TODO draw paddles using FillRectangle
            e.Graphics.FillRectangle(drawBrush, p1);
            e.Graphics.FillRectangle(drawBrush, p2);
            // TODO draw ball using FillRectangle
            e.Graphics.FillEllipse(drawBrush, ball);
            // TODO draw scores to the screen using DrawString
            e.Graphics.DrawString(player1Score + "", drawFont, drawBrush, 40, 30);
            e.Graphics.DrawString(player2Score + "", drawFont, drawBrush, this.Width - 40, 30);
        }

    }
}
