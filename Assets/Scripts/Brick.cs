using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class Brick : MonoBehaviour
{
    [field: SerializeReference]
    public int Lives { get; private set; } = 1;
    private float _livesMax;
    [field: SerializeReference]
    public int Points { get; private set; } = 10;

    [SerializeField]
    private SpriteRenderer[] _sprites;

    public UnityEvent<Brick> OnBreak = new();

    public bool Breakable => Lives >= 0;

    private void Start()
    {
        _livesMax = Lives;
    }

    public void Hit()
    {
        if (!Breakable) return;

        if (--Lives == 0)
            StartCoroutine(Destroy());
        else if (Lives > 0)
        {
            float alpha = Lives / _livesMax;

            foreach (var sprite in _sprites)
            {
                Color color = sprite.color;
                color.a = alpha;
                sprite.color = color;
            }
        }
    }

    public IEnumerator Destroy()
    {
        Transform transform = GetComponent<Transform>();
        GetComponent<BoxCollider2D>().enabled = false;
        OnBreak.Invoke(this);

        for (float t = 0; t < 1; t += Time.deltaTime)
        {
            transform.localScale = new Vector3(1f - t, 1f - t, 1);
            transform.eulerAngles = new Vector3(720f * t, 720f * t, 720f * t);
            yield return null;
        }

        Destroy(gameObject);
    }
}
