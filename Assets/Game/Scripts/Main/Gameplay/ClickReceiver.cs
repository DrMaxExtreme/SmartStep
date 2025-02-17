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
            // Проверяем, есть ли на объекте компонент Cell
            Cell cell = GetComponent<Cell>();
            if (cell != null && !cell.IsEmpty)
            {
                // Вызываем событие, передавая ячейку
                OnCellClicked?.Invoke(cell);
            }
        }
    }
}
