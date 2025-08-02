using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }

    public event Action<int> ObjectsCleared;
    public event Action DidStep;

    [SerializeField] private SpawnCountManager _spawnCountManager;

    [Header("Grid settings")]
    [SerializeField] private RectTransform _gridContainer;
    [SerializeField] private GameObject _cellPrefab;
    [SerializeField] private GameObject[] _spawnPrefabs;
    [SerializeField] private int _width = 7;
    [SerializeField] private int _height = 10;

    [Tooltip("Count of filled lines")]
    [SerializeField] private int _initialFillRows = 5;

    private int _newObjectsPerStep;

    private Cell[,] _cells;

    private bool _isProcessing;
    private int _pendingAnimations;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void OnCellClicked(Cell selectedCell)
    {
        if (_isProcessing)
        {
            return;
        }

        ProcessStep(selectedCell);
    }

    private void CreateGrid()
    {
        _cells = new Cell[_width, _height];

        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                var gameObject = Instantiate(_cellPrefab, _gridContainer);
                var cell = gameObject.GetComponent<Cell>();
                cell.Initialize(x, y);
                _cells[x, y] = cell;
            }
        }
    }

    private void InitialFill()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _initialFillRows; y++)
            {
                SpawnCellObject(x, y);
            }
        }
    }

    private void ProcessStep(Cell selectedCell)
    {
        _isProcessing = true;
        _pendingAnimations = 0;

        DidStep?.Invoke();

        ClearCells(selectedCell);
        ApplyGravity();
        SpawnNewObjects();
        ApplyGravity();
    }

    private void ClearCells(Cell selectedCell)
    {
        int type = selectedCell.Type;

        List<Cell> toClear = new List<Cell>();

        for (int x = 0; x < _width; x++)
        {
            var cellToCline = _cells[x, selectedCell.Coordinates.y];

            if (!cellToCline.IsEmpty() && cellToCline.Type == type)
            {
                toClear.Add(cellToCline);
            }
        }

        for (int y = 0; y < _height; y++)
        {
            var cellToCline = _cells[selectedCell.Coordinates.x, y];

            if (!cellToCline.IsEmpty() && cellToCline.Type == type)
                toClear.Add(cellToCline);
        }

        ObjectsCleared?.Invoke(toClear.Count - 1); //дважды выбирается "центральный" объект

        foreach (var cellToCline in toClear)
        {
            cellToCline.ClearObject();
        }
    }

    private void ApplyGravity()
    {
        for (int x = 0; x < _width; x++)
        {
            int bottomY = 0;

            for (int y = 0; y < _height; y++)
            {
                Cell currentCell = _cells[x, y];

                if (currentCell.OccupiedObject != null)
                {
                    if (y != bottomY)
                    {
                        var obj = currentCell.OccupiedObject;
                        int objType = currentCell.Type;

                        Vector3 oldWorldPos = obj.transform.position;

                        currentCell.SetOccupiedObject(null);
                        currentCell.SetType(-1);

                        Cell targetCell = _cells[x, bottomY];
                        targetCell.SetOccupiedObject(obj);
                        targetCell.SetType(objType);
                        obj.transform.SetParent(targetCell.transform, worldPositionStays: true);

                        Vector3 newWorldPos = targetCell.transform.position;
                        StartCoroutine(AnimateLinearMove(obj.transform, oldWorldPos, newWorldPos));
                        _pendingAnimations++;
                    }
                    bottomY++;
                }
            }

            for (int y = bottomY; y < _height; y++)
            {
                _cells[x, y].SetOccupiedObject(null);
                _cells[x, y].SetType(-1);
            }
        }
    }

    private IEnumerator AnimateLinearMove(Transform transform, Vector2 start, Vector2 end)
    {
        float elapsed = 0f;
        float distance = Vector2.Distance(start, end);
        float animationSpeed = 800f;

        float duration = distance / animationSpeed;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float tNorm = Mathf.Clamp01(elapsed / duration);

            transform.position = Vector2.Lerp(start, end, tNorm);
            yield return null;
        }

        transform.position = end;

        _pendingAnimations--;

        if (_pendingAnimations <= 0)
        {
            _isProcessing = false;
        }
    }

    private void SpawnNewObjects()
    {
        List<int> column = new List<int>();

        for (int i = 0; i < _width; i++)
        {
            column.Add(i);
        }

        for (int i = 0; i < _newObjectsPerStep && column.Count > 0; i++)
        {
            int idx = UnityEngine.Random.Range(0, column.Count);
            int x = column[idx]; column.RemoveAt(idx);

            int spawnY = -1;
            for (int y = _height - 1; y >= 0; y--)
            {
                if (_cells[x, y].IsEmpty()) 
                { 
                    spawnY = y; break; 
                }
            }

            if (spawnY < 0)
            {
                Debug.Log("Game Over!");
                return;
            }
            SpawnCellObject(x, spawnY);
        }
    }

    private void SpawnCellObject(int x, int y)
    {
        int type = UnityEngine.Random.Range(0, _spawnPrefabs.Length);
        var obj = Instantiate(_spawnPrefabs[type]);
        _cells[x, y].SetPosition(obj, type);
    }

    private void ChangeSpawnCount(int newCount)
    {
        _newObjectsPerStep = newCount;
    }

    private void OnEnable()
    {
        _spawnCountManager.Changed += ChangeSpawnCount;

        CreateGrid();
        InitialFill();
        _newObjectsPerStep = _spawnCountManager.StartCountSpawn;
    }

    private void OnDisable()
    {
        _spawnCountManager.Changed -= ChangeSpawnCount;
    }
}
