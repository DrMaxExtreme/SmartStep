using UnityEngine;

namespace SmartStep.Gameplay
{
    public class ProcessingMoveState : IGameState
    {
        private GameplayStateMachine _stateMachine;
        private Cell _selectedCell;

        public ProcessingMoveState(GameplayStateMachine stateMachine, Cell selectedCell)
        {
            _stateMachine = stateMachine;
            _selectedCell = selectedCell;
        }

        public void Enter()
        {
            Debug.Log("Вход в состояние: ProcessingMove для ячейки " + _selectedCell);
            // Здесь можно реализовать обработку хода, используя _selectedCell
        }

        public void Execute()
        {
            // Обработка хода...
            // После завершения обработки можно переключить состояние, например:
            // _stateMachine.ChangeState(new DestroyingObjectsState(_stateMachine));
        }

        public void Exit()
        {
            Debug.Log("Выход из состояния: ProcessingMove");
        }
    }
}
