using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameoverDialog : Dialog
{
    public Text bestScoreText;
    bool m_replayBtnClicked;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void Show(bool isShow)
    {
        base.Show(isShow);

        if (bestScoreText)
            bestScoreText.text = Prefs.bestScore.ToString();
    }

    public void Replay()
    {
        m_replayBtnClicked = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    public void BackToHome()
    {
        GameGUIManager.Ins.ShowGameGui(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (m_replayBtnClicked)
        {
            GameGUIManager.Ins.ShowGameGui(true);

            GameManager.Ins.PlayGame();
        }

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
