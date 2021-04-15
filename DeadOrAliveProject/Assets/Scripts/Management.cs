//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class Management : MonoBehaviour
//{
//    public GameObject enemyObject;  // �� ������Ʈ
//    public GameObject judgeObject;  // ���� ������Ʈ
//    public GameObject heroObject;   // ���ΰ�(ī��) ������Ʈ

//    public Text countingTxt;
//    public Text scoreTxt;

//    int score;
//    int highestScore;
//    float timer = 0f;
//    float randomTime;
//    float loadingTime = 1f;  // ���� �����ϴ°� �ɸ��� �ð�
//    float bossLoadingTime;
//    int lvLimit = 0;    // ���̵� ����, ��ȹ�� ���ϸ� 43����
//    float endTime;
//    int countingNum;
//    int bossRandomEncount;
//    int bossProb = 20;
//    bool bossEmerge = false;    // ���� ������ ��Ÿ���� ����
//    bool startingGame = true;
//    bool startTrigger = false;
//    bool timerStart = false;
//    bool whileGame = false;
//    bool counting = false;
//    bool forceStop = false; // ���� ������ ���� ���� ���� �ÿ��� timer�� �帣�� �� �����ϱ� ���� ����
//    bool gameOver = false;

//    SpriteRenderer judgeRenderer;


//    IEnumerator StartPlay()
//    {
//        randomTime = Random.Range(0.5f, 5);
//        if (bossEmerge)
//        {
//            endTime = bossLoadingTime + randomTime;
//        }
//        else
//        {
//            endTime = loadingTime + randomTime;
//        }
//        timerStart = true;
//        yield return new WaitForSecondsRealtime(randomTime);
//        judgeRenderer.color = Color.blue;
//        startTrigger = true;
//        whileGame = true;
//    }

//    IEnumerator CountWait()
//    {
//        if (timerStart)
//        {
//            counting = false;
//            yield return new WaitForSecondsRealtime(1f);
//            countingNum--;
//            if (countingNum > 0)
//            {
//                counting = true;
//            }
//        }
//    }

//    IEnumerator RemoveCountTxt()
//    {
//        countingTxt.text = "";
//        yield return null;
//    }

//    IEnumerator NextRound()
//    {
//        yield return new WaitForSecondsRealtime(3f);
//        startingGame = true;
//    }

//    // Start is called before the first frame update
//    void Start()
//    {
//        score = 0;

//        judgeRenderer = judgeObject.GetComponent<SpriteRenderer>();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        scoreTxt.text = "Score: " + score.ToString();

//        if (startingGame)
//        {
//            bossRandomEncount = Random.Range(0, 100);
//            bossLoadingTime = loadingTime * 0.7f;
//            if(bossRandomEncount <= bossProb)
//            {
//                bossEmerge = true;
//            }
//            StartCoroutine(StartPlay());
//            forceStop = false;
//            startingGame = false;
//            countingNum = 5;
//            counting = true;
//        }

//        if (counting)
//        {
//            if(!startTrigger && Input.GetMouseButton(0))
//            {
//                counting = false;
//                timerStart = false;
//                gameOver = true;
//            }

//            if (startTrigger)
//            {
//                StartCoroutine(RemoveCountTxt());
//                counting = false;
//            }

//            if (countingNum > 0) // �ڷ�ƾ ��ø�� ������ ����. �����ʿ�
//            {
//                countingTxt.text = countingNum.ToString();
//                StartCoroutine(CountWait());
//            }
//            else
//            {
//                StartCoroutine(RemoveCountTxt());
//                counting = false;
//            }
//        }

//        if (timerStart)
//        {
//            timer += Time.deltaTime;
//        }

//        if (whileGame)
//        {


//            //if (timer < randomTime && Input.GetMouseButton(0))
//            //{
//            //    forceStop = true;
//            //    gameOver = true;
//            //}

//            if (timer > endTime)    // ���� ��⿣ �ʹ� ����
//            {
//                forceStop = true;
//                timerStart = false;
//                gameOver = true;
//            }
//            else
//            {
//                if (Input.GetMouseButton(0))    // �˸��� Ÿ�ֿ̹� ���� ��
//                {
//                    judgeRenderer.color = Color.white;
//                    StartCoroutine(NextRound());
//                    score++;
//                    timer = 0;
//                    timerStart = false;
//                    whileGame = false;
//                    if(lvLimit < 43)    // 43�ܰ������ ���̵��� ���� ��������ϴ�.
//                    {
//                        loadingTime -= 0.02f;
//                        bossEmerge = false;
//                        lvLimit++;
//                        bossProb++;
//                    }
//                }
//            }
//        }

//        if (gameOver)
//        {
//            // ���� ����
//            timer = 0f;
//            if(highestScore < score)
//            {
                
//            }
//        }
//    }
//}
