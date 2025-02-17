using SmartStep.Gameplay;
using UnityEngine;

namespace SmartStep.Gameplay
{
    public class ClickReceiver : MonoBehaviour
    {
        public delegate void ClickAction(Cell cell);
        public static event ClickAction OnCellClicked;

        private void OnMouseDown()
        {
            // ���������, ���� �� �� ������� ��������� Cell
            Cell cell = GetComponent<Cell>();
            if (cell != null && !cell.IsEmpty)
            {
                // �������� �������, ��������� ������
                OnCellClicked?.Invoke(cell);
            }
        }
    }
}
