using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public GameObject enemyObject;  // �� ������Ʈ
    public GameObject judgeObject;  // ���� ������Ʈ
    public GameObject heroObject;   // ���ΰ�(ī��) ������Ʈ

    public Text countingTxt;
    public Text scoreTxt;

    int score;
    int highestScore;
    float randomTime;
    float endTime;
    float loadingTime = 1f;  // ���� �����ϴ°� �ɸ��� �ð�
    float bossLoadingTime;

    int lvLimit = 0;    // ���̵� ����, ��ȹ�� ���ϸ� 43����

    int bossRandomEncount;
    int bossProb = 20;
    bool bossEmerge = false;    // ���� ������ ��Ÿ���� ����

    bool startGame = true;
    bool timerstart = false;
    bool counting = false;
    bool shootTrigger = false;
    bool gameOver = false;

    float timer = 0f;

    int countingNum = 5;

    SpriteRenderer judgeRenderer;

    IEnumerator EnemyWalking()
    {
        yield return null;
    }

    IEnumerator StartPlay()
    {


        yield return null;
    }

    IEnumerator WaitForOneSec()
    {

        while (countingNum > 0 && !shootTrigger)
        {
            countingTxt.text = countingNum.ToString();
            countingNum--;
            yield return new WaitForSecondsRealtime(1f);
        }
        countingTxt.text = "";
        counting = false;
        yield return null;

    }

    IEnumerator NextRound()
    {
        startGame = true;
        bossEmerge = false;
        timerstart = false;
        if (lvLimit < 43)    // 43�ܰ������ ���̵��� ���� ��������ϴ�.
        {
            loadingTime -= 0.02f;
            lvLimit++;
            bossProb++;
        }
        yield return null;
    }

    // Start is called before the first frame update
    void Start()
    {
        score = 0;

        judgeRenderer = judgeObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //scoreTxt.text = "Score: " + score.ToString();

        if (startGame)
        {
            bossRandomEncount = Random.Range(0, 100);
            bossLoadingTime = loadingTime * 0.7f;
            if (bossRandomEncount <= bossProb)
            {
                bossEmerge = true;
            }

            randomTime = Random.Range(0.5f, 5);
            if (bossEmerge)
            {
                endTime = randomTime + bossLoadingTime;
            }
            else
            {
                endTime = randomTime + loadingTime;
            }
            timerstart = true;
            startGame = false;
            counting = true;
        }

        if (timerstart)
        {
            timer += Time.deltaTime;
        }

        if(timer >= randomTime && timer <= endTime && timerstart)
        {
            judgeRenderer.color = Color.blue;
            shootTrigger = true;
        }

        if (counting)
        {
            StartCoroutine(WaitForOneSec());
            counting = false;
        }

        if(!shootTrigger && Input.GetMouseButton(0))
        {
            gameOver = true;
        }
        if(timer > endTime)
        {
            shootTrigger = false;
            gameOver = true;
        }

        if(shootTrigger && Input.GetMouseButton(0))
        {
            StartCoroutine(NextRound());
        }

        if (gameOver)
        {
            judgeRenderer.color = Color.black;
            timer = 0;
            timerstart = false;
            bossEmerge = false;


            if(score > highestScore)
            {
                highestScore = score;
            }
        }
    }
}
