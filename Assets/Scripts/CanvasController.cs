using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText, _levelClearedText;
    [SerializeField]
    private Animator _levelCleared;
    private string _scoreFormat;

    private int _activate = Animator.StringToHash("Activate");

    private void Awake()
    {
        _scoreFormat = _scoreText.text;
    }

    private void SetScore(int score, int level, int lives)
    {
        _scoreText.text = string.Format(_scoreFormat, score, level, lives);
    }

    private void ShowLevelCleared()
    {
        _levelCleared.SetTrigger(_activate);
    }

    private void Update()
    {
        SetScore(GameController.Instance.Score, GameController.Instance.Level, GameController.Instance.Lives);
        if (GameController.Instance.Bricks == 0)
            ShowLevelCleared();
        else if (GameController.Instance.Lives == 0)
        {
            _levelClearedText.text = "Уровень провален";
            ShowLevelCleared();
        }
    }
}
