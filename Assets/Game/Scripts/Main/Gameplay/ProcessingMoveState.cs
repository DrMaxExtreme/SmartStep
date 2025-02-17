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
            Debug.Log("���� � ���������: ProcessingMove ��� ������ " + _selectedCell);
            // ����� ����� ����������� ��������� ����, ��������� _selectedCell
        }

        public void Execute()
        {
            // ��������� ����...
            // ����� ���������� ��������� ����� ����������� ���������, ��������:
            // _stateMachine.ChangeState(new DestroyingObjectsState(_stateMachine));
        }

        public void Exit()
        {
            Debug.Log("����� �� ���������: ProcessingMove");
        }
    }
}
