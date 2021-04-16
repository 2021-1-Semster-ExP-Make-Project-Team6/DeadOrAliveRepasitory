using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public GameObject enemyObject;  // �� ������Ʈ
    public GameObject judgeObject;  // ���� ������Ʈ
    public GameObject heroObject;   // ���ΰ�(ī��) ������Ʈ

    public Text countingTxt;
    public Text scoreTxt;

    int score = 0;
    int highestScore;
    float randomTime;
    float endTime;
    float loadingTime = 1f;  // ���� �����ϴ°� �ɸ��� �ð�
    float bossLoadingTime;

    int lvLimit = 0;    // ���̵� ����, ��ȹ�� ���ϸ� 43����

    int bossRandomEncount;
    int bossProb = 20;
    bool bossEmerge = false;    // ���� ������ ��Ÿ���� ����

    bool isPlaying = false;
    bool restartAble = false;

    float timer = 0f;

    float countingNum = 5;

    SpriteRenderer judgeRenderer;


    IEnumerator CountingCoroutine()
    {
        countingNum = 5;
        while (timer < randomTime && isPlaying)
        {
            countingNum -= Time.deltaTime;
            countingTxt.text = countingNum.ToString("F2");
            yield return null;

        }
        countingTxt.text = "";
    }

    IEnumerator StartRound()
    {

        //��۴� ����ٰ� �ִϸ��̼�~
        judgeRenderer.color = Color.white;
        bossEmerge = false;
        isPlaying = false;
        restartAble = false;
        scoreTxt.text = score.ToString();
        yield return new WaitForSeconds(3f);
        isPlaying = true;
        timer = 0;
        if (lvLimit < 43)    // 43�ܰ������ ���̵��� ���� ��������ϴ�.
        {
            loadingTime -= 0.02f;
            lvLimit++;
            bossProb++;
        }
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

        StartCoroutine(CountingCoroutine());


    }

    void OnGameOver()
    {
        judgeRenderer.color = Color.black;
        timer = 0;
        bossEmerge = false;
        isPlaying = false;
        scoreTxt.text = "����������";
        if (score > highestScore)
        {
            highestScore = score;
        }
        restartAble = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        score = 0;

        judgeRenderer = judgeObject.GetComponent<SpriteRenderer>();
        isPlaying = false;
        StartCoroutine(StartRound());
    }

    // Update is called once per frame
    void Update()
    {
        //scoreTxt.text = "Score: " + score.ToString();
        if (isPlaying)
        {
            timer += Time.deltaTime;
            if (timer >= randomTime && timer <= endTime)
            {
                judgeRenderer.color = Color.blue;
                if (Input.GetMouseButton(0))
                {
                    //�������߾��~~
                    score++;
                    StartCoroutine(StartRound());
                }
            }
            else
            {
                if (Input.GetMouseButton(0))
                {
                    //���ﴭ���� �׿���
                    OnGameOver();
                }
            }


            if (timer > endTime)
            {
                //�ð��Ѱܼ� �׿���
                OnGameOver();
            }
        }

        if (restartAble)
        {

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("����");
                //����ŸƮ ����
                StartCoroutine(StartRound());
            }
        }


    }
}
