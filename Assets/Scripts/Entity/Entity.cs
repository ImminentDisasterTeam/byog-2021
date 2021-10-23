using UnityEngine;

public abstract class Entity : MonoBehaviour {
    public abstract bool CanBeMoved();

    public void SetPosition(Vector2Int pos, bool instant = false) {
        transform.position = new Vector3(pos.x, pos.y, 0);
    }
}
