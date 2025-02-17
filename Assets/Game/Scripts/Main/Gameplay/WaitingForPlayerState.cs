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
            Debug.Log("���� � ���������: WaitingForPlayer");
            // ������������� �� ������� ����� �� ������
            ClickReceiver.OnCellClicked += HandleCellClicked;
        }

        public void Execute()
        {
            // Execute ����� �������� ������, ��� ��� ������ ��������� ����� ���������� � �������
        }

        public void Exit()
        {
            Debug.Log("����� �� ���������: WaitingForPlayer");
            ClickReceiver.OnCellClicked -= HandleCellClicked;
        }

        private void HandleCellClicked(Cell cell)
        {
            Debug.Log("������ �������: " + cell);
            _stateMachine.ChangeState(new ProcessingMoveState(_stateMachine, cell));
        }
    }
}
