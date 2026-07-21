using System.Security.Cryptography.X509Certificates;
using Raylib_cs;

class Paddle
{
    int paddleWidth;
    int paddleHeight;
    float paddleY;
    int paddleX;
    float paddleSpeed;
    int paddleMaxY;
    int paddleMinY;
    int windowY;
    public int X => paddleX;
    public float Y => paddleY;
    public int Width => paddleWidth;
    public int Height => paddleHeight;
    PaddleController paddleController;
    

    public Paddle(int PaddleWidth, int PaddleHeight,int PaddleX, float PaddleY, float PaddleSpeed, int WindowY, PaddleController PaddleController)
    {
        paddleWidth = PaddleWidth;
        paddleHeight = PaddleHeight;
        paddleY = PaddleY;
        paddleX = PaddleX;
        paddleSpeed = PaddleSpeed;
        windowY = WindowY;
        paddleMinY = 0;
        paddleMaxY = windowY - paddleHeight;
        paddleController = PaddleController;
    }

    public void Move(float deltaTime)
    {
            paddleY += paddleSpeed * paddleController.GetMovementDirection() * deltaTime; 
            paddleY = Math.Clamp(paddleY, paddleMinY, paddleMaxY);
    }

    public void Draw()
    {
        Raylib.DrawRectangle(paddleX, (int)paddleY, paddleWidth, paddleHeight, Color.White);
    }
}



