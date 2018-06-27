using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour {

    [SerializeField] Transform _ballPrefab;

    [SerializeField] float _spawnCooldown = 1f;
    [SerializeField] float _minSize = 0.2f;
    [SerializeField] float _maxSize = 2f;

    [SerializeField] float _baseSpeed = 10;

    [SerializeField] Color[] _Colors;

    bool _generateRandomColors;

    ScreenSpaceBounds _screenSpaceBounds;

    struct ScreenSpaceBounds {
        public Vector2 HorizontalBounds;
        public Vector2 VerticalBounds;

        public ScreenSpaceBounds(Vector2 horizontal, Vector2 vertical) {
            HorizontalBounds = horizontal;
            VerticalBounds = vertical;
        }

        public ScreenSpaceBounds(float left, float right, float bottom, float up) {
            HorizontalBounds = new Vector2(left, right);
            VerticalBounds = new Vector2(bottom, up);
        }

        public override string ToString() {
            return $"Left corner: {HorizontalBounds.x}, Right Corner: {HorizontalBounds.y} \n Bottom Corner: {VerticalBounds.x}, UpperCorner: {VerticalBounds.y}";
        }
    }

	void Awake () {
        _screenSpaceBounds = GetScreenSpaceBounds();
    }

    void Start() {
        StartCoroutine(SpawningCoroutine());
    }

    ScreenSpaceBounds GetScreenSpaceBounds() {

        var camera = Camera.main;

        var leftCorner = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.nearClipPlane)).x;
        var rightCorner = camera.ViewportToWorldPoint(new Vector3(1, 0, camera.nearClipPlane)).x;
        var bottomCorner = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.nearClipPlane)).y;
        var upperCorner = camera.ViewportToWorldPoint(new Vector3(0, 1, camera.nearClipPlane)).y;

        var bounds = new ScreenSpaceBounds(leftCorner, rightCorner, bottomCorner, upperCorner);



        return bounds;

    }

    IEnumerator SpawningCoroutine() {
        while (true) {
            var size = Random.Range(_minSize, _maxSize);
            var yToSpawn = _screenSpaceBounds.VerticalBounds.x - (size / 2f);
            var xToSpawn = Random.Range(_screenSpaceBounds.HorizontalBounds.x + (size / 2f), _screenSpaceBounds.HorizontalBounds.y - (size / 2f));
            var spawnedBall = Instantiate(_ballPrefab, new Vector2(xToSpawn, yToSpawn), Quaternion.identity);
            spawnedBall.localScale = Vector3.one * size;
            yield return new WaitForSeconds(_spawnCooldown);
        }
    }
}
