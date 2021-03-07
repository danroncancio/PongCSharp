using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.ConfigFlag;
using System.Numerics;
using System;
using System.Windows;

namespace PongCSharp
{
    static class Game
    {
        const int SCREEN_WIDTH = 1280;
        public const int SCREEN_HEIGHT = 720;

        static int framesCounter;

        static Player player1 = new Player();
        static Player player2 = new Player();
        static Ball ball = new Ball();

        static void Main()
        {
            SetConfigFlags(FLAG_VSYNC_HINT | FLAG_MSAA_4X_HINT | FLAG_WINDOW_HIGHDPI);
            InitWindow(SCREEN_WIDTH, SCREEN_HEIGHT, "Pongboi");


            InitGame();

            framesCounter = 0;

            SetTargetFPS(60);

            while (!WindowShouldClose())
            {
                // Change state of ball
                if (IsKeyPressed(KeyboardKey.KEY_P))
                {
                    if (!ball.active) ball.active = true;
                    else ball.active = false;
                }

                if (IsKeyPressed(KeyboardKey.KEY_M))
                {
                    // NOTE: Requires FLAG_WINDOW_RESIZABLE enabled!
                    if (IsWindowState(FLAG_WINDOW_RESIZABLE)) ClearWindowState(FLAG_WINDOW_RESIZABLE);
                    else SetWindowState(FLAG_WINDOW_RESIZABLE);
                    if (IsWindowState(FLAG_WINDOW_MAXIMIZED)) RestoreWindow();
                    else MaximizeWindow();
                }

                // Player 1 movement
                if (player1.body.y > 10)
                {
                    if (IsKeyDown(KeyboardKey.KEY_W)) player1.body.y -= player1.speed;

                }
                if (player1.body.y < 560)
                {
                    if (IsKeyDown(KeyboardKey.KEY_S)) player1.body.y += player1.speed;
                }

                // Player 2 movement
                if (player2.body.y > 10)
                {
                    if (IsKeyDown(KeyboardKey.KEY_UP)) player2.body.y -= player2.speed;

                }
                if (player2.body.y < 560)
                {
                    if (IsKeyDown(KeyboardKey.KEY_DOWN)) player2.body.y += player2.speed;
                }

                BeginDrawing();

                ClearBackground(Color.BLACK);
                DrawRectangleRec(player1.body, Color.GOLD);
                DrawRectangleRec(player2.body, Color.GOLD);
                DrawCircleV(ball.position, ball.radius, Color.GOLD);

                DrawText("PLAYER 1 : " + player1.score, 60, 685, 25, Color.WHITE);
                DrawText("PLAYER 2 : " + player2.score, 1060, 685, 25, Color.WHITE);

                EndDrawing();

                if (ball.active)
                {
                    // Bouncing ball logic
                    ball.position.X += ball.speed.X;
                    ball.position.Y += ball.speed.Y;

                    if ((ball.position.Y >= (GetScreenHeight() - ball.radius)) || (ball.position.Y <= ball.radius)) ball.speed.Y *= -1.0f;
                    if (CheckCollisionCircleRec(ball.position, ball.radius, player1.body)) ball.speed.X *= -1.0f;
                    if (CheckCollisionCircleRec(ball.position, ball.radius, player2.body)) ball.speed.X *= -1.0f;

                    //Score
                    if ((ball.position.X >= (GetScreenWidth() - ball.radius)))
                    {
                        player1.score += 1;
                        ball.position.Y = GetScreenHeight() / 2;
                        ball.position.X = GetScreenWidth() / 2;
                        ball.active = false;
                    }
                    if (ball.position.X <= ball.radius)
                    {
                        player2.score += 1;
                        ball.position.Y = GetScreenHeight() / 2;
                        ball.position.X = GetScreenWidth() / 2;
                        ball.active = false;
                    }

                }

                Console.WriteLine(++framesCounter);

            }

            CloseWindow();
        }

        static void InitGame()
        {
            framesCounter = 0;
            player1.score = 0;
            player2.score = 0;

            player1.body = new Rectangle(15, (SCREEN_HEIGHT / 2) - 75, 30, 150);
            player2.body = new Rectangle(1235, (SCREEN_HEIGHT / 2) - 75, 30, 150);
            player1.speed = 8;
            player2.speed = 8;

            ball.position.Y = GetScreenHeight() / 2;
            ball.position.X = GetScreenWidth() / 2;
            ball.radius = 10;
            ball.speed = new Vector2(18f, 13f);
            ball.active = false;
        }
    }

    public class Ball
    {
        public Vector2 position;
        public Vector2 speed;
        public float radius;
        public bool active;
    }

    public class Player
    {
        public int speed;
        public Rectangle body;
        public int score;
    }
}