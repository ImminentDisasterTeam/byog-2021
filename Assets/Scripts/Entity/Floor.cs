using UnityEngine;

public class Floor : MonoBehaviour {
    [SerializeField] private SpriteRenderer _renderer;
    public Sprite Sprite {
        set => _renderer.sprite = value;
    }

    public void SetPosition(Vector2Int pos) {
        transform.position = new Vector3(pos.x, pos.y, 0);
    }
}
