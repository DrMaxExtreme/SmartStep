using UnityEngine;

namespace SmartStep.Gameplay
{
    public class WaitingForPlayerState : IGameState
    {
        private GameplayStateMachine _stateMachine;

        public WaitingForPlayerState(GameplayStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            Debug.Log("Вход в состояние: WaitingForPlayer");
            // Подписываемся на событие клика по ячейке
            ClickReceiver.OnCellClicked += HandleCellClicked;
        }

        public void Execute()
        {
            // Execute можно оставить пустым, так как логика обработки ввода происходит в событии
        }

        public void Exit()
        {
            Debug.Log("Выход из состояния: WaitingForPlayer");
            ClickReceiver.OnCellClicked -= HandleCellClicked;
        }

        private void HandleCellClicked(Cell cell)
        {
            Debug.Log("Ячейка выбрана: " + cell);
            _stateMachine.ChangeState(new ProcessingMoveState(_stateMachine, cell));
        }
    }
}
