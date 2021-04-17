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
        score = 0;
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

    //���ӿ����ϸ� �� �ʱ�ȭ.
    void OnGameOver()
    {
        //���ӿ��� �ִϸ��̼�
        judgeRenderer.color = Color.black;
        timer = 0;
        bossEmerge = false;
        isPlaying = false;
        scoreTxt.text = "����!";
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
