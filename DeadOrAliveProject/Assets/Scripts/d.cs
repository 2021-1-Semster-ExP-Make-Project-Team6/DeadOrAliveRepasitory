using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class d : MonoBehaviour
{
    [SerializeField] public Animator heroAnim;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            heroAnim.SetInteger("HeroGun", 1); //카일 총 발사
            Debug.Log("HeroGun");
        }
    }
}
