using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public SoundManager soundManager;

    public GameObject enemyParent;  // �� ������Ʈ
    public GameObject judgeObject;  // ���� ������Ʈ
    public GameObject heroObject;   // ���ΰ�(ī��) ������Ʈ

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
    float loadingTime = 1f;  // ���� �����ϴ°� �ɸ��� �ð�
    float bossLoadingTime;

    int lvLimit = 0;    // ���̵� ����, ��ȹ�� ���ϸ� 43����

    int bossRandomEncount;
    int bossProb = 20;
    bool bossEmerge = false;    // ���� ������ ��Ÿ���� ����

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
    [SerializeField] public Animator thirdEnemyAnim; //�ִϸ��̼�



    IEnumerator StartRound()
    {

        //��۴� ����ٰ� �ִϸ��̼�~
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


            heroAnim.SetBool("HeroAim", false); //ī�� �� �߻����
            bossAnim.SetBool("BossDead", false); //����
            firstEnemyAnim.SetBool("FirstEnemyDead", false); //1 
            secondEnemyAnim.SetBool("SecondEnemyDead", false); //2 
            thirdEnemyAnim.SetBool("ThirdEnemyDead", false); // 3
            bossRandomEncount = Random.Range(0, 100);
            bossLoadingTime = loadingTime * 0.7f;
            if (bossRandomEncount <= bossProb) //���� �⿬Ȯ��(*)
            {
                bossEmerge = true;
            }
            if (bossEmerge) //����
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
            else //���� ���� ����
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
                enemyParent.transform.position = Vector2.Lerp(enemyStartPos, enemyEndPos, timer); //Enemy ���ڸ����� �̵�(*)
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
        
        if (lvLimit < 43)    // 43�ܰ������ ���̵��� ���� ��������ϴ�.
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

    //���ӿ����ϸ� �� �ʱ�ȭ.
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
        //���ӿ��� �ִϸ��̼� �ʱ�ȭ
        soundManager.OpenSound();
        judgeAnim.SetBool("JudgeStartFire", false); // ���� Idle
        judgeAnim.SetBool("JudgeShootHero", false); // ���� Idle
        bossAnim.SetBool("BossGunFire", false); //���� Idle
        heroAnim.SetBool("HeroDie", false); //hero Idle

        EnemyAnimationInitialize();

        judgeRenderer.color = Color.black;
        scoreTxt.text = "����!";
        if (score > highestScore)
        {
            highestScore = score;
            PlayerPrefs.SetInt(KeyString, score);
        }
        score = 0;  // score �ʱ�ȭ�� ��� �־����ϴ�.
        restartAble = true;
        //���ӿ��� �� �ٷ� ���ӿ��� �̹��� ǥ��
        restartCanvas.SetActive(true);
    }

    public void Restart()
    {
        //���ӿ����� ����� ��ư
        onRestart = true;
        restartCanvas.SetActive(false);

        heroAnim.SetBool("HeroAim", false); //ī�� �� �߻����
        heroAnim.SetBool("HeroDie", false); //ī�� �� �߻����

        EnemyAnimationInitialize();


        StartCoroutine(StartRound());
    }

    void EnemyAnimationInitialize()
    {
        judgeAnim.SetBool("JudgeSignal", false);
        bossAnim.SetBool("BossDead", false); //����
        firstEnemyAnim.SetBool("FirstEnemyDead", false); //1 
        secondEnemyAnim.SetBool("SecondEnemyDead", false); //2 
        thirdEnemyAnim.SetBool("ThirdEnemyDead", false); // 3

        bossAnim.SetBool("BossAim", false); //���� ����
        firstEnemyAnim.SetBool("FirstEnemyAim", false);
        secondEnemyAnim.SetBool("SecondEnemyAim", false);
        thirdEnemyAnim.SetBool("ThirdEnemyAim", false);

        bossAnim.SetBool("BossGunFire", false); //���� ����
        firstEnemyAnim.SetBool("FirstEnemyGunFire", false);//FirstEnemy ����
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
        highestScore = PlayerPrefs.GetInt(KeyString);  // ���̽��ھ� ����Ʈ�� 0
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
        if (bossRandomEncount <= bossProb) //���� �⿬Ȯ��(*)
        {
            bossEmerge = true;
        }
        if (bossEmerge) //����
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
        else //���� ���� ����
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
        bossAnim.SetBool("BossDead", true); //���� ���
        firstEnemyAnim.SetBool("FirstEnemyDead", true);
        secondEnemyAnim.SetBool("SecondEnemyDead", true);
        thirdEnemyAnim.SetBool("ThirdEnemyDead", true);
        heroAnim.SetBool("HeroAim", true); //ī�� �� �߻�
        yield return new WaitForSeconds(0.5f);
        soundManager.KyleDie();

        judgeAnim.SetBool("JudgeShootHero", true); // ������ ī�� ���� �� ��(*)
        heroAnim.SetBool("HeroAim", false); //ī�� �� �߻�
        heroAnim.SetBool("HeroDie", true); //ī�� die
        //yield return new WaitForSeconds(1f);
        OnGameOver(); //��� �ּ�ó�� - GameOver â�� �ʹ� ������ ���� ī��/���� �״� �ִϸ��̼��� �� ����

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
            judgeAnim.SetBool("JudgeSignal", true); // ���� ��ȣź �غ� �ִϸ��̼�(*)
            if (Input.GetMouseButtonDown(0))
            {
                //�̰� ���������µ� ��ġ�� �νĵǱ淡 ������ 
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
         
                 judgeAnim.SetBool("JudgeSignal", false); // ���� ��ȣź �غ� �ִϸ��̼�(*)
                 judgeAnim.SetBool("JudgeStartFire", true); // ���� ��ȣź �߻� �ִϸ��̼�(*)
                 bossAnim.SetBool("BossAim", true); //���� ����
                 firstEnemyAnim.SetBool("FirstEnemyAim", true);
                 secondEnemyAnim.SetBool("SecondEnemyAim", true);
                 thirdEnemyAnim.SetBool("ThirdEnemyAim", true);

                if (Input.GetMouseButtonDown(0))
                {
                    //�������߾��~~
                    heroAnim.SetBool("HeroAim", true); //ī�� �� �߻�
                    soundManager.EnemyDie(nowEnemyIndex);
                    
                    bossAnim.SetBool("BossAim", false); //���� ����
                    firstEnemyAnim.SetBool("FirstEnemyAim", false);
                    secondEnemyAnim.SetBool("SecondEnemyAim", false);
                    thirdEnemyAnim.SetBool("ThirdEnemyAim",false);
                    
                    bossAnim.SetBool("BossDead", true); //���� ���
                    firstEnemyAnim.SetBool("FirstEnemyDead", true); //1 
                    secondEnemyAnim.SetBool("SecondEnemyDead", true); //2 
                    thirdEnemyAnim.SetBool("ThirdEnemyDead", true); // 3

                    score++;
                    judgeAnim.SetBool("JudgeStartFire", false); // ���� Idle(*)
                    StartCoroutine(StartRound());
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    //���ﴭ���� �׿���
                    StartCoroutine(JudgeMotionCoroutine());
                }
            }


            if (timer > endTime)
            {
                //�ð��Ѱܼ� �׿���

                bossAnim.SetBool("BossGunFire", true); //���� ����
                firstEnemyAnim.SetBool("FirstEnemyGunFire", true);//FirstEnemy ����
                secondEnemyAnim.SetBool("SecondEnemyGunFire", true);
                thirdEnemyAnim.SetBool("ThirdEnemyGunFire", true);
                soundManager.KyleDie();
                heroAnim.SetBool("HeroDie", true); //ī�� ���
                OnGameOver(); //��� �ּ�ó�� - GameOver â ������ ī���� �״� �ִϸ��̼��� �� ����
            }
        }

    }
}
