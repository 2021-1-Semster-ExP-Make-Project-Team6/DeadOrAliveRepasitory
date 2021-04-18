using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{   
    public GameObject enemyParent;  // 적 오브젝트
    public GameObject judgeObject;  // 심판 오브젝트
    public GameObject heroObject;   // 주인공(카일) 오브젝트

    public GameObject[] enemyObjectArray;
    public GameObject bossObject;

    Vector3 enemyStartPos;
    Vector3 enemyEndPos;

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
    bool onGameBeginning = true;

    [SerializeField] public Animator judgeAnim;
    [SerializeField] public Animator heroAnim;
    [SerializeField] public Animator bossAnim;
    [SerializeField] public Animator firstEnemyAnim;
    [SerializeField] public Animator secondEnemyAnim;
    [SerializeField] public Animator thirdEnemyAnim; //애니메이션



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
        judgeRenderer.color = Color.white;
        
        bossEmerge = false;
        isPlaying = false;
        restartAble = false;
        scoreTxt.text = score.ToString();

        bossRandomEncount = Random.Range(0, 100);
        bossLoadingTime = loadingTime * 0.7f;
        if (bossRandomEncount <= bossProb) //보스 출연확률(*)
        {
            bossEmerge = true;
        }
        if (bossEmerge) //보스
        {
            for (int i = 0; i < enemyObjectArray.Length; i++)
            {
                if (enemyObjectArray[i].activeSelf)
                {
                    enemyObjectArray[i].SetActive(false);
                }
            }
            bossObject.SetActive(true);
        }
        else //졸개 랜덤 출현
        {
            bossObject.SetActive(false);
            int randomIndex = Random.Range(0, 3);
            for (int i = 0; i < enemyObjectArray.Length; i++)
            {
                if (randomIndex == i)
                {
                    enemyObjectArray[i].SetActive(true);
                }
                else
                {
                    enemyObjectArray[i].SetActive(false);
                }
            }
        }
       
        if (!onGameBeginning)
        {
            float timer = 0;
            while (timer < 1)
            {
                timer += Time.deltaTime;
                enemyParent.transform.position = Vector2.Lerp(enemyStartPos, enemyEndPos, timer); //Enemy 제자리까지 이동(*)
                yield return null; 
            }
        }
        onGameBeginning = false;

        yield return null;
        isPlaying = true;
        timer = 0;
        
        if (lvLimit < 43)    // 43단계까지는 난이도가 점점 어려워집니다.
        {
            loadingTime -= 0.02f;
            lvLimit++;
            bossProb++;
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

        bossAnim.SetBool("BossDead", false);//보스 Idle 여기다가 초기화 시키면 안되나요?
    }

    //게임오버하면 다 초기화.
    void OnGameOver()
    {
        //게임오버 애니메이션 초기화
        judgeAnim.SetBool("JudgeStartFire", false); // 심판 Idle
        judgeAnim.SetBool("JudgeShootHero", false); // 심판 Idle
        //heroAnim.SetInteger("HeroGun", 0); //카일 Idle
        bossAnim.SetBool("BossGunFire", false); //보스 Idle
        //bossAnim.SetBool("BossDead", false);//보스 Idle
        heroAnim.SetBool("HeroDie", false); //hero Idle

        judgeRenderer.color = Color.black;
        timer = 0;
        bossEmerge = false;
        isPlaying = false;
        scoreTxt.text = "실패!";
        Debug.Log("겜오버");
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

        enemyEndPos = new Vector3(1.72f, -1.51f, 0);
        enemyStartPos = new Vector3(4.33f, -1.51f, 0);
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
            judgeAnim.SetBool("JudgeSignal", true); // 심판 신호탄 준비 애니메이션(*)
            if (Input.GetMouseButtonDown(0))
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
                //judgeRenderer.color = Color.blue;
                 judgeAnim.SetBool("JudgeSignal", false); // 심판 신호탄 준비 애니메이션(*)
                 judgeAnim.SetBool("JudgeStartFire", true); // 심판 신호탄 발사 애니메이션(*)
                 bossAnim.SetInteger("BossDie", 1); //보스 조준
                 firstEnemyAnim.SetInteger("FirstEnemyDie", 1);
                 secondEnemyAnim.SetInteger("SecondEnemyDie", 1);
                 thirdEnemyAnim.SetInteger("ThirdEnemyDie", 1);

                if (Input.GetMouseButtonDown(0))
                {
                    //아주잘했어요~~
                    heroAnim.SetInteger("HeroGun", 1); //카일 총 발사
                    bossAnim.SetBool("BossDead", true); //보스 사망
                    firstEnemyAnim.SetBool("FirstEnemyDead", true); //1 
                    secondEnemyAnim.SetBool("SecondEnemyDead", true); //2 
                    thirdEnemyAnim.SetBool("ThirdEnemyDead", true); // 3
                    heroAnim.SetInteger("HeroGun", 2); // 카일 총 돌리기
                    //Debug.Log("HeroGun");
                    //enemyAnim.SetBool


                    score++;
                    judgeAnim.SetBool("JudgeStartFire", false); // 심판 Idle(*)
                    StartCoroutine(StartRound());
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    //일찍눌러서 겜오버
                    Debug.Log("일찍눌림");
                    bossAnim.SetBool("BossDead", true); //보스 사망
                    firstEnemyAnim.SetBool("FirstEnemyDead", true); 
                    secondEnemyAnim.SetBool("SecondEnemyDead", true);
                    thirdEnemyAnim.SetBool("ThirdEnemyDead", true);

                    judgeAnim.SetBool("JudgeShootHero", true); // 심판이 카일 조준 및 쏨(*)
                    heroAnim.SetBool("HeroDie", true); //카일 die
                    //heroAnim.SetInteger("HeroGun", 3); //임시
                    //OnGameOver(); //잠시 주석처리 - GameOver 창이 너무 빠르게 떠서 카일/적이 죽는 애니메이션이 안 보임
                }
            }


            if (timer > endTime)
            {
                Debug.Log("시간넘김");
                //시간넘겨서 겜오버

                bossAnim.SetBool("BossGunFire", true); //보스 공격
                firstEnemyAnim.SetBool("FirstEnemyGunFire", true);//FirstEnemy 공격
                secondEnemyAnim.SetBool("SecondEnemyGunFire", true);
                thirdEnemyAnim.SetBool("ThirdEnemyGunFire", true);

                Debug.Log("Enemy의 공격 성공");
                heroAnim.SetBool("HeroDie", true); //카일 사망
                Debug.Log("카일 사망");
                //OnGameOver(); //잠시 주석처리 - GameOver 창 때문에 카일이 죽는 애니메이션이 안 보임
            }
        }

    }
}
