using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _width = 3;
    [SerializeField] private int _height = 5;

    [SerializeField] private Node _nodePrefab;
    [SerializeField] private Block _blockPrefab;
    [SerializeField] private SpriteRenderer _boardPrefab;

    [SerializeField] private List<BlockType> _types;

    private List<Node> _nodes;
    private List<Block> _blocks;

    private GameState _state;

    private int _round;

    private BlockType GetBlockTypeByValue(int value) => _types.First(t => t.Value == value);

    private void Start()
    {
        ChangeState(GameState.GenerateLevel);
    }

    private void ChangeState(GameState newState)
    {
        _state = newState;

        switch (newState)
        {
            case GameState.GenerateLevel:
                GenerateGrid();
                break;
            case GameState.SpawningBlocks:
                SpawnBlocks(_round++ == 0 ? 2 : 1);
                break;
            case GameState.WaitingInput:
                break;
            case GameState.Moving:
                break;
            case GameState.Win:
                break;
            case GameState.Lose:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }

    private void Update()
    {
        if(_state != GameState.WaitingInput)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Shift(Vector2.left);
        }
    }

    private void GenerateGrid()
    {
        _round = 0;
        _nodes = new List<Node>();
        _blocks = new List<Block>();

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var node = Instantiate(_nodePrefab, new Vector2(x, y), Quaternion.identity); 

                _nodes.Add(node);
            }
        }

        var center = new Vector2((float)_width /2 - 0.5f, (float)_height / 2 - 0.5f);
        var board = Instantiate(_boardPrefab, center, Quaternion.identity);
        board.size = new Vector2(_width, _height);

        Camera.main.transform.position = new Vector3(center.x, center.y, -10);

        ChangeState(GameState.SpawningBlocks);
    }

    private void SpawnBlocks(int amout)
    {
        var freeNodes = _nodes.Where(n => n.OccupiedBlock == null).OrderBy(b => UnityEngine.Random.value).ToList();

        foreach (var node in freeNodes.Take(amout))
        {
            var block = Instantiate(_blockPrefab, node.Position, Quaternion.identity);

            block.Init(GetBlockTypeByValue(UnityEngine.Random.value > 0.8f ? 4 : 2));
        }

        if(freeNodes.Count() == 1)
        {
            // Lost the game
            return;
        }

        ChangeState(GameState.WaitingInput);
    }

    private void Shift(Vector2 dir)
    {
        var orderBlocks = _blocks.OrderBy(b => b.Position.x).ThenBy(b => b.Position.y).ToList();

        if (dir == Vector2.right || dir == Vector2.up)
        {
            orderBlocks.Reverse();
        }

        foreach (var block in orderBlocks)
        {

        }
    }
}

[Serializable]
public struct BlockType
{
    public int Value;
    public Color Color;
}

public enum GameState
{
    GenerateLevel,
    SpawningBlocks,
    WaitingInput,
    Moving,
    Win,
    Lose
}
