using UnityEngine;
using UnityEngine.UI;

public class GridCreator : MonoBehaviour
{
    [Header("Ќастройки пол€")]
    public int gridWidth = 7;      // количество столбцов
    public int gridHeight = 10;    // количество строк

    [Header("Ќастройки €чеек")]
    public GameObject cellPrefab;  // префаб €чейки (UI-элемент с RectTransform)
    public float cellSize = 100f;  // размер €чейки (ширина и высота)
    public float spacing = 10f;    // отступ между €чейками

    [Header(" онтейнер дл€ €чеек")]
    public RectTransform gridContainer; // UI-контейнер (например, Panel) дл€ размещени€ €чеек

    void Start()
    {
        CreateGrid();
    }

    /// <summary>
    /// —оздает игровое поле, генериру€ €чейки по заданным параметрам.
    /// </summary>
    void CreateGrid()
    {
        // ќчистка контейнера (на случай, если в нем уже есть дочерние объекты)
        foreach (Transform child in gridContainer)
        {
            Destroy(child.gameObject);
        }

        // –асчЄт общего размера пол€
        float totalWidth = gridWidth * cellSize + (gridWidth - 1) * spacing;
        float totalHeight = gridHeight * cellSize + (gridHeight - 1) * spacing;

        // ƒл€ центрировани€ пол€ рассчитываем стартовую позицию
        // ≈сли pivot контейнера (и €чейки) = (0.5, 0.5), то (0,0) Ц центр
        // Ќачинаем с верхнего левого угла
        Vector2 startPos = new Vector2(-totalWidth / 2 + cellSize / 2, totalHeight / 2 - cellSize / 2);

        // √енерируем €чейки
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                // —оздаем экземпл€р €чейки как дочерний элемент контейнера
                GameObject cell = Instantiate(cellPrefab, gridContainer);

                // ѕолучаем RectTransform дл€ задани€ позиции и размера
                RectTransform rt = cell.GetComponent<RectTransform>();
                if (rt == null)
                {
                    Debug.LogError("ѕрефаб €чейки должен содержать компонент RectTransform.");
                    return;
                }

                // ¬ычисл€ем позицию €чейки
                float posX = startPos.x + x * (cellSize + spacing);
                float posY = startPos.y - y * (cellSize + spacing);
                rt.anchoredPosition = new Vector2(posX, posY);

                // ”станавливаем размер €чейки
                rt.sizeDelta = new Vector2(cellSize, cellSize);

                // ѕрисваиваем им€ дл€ удобства отладки
                cell.name = $"Cell_{x}_{y}";
            }
        }
    }
}
