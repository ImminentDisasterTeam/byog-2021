using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UI;
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
        _map.Move(_player, direction, moves => {
            if (moves != null && moves.Count != 0) {
                _moves.AddRange(moves);
            }

            _player.MovementAllowed = true;
        }, entities => {
            var soundController = SoundController.Instance;
            if (entities.FindIndex(e => e is SlidingEntity) != -1) {
                soundController.PlaySound(soundController.NotPlayerMoveClip).pitch = 1.2f;
            } else if (entities.FindIndex(e => !(e is PlayerEntity)) != -1) {
                soundController.PlaySound(soundController.NotPlayerMoveClip);
            } else if (entities.FindIndex(e => e is PlayerEntity) != -1) {
                soundController.PlaySound(soundController.PlayerMoveClip);
            }
        });
    }

    private void OnPlayerReset() {
        if (_moves.Count == 0)
            return;

        var soundController = SoundController.Instance;
        var sound = soundController.PlaySound(soundController.ResetClip);
        Reset(() => {
            if (sound.isPlaying)
                sound.DOFade(0, 0.2f).OnComplete(() => {
                    sound.Stop();
                    FinishReset();
                });
            else
                FinishReset();
            
            void FinishReset() {
                UIController.Instance.StopRewind();
                _player.MovementAllowed = true;
            }
        });
    }

    private void Reset(Action onDone = null) {
        _player.MovementAllowed = false;
        UIController.Instance.StartRewind();

        void Finish() {
            _moves.Clear();
            onDone?.Invoke();
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
