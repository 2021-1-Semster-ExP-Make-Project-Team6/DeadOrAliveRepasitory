//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class ObjectManager : MonoBehaviour
//{
//    public GameObject enemyObject;  // �� ������Ʈ
//    public GameObject judgeObject;  // ���� ������Ʈ
//    public GameObject heroObject;   // ���ΰ�(ī��) ������Ʈ

//    public Text ObjTxt;
//    SpriteRenderer enemyRenderer;
//    float timer = 0;    // ������ �����ϰ� ���� �߻��ϴ� �ð�
//    float loadingTime = 2f;  // ���� �����ϴ°� �ɸ��� �ð�
//    float endTime;
//    bool forceStop = false; // 

//    // Start is called before the first frame update
//    void Start()
//    {
//        enemyRenderer = enemyObject.GetComponent<SpriteRenderer>();

//        // ���� ��� �ð� ���� ����
//        timer = Random.Range(0.5f, 5);     // �ð��� �����ϰ� �־���
//        endTime = timer + loadingTime;  // endTime�� �ʰ��ϰ� �Ǹ� ���� ����
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (timer > 0 && Input.GetMouseButton(0))   // �б� 1. ������ ���� ���� �ʾҴµ� �ʹ� ���� ���� �� ���
//        {
//            // ���� �� �߻�, ���ΰ� �� �´� ���
//            // �� ������ �ְ����� ��, ���ӿ��� ȭ��
//            enemyRenderer.color = Color.black;
//            ObjTxt.text = "����!";
//            // Debug.Log("�ʹ� ���� ��!");
//            forceStop = true;
//        }

//        if (endTime <= 0)   // �б� 3. ���� ��� �ʹ� ���� ���
//        {
//            // ���ΰ� �� �´� ���
//            // �� ������ �ְ����� ��, ���ӿ��� ȭ��
//            enemyRenderer.color = Color.black;
//            ObjTxt.text = "����!";
//            //Debug.Log("���� ��� �ʹ� ����...");
//        }
//        else
//        {
//            if (timer <= 0) // �� �� ������ ���� ���� �մϴ�.
//            {
//                if (!forceStop) // �б� 1�� ���� ���ӿ������� ���� ���
//                {
//                    /*
//                     ���� �� ��� ���, �� �����ϴ� ���
//                     */
//                    enemyRenderer.color = Color.blue;
//                }

//                if (Input.GetMouseButton(0))    // �б� 2. ���ΰ��� �˸��� �ð��� ���� �� ���
//                {
//                    // ���ΰ� �� ��� ���, ���� �� �´� ���
//                    // ���� +1
//                    // ���ΰ� �� �ִ� ���, ������ �� ����. ���� ��밡 �ɾ��(�� ���� �����)
//                    enemyRenderer.color = Color.red;
//                    ObjTxt.text = "����!";
//                    //Debug.Log("�� óġ ����!");
//                    forceStop = true;
//                }
//            }
//            if (!forceStop)     // �б� 1, 2�� ���� ���ӿ����� ���� ����� �Ѿ�� �ʴ� ���, timer�� endtime ���� �پ��鼭 �ΰ��� �ð��� �帨�ϴ�.
//            {
//                timer -= Time.deltaTime;
//                endTime -= Time.deltaTime;
//            }


//        }

//    }
//}
