using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameLogic {
    public Action OnLevelWin;

    private readonly Map _map;
    private PlayerEntity _player;
    private ExitEntity _exit;
    private readonly List<(Entity, Vector2Int)> _moves = new List<(Entity, Vector2Int)>();
    private MonoBehaviour _coroRunner;
    private List<Button> _buttons;

    public GameLogic(Map map) {
        _map = map;
    }

    public void SetEntities(PlayerEntity player, ExitEntity exit, List<Button> buttons, MonoBehaviour coroRunner) {
        _player = player;
        _exit = exit;
        _buttons = buttons;
        _coroRunner = coroRunner;
        _player.OnPlayerMove += OnPlayerMove;
        _player.OnPlayerReset += OnPlayerRewind;
        _moves.Clear();
    }

    public void CheckButtons() {
        _exit.IsActive = _buttons.All(b => b.IsActive());
    }

    private void OnPlayerMove(Vector2Int direction) {
        var playerPos = _map.GetPos(_player);
        var destinationEntity = _map.GetEntity(playerPos + direction);
        if (destinationEntity == _exit && _exit.IsActive) {
            OnLevelWin?.Invoke();
            return;
        }

        _player.MovementAllowed = false;
        _map.Move(_player, direction, moves => {
            if (moves != null)
                _moves.AddRange(moves);
            _player.MovementAllowed = true;
        });
    }

    private void OnPlayerRewind() {
        if (_moves.Count == 0)
            return;

        Reset();
    }

    private void Reset() {
        _player.MovementAllowed = false;

        void Finish() {
            _moves.Clear();
            _player.MovementAllowed = true;
        }

        var idx = -1;
        void ApplyReverseMove() {
            if (++idx >= _moves.Count) {
                Finish();
                return;
            }

            var (entity, move) = _moves[idx];
            _map.Move(entity, move * -1, _ => ApplyReverseMove());
        }

        ApplyReverseMove();
    }
}
