using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    #region Variables

    [Header("Score")]
    [SerializeField]
    private TextMeshProUGUI _scoreText;

    [Header("Lives")]
    [SerializeField]
    private Image _livesDisplay;

    [SerializeField]
    private Sprite[] _liveSprites;

    [Header("Shield")]
    [SerializeField]
    private TextMeshProUGUI _shieldText;

    [Header("Projectiles")]
    [SerializeField]
    private TextMeshProUGUI _ammoText;

    [SerializeField]
    private TextMeshProUGUI _gameOverText;

    private GameManager _gameManager;
    #endregion

    void Start()
    {
        _gameOverText.gameObject.SetActive(false);
        _scoreText.text = "Score: " + 0;
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        if (_gameManager == null)
        {
            Debug.LogError("Game Manager is NULL");
        }
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        _livesDisplay.sprite = _liveSprites[currentLives];

        if (currentLives == 0)
        {
            GameOverSequence();
        }
    }

    public void UpdateShield(int shieldStrength)
    {
        _shieldText.text = "Shield Strength: " + shieldStrength;
    }

    public void UpdateAmmo(int ammoCount, int ammoCapacity)
    {
        _ammoText.text = ammoCount + " / " + ammoCapacity;
    }

    void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        StartCoroutine("GameOverFlicker");
    }

    IEnumerator GameOverFlicker()
    {
        while (true)
        {
            _gameOverText.text = "Game Over!";
            yield return new WaitForSeconds(.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(.5f);
        }
    }
}
