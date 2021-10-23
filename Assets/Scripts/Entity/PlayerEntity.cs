using System;
using UnityEngine;

public class PlayerEntity : Entity {
    const string UP = "UP";
    const string DOWN = "DOWN";
    const string LEFT = "LEFT";
    const string RIGHT = "RIGHT";
    const string RESET = "RESET";
    const string ESCAPE = "ESCAPE";

    public Action<Vector2Int> OnPlayerMove;
    public Action OnPlayerReset;
    public bool MovementAllowed { get; set; } = true;

    public override bool CanBeMoved() {
        return true;
    }

    public void Update() {
        HandleInput();
    }

    private void HandleInput() {
        if (!MovementAllowed)
            return;

        Vector2Int direction;
        if (Input.GetButtonDown(UP))
            direction = Vector2Int.up;
        else if (Input.GetButtonDown(DOWN))
            direction = Vector2Int.down;
        else if (Input.GetButtonDown(RIGHT))
            direction = Vector2Int.right;
        else if (Input.GetButtonDown(LEFT))
            direction = Vector2Int.left;
        else if (Input.GetButtonDown(RESET))
            direction = Vector2Int.zero;
        else
            return;

        if (direction == Vector2Int.zero) {
            OnPlayerReset?.Invoke();
            return;
        }

        OnPlayerMove?.Invoke(direction);
    }
}
