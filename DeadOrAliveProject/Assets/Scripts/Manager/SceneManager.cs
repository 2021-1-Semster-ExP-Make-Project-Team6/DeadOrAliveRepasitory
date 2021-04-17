using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject creditButton;
    public GameObject title;
    public GameObject startButton;
    public GameObject startCanvas;

    public GameObject settingCanvas;
    public GameObject creditCanvas;

    public AudioSource bgmSource;

    RectTransform creditRect;
    RectTransform titleRect;

    Vector2 creditStartPos;
    Vector2 creditEndPos;

    Vector2 titleStartPos;
    Vector2 titleEndPos;

    void Start()
    {
        Screen.SetResolution(1440, 2560, true);

        creditRect = creditButton.GetComponent<RectTransform>();
        titleRect = title.GetComponent<RectTransform>();

        creditStartPos = creditRect.anchoredPosition;
        creditEndPos = new Vector2(0, 1300);

        titleStartPos = titleRect.anchoredPosition;
        titleEndPos = new Vector2(502, -1382);
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
        creditCanvas.SetActive(!creditCanvas.activeSelf);
    }

    public void OnSettingButton()
    {
       gameManager.settingTimeStop = !gameManager.settingTimeStop;

        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
        
        settingCanvas.SetActive(!settingCanvas.activeSelf);
    }

    public void GameStartButton()
    {
        UIGetOut();
        gameManager.OnGameStartButton();
        startButton.SetActive(false);
    }

}