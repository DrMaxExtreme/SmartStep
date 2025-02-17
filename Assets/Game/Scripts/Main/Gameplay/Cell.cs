using UnityEngine;

namespace SmartStep.Gameplay
{
    public class Cell : MonoBehaviour
    {
        private int _x;
        private int _y;
        private Object _gamePiece;

        public bool IsEmpty => _gamePiece != null;

        public void Initialize(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public int X => _x;
        public int Y => _y;

        public void SpawnGamePiece(int type)
        {
            if (_gamePiece != null)
                Destroy(_gamePiece.gameObject);

            GameObject pieceObj = new GameObject($"GamePiece_{_x}_{_y}");
            pieceObj.transform.SetParent(transform);
            pieceObj.transform.localScale = Vector3.one;

            _gamePiece = pieceObj.AddComponent<Object>();
            _gamePiece.Initialize(type);
        }

        public Object GetGamePiece()
        {
            return _gamePiece;
        }

        public void ClearGamePiece()
        {
            if (_gamePiece != null)
            {
                Destroy(_gamePiece.gameObject);
                _gamePiece = null;
            }
        }
    }
}
