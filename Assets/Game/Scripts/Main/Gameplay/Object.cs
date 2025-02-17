using UnityEngine;
using UnityEngine.UI;

namespace SmartStep.Gameplay
{
    public class Object : MonoBehaviour
    {
        [SerializeField] private int _type;
        [SerializeField] private Sprite[] _typeSprites;

        private Image _image;

        public void Initialize(int type)
        {
            _type = type;
            _image = GetComponent<Image>();

            if (_image == null)
            {
                _image = gameObject.AddComponent<Image>();
            }

            UpdateVisual();
        }

        public int Type => _type;

        private void UpdateVisual()
        {
            if (_typeSprites != null && _type > 0 && _type < _typeSprites.Length)
            {
                _image.sprite = _typeSprites[_type];
            }
            else
            {
                _image.color = Color.clear;
            }
        }
    }
}
