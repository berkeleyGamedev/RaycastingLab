using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class GameManager : MonoBehaviour
{
    #region Delegates and Events
    public delegate void EmptyDelegate();
    public static event EmptyDelegate GameStartedEvent;
    public static event EmptyDelegate GameEndedEvent;
    #endregion

    #region Editor Variables
    [Header("In Game Menu")]

    [SerializeField]
    [Tooltip("The panel to use when the game starts.")]
    private RectTransform m_GamePanel;

    [SerializeField]
    [Tooltip("The text UI game object that shows health left.")]
    private Text m_HealthText;

    [SerializeField]
    [Tooltip("The text UI game object that shows the current score.")]
    private Text m_ScoreText;

    [Header("Main Menu")]
    
    [SerializeField]
    [Tooltip("The panel to use as a main menu.")]
    private RectTransform m_MenuPanel;

    [SerializeField]
    [Tooltip("The text UI game object that shows the best score.")]
    private Text m_BestScoreText;
    #endregion

    #region Private Variables
    private int p_HealthLeft;

    private int p_CurrentScore;

    private int p_BestScore;
    #endregion

    #region Initialization
    private void Awake()
    {
        p_BestScore = 0;

        m_GamePanel.gameObject.SetActive(false);
        m_MenuPanel.gameObject.SetActive(true);
    }
    #endregion

    #region Starter and Ender Methods
    public void StartGame()
    {
        GameStartedEvent?.Invoke();

        m_GamePanel.gameObject.SetActive(true);
        m_MenuPanel.gameObject.SetActive(false);

        p_HealthLeft = 10;
        m_HealthText.text = "Health Left: " + p_HealthLeft.ToString();

        p_CurrentScore = 0;
        m_ScoreText.text = "Score: " + p_CurrentScore.ToString();
    }

    private void EndGame()
    {
        GameEndedEvent?.Invoke();

        m_GamePanel.gameObject.SetActive(false);
        m_MenuPanel.gameObject.SetActive(true);

        if (p_CurrentScore > p_BestScore)
            m_BestScoreText.text = "Best Score: " + p_CurrentScore.ToString();
    }
    #endregion
    
    #region OnEnable and OnDisable
    private void OnEnable()
    {
        Enemy.EnemyPassedEvent += DecreaseHealth;
        Attack.EnemyKilledEvent += IncreaseScore;
    }

    private void OnDisable()
    {
        Enemy.EnemyPassedEvent -= DecreaseHealth;
        Attack.EnemyKilledEvent -= IncreaseScore;
    }
    #endregion
    
    #region Health and Score Updates
    private void DecreaseHealth()
    {
        p_HealthLeft--;
        m_HealthText.text = "Health Left: " + p_HealthLeft.ToString();

        if (p_HealthLeft == 0)
            EndGame();
    }

    private void IncreaseScore()
    {
        p_CurrentScore++;
        m_ScoreText.text = "Score: " + p_CurrentScore.ToString();
    }
    #endregion
}
