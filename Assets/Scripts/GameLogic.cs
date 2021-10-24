using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class GameLogic {
    public Action OnLevelWin;

    private readonly Map _map;
    private PlayerEntity _player;
    private ExitEntity _exit;
    private readonly List<(Entity, Vector2Int)> _moves = new List<(Entity, Vector2Int)>();
    private List<Button> _buttons;

    public GameLogic(Map map) {
        _map = map;
    }

    public void SetEntities(PlayerEntity player, ExitEntity exit, List<Button> buttons) {
        _player = player;
        _exit = exit;
        _buttons = buttons;
        _player.OnPlayerMove += OnPlayerMove;
        _player.OnPlayerReset += OnPlayerReset;
        _moves.Clear();
    }

    public void CheckButtons() {
        _exit.IsActive = _buttons.All(b => b.IsActive());
    }

    private void OnPlayerMove(Vector2Int direction) {
        var playerPos = _map.GetPos(_player);
        var destinationEntity = _map.GetEntity(playerPos + direction);
        if (destinationEntity == _exit && _exit.IsActive) {
            var soundController = SoundController.Instance;
            soundController.PlaySound(soundController.DoorEnterClip);
            OnLevelWin?.Invoke();
            return;
        }

        _player.MovementAllowed = false;
        var oldPos = _map.GetPos(_player);
        _map.Move(_player, direction, moves => {
            var soundController = SoundController.Instance;
            if (moves != null && moves.Count != 0) {
                _moves.AddRange(moves);
                soundController.PlaySound(soundController.NotPlayerMoveClip);
            } else if (_map.GetPos(_player) != oldPos) {
                soundController.PlaySound(soundController.PlayerMoveClip);
            }

            _player.MovementAllowed = true;
        });
    }

    private void OnPlayerReset() {
        if (_moves.Count == 0)
            return;

        var soundController = SoundController.Instance;
        var sound = soundController.PlaySound(soundController.ResetClip);
        Reset(() => sound.DOFade(0, 1f).OnComplete(sound.Stop));
    }

    private void Reset(Action onDone = null) {
        _player.MovementAllowed = false;

        void Finish() {
            _moves.Clear();
            onDone?.Invoke();
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
