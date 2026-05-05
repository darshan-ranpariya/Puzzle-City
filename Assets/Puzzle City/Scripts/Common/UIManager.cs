using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager inst;
    [Header("Panels")]
    public Panel mainMenuPanel;
    public Panel gamePlayPanel;
    public GameObject gamePlayObject;
    public Panel gameOverPanel;
    public Panel infoPanel;
    public Panel pausePanel;
    public Panel winPanel;
    public Panel limitExceedPanel;
    [Header("UI Elements")]
    public uNumber currScore;
    public uNumber highScore;
    public Image fillImg;
    public float totalTime;
    internal float currTime;
    public GameObject loader;
    public uString gameOverMsg;
    public uString gameWinMsg;
    [Header("Others")]
    public UISwitch[] audioToggles;
    [HideInInspector]
    public bool gameStarted;
    short infoCount;
    private void Awake()
    {
        inst = this;
    }

    private void OnLevelWasLoaded(int level)
    {
        if(level == SceneManager.GetActiveScene().buildIndex)
        {
            infoCount = 0;
        }
    }

    private void OnEnable()
    {
        currScore.Value = 0;
        highScore.Value = PlayerPrefs.GetInt("HighScore", 0);
        UserStatics.isWinApiCalled = false;
        foreach (var sw in audioToggles)
        {
            sw.Set(AudioPlayer.effectsOn);
        }
        currTime = totalTime;
    }

    void ResetGame()
    {
        currTime = totalTime;
        fillImg.fillAmount = currTime / totalTime;
        StopCoroutine(StartTime());
        
    }


    #region ClickMethods
    public void StartBtnClick()
    {
        if (infoCount < 3)
        {
            infoPanel.Activate();
            infoCount++;
            return;
        }
        gamePlayPanel.Activate();
    }

    public void PauseBtnClick()
    {
        StopCoroutine(StartTime());
        pausePanel.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void ResumeBtnClick()
    {
        Time.timeScale = 1;
        pausePanel.gameObject.SetActive(false);
    }

    public void HomeBtnClick()
    {
#if isDebug
        Application.OpenURL("http://www.triangulargamestudio.com/cardList/gamelist.html?customer_id=" + UserStatics.customer_id + "&card_no=" + UserStatics.card_no + "&account_id=" + UserStatics.account_id + "&site_id=" + UserStatics.site_id);
#else
        Application.OpenURL("http://52.172.51.202/gamelist.html?customer_id=" + UserStatics.customer_id + "&card_no=" + UserStatics.card_no + "&account_id=" + UserStatics.account_id + "&site_id=" + UserStatics.site_id);
#endif
        Application.Quit();
    }

    public void RestartBtnClick()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void SoundToggle()
    {
        AudioPlayer.effectsOn = !AudioPlayer.effectsOn;
        AudioPlayer.musicOn = AudioPlayer.effectsOn;
    }

    public void OnInfoPanelOpen()
    {
        infoPanel.Activate();
        Time.timeScale = 0f;
    }

    public void OnInfoPanelClose()
    {
        infoPanel.Deactivate();
        if (gamePlayPanel.gameObject.activeInHierarchy) gamePlayObject.SetActive(true);
        else gamePlayPanel.Activate();
    }

    #endregion

    IEnumerator StartTime()
    {
        while (gameStarted && currTime > 0)
        {
            currTime = Mathf.Clamp(currTime, 0, totalTime);
            yield return new WaitForSeconds(Time.deltaTime * .5f);
            currTime -= Time.deltaTime * 0.5f;
            //print("currTime : " + currTime);
            fillImg.fillAmount = currTime / totalTime;
        }
        if (currTime <= 0)
        {
            OnGameOver();
            ApiCall.inst.CallApi(false);
        }
    }


    public void OnGameOver()
    {
        if (highScore.Value < currScore.Value)
        {
            highScore.Value = currScore.Value;
            PlayerPrefs.SetInt("HighScore", highScore.ValueAsInt);
        }
        gameOverPanel.Activate();
        AudioPlayer.PlaySFX("GameOver");
        //ApiCall.inst.CallApi(false);
    }
    public double score;
    public void UpdateScore()
    {
        currScore.Value += score;
        currScore.Value = currScore.Value >= 0 ? currScore.Value : 0;
        if (!UserStatics.isWinApiCalled && currScore.Value >= UserStatics.requiredScore)
        {
            ApiCall.inst.CallApi(true);
            UserStatics.isWinApiCalled = true;
        }
    }

    
}
