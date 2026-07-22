using Raylib_cs;


//Default Paddle Values
float paddleMovementSpeed = 350f;
float paddleY = 150f;
int paddleWidth= 10;
int paddleHeight = 200;
int playerOnePaddleX = 15;
int playerTwoPaddleX = 775;

//Default Player One Controls
KeyboardKey playerOneUp = KeyboardKey.W;
KeyboardKey playerOneDown = KeyboardKey.S;

//Default Player Two Controls
KeyboardKey playerTwoUp = KeyboardKey.U;
KeyboardKey playerTwoDown = KeyboardKey.J;

//Default Ball Values
int ballMiddleY = 300;
int ballMiddleX = 400;
int ballSpeed = 300;
int ballSize = 10;
int ballAreaYMin = 0;
float maxBallSpeed = 3f;
Color ballColor = Color.Purple;

//Window Setup
const int windowX = 800;
const int windowY = 600;

//Text Strings
const string gameOverText = "Game Over!";
const string playerOneWin = "Player One Wins!";
const string playerTwoWin = "Player Two Wins!";
const string pauseText = "Paused!";

//Default Gamestate
GameState currentState = GameState.Playing;

//Default Point Values
int pointsPlayerOne = 0;
int pointsPlayerTwo = 0;
const int winningScore = 3;

//Default Class Init
IInputController paddlePlayerOneController = new PaddleController(playerOneUp, playerOneDown);
IInputController paddlePlayerTwoController = new PaddleController(playerTwoUp, playerTwoDown);
Paddle paddlePlayerOne = new Paddle(paddleWidth, paddleHeight, playerOnePaddleX, paddleY, paddleMovementSpeed, windowY);
Paddle paddlePlayerTwo = new Paddle(paddleWidth, paddleHeight, playerTwoPaddleX, paddleY, paddleMovementSpeed, windowY);

Ball ball = new Ball(ballMiddleX, ballMiddleY, ballSpeed, ballSize, ballColor, ballAreaYMin, windowY, maxBallSpeed);





Raylib.InitWindow(windowX, windowY, "Pong");

int gameOverWidth = Raylib.MeasureText(gameOverText, 50);
int gameOverX = (windowX - gameOverWidth) / 2;

int playerOneWinWidth = Raylib.MeasureText(playerOneWin, 50);
int playerOneWinX = (windowX - playerOneWinWidth) / 2;

int playerTwoWinWidth = Raylib.MeasureText(playerTwoWin, 50);
int playerTwoWinX = (windowX - playerTwoWinWidth) / 2;

int pauseTextWidth = Raylib.MeasureText(pauseText, 40);
int pauseTextX = (windowX - pauseTextWidth) / 2;

NetworkHost networkHost = new NetworkHost();
networkHost.Start();

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
networkHost.Stop();





void Update()
{
    float deltaTime = Raylib.GetFrameTime();

    networkHost.CheckForClient();



    if(currentState == GameState.Playing)
    {
        if (Raylib.IsKeyPressed(KeyboardKey.R))
        {
            PauseGame();
            return;
        }

        CheckPaddleMovement(deltaTime);

        CheckBallMovement(deltaTime);
        
        CheckPaddleCollision();

        int goal = ball.CheckGoal(windowX);

        CheckScore(goal);

        CheckRestartOrGameover(goal);
    }

    else if (currentState == GameState.Restart)
    {
        RestartRound();
    }

    else if (currentState == GameState.GameOver)
    {
        if(Raylib.IsKeyPressed(KeyboardKey.Space))
        {
            RestartGame();
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



void RestartGame()
{
    ball = new Ball(ballMiddleX, ballMiddleY, ballSpeed, ballSize, ballColor, ballAreaYMin, windowY, maxBallSpeed);
    paddlePlayerOne = new Paddle(paddleWidth, paddleHeight, playerOnePaddleX, paddleY, paddleMovementSpeed, windowY);
    paddlePlayerTwo = new Paddle(paddleWidth, paddleHeight, playerTwoPaddleX, paddleY, paddleMovementSpeed, windowY);
    pointsPlayerOne = 0;
    pointsPlayerTwo = 0;
    currentState = GameState.Playing;
}

void RestartRound()
{
    ball = new Ball(ballMiddleX, ballMiddleY, ballSpeed, ballSize, ballColor, ballAreaYMin, windowY, maxBallSpeed);
    currentState = GameState.Playing;
}

void PauseGame()
{
    currentState = GameState.Paused;
}

void CheckRestartOrGameover(int goal)
{
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

void CheckPaddleMovement(float deltaTime)
{
    paddlePlayerOne.Move(deltaTime, paddlePlayerOneController.GetMovementDirection());
    paddlePlayerTwo.Move(deltaTime, paddlePlayerTwoController.GetMovementDirection());
}

void CheckBallMovement(float deltaTime)
{
    ball.Move(deltaTime);
}

void CheckPaddleCollision()
{
    ball.CheckPaddleCollision(paddlePlayerOne, true);
    ball.CheckPaddleCollision(paddlePlayerTwo, false);
}

void CheckScore(int goal)
{
    if(goal == 1)
    {
        pointsPlayerTwo++;
    }
    else if(goal == -1)
    {
        pointsPlayerOne++;
    }
    else
    {
        return;
    }
}