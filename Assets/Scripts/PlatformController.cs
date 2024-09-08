using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PlatformController : MonoBehaviour
{
    private Transform _transform;
    private Camera _camera;
    private float _boundary, _y;
    private Vector3 _ballOffset;
    private Ball _ball;

    private void Awake()
    {
        _camera = Camera.main;
        _transform = GetComponent<Transform>();
    }

    private void Start()
    {
        SpawnBall();
        Vector2 pos = GameController.Instance.FieldSize / 2;
        Vector2 sizeRadius = GetComponent<BoxCollider2D>().size / 2;
        _y = sizeRadius.y - pos.y;
        _boundary = pos.x - sizeRadius.x;
        _ballOffset = new(0f, sizeRadius.y + _ball.Radius + 0.125f);

        _ball.Angle = 0f;
    }

    public void SpawnBall()
    {
        _ball = Instantiate(GameController.Instance.BallPrefab, new Vector3(0f, -10f), Quaternion.identity);
    }

    private float Clamp(float x) => Mathf.Clamp(x, -_boundary, _boundary);

    private void Update()
    {
        Vector3 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);

        _transform.position = new Vector3(Clamp(mousePos.x), _y);

        if (_ball)
        {
            _ball.SetPos(_transform.position + _ballOffset);

            if (Input.GetMouseButtonDown(0))
            {
                _ball.UpdateVelocity();
                _ball = null;
            }
        }
    }
}
