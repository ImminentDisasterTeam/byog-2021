using UnityEngine;

public class Button : MonoBehaviour {
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private Color _pressed;
    [SerializeField] private Color _notPressed;
    private bool _isPressed;

    public bool IsPressed {
        get => _isPressed;
        set {
            _isPressed = value;
            SetSpiteState();
        }
    }

    private void SetSpiteState() {
        _sprite.color = _isPressed ? _pressed : _notPressed;
    }

    public void SetPosition(Vector2Int pos) {
        transform.position = new Vector3(pos.x, pos.y, 0);
    }
}
