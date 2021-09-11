using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    static public ScoreManager singleton;
    #region Private Variables
    private int m_Curscore;
    #endregion
    #region Initialization
    private void Awake() {
        if (singleton == null)
        {
            singleton = this;
        } else if (singleton != this)
        {
            Destroy(gameObject);
        }
    }
    #endregion
    #region Score Methods
    public void IncreaseScore(int amount)
    {
        m_Curscore += amount;
    }

    public void UpdateHighScore()
    {
        if (PlayerPrefs.HasKey("HS"))
        {
            PlayerPrefs.SetInt("HS", m_Curscore);
            return;
        }
        int hs = PlayerPrefs.GetInt("HS");
        if (m_Curscore > hs) 
        {
            PlayerPrefs.SetInt("HS", m_Curscore);
        }
    }
    #endregion

    #region Destruction
    public void OnDisable()
    {
        UpdateHighScore();
    }
    #endregion
}
