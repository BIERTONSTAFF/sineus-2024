using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PostProcessVolume))]
public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    [field: SerializeReference]
    public Vector2 FieldSize { get; private set; }

    [field: SerializeReference]
    public Ball BallPrefab { get; private set; }

    [SerializeField]
    private Transform _background;

    [SerializeField]
    public float _speed;
    [SerializeField]
    public float _speedToAdd;

    private PostProcessVolume _volume;


    [field: SerializeReference]
    public int Score { get; private set; }
    [field: SerializeReference]
    public int Lives { get; private set; } = 3;
    public int Level => SceneManager.GetActiveScene().buildIndex;

    public float Speed => _speed + (Level - 1) * _speedToAdd;

    public int Bricks { get; private set; }


    public bool DecLives()
    {
        if (Bricks == 0) return false;
        if (--Lives == 0)
            StartCoroutine(AddLevel(0));
        return Lives > 0;
    }

    private void Awake()
    {
        Instance = this;

        _volume = GetComponent<PostProcessVolume>();

        BoxCollider2D left = gameObject.AddComponent<BoxCollider2D>(),
                      right = gameObject.AddComponent<BoxCollider2D>(),
                      bottom = gameObject.AddComponent<BoxCollider2D>(),
                      top = gameObject.AddComponent<BoxCollider2D>();

        right.size = left.size = new Vector2(1f, FieldSize.y);
        bottom.size = top.size = new Vector2(FieldSize.x, 1f);


        right.offset = new Vector2(FieldSize.x * .5f + .5f, 0f);
        left.offset = -right.offset;
        top.offset = new Vector2(0f, FieldSize.y * .5f + .5f);
        bottom.offset = -top.offset;

        bottom.isTrigger = true;
        float cameraRadiusY = Camera.main.orthographicSize;
        _background.localScale = new Vector3(FieldSize.x, cameraRadiusY + FieldSize.y * .5f, 1f);
        _background.position = new Vector3(0f, _background.localScale.y * 0.5f - cameraRadiusY);
    }

    private void Start()
    {
        foreach (Brick brick in GameObject.FindGameObjectsWithTag("Brick").Select(x => x.GetComponent<Brick>()).Where(x => x.Breakable))
        {
            brick.OnBreak.AddListener(x =>
            {
                Score += brick.Points;
                if (--Bricks == 0)
                    StartCoroutine(AddLevel(1));
            });
            Bricks++;
        }
    }

    private void Update()
    {
        bool slowMo = Input.GetMouseButton(1);
        _volume.weight = Mathf.Lerp(_volume.weight, slowMo ? 1f : 0f, Time.deltaTime * 5f);
        Time.timeScale = 1f - _volume.weight * .8f;
    }

    private IEnumerator AddLevel(int level)
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(Level + level);
    }
}
