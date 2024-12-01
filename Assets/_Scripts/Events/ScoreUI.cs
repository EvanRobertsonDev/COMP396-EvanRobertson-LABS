using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Start()
    {
        UpdateScore();
    }

    public void UpdateScore()
    {
        StartCoroutine(UpdateScoreNextFrame());
    }

    private IEnumerator UpdateScoreNextFrame()
    {
        yield return new WaitForEndOfFrame();
        scoreText.text = GameManager.Instance.Score.ToString();
    }
}
