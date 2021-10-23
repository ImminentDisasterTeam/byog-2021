using UnityEngine;

public class ExitEntity : Entity {
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private Sprite _active;
    [SerializeField] private Sprite _notActive;
    private bool _isActive;

    public bool IsActive {
        get => _isActive;
        set {
            _isActive = value;
            SetSpiteState();
        }
    }

    public Sprite ActiveSprite {
        set => _active = value;
    }

    private void SetSpiteState() {
        _sprite.sprite = _isActive ? _active : _notActive;
    }

    public override bool CanBeMoved() {
        return false;
    }
}
