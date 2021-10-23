using UnityEngine;

public class ExitEntity : Entity {
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private Color _active;
    [SerializeField] private Color _notActive;
    private bool _isActive;

    public bool IsActive {
        get => _isActive;
        set {
            _isActive = value;
            SetSpiteState();
        }
    }

    private void SetSpiteState() {
        _sprite.color = _isActive ? _active : _notActive;
    }

    public override bool CanBeMoved() {
        return false;
    }
}
