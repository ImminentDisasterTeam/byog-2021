using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;
using Object = UnityEngine.Object;

public class Map {
    public Action OnButtonsUpdate;
    public static readonly Vector2Int NoPos = Vector2Int.one * -1;
    private List<List<Entity>> _map;
    private List<List<Button>> _buttons;
    private List<List<Floor>> _floors;

    public void SetMap(List<List<Entity>> map, List<List<Button>> buttons, List<List<Floor>> floors) {
        Clear();
        _map = map;
        _buttons = buttons;
        _floors = floors;
        for (var i = 0; i < _map.Count; i++) {
            for (var j = 0; j < _map[i].Count; j++) {
                _map[i][j]?.SetPosition(new Vector2Int(i, j));
                _buttons[i][j]?.SetPosition(new Vector2Int(i, j));
                _floors[i][j]?.SetPosition(new Vector2Int(i, j));
            }
        }
        UpdateButtons(true);
    }

    public Vector2Int GetPos(Entity entity) {
        var position = NoPos;
        for (var i = 0; i < _map.Count; i++) {
            for (var j = 0; j < _map[i].Count; j++) {
                if (_map[i][j] == entity) {
                    position = new Vector2Int(i, j);
                    break;
                }
            }
        }

        if (position == NoPos)
            throw new Exception("NO ENTITY");

        return position;
    }

    public Entity GetEntity(Vector2Int position) {
        return OnBoard(position) ? _map[position.x][position.y] : null;
    }

    private bool OnBoard(Vector2Int p) {
        return p.x >= 0 && p.y >= 0 && p.x < _map.Count && p.y < _map[0].Count;
    }

    public void Clear() {
        if (_map == null) {
            return;
        }

        foreach (var row in _map) {
            foreach (var tile in row) {
                if (tile != null)
                    Object.Destroy(tile.gameObject);
            }
        }

        foreach (var row in _buttons) {
            foreach (var button in row) {
                if (button != null)
                    Object.Destroy(button.gameObject);
            }
        }

        foreach (var row in _floors) {
            foreach (var floor in row) {
                if (floor != null)
                    Object.Destroy(floor.gameObject);
            }
        }

        _map = null;
        _buttons = null;
    }

    public void Move(Entity entity, Vector2Int direction, Action<List<(Entity, Vector2Int)>> onDone) {
        if (direction.magnitude != 1)
            throw new Exception($"DIRECTION MAGNITUDE != 1; direction: {direction}");

        var position = GetPos(entity);
        var positionsToMove = new List<Vector2Int> {position};
        var movedEntities = new List<Entity>();
        if (!(entity is PlayerEntity))
            movedEntities.Add(entity);

        for (var nextPos = position + direction; OnBoard(nextPos); nextPos += direction) {
            var currentEntity = GetEntity(nextPos);
            if (currentEntity == null)
                break;

            if (!currentEntity.CanBeMoved()) {
                onDone(null);
                return;
            }

            positionsToMove.Add(nextPos);
            movedEntities.Add(currentEntity);
        }

        var moveFuncs = new List<Action<Action>>();
        for (var i = positionsToMove.Count - 1; i >= 0; i--) {
            var pos = positionsToMove[i];
            var nextPos = pos + direction;

            var objToMove = _map[pos.x][pos.y];
            _map[nextPos.x][nextPos.y] = objToMove;
            _map[pos.x][pos.y] = null;
            
            moveFuncs.Add(onDone => {
                objToMove.SetPosition(nextPos, onDone);
            });
        }

        new Gather(moveFuncs, AfterMove);

        void AfterMove() {
            UpdateButtons();
            var appliedMoves = movedEntities.Select(e => (e, direction)).ToList();
            var slider = entity as SlidingEntity ??
                         movedEntities.Count > 0
                ? movedEntities.FirstOrDefault(e => e is SlidingEntity) as SlidingEntity
                : null;
            if (slider != null) {
                Move(slider, direction, nextMoves => {
                    if (nextMoves != null)
                        appliedMoves.AddRange(nextMoves);
                    onDone(appliedMoves);
                });

                return;
            }

            onDone(appliedMoves);
        }
    }

    private void UpdateButtons(bool setup = false) {
        bool anyActive = false;
        for (var i = 0; i < _map.Count; i++) {
            for (var j = 0; j < _map[i].Count; j++) {
                var btn = _buttons[i][j];
                if (btn != null) {
                    var pressed = _map[i][j] != null;
                    var oldActive = btn.IsActive();
                    btn.IsPressed = pressed;
                    anyActive = anyActive || !oldActive && btn.IsActive();
                }
            }
        }

        if (!setup && anyActive) {
            var soundController = SoundController.Instance;
            soundController.PlaySound(soundController.LevelButtonPressClip);
        }

        OnButtonsUpdate?.Invoke();
    }
}
