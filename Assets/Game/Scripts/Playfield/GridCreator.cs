using UnityEngine;
using UnityEngine.UI;

public class GridCreator : MonoBehaviour
{
    [Header("��������� ����")]
    public int gridWidth = 7;      // ���������� ��������
    public int gridHeight = 10;    // ���������� �����

    [Header("��������� �����")]
    public GameObject cellPrefab;  // ������ ������ (UI-������� � RectTransform)
    public float cellSize = 100f;  // ������ ������ (������ � ������)
    public float spacing = 10f;    // ������ ����� ��������

    [Header("��������� ��� �����")]
    public RectTransform gridContainer; // UI-��������� (��������, Panel) ��� ���������� �����

    void Start()
    {
        CreateGrid();
    }

    /// <summary>
    /// ������� ������� ����, ��������� ������ �� �������� ����������.
    /// </summary>
    void CreateGrid()
    {
        // ������� ���������� (�� ������, ���� � ��� ��� ���� �������� �������)
        foreach (Transform child in gridContainer)
        {
            Destroy(child.gameObject);
        }

        // ������ ������ ������� ����
        float totalWidth = gridWidth * cellSize + (gridWidth - 1) * spacing;
        float totalHeight = gridHeight * cellSize + (gridHeight - 1) * spacing;

        // ��� ������������� ���� ������������ ��������� �������
        // ���� pivot ���������� (� ������) = (0.5, 0.5), �� (0,0) � �����
        // �������� � �������� ������ ����
        Vector2 startPos = new Vector2(-totalWidth / 2 + cellSize / 2, totalHeight / 2 - cellSize / 2);

        // ���������� ������
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                // ������� ��������� ������ ��� �������� ������� ����������
                GameObject cell = Instantiate(cellPrefab, gridContainer);

                // �������� RectTransform ��� ������� ������� � �������
                RectTransform rt = cell.GetComponent<RectTransform>();
                if (rt == null)
                {
                    Debug.LogError("������ ������ ������ ��������� ��������� RectTransform.");
                    return;
                }

                // ��������� ������� ������
                float posX = startPos.x + x * (cellSize + spacing);
                float posY = startPos.y - y * (cellSize + spacing);
                rt.anchoredPosition = new Vector2(posX, posY);

                // ������������� ������ ������
                rt.sizeDelta = new Vector2(cellSize, cellSize);

                // ����������� ��� ��� �������� �������
                cell.name = $"Cell_{x}_{y}";
            }
        }
    }
}
