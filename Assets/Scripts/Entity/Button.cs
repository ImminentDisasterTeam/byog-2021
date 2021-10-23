using UnityEngine;

public class Button : MonoBehaviour {
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private Sprite _pressed;
    [SerializeField] private Sprite _notPressed;
    protected bool _isPressed;

    public bool IsPressed {
        set {
            _isPressed = value;
            SetSpiteState();
        }
    }

    public virtual bool IsActive() {
        return _isPressed;
    }

    private void SetSpiteState() {
        _sprite.sprite = _isPressed ? _pressed : _notPressed;
    }

    public void SetPosition(Vector2Int pos) {
        transform.position = new Vector3(pos.x, pos.y, 0);
    }
}
