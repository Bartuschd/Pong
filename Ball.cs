
using Raylib_cs;

class Ball
{
    int areaYMin;
    int areaYMax;
    float middleY;
    float middleX;
    int ballSize;
    float xVelocity;
    float yVelocity;
    int ballSpeed;
    float maxBallSpeed;
    int ballDirection;
    float ballAngle;
    Color color;
    Random rnd = new Random();



    public Ball(int MiddleY, int MiddleX, int BallSpeed, int BallSize, Color Color, int AreaYMin, int AreaYMax, float MaxBallSpeed)
    {
        areaYMin = AreaYMin;
        areaYMax = AreaYMax;
        middleY = MiddleY;
        middleX = MiddleX;
        ballSize = BallSize;
        color = Color;
        ballSpeed = BallSpeed;
        maxBallSpeed = ballSpeed * MaxBallSpeed;
        ballDirection = (rnd.Next(2) == 0) ? -1 : 1;
        ballAngle = rnd.Next(-30, 31);

        //Test Variablen
        // ballDirection = -1;
        // ballAngle = 40;

        float angleRadians = ballAngle * (MathF.PI / 180f);

        xVelocity = MathF.Cos(angleRadians) * ballSpeed * ballDirection;
        yVelocity = MathF.Sin(angleRadians) * ballSpeed;
        
    }


    public void Move(float deltaTime)
    {
        middleX += xVelocity * deltaTime;
        middleY += yVelocity * deltaTime;

        if (middleY - ballSize <= areaYMin)
        {
            middleY = areaYMin + ballSize;
            yVelocity *= -1;
        }
        else if (middleY + ballSize >= areaYMax)
        {
            middleY = areaYMax - ballSize;
            yVelocity *= -1;
        }


    }

    public void Draw()
    {
        Raylib.DrawCircle((int)middleX, (int)middleY, ballSize, color);
    }

    public void CheckPaddleCollision(Paddle paddle, bool isLeftPaddle)
    {
        float ballLeft = middleX - ballSize;
        float ballRight = middleX + ballSize;
        float ballTop = middleY - ballSize;
        float ballBottom = middleY + ballSize;

        float paddleLeft = paddle.X;
        float paddleRight = paddle.X + paddle.Width;
        float paddleTop = paddle.Y;
        float paddleBottom = paddle.Y + paddle.Height;

        bool horizontalCollision = ballRight >= paddleLeft && ballLeft <= paddleRight;
        bool verticalCollision = ballBottom >= paddleTop && ballTop <= paddleBottom;

        bool movingTowardPaddle = isLeftPaddle ? xVelocity < 0 : xVelocity > 0;

        if (movingTowardPaddle && horizontalCollision && verticalCollision)
        {
            float bounceAngle = BounceFromPaddle(paddle);
            float angleRadians = bounceAngle * (MathF.PI / 180f);

            float currentSpeed = MathF.Sqrt(xVelocity * xVelocity + yVelocity * yVelocity);

            currentSpeed *= 1.04f;
            currentSpeed = MathF.Min(currentSpeed, maxBallSpeed);

            int outgoingDirection = isLeftPaddle ? 1 : -1;

            xVelocity = MathF.Cos(angleRadians) * currentSpeed * outgoingDirection;
            yVelocity = MathF.Sin(angleRadians) * currentSpeed;

            if (isLeftPaddle)
            {
                middleX = paddleRight + ballSize;
            }
            else
            {
                middleX = paddleLeft - ballSize;
            }
        }
    }

    public float BounceFromPaddle(Paddle paddle)
    {
        float paddleCenterY = paddle.Y + paddle.Height / 2f;
        float distanceFromCenter = middleY - paddleCenterY;

        float relativeHitPosition = distanceFromCenter / (paddle.Height / 2f);
        relativeHitPosition = Math.Clamp(relativeHitPosition, -1f, 1f);

        float maxBounceAngle = 60f;

        return relativeHitPosition * maxBounceAngle;
    }
    
    public int CheckGoal(int windowX)
    {
        if(middleX - ballSize > windowX)
        {
            return -1;
        }
        else if (middleX + ballSize < 0) {
            return +1;

        }
        else
        {
            return 0;
        }
    }
}
