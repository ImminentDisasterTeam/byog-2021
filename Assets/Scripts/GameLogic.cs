using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Object = System.Object;

public class GameLogic {
    private static GameLogic _instance;
    public static GameLogic Instance => _instance ??= new GameLogic();
    private GameLogic() {}

    public Action OnLevelWin;
    
    private PlayerEntity _player;
    private ExitEntity _exit;
    private readonly List<(Entity, Vector2Int)> _moves = new List<(Entity, Vector2Int)>();
    private MonoBehaviour _coroRunner;


    public void SetEntities(PlayerEntity player, ExitEntity exit, MonoBehaviour coroRunner) {
        _player = player;
        _exit = exit;
        _coroRunner = coroRunner;
        _player.OnPlayerMove += OnPlayerMove;
        _player.OnPlayerRewind += OnPlayerRewind;
        _moves.Clear();
    }

    private void OnPlayerMove(Vector2Int direction) {
        var playerPos = Map.Instance.GetPos(_player);
        var destinationEntity = Map.Instance.GetEntity(playerPos + direction);
        if (destinationEntity == _exit && _exit.active) {
            OnLevelWin?.Invoke();
            return;
        }
        
        var moves = Map.Instance.Move(_player, direction);
        if (moves != null)
            _moves.AddRange(moves);
    }

    private void OnPlayerRewind() {
        if (_moves.Count == 0)
            return;

        _player.movementAllowed = false;
        _coroRunner.StartCoroutine(Reset());
    }

    IEnumerator Reset() {
        const float timePerMove = 0.2f;

        foreach (var (entity, move) in _moves) {
            Debug.Log($"{_moves.Count}, {entity}, {move}");
            Map.Instance.Move(entity, move * -1);

            yield return new WaitForSeconds(timePerMove);
        }

        _moves.Clear();
        _player.movementAllowed = true;
    }
}
