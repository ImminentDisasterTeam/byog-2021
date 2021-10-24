using System;
using DG.Tweening;
using UnityEngine;

public abstract class Entity : MonoBehaviour {
    public abstract bool CanBeMoved();

    public void SetPosition(Vector2Int pos, Action onDone = null) {
        var newPos = new Vector3(pos.x, pos.y, 0);
        if (onDone == null) {
            transform.position = newPos;
            return;
        }

        if (transform.position == newPos) {
            onDone();
            return;
        }

        const float moveTime = 0.15f;
        transform
            .DOMove(newPos, moveTime)
            .SetEase(Ease.InOutSine)
            .OnComplete(() => onDone());
    }
}
