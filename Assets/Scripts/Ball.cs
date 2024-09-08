using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Ball : MonoBehaviour
{
    private Rigidbody2D _rb;
    public float Angle;
    public float Radius { get; private set; }

    public Vector2 Directory
    {
        get => new Vector2(Mathf.Sin(Angle * Mathf.Deg2Rad), Mathf.Cos(Angle * Mathf.Deg2Rad)) * GameController.Instance.Speed;
        set { Angle = Vector2.SignedAngle(Vector2.up, value); }
    }


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.simulated = false;

        Radius = GetComponent<BoxCollider2D>().size.x * .5f;
    }

    public void SetPos(Vector3 pos)
    {
        transform.position = pos;
    }

    public void UpdateVelocity()
    {
        _rb.simulated = true;
        _rb.velocity = Directory;
    }

    private void FixedUpdate()
    {
        if (_rb.velocity.x == 0 || _rb.velocity.y == 0)
        {
            Angle = Random.Range(-60f, 60f);
            UpdateVelocity();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 dir = gameObject.transform.position - collision.gameObject.transform.position;
            Angle = dir.x / collision.collider.bounds.size.x * 120f;
            UpdateVelocity();
        }
        else if (collision.gameObject.CompareTag("Brick"))
            collision.gameObject.GetComponent<Brick>().Hit();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GetComponent<BoxCollider2D>().enabled = false;

        if (GameController.Instance.DecLives())
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlatformController>().SpawnBall();
        }
    }
}
