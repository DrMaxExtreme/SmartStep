using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour, IPointerClickHandler
{
    public Vector2Int Coordinates { get; private set; } 
    public GameObject OccupiedObject { get; private set; }
    public int Type { get; private set; } = -1;
    public void Initialize(int x, int y)
    {
        Coordinates = new Vector2Int(x, y);
        OccupiedObject = null;
        name = $"Cell_{x}_{y}";
    }

    public bool IsEmpty() => OccupiedObject == null;

    public void SetPosition(GameObject obj, int type)
    {
        if (!IsEmpty() || obj == null)
        {
            return;
        }

        OccupiedObject = obj;
        Type = type;
        obj.transform.SetParent(transform, false);
        obj.transform.localPosition = Vector3.zero;
    }

    public void ClearObject()
    {
        if (OccupiedObject != null)
        {
            Destroy(OccupiedObject);
        }

        OccupiedObject = null;
    }

    // ќбрабатывает клик по €чейке, передава€ событие менеджеру.
    public void OnPointerClick(PointerEventData eventData)
    {
        GridManager.Instance.OnCellClicked(this);
    }

    public void SetOccupiedObject(GameObject newOccupiedObject)
    {
        OccupiedObject = newOccupiedObject;
    }

    public void SetType(int newType)
    {
        Type = newType;
    }
}
