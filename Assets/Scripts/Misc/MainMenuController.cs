using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    #region Editor Variable
    [SerializeField]
    [Tooltip("The text component holding the current high score")]
    private Text m_HighScore;
    #endregion

    #region Private Variables
    private string m_DefaultHighScoreText;
    #endregion

    #region Initialization

    private void Start()
    {
        UpdateHighScore();
    }
    private void Awake() {
        Cursor.lockState = CursorLockMode.None;
        m_DefaultHighScoreText = m_HighScore.text;
    }
    #endregion

    #region Play Button methods
    public void PlayArena() {
        SceneManager.LoadScene("Arena");
    }
    #endregion

    #region General Application Button Methods
    public void Quit()
    {
        Application.Quit();
    }
    #endregion

    #region High Score Methods
    public void UpdateHighScore()
    {
        if (PlayerPrefs.HasKey("HS"))
        {
            m_HighScore.text = m_DefaultHighScoreText.Replace("%s", PlayerPrefs.GetInt("HS").ToString());
        } else
        {
            PlayerPrefs.SetInt("HS", 0);
            m_HighScore.text = m_DefaultHighScoreText.Replace("%s", "0");
        }
    }

    public void ResetHighScore()
    {
        PlayerPrefs.SetInt("HS", 0);
        UpdateHighScore();
    }
    #endregion
}
