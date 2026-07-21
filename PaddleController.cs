using Raylib_cs;

class PaddleController
{
    KeyboardKey upKey;
    KeyboardKey downKey;

    public PaddleController(KeyboardKey UpKey, KeyboardKey DownKey)
    {
        upKey = UpKey;
        downKey = DownKey;
    }

    public int GetMovementDirection()
    {
        if(Raylib.IsKeyDown(downKey) && Raylib.IsKeyDown(upKey))
        {
            return 0;
        }
        else if(Raylib.IsKeyDown(upKey)) {
            return -1;
        }
        else if(Raylib.IsKeyDown(downKey))
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
}