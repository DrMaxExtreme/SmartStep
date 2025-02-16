using System.Linq;
using UnityEngine;

namespace GameplaySimulation
{
    public class PlayfieldManager
    {
        private const int Height = 10;
        private const int Width = 7;
        private const int ObjectTypes = 6;
        private const int InitialFilledLines = 3;
        private const int StartCountSpawn = 4;

        public int TotalDestroyedObjects { get; private set; }

        private int[,] _playfield;
        private SpawnManager _spawnManager = new SpawnManager();

        public PlayfieldManager()
        {
            _playfield = new int[Height, Width];
        }

        public void ClearTotalDestroyedObjects()
        {
            TotalDestroyedObjects = 0;
        }

        public void CreateEmptyPlayfield()
        {
            Enumerable.Range(0, _playfield.GetLength(0)).ToList().ForEach(i =>
                Enumerable.Range(0, _playfield.GetLength(1)).ToList().ForEach(j => _playfield[i, j] = 0));
        }

        public void FillStartPlayfield()
        {
            Enumerable.Range(Height - InitialFilledLines, InitialFilledLines).ToList().ForEach(i =>
                Enumerable.Range(0, _playfield.GetLength(1)).ToList().ForEach(j =>
                    _playfield[i, j] = Random.Range(0, ObjectTypes) + 1));
        }

        public (int row, int column, int destroyedCount) FindBestMove()
        {
            int[] biggerColumns = FindBiggerColumns();

            var bestMove = biggerColumns
                .SelectMany(column => Enumerable.Range(0, _playfield.GetLength(0))
                    .Where(row => _playfield[row, column] > 0)
                    .Select(row => new
                    {
                        Row = row,
                        Column = column,
                        DestroyedCount = CountDestroyedObjects(row, column)
                    }))
                .OrderByDescending(move => move.DestroyedCount)
                .FirstOrDefault();

            return bestMove != null
                ? (bestMove.Row, bestMove.Column, bestMove.DestroyedCount)
                : (-1, -1, 0);
        }

        public void MakeMove(int row, int column, int destroyedCount)
        {
            if (row == -1 || column == -1 || destroyedCount == 0)
            {
                return;
            }

            int targetValue = _playfield[row, column];
            if (targetValue <= 0) return;

            void ClearHorizontal(int targetRow)
            {
                for (int col = 0; col < _playfield.GetLength(1); col++)
                {
                    if (_playfield[targetRow, col] == targetValue)
                    {
                        _playfield[targetRow, col] = 0;
                    }
                }
            }

            void ClearVertical(int targetColumn)
            {
                for (int targetRow = 0; targetRow < _playfield.GetLength(0); targetRow++)
                {
                    if (_playfield[targetRow, targetColumn] == targetValue)
                    {
                        _playfield[targetRow, targetColumn] = 0;
                    }
                }
            }

            _playfield[row, column] = 0;

            ClearHorizontal(row);
            ClearVertical(column);

            TotalDestroyedObjects += destroyedCount;
        }

        public void DropNumbersDown()
        {
            for (int col = 0; col < _playfield.GetLength(1); col++)
            {
                int emptyRow = _playfield.GetLength(0) - 1;

                for (int row = _playfield.GetLength(0) - 1; row >= 0; row--)
                {
                    if (_playfield[row, col] > 0)
                    {
                        _playfield[emptyRow, col] = _playfield[row, col];

                        if (emptyRow != row)
                        {
                            _playfield[row, col] = 0;
                        }

                        emptyRow--;
                    }
                }
            }
        }

        public bool FillRandomNumbersInTopRow()
        {
            int countCubesSpawn = GetCountCubesSpawn();

            int[] randomColumns = GetRandomIndicesColumns(countCubesSpawn);

            foreach (var column in randomColumns)
            {
                if (_playfield[0, column] > 0)
                {
                    Debug.Log("Игра окончена: поле переполнено");
                    return true;
                }
            }

            foreach (var column in randomColumns)
            {
                _playfield[0, column] = Random.Range(1, ObjectTypes + 1);
            }

            return false;
        }

        private int[] FindBiggerColumns()
        {
            var columnCounts = Enumerable.Range(0, _playfield.GetLength(1))
                .Select(j => new
                {
                    Index = j,
                    Count = Enumerable.Range(0, _playfield.GetLength(0)).Count(i => _playfield[i, j] > 0)
                })
                .ToList();

            int maxCount = columnCounts.Max(column => column.Count);

            var biggerColumns = columnCounts
                .Where(column => column.Count == maxCount)
                .Select(column => column.Index)
                .ToList();

            return biggerColumns.ToArray();
        }

        private int CountDestroyedObjects(int startRow, int startColumn)
        {
            int targetValue = _playfield[startRow, startColumn];
            if (targetValue <= 0) return 0;

            bool[,] visited = new bool[_playfield.GetLength(0), _playfield.GetLength(1)];
            int destroyedCount = 0;

            void TraverseHorizontal(int row)
            {
                for (int col = 0; col < _playfield.GetLength(1); col++)
                {
                    if (!visited[row, col] && _playfield[row, col] == targetValue)
                    {
                        visited[row, col] = true;
                        destroyedCount++;
                    }
                }
            }

            void TraverseVertical(int column)
            {
                for (int row = 0; row < _playfield.GetLength(0); row++)
                {
                    if (!visited[row, column] && _playfield[row, column] == targetValue)
                    {
                        visited[row, column] = true;
                        destroyedCount++;
                    }
                }
            }

            visited[startRow, startColumn] = true;
            destroyedCount++;

            TraverseHorizontal(startRow);
            TraverseVertical(startColumn);

            return destroyedCount;
        }

        private int GetCountCubesSpawn()
        {
            int countSpawn = StartCountSpawn;

            if (TotalDestroyedObjects >= _spawnManager.GetCurrentSpawnThreshold())
            {
                countSpawn++;
            }

            if (TotalDestroyedObjects >= _spawnManager.GetCurrentSpawnThreshold() + _spawnManager.GetCurrentSpawnThreshold())
            {
                countSpawn++;
            }

            return countSpawn;
        }

        private int[] GetRandomIndicesColumns(int countCubesSpawn)
        {
            int[] indicesColumns = Enumerable.Range(0, Width).ToArray();
            System.Random random = new System.Random();
            int[] randomIndices = indicesColumns.OrderBy(_ => random.Next()).Take(countCubesSpawn).ToArray();

            return randomIndices;
        }
    }
}
