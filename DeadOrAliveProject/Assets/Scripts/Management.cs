//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class Management : MonoBehaviour
//{
//    public GameObject enemyObject;  // 적 오브젝트
//    public GameObject judgeObject;  // 심판 오브젝트
//    public GameObject heroObject;   // 주인공(카일) 오브젝트

//    public Text countingTxt;
//    public Text scoreTxt;

//    int score;
//    int highestScore;
//    float timer = 0f;
//    float randomTime;
//    float loadingTime = 1f;  // 적이 장전하는게 걸리는 시간
//    float bossLoadingTime;
//    int lvLimit = 0;    // 난이도 제한, 기획에 의하면 43까지
//    float endTime;
//    int countingNum;
//    int bossRandomEncount;
//    int bossProb = 20;
//    bool bossEmerge = false;    // 보스 출현을 나타내는 변수
//    bool startingGame = true;
//    bool startTrigger = false;
//    bool timerStart = false;
//    bool whileGame = false;
//    bool counting = false;
//    bool forceStop = false; // 게임 오버나 다음 라운드 진출 시에도 timer가 흐르는 걸 방지하기 위한 변수
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

//            if (countingNum > 0) // 코루틴 중첩의 여지가 있음. 수정필요
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

//            if (timer > endTime)    // 총을 쏘기엔 너무 늦음
//            {
//                forceStop = true;
//                timerStart = false;
//                gameOver = true;
//            }
//            else
//            {
//                if (Input.GetMouseButton(0))    // 알맞은 타이밍에 총을 쏨
//                {
//                    judgeRenderer.color = Color.white;
//                    StartCoroutine(NextRound());
//                    score++;
//                    timer = 0;
//                    timerStart = false;
//                    whileGame = false;
//                    if(lvLimit < 43)    // 43단계까지는 난이도가 점점 어려워집니다.
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
//            // 게임 오버
//            timer = 0f;
//            if(highestScore < score)
//            {
                
//            }
//        }
//    }
//}
