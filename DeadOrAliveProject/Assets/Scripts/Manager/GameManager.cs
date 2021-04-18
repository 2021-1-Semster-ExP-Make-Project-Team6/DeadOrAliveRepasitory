using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{   
    public GameObject enemyParent;  // �� ������Ʈ
    public GameObject judgeObject;  // ���� ������Ʈ
    public GameObject heroObject;   // ���ΰ�(ī��) ������Ʈ

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

    [SerializeField] public Animator judgeAnim;
    [SerializeField] public Animator heroAnim;
    [SerializeField] public Animator bossAnim;
    [SerializeField] public Animator firstEnemyAnim;
    [SerializeField] public Animator secondEnemyAnim;
    [SerializeField] public Animator thirdEnemyAnim; //�ִϸ��̼�



    //�ð��� ���°�
    IEnumerator CountingCoroutine()
    {
        countingNum = 5;
        while (timer < randomTime && isPlaying)
        {
            //����â���� ���߰�
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

        //��۴� ����ٰ� �ִϸ��̼�~
        judgeRenderer.color = Color.white;
        
        bossEmerge = false;
        isPlaying = false;
        restartAble = false;
        scoreTxt.text = score.ToString();

        bossRandomEncount = Random.Range(0, 100);
        bossLoadingTime = loadingTime * 0.7f;
        if (bossRandomEncount <= bossProb) //���� �⿬Ȯ��(*)
        {
            bossEmerge = true;
        }
        if (bossEmerge) //����
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
        else //���� ���� ����
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
                enemyParent.transform.position = Vector2.Lerp(enemyStartPos, enemyEndPos, timer); //Enemy ���ڸ����� �̵�(*)
                yield return null; 
            }
        }
        onGameBeginning = false;

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

        StartCoroutine(CountingCoroutine());

        bossAnim.SetBool("BossDead", false);//���� Idle ����ٰ� �ʱ�ȭ ��Ű�� �ȵǳ���?
    }

    //���ӿ����ϸ� �� �ʱ�ȭ.
    void OnGameOver()
    {
        //���ӿ��� �ִϸ��̼� �ʱ�ȭ
        judgeAnim.SetBool("JudgeStartFire", false); // ���� Idle
        judgeAnim.SetBool("JudgeShootHero", false); // ���� Idle
        //heroAnim.SetInteger("HeroGun", 0); //ī�� Idle
        bossAnim.SetBool("BossGunFire", false); //���� Idle
        //bossAnim.SetBool("BossDead", false);//���� Idle
        heroAnim.SetBool("HeroDie", false); //hero Idle

        judgeRenderer.color = Color.black;
        timer = 0;
        bossEmerge = false;
        isPlaying = false;
        scoreTxt.text = "����!";
        Debug.Log("�׿���");
        if (score > highestScore)
        {
            highestScore = score;
            highScoreTxt.text = score.ToString();
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
        highestScore = PlayerPrefs.GetInt(KeyString);  // ���̽��ھ� ����Ʈ�� 0
        highScoreTxt.text = highestScore.ToString();    // ���̽��ھ �����ϴ� �ؽ�Ʈ
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
            judgeAnim.SetBool("JudgeSignal", true); // ���� ��ȣź �غ� �ִϸ��̼�(*)
            if (Input.GetMouseButtonDown(0))
            {
                //�̰� ���������µ� ��ġ�� �νĵǱ淡 ������ 
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
                 judgeAnim.SetBool("JudgeSignal", false); // ���� ��ȣź �غ� �ִϸ��̼�(*)
                 judgeAnim.SetBool("JudgeStartFire", true); // ���� ��ȣź �߻� �ִϸ��̼�(*)
                 bossAnim.SetInteger("BossDie", 1); //���� ����
                 firstEnemyAnim.SetInteger("FirstEnemyDie", 1);
                 secondEnemyAnim.SetInteger("SecondEnemyDie", 1);
                 thirdEnemyAnim.SetInteger("ThirdEnemyDie", 1);

                if (Input.GetMouseButtonDown(0))
                {
                    //�������߾��~~
                    heroAnim.SetInteger("HeroGun", 1); //ī�� �� �߻�
                    bossAnim.SetBool("BossDead", true); //���� ���
                    firstEnemyAnim.SetBool("FirstEnemyDead", true); //1 
                    secondEnemyAnim.SetBool("SecondEnemyDead", true); //2 
                    thirdEnemyAnim.SetBool("ThirdEnemyDead", true); // 3
                    heroAnim.SetInteger("HeroGun", 2); // ī�� �� ������
                    //Debug.Log("HeroGun");
                    //enemyAnim.SetBool


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
                    Debug.Log("���ﴭ��");
                    bossAnim.SetBool("BossDead", true); //���� ���
                    firstEnemyAnim.SetBool("FirstEnemyDead", true); 
                    secondEnemyAnim.SetBool("SecondEnemyDead", true);
                    thirdEnemyAnim.SetBool("ThirdEnemyDead", true);

                    judgeAnim.SetBool("JudgeShootHero", true); // ������ ī�� ���� �� ��(*)
                    heroAnim.SetBool("HeroDie", true); //ī�� die
                    //heroAnim.SetInteger("HeroGun", 3); //�ӽ�
                    //OnGameOver(); //��� �ּ�ó�� - GameOver â�� �ʹ� ������ ���� ī��/���� �״� �ִϸ��̼��� �� ����
                }
            }


            if (timer > endTime)
            {
                Debug.Log("�ð��ѱ�");
                //�ð��Ѱܼ� �׿���

                bossAnim.SetBool("BossGunFire", true); //���� ����
                firstEnemyAnim.SetBool("FirstEnemyGunFire", true);//FirstEnemy ����
                secondEnemyAnim.SetBool("SecondEnemyGunFire", true);
                thirdEnemyAnim.SetBool("ThirdEnemyGunFire", true);

                Debug.Log("Enemy�� ���� ����");
                heroAnim.SetBool("HeroDie", true); //ī�� ���
                Debug.Log("ī�� ���");
                //OnGameOver(); //��� �ּ�ó�� - GameOver â ������ ī���� �״� �ִϸ��̼��� �� ����
            }
        }

    }
}
