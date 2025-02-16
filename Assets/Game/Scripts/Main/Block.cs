using TMPro;
using UnityEngine;

public class Block : MonoBehaviour
{
    public int Value;
    public Node Node;

    public Vector2 Position => transform.position;

    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private TextMeshPro _text;

    public void Init(BlockType type)
    {
        Value = type.Value;
        _renderer.color = type.Color;
        _text.text = type.Value.ToString();
    }

    public void SetBlock(Node node)
    {

    }
}
