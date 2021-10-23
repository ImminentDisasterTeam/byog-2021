using System;
using UnityEngine;

public class PlayerEntity : Entity {
    private const string UP = "UP";
    private const string DOWN = "DOWN";
    private const string LEFT = "LEFT";
    private const string RIGHT = "RIGHT";
    private const string RESET = "RESET";
    private const string ESCAPE = "ESCAPE";

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
