using UnityEngine;

namespace SmartStep.Gameplay
{
    public class GameField : MonoBehaviour
    {
        [Header("Параметры поля")]
        [SerializeField] private int _gridWidth = 7;
        [SerializeField] private int _gridHeight = 10;
        [SerializeField] private int _startingFillRows = 5;
        [SerializeField] private float _cellSize = 140f;
        [SerializeField] private float _spacing = 4f;
        [SerializeField] private RectTransform _container;

        [Header("Префаб ячейки")]
        [SerializeField] private Cell _cellPrefab;

        private Cell[,] _cells;

        private float _half = 2f;

        public void Init()
        {
            _cells = new Cell[_gridWidth, _gridHeight];

            float totalWidth = _gridWidth * _cellSize + (_gridWidth - 1) * _spacing;
            float totalHeight = _gridHeight * _cellSize + (_gridHeight - 1) * _spacing;
            Vector2 startPosition = new Vector2(-totalWidth / _half + _cellSize / _half, totalHeight / _half - _cellSize / _half);

            for (int y = 0; y < _gridHeight; y++)
            {
                for (int x = 0; x < _gridWidth; x++)
                {
                    Cell newCell = Instantiate(_cellPrefab, _container);
                    RectTransform rectTransform = newCell.GetComponent<RectTransform>();

                    if (rectTransform == null)
                    {
                        Debug.LogError("Префаб ячейки должен содержать компонент RectTransform.");
                        return;
                    }

                    float positionX = startPosition.x + x * (_cellSize + _spacing);
                    float positionY = startPosition.y - y * (_cellSize + _spacing);

                    rectTransform.anchoredPosition = new Vector2(positionX, positionY);
                    rectTransform.sizeDelta = new Vector2(_cellSize, _cellSize);

                    newCell.Initialize(x, y);

                    if (y < _startingFillRows)
                    {
                        int randomType = Random.Range(1, 7);
                        newCell.SpawnGamePiece(randomType);
                    }

                    newCell.name = $"Cell_{x}_{y}";
                    _cells[x, y] = newCell;
                }
            }
        }

        public Cell GetCell(int x, int y)
        {
            if (x < 0 || x >= _gridWidth || y < 0 || y >= _gridHeight)
                return null;
            return _cells[x, y];
        }
    }
}
