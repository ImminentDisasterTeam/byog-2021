﻿using UnityEngine;

public class CameraController : MonoBehaviour {
    [SerializeField] private new Camera camera;
    private const float Delta = 1f;

    public void Set(Vector2Int size) {
        camera.transform.position = new Vector3(size.x / 2f, size.y / 2f, camera.transform.position.z);
        var levelAspect = (float) size.x / size.y;
        if (levelAspect >= camera.aspect)
            camera.orthographicSize = (size.x + Delta * 2) / camera.aspect / 2;
        else
            camera.orthographicSize = (size.y + Delta * 2) / 2;
    }
}
