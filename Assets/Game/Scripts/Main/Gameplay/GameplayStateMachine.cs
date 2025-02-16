using UnityEngine;
using System.Collections;

public class GameplayStateMachine : MonoBehaviour
{
    [Header("������ �� ����������")]
    [SerializeField] private GameField _gameField;
    [SerializeField] private ObjectSpawner _objectSpawner;

    [Header("��������� ����")] // ������� � ������ �����
    [SerializeField] private int _victoryScore = 1000;
    [SerializeField] private int _currentScore = 0;

    private GameState _currentState;

    private void OnEnable()
    {
        Init();
        StartCoroutine(GameLoop());
    }

    private IEnumerator GameLoop()
    {
        bool isWin = false;
        bool isLose = false;

        switch (_currentState)
        {
            case GameState.WaitingForPlayer:
                //���� RaycastHit.collision.typeCell > 0, �� ��������� �������� � int _typeSelectedCell � ������������� � DestroyingObjects
                yield return null;
                break;

            case GameState.DestroyingObjects:
                DestroyObjects();
                _currentState = GameState.CalculationResults;
                break;

            case GameState.CalculationResults:
                CalculateResults();
                AnimateUI();

                //CheckingVictory

                _currentState = GameState.AnimatingFill;
                break;

            case GameState.AnimatingFill:
                yield return StartCoroutine(AnimateFalling());
                _currentState = GameState.CheckingVictory;
                break;

            case GameState.CheckingVictory:
                isWin = CheckWin();

                if (isWin) 
                {
                    _currentState = GameState.ShowingLose;
                }



                _currentState = GameState.SpawningObjects;
                break;

            case GameState.SpawningObjects:
                SpawnNewObjects();
                _objectSpawner.OnMoveCompleted();
                _currentState = GameState.CheckingVictory;
                break;

            case GameState.CheckingVictory:
                if (_currentScore >= _victoryScore)
                {
                    Debug.Log("������!");
                    _currentState = GameState.GameOver;
                }
                else if (CheckOverflow())
                {
                    Debug.Log("���� �������� (������������)!");
                    _currentState = GameState.GameOver;
                }
                else
                {
                    _currentState = GameState.WaitingForPlayer;
                }
                break;
        }
        yield return null;
    }

    private void Init()
    {
        _gameField.InitializeField();
        _objectSpawner.InitializeSpawner();

        _currentState = GameState.WaitingForPlayer;
    }

    private void DestroyObjects()
    {
        _currentScore += CalculateResults();
    }

    private int CalculateResults()
    {
        //fibonachi formula
        return 50;
    }

    private void AnimateUI()
    {
        //����������� �������� ������� � ���� UI ���������: text, fill
    }

    private IEnumerator AnimateFalling()
    {
        yield return new WaitForSeconds(0.5f);
    }

    private void SpawnNewObjects()
    {

    }

    private bool CheckWin()
    {
        return false;
    }

    private bool CheckOverflow()
    {
        return false;
    }

    private void OnDisable()
    {
        StopCoroutine(GameLoop());
    }
}

public enum GameState
{
    WaitingForPlayer,
    DestroyingObjects,
    CalculationResults,
    AnimatingFill,
    CheckingVictory,
    ShowingVictory,
    SpawningObjects,
    CheckingLose,
    ShowingLose,
    SavingStats
}
