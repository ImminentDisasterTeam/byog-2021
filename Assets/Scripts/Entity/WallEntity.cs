﻿using UnityEngine;

public class WallEntity : Entity {
    [SerializeField] private SpriteRenderer _renderer;
    public Sprite Sprite {
        set => _renderer.sprite = value;
    }

    public override bool CanBeMoved() {
        return false;
    }
}
