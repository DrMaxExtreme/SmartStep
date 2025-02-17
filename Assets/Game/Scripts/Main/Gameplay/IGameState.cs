namespace SmartStep.Gameplay
{
    public interface IGameState
    {
        void Enter();

        void Execute();

        void Exit();
    }
}
