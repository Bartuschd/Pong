using System.Data;
using Raylib_cs;



const int windowX = 800;
const int windowY = 600;

const string gameOverText = "Game Over!";

const string playerOneWin = "Player One Wins!";

const string playerTwoWin = "Player Two Wins!";

const string pauseText = "Paused!";

GameState currentState = GameState.Playing;

int pointsPlayerOne = 0;
int pointsPlayerTwo = 0;
const int winningScore = 3;


PaddleController paddlePlayerOneController = new PaddleController(KeyboardKey.W, KeyboardKey.S);
PaddleController paddlePlayerTwoController = new PaddleController(KeyboardKey.U, KeyboardKey.J);
Paddle paddlePlayerOne = new Paddle(10, 200, 25, 150f, 200f, windowY, paddlePlayerOneController);
Paddle paddlePlayerTwo = new Paddle(10, 200, 775, 150f, 200f, windowY, paddlePlayerTwoController);

Ball ball = new Ball(300, 400, 300, 10, Color.Purple, 0, windowY);





Raylib.InitWindow(windowX, windowY, "Pong");

int gameOverWidth = Raylib.MeasureText(gameOverText, 50);
int gameOverX = (windowX - gameOverWidth) / 2;

int playerOneWinWidth = Raylib.MeasureText(playerOneWin, 50);
int playerOneWinX = (windowX - playerOneWinWidth) / 2;

int playerTwoWinWidth = Raylib.MeasureText(playerTwoWin, 50);
int playerTwoWinX = (windowX - playerTwoWinWidth) / 2;

int pauseTextWidth = Raylib.MeasureText(pauseText, 40);
int pauseTextX = (windowX - pauseTextWidth) / 2;


while (!Raylib.WindowShouldClose())
{
    Update();
    
    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.Black);

    paddlePlayerOne.Draw();
    paddlePlayerTwo.Draw();
    ball.Draw();

    if (currentState == GameState.Paused)
    {
        Raylib.DrawText(pauseText, pauseTextX, 300, 40, Color.Red);
    }

    if (currentState !=GameState.GameOver)
    {
        Raylib.DrawText(pointsPlayerOne.ToString(), 200, 20, 40, Color.White);
        Raylib.DrawText(pointsPlayerTwo.ToString(), 600, 20, 40, Color.White);
    }
    else
    {
        Raylib.DrawText(gameOverText, gameOverX, 300, 50, Color.Red);
        if (pointsPlayerOne == winningScore)
        {
            Raylib.DrawText(playerOneWin, playerOneWinX, 200, 50, Color.Red);
        }
        else if (pointsPlayerTwo == winningScore)
        {
            Raylib.DrawText(playerTwoWin, playerTwoWinX, 200, 50, Color.Red);
        }
    }

    Raylib.EndDrawing();
}

Raylib.CloseWindow();





void Update()
{
    float deltaTime = Raylib.GetFrameTime();



    if(currentState == GameState.Playing)
    {
        if (Raylib.IsKeyPressed(KeyboardKey.R))
        {
            currentState = GameState.Paused;
            return;
        }
        paddlePlayerOne.Move(deltaTime);
        paddlePlayerTwo.Move(deltaTime);
        ball.Move(deltaTime);
        
        int goal = ball.CheckGoal(windowX);

        ball.CheckPaddleCollision(paddlePlayerOne, true);
        ball.CheckPaddleCollision(paddlePlayerTwo, false);

        if(goal == 1)
        {
            pointsPlayerTwo++;
        }
        else if(goal == -1)
        {
            pointsPlayerOne++;
        }

        if(goal != 0)
        {
            if (pointsPlayerOne >= winningScore || pointsPlayerTwo >= winningScore)
            {
                currentState = GameState.GameOver;
            }
            else
            {
                currentState = GameState.Restart;
            }

        }
    }
    else if (currentState == GameState.Restart)
    {
            ball = new Ball(300, 400, 300, 10, Color.Purple, 0, windowY);
            currentState = GameState.Playing;
    }
    else if (currentState == GameState.GameOver)
    {
        if(Raylib.IsKeyPressed(KeyboardKey.Space))
        {
            ball = new Ball(300, 400, 300, 10, Color.Purple, 0, windowY);
            paddlePlayerOne = new Paddle(10, 200, 25, 150f, 200f, windowY, paddlePlayerOneController);
            paddlePlayerTwo = new Paddle(10, 200, 775, 150f, 200f, windowY, paddlePlayerTwoController);
            pointsPlayerOne = 0;
            pointsPlayerTwo = 0;
            currentState = GameState.Playing;

        }
    }
    else if (currentState == GameState.Paused)
    {
        if (Raylib.IsKeyPressed(KeyboardKey.R))
        {
            currentState = GameState.Playing;
        }
        
    }
}






