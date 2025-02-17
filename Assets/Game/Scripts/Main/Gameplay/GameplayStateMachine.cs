using UnityEngine;
using System.Collections;

namespace SmartStep.Gameplay
{
    public class GameplayStateMachine : MonoBehaviour
    {
        private IGameState _currentState;

        private void Start()
        {
            ChangeState(new WaitingForPlayerState(this));
        }

        public void ChangeState(IGameState newState)
        {
            if (_currentState != null)
                _currentState.Exit();

            _currentState = newState;
            _currentState.Enter();
        }
    }
}
