using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public SoundManager soundManager;

    public GameObject enemyParent;  // 적 오브젝트
    public GameObject judgeObject;  // 심판 오브젝트
    public GameObject heroObject;   // 주인공(카일) 오브젝트

    public GameObject[] enemyObjectArray;
    public GameObject bossObject;

    Vector3 enemyStartPos;
    Vector3 enemyEndPos;

    public Text scoreTxt;
    public Text restartScoreText;

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
    bool onRestart = false;
    bool judgeSoundShotted = false;
    int nowEnemyIndex = 0;

    [SerializeField] public Animator judgeAnim;
    [SerializeField] public Animator heroAnim;
    [SerializeField] public Animator bossAnim;
    [SerializeField] public Animator firstEnemyAnim;
    [SerializeField] public Animator secondEnemyAnim;
    [SerializeField] public Animator thirdEnemyAnim; //애니메이션



    IEnumerator StartRound()
    {

        //희송님 여기다가 애니메이션~
        judgeRenderer.color = Color.white;
        judgeSoundShotted = false;
        bossEmerge = false;
        isPlaying = false;
        restartAble = false;
        scoreTxt.text = score.ToString();
        if (!onRestart)
        {
            yield return new WaitForSeconds(1f);
        }
        scoreTxt.gameObject.SetActive(true);
        if (!onGameBeginning)
        {


            heroAnim.SetBool("HeroAim", false); //카일 총 발사취소
            bossAnim.SetBool("BossDead", false); //보스
            firstEnemyAnim.SetBool("FirstEnemyDead", false); //1 
            secondEnemyAnim.SetBool("SecondEnemyDead", false); //2 
            thirdEnemyAnim.SetBool("ThirdEnemyDead", false); // 3
            bossRandomEncount = Random.Range(0, 100);
            bossLoadingTime = loadingTime * 0.7f;
            if (bossRandomEncount <= bossProb) //보스 출연확률(*)
            {
                bossEmerge = true;
            }
            if (bossEmerge) //보스
            {
                nowEnemyIndex = 3;
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
                nowEnemyIndex = randomIndex;
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
            firstEnemyAnim.SetBool("FirstEnemyWalk", true); //1 
            secondEnemyAnim.SetBool("SecondEnemyWalk", true); //2 
            thirdEnemyAnim.SetBool("ThirdEnemyWalk", true); // 3
            bossAnim.SetBool("BossWalk", true); //boss walk


            float timer = 0;
            while (timer < 1)
            {
                timer += Time.deltaTime;
                enemyParent.transform.position = Vector2.Lerp(enemyStartPos, enemyEndPos, timer); //Enemy 제자리까지 이동(*)
                yield return null; 
            }

            firstEnemyAnim.SetBool("FirstEnemyWalk", false); //1 
            secondEnemyAnim.SetBool("SecondEnemyWalk", false); //2 
            thirdEnemyAnim.SetBool("ThirdEnemyWalk", false); // 3
            bossAnim.SetBool("BossWalk", false); //boss walk
        }
        onGameBeginning = false;
        onRestart = false;

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

       // StartCoroutine(CountingCoroutine());

       
    }

    //게임오버하면 다 초기화.
    void OnGameOver()
    {
        timer = 0;
        bossEmerge = false;
        isPlaying = false;
        restartAble = false;
        if(score == 0)
        {
            restartScoreText.text = "$0";
        }
        else
        {
            restartScoreText.text = "$" + score.ToString() + ",000";
        }
        
        StartCoroutine(GameOverCoroutine());
    }

    IEnumerator GameOverCoroutine()
    {
        yield return new WaitForSeconds(2f);
        //게임오버 애니메이션 초기화
        soundManager.OpenSound();
        judgeAnim.SetBool("JudgeStartFire", false); // 심판 Idle
        judgeAnim.SetBool("JudgeShootHero", false); // 심판 Idle
        bossAnim.SetBool("BossGunFire", false); //보스 Idle
        heroAnim.SetBool("HeroDie", false); //hero Idle

        EnemyAnimationInitialize();

        judgeRenderer.color = Color.black;
        scoreTxt.text = "실패!";
        if (score > highestScore)
        {
            highestScore = score;
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
        onRestart = true;
        restartCanvas.SetActive(false);

        heroAnim.SetBool("HeroAim", false); //카일 총 발사취소
        heroAnim.SetBool("HeroDie", false); //카일 총 발사취소

        EnemyAnimationInitialize();


        StartCoroutine(StartRound());
    }

    void EnemyAnimationInitialize()
    {
        judgeAnim.SetBool("JudgeSignal", false);
        bossAnim.SetBool("BossDead", false); //보스
        firstEnemyAnim.SetBool("FirstEnemyDead", false); //1 
        secondEnemyAnim.SetBool("SecondEnemyDead", false); //2 
        thirdEnemyAnim.SetBool("ThirdEnemyDead", false); // 3

        bossAnim.SetBool("BossAim", false); //보스 조준
        firstEnemyAnim.SetBool("FirstEnemyAim", false);
        secondEnemyAnim.SetBool("SecondEnemyAim", false);
        thirdEnemyAnim.SetBool("ThirdEnemyAim", false);

        bossAnim.SetBool("BossGunFire", false); //보스 공격
        firstEnemyAnim.SetBool("FirstEnemyGunFire", false);//FirstEnemy 공격
        secondEnemyAnim.SetBool("SecondEnemyGunFire", false);
        thirdEnemyAnim.SetBool("ThirdEnemyGunFire", false);
    }

    void Awake()
    {
        if (!PlayerPrefs.HasKey(KeyString))
        {
            PlayerPrefs.SetInt(KeyString, 0);
        }
        settingTimeStop = false;
        highestScore = PlayerPrefs.GetInt(KeyString);  // 하이스코어 디폴트값 0
    }

    // Start is called before the first frame update
    void Start()
    {

        enemyEndPos = new Vector3(1.27f, -0.99f, 0);
        enemyStartPos = new Vector3(4.33f, -0.99f, 0);
        judgeRenderer = judgeObject.GetComponent<SpriteRenderer>();
        isPlaying = false;

        bossRandomEncount = Random.Range(0, 100);
        bossLoadingTime = loadingTime * 0.7f;
        if (bossRandomEncount <= bossProb) //보스 출연확률(*)
        {
            bossEmerge = true;
        }
        if (bossEmerge) //보스
        {
            nowEnemyIndex = 3;
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
            nowEnemyIndex = randomIndex;
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

    }

    public void OnGameStartButton()
    {
        
         StartCoroutine(StartRound());
    }

    IEnumerator JudgeMotionCoroutine()
    {
        timer = 0;
        bossEmerge = false;
        isPlaying = false;
        restartAble = false;
        soundManager.EnemyDie(nowEnemyIndex);
        bossAnim.SetBool("BossDead", true); //보스 사망
        firstEnemyAnim.SetBool("FirstEnemyDead", true);
        secondEnemyAnim.SetBool("SecondEnemyDead", true);
        thirdEnemyAnim.SetBool("ThirdEnemyDead", true);
        heroAnim.SetBool("HeroAim", true); //카일 총 발사
        yield return new WaitForSeconds(0.5f);
        soundManager.KyleDie();

        judgeAnim.SetBool("JudgeShootHero", true); // 심판이 카일 조준 및 쏨(*)
        heroAnim.SetBool("HeroAim", false); //카일 총 발사
        heroAnim.SetBool("HeroDie", true); //카일 die
        //yield return new WaitForSeconds(1f);
        OnGameOver(); //잠시 주석처리 - GameOver 창이 너무 빠르게 떠서 카일/적이 죽는 애니메이션이 안 보임

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

                if (mouseX > 324.4 && mouseX < 646.4 && mouseY > 869.8 && mouseY < 1223.8)
                {
                    return;
                }

            }
            timer += Time.deltaTime;

            if (timer >= randomTime && timer <= endTime)
            {
                //judgeRenderer.color = Color.blue;
                if (!judgeSoundShotted)
                {
                    soundManager.JudgeShot();
                    judgeSoundShotted = true;
                }
         
                 judgeAnim.SetBool("JudgeSignal", false); // 심판 신호탄 준비 애니메이션(*)
                 judgeAnim.SetBool("JudgeStartFire", true); // 심판 신호탄 발사 애니메이션(*)
                 bossAnim.SetBool("BossAim", true); //보스 조준
                 firstEnemyAnim.SetBool("FirstEnemyAim", true);
                 secondEnemyAnim.SetBool("SecondEnemyAim", true);
                 thirdEnemyAnim.SetBool("ThirdEnemyAim", true);

                if (Input.GetMouseButtonDown(0))
                {
                    //아주잘했어요~~
                    heroAnim.SetBool("HeroAim", true); //카일 총 발사
                    soundManager.EnemyDie(nowEnemyIndex);
                    
                    bossAnim.SetBool("BossAim", false); //보스 조준
                    firstEnemyAnim.SetBool("FirstEnemyAim", false);
                    secondEnemyAnim.SetBool("SecondEnemyAim", false);
                    thirdEnemyAnim.SetBool("ThirdEnemyAim",false);
                    
                    bossAnim.SetBool("BossDead", true); //보스 사망
                    firstEnemyAnim.SetBool("FirstEnemyDead", true); //1 
                    secondEnemyAnim.SetBool("SecondEnemyDead", true); //2 
                    thirdEnemyAnim.SetBool("ThirdEnemyDead", true); // 3

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
                    StartCoroutine(JudgeMotionCoroutine());
                }
            }


            if (timer > endTime)
            {
                //시간넘겨서 겜오버

                bossAnim.SetBool("BossGunFire", true); //보스 공격
                firstEnemyAnim.SetBool("FirstEnemyGunFire", true);//FirstEnemy 공격
                secondEnemyAnim.SetBool("SecondEnemyGunFire", true);
                thirdEnemyAnim.SetBool("ThirdEnemyGunFire", true);
                soundManager.KyleDie();
                heroAnim.SetBool("HeroDie", true); //카일 사망
                OnGameOver(); //잠시 주석처리 - GameOver 창 때문에 카일이 죽는 애니메이션이 안 보임
            }
        }

    }
}
