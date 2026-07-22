class NetworkController : IInputController
{
    int savedDirection;

    public void SetMovementDirection(int direction)
    {
        savedDirection = direction;
    }

    public int GetMovementDirection()
    {
        return savedDirection;
    }
}