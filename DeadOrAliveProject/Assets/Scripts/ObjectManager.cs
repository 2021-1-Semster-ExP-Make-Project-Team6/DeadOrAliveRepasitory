//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class ObjectManager : MonoBehaviour
//{
//    public GameObject enemyObject;  // 적 오브젝트
//    public GameObject judgeObject;  // 심판 오브젝트
//    public GameObject heroObject;   // 주인공(카일) 오브젝트

//    public Text ObjTxt;
//    SpriteRenderer enemyRenderer;
//    float timer = 0;    // 심판이 랜덤하게 총을 발사하는 시간
//    float loadingTime = 2f;  // 적이 장전하는게 걸리는 시간
//    float endTime;
//    bool forceStop = false; // 

//    // Start is called before the first frame update
//    void Start()
//    {
//        enemyRenderer = enemyObject.GetComponent<SpriteRenderer>();

//        // 적이 쏘는 시간 랜덤 지정
//        timer = Random.Range(0.5f, 5);     // 시간이 랜덤하게 주어짐
//        endTime = timer + loadingTime;  // endTime을 초과하게 되면 게임 오버
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (timer > 0 && Input.GetMouseButton(0))   // 분기 1. 심판이 총을 쏘지 않았는데 너무 일찍 총을 쏜 경우
//        {
//            // 심판 총 발사, 주인공 총 맞는 모션
//            // 현 점수와 최고점수 비교, 게임오버 화면
//            enemyRenderer.color = Color.black;
//            ObjTxt.text = "실패!";
//            // Debug.Log("너무 일찍 쏨!");
//            forceStop = true;
//        }

//        if (endTime <= 0)   // 분기 3. 총을 쏘긴 너무 늦은 경우
//        {
//            // 주인공 총 맞는 모션
//            // 현 점수와 최고점수 비교, 게임오버 화면
//            enemyRenderer.color = Color.black;
//            ObjTxt.text = "실패!";
//            //Debug.Log("총을 쏘긴 너무 늦음...");
//        }
//        else
//        {
//            if (timer <= 0) // 이 때 심판이 통을 쏴야 합니다.
//            {
//                if (!forceStop) // 분기 1로 인해 게임오버되지 않은 경우
//                {
//                    /*
//                     심판 총 쏘는 모션, 적 장전하는 모션
//                     */
//                    enemyRenderer.color = Color.blue;
//                }

//                if (Input.GetMouseButton(0))    // 분기 2. 주인공이 알맞은 시간에 총을 쏜 경우
//                {
//                    // 주인공 총 쏘는 모션, 상대방 총 맞는 모션
//                    // 점수 +1
//                    // 주인공 총 넣는 모션, 심판이 총 내림. 다음 상대가 걸어나옴(앞 상대는 사라짐)
//                    enemyRenderer.color = Color.red;
//                    ObjTxt.text = "성공!";
//                    //Debug.Log("적 처치 성공!");
//                    forceStop = true;
//                }
//            }
//            if (!forceStop)     // 분기 1, 2로 인해 게임오버나 다음 라운드로 넘어가지 않는 경우, timer와 endtime 값이 줄어들면서 인게임 시간이 흐릅니다.
//            {
//                timer -= Time.deltaTime;
//                endTime -= Time.deltaTime;
//            }


//        }

//    }
//}
