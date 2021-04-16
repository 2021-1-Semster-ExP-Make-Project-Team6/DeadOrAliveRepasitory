using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public GameObject enemyObject;  // 적 오브젝트
    public GameObject judgeObject;  // 심판 오브젝트
    public GameObject heroObject;   // 주인공(카일) 오브젝트

    public Text countingTxt;
    public Text scoreTxt;

    int score = 0;
    int highestScore;
    float randomTime;
    float endTime;
    float loadingTime = 1f;  // 적이 장전하는게 걸리는 시간
    float bossLoadingTime;

    int lvLimit = 0;    // 난이도 제한, 기획에 의하면 43까지

    int bossRandomEncount;
    int bossProb = 20;
    bool bossEmerge = false;    // 보스 출현을 나타내는 변수

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

        //희송님 여기다가 애니메이션~
        judgeRenderer.color = Color.white;
        bossEmerge = false;
        isPlaying = false;
        restartAble = false;
        scoreTxt.text = score.ToString();
        yield return new WaitForSeconds(3f);
        isPlaying = true;
        timer = 0;
        if (lvLimit < 43)    // 43단계까지는 난이도가 점점 어려워집니다.
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
        scoreTxt.text = "ㅋㅋ개못핵";
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
                    //아주잘했어요~~
                    score++;
                    StartCoroutine(StartRound());
                }
            }
            else
            {
                if (Input.GetMouseButton(0))
                {
                    //일찍눌러서 겜오버
                    OnGameOver();
                }
            }


            if (timer > endTime)
            {
                //시간넘겨서 겜오버
                OnGameOver();
            }
        }

        if (restartAble)
        {

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("뭐냐");
                //리스타트 더미
                StartCoroutine(StartRound());
            }
        }


    }
}
