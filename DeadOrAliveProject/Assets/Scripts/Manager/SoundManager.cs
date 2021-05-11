using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource[] effectSource;
    public AudioSource bgmSource;

    public AudioClip kennyDieClip;
    public AudioClip[] enemyDieClips;
    public AudioClip[] judgeShotClips;
    public AudioClip openClip;

    public AudioClip creditClip;
    public AudioClip gameOverClip;
    public AudioClip settingClip;
    public AudioClip startClip;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Mutation()
    {
        for(int i = 0; i < effectSource.Length; i++)
        {
            effectSource[i].mute = !effectSource[i].mute;
        }

        bgmSource.mute = !bgmSource.mute;
    }

    public void EnemyDie(int index)
    {
        effectSource[0].clip = enemyDieClips[index];
        effectSource[0].Play();
    }

    public void StartSound()
    {
        effectSource[3].clip = startClip;
        effectSource[3].Play();
    }
    public void CreditSound()
    {
        effectSource[3].clip = creditClip;
        effectSource[3].Play();
    }
    public void GameOverSound()
    {
        effectSource[3].clip = gameOverClip;
        effectSource[3].Play();
    }
    public void SettingSound()
    {
        effectSource[3].clip = settingClip;
        effectSource[3].Play();
    }

    public void GameOverSoundStop()
    {
        effectSource[3].Stop();
    }

    public void KyleDie()
    {
        effectSource[1].clip = kennyDieClip;
        effectSource[1].Play();
    }
    public void JudgeShot()
    {
        int rand = Random.Range(0, judgeShotClips.Length);
        effectSource[2].clip = judgeShotClips[rand];
        effectSource[2].Play();
    }

    public void OpenSound()
    {
        effectSource[0].clip = openClip;
        effectSource[0].Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
