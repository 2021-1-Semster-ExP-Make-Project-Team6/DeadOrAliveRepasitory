using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
    public GameManager gameManager;
    public SoundManager soundManager;
    public GameObject creditButton;
    public GameObject title;
    public GameObject startButton;
    public GameObject startCanvas;

    public GameObject settingCanvas;
    public GameObject creditCanvas;
    public Text highScoreText;

    public AudioSource bgmSource;

    RectTransform creditRect;
    RectTransform titleRect;

    Vector2 creditStartPos;
    Vector2 creditEndPos;

    Vector2 titleStartPos;
    Vector2 titleEndPos;
    const string KeyString = "HighScore";

    void Start()
    {
        Screen.SetResolution(1440, 2560, true);

        if (!PlayerPrefs.HasKey(KeyString))
        {
            PlayerPrefs.SetInt(KeyString, 0);
        }

        creditRect = creditButton.GetComponent<RectTransform>();
        titleRect = title.GetComponent<RectTransform>();

        creditStartPos = creditRect.anchoredPosition;
        creditEndPos = new Vector2(502, -1382);

        titleStartPos = titleRect.anchoredPosition;
        titleEndPos =  new Vector2(0, 1600);
        highScoreText.text = PlayerPrefs.GetInt(KeyString).ToString();
    }

    //UI기어나가는거
    IEnumerator UIGetOutCor()
    {
        Vector2 creditLerp;
        Vector2 titleLerp;
        float timer = 0;

        while (timer < 1)
        {
            creditLerp = Vector2.Lerp(creditStartPos, creditEndPos, timer);
            titleLerp = Vector2.Lerp(titleStartPos, titleEndPos, timer);
            creditRect.anchoredPosition = creditLerp;
            titleRect.anchoredPosition = titleLerp;
            timer += Time.deltaTime;

            yield return null;
        }
        startCanvas.SetActive(false);
    }

    IEnumerator UIGetInCor()
    {
        Vector2 creditLerp;
        Vector2 titleLerp;
        float timer = 0;

        while (timer < 1)
        {
            creditLerp = Vector2.Lerp(creditEndPos, creditStartPos, timer);
            titleLerp = Vector2.Lerp(titleEndPos, titleStartPos, timer);
            creditRect.anchoredPosition = creditLerp;
            titleRect.anchoredPosition = titleLerp;
            timer += Time.deltaTime;

            yield return null;
        }

    }

    void UIGetIn()
    {
        StartCoroutine(UIGetInCor());
    }

    void UIGetOut()
    {
        StartCoroutine(UIGetOutCor());
    }

    public void MuteButton()
    {
        bgmSource.mute = true;
        bgmSource.volume = 0.5f;
    }

    public void OnCreditButton()
    {
        if (Time.timeScale == 0)
        {
            return;
        }
        soundManager.OpenSound();
        creditCanvas.SetActive(!creditCanvas.activeSelf);
    }

    public void OnSettingButton()
    {

        gameManager.settingTimeStop = !gameManager.settingTimeStop;

        if (Time.timeScale == 1)
        {
            highScoreText.text = PlayerPrefs.GetInt(KeyString).ToString();
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
        soundManager.OpenSound();
        settingCanvas.SetActive(!settingCanvas.activeSelf);
    }

    public void OnSoundToggle()
    {
        soundManager.Mutation();
    }
    public void EndButton()
    {
        Application.Quit();
    }
    public void GameStartButton()
    {
        soundManager.OpenSound();
        UIGetOut();
        gameManager.OnGameStartButton();
        startButton.SetActive(false);
    }

}