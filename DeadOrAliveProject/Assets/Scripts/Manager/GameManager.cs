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
    public Text highScoreTxt;

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

    public bool settingTimeStop;
    float timer = 0f;

    float countingNum = 5;

    SpriteRenderer judgeRenderer;

    string KeyString = "HighScore";

    public GameObject restartCanvas;

    //시간초 세는거
    IEnumerator CountingCoroutine()
    {
        countingNum = 5;
        while (timer < randomTime && isPlaying)
        {
            //설정창들어가면 멈추게
            if (settingTimeStop)
            {
                yield return null;
            }
            else
            {
                countingNum -= Time.deltaTime;
                countingTxt.text = countingNum.ToString("F2");
                yield return null;
            }


        }
        countingTxt.text = "";
    }

    IEnumerator StartRound()
    {

        //희송님 여기다가 애니메이션~
        score = 0;
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

    //게임오버하면 다 초기화.
    void OnGameOver()
    {
        //게임오버 애니메이션
        judgeRenderer.color = Color.black;
        timer = 0;
        bossEmerge = false;
        isPlaying = false;
        scoreTxt.text = "실패!";
        if (score > highestScore)
        {
            highestScore = score;
            highScoreTxt.text = score.ToString();
            PlayerPrefs.SetInt(KeyString, score);
        }
        score = 0;  // score 초기화가 없어서 넣었습니다.
        restartAble = true;
        //게임오버 시 바로 게임오버 이미지 표출
        restartCanvas.SetActive(true);
    }

    public void Restart()
    {
        //게임오버시 재시작 버튼
        restartCanvas.SetActive(false);
        StartCoroutine(StartRound());
    }

    void Awake()
    {

        if (!PlayerPrefs.HasKey(KeyString))
        {
            PlayerPrefs.SetInt(KeyString, 0);
        }
        settingTimeStop = false;
        highestScore = PlayerPrefs.GetInt(KeyString);  // 하이스코어 디폴트값 0
        highScoreTxt.text = highestScore.ToString();    // 하이스코어를 저장하는 텍스트
    }

    // Start is called before the first frame update
    void Start()
    {


        judgeRenderer = judgeObject.GetComponent<SpriteRenderer>();
        isPlaying = false;
        
    }

    public void OnGameStartButton()
    {
         StartCoroutine(StartRound());
    }

    // Update is called once per frame
    void Update()
    {
        if (settingTimeStop)
        {
            return;
        }
        //scoreTxt.text = "Score: " + score.ToString();
        if (isPlaying)
        {
            if (Input.GetMouseButton(0))
            {
                //이거 설정누르는데 터치로 인식되길래 일케함 
                float mouseX = Input.mousePosition.x - 720f;
                float mouseY = Input.mousePosition.y - 1280f;

                Debug.Log(mouseX);
                Debug.Log(mouseY);

                if (mouseX > 324.4 && mouseX < 646.4 && mouseY > 869.8 && mouseY < 1223.8)
                {
                    return;
                }

            }
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
