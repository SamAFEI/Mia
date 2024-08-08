using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioClip BGMClip;
    public AudioClip HitClip;
    public AudioClip AttackClip;
    public AudioClip DashClip;
    public AudioClip ShieldClip;
    public AudioClip StatueClip;
    public AudioClip DarkClip;
    public AudioClip SpellClip;
    public AudioClip ShockWaveClip;
    public AudioClip MainClip;
    public List<AudioClip> BattleList;
    public List<AudioClip> BossList;
    public AudioClip CheckPointClip;
    public AudioClip DeathClip;
    public AudioClip sighClip;
    public AudioClip footstepsClip;

    public AudioSource BGMSource;
    AudioSource _audioPlayerEffect;
    AudioSource _audioEnemyEffect;
    AudioSource HitSource;
    AudioSource AttackSource;
    AudioSource DashSource;
    AudioSource ShieldSource;
    AudioSource StatueSource;
    AudioSource DarkSource;
    AudioSource SpellSource;
    AudioSource ShockWaveSource;
    AudioSource CheckPointSource;
    AudioSource DeathSource;
    AudioSource sighSource;
    AudioSource footstepsSource;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(this.gameObject);
        }
        else if (this != Instance)
        {
            Destroy(this.gameObject);
        }
        BGMSource = gameObject.AddComponent<AudioSource>();
        HitSource = gameObject.AddComponent<AudioSource>();
        AttackSource = gameObject.AddComponent<AudioSource>();
        DashSource = gameObject.AddComponent<AudioSource>();
        ShieldSource = gameObject.AddComponent<AudioSource>();
        StatueSource = gameObject.AddComponent<AudioSource>();
        DarkSource = gameObject.AddComponent<AudioSource>();
        SpellSource = gameObject.AddComponent<AudioSource>();
        ShockWaveSource = gameObject.AddComponent<AudioSource>();

        CheckPointSource = gameObject.AddComponent<AudioSource>();
        DeathSource = gameObject.AddComponent<AudioSource>();
        sighSource = gameObject.AddComponent<AudioSource>();
        footstepsSource = gameObject.AddComponent<AudioSource>();

        float volume = 0.3f;
        BGMSource.volume = volume;
        HitSource.volume = volume;
        AttackSource.volume = volume;
        DashSource.volume = volume;
        ShieldSource.volume = volume;
        StatueSource.volume = volume;
        DarkSource.volume = volume;
        SpellSource.volume = volume;
        ShockWaveSource.volume = volume;

        CheckPointSource.volume = volume;
        DeathSource.volume = volume;
        sighSource.volume = volume;
        footstepsSource.volume = volume;
    }

    private void Start()
    {
        BGMSource.loop = true;
        //PlayBGM();
    }

    public void PlayBGM(bool isBattle=false,int battleList=0,bool isBoss=false)
    {
        if (isBattle)
        {
            BGMSource.Stop();
            if (isBoss)
            {
                BGMSource.clip = BossList[Random.Range(0, BossList.Count - 1)];
            }
            else
            {
                BGMSource.clip = BattleList[battleList];
            }
        }
        else
        {
            BGMSource.Stop();
            BGMSource.clip = MainClip;
        }
        BGMSource.Play();
    }
    public void StopBGM()
    {
        BGMSource.Stop();
    }
    public IEnumerator PlayCheckPoint()
    {
        if (CheckPointClip == null) yield break;
        CheckPointSource.clip = CheckPointClip;
        yield return null;
        CheckPointSource.Play();
    }
    public IEnumerator PlayDeath()
    {
        if (DeathClip == null) yield break;
        DeathSource.clip = DeathClip;
        yield return null;
        DeathSource.Play();
    }
    public IEnumerator PlaySigh()
    {
        if (sighClip == null) yield break;
        sighSource.clip = sighClip;
        yield return null;
        sighSource.Play();
    }
    public IEnumerator PlayFootsteps()
    {
        if (footstepsClip == null) yield break;
        footstepsSource.clip = footstepsClip;
        footstepsSource.volume = 0.5f;
        yield return null;
        footstepsSource.Play();
    }

    public IEnumerator PlayHit()
    {
        if (HitClip == null) yield break;
        HitSource.clip = HitClip;
        yield return null;
        HitSource.Play();
    }
    public IEnumerator PlayShield()
    {
        if (ShieldClip == null) yield break;
        ShieldSource.clip = ShieldClip;
        yield return null;
        ShieldSource.Play();
    }
    public IEnumerator PlayDark()
    {
        if (DarkClip == null) yield break;
        DarkSource.clip = DarkClip;
        yield return null;
        DarkSource.Play();
    }
    public IEnumerator PlaySpell()
    {
        if (SpellClip == null) yield break;
        SpellSource.clip = SpellClip;
        yield return null;
        SpellSource.Play();
    }
    public IEnumerator PlayStatue()
    {
        if (StatueClip == null) yield break;
        StatueSource.clip = StatueClip;
        StatueSource.pitch = 1.5f;
        yield return null;
        StatueSource.Play();
    }
    public IEnumerator PlayAttack(bool loop=false)
    {
        if (AttackClip == null) yield break;
        AttackSource.clip = AttackClip;
        AttackSource.loop = loop;
        if (loop) { AttackSource.pitch = 2; }
        else { AttackSource.pitch = 1; }
        yield return null;
        AttackSource.Play();
    }
    public void StopAttack()
    {
        AttackSource.Stop();
    }
    public IEnumerator PlayDash()
    {
        if (DashClip == null) yield break;
        DashSource.clip = DashClip;
        DashSource.loop = true;
        yield return null;
        DashSource.Play();
    }
    public IEnumerator PlayShockWave()
    {
        if (ShockWaveClip == null) yield break;
        ShockWaveSource.clip = ShockWaveClip;
        AttackSource.pitch = 3;
        yield return null;
        ShockWaveSource.Play();
    }
    public void StopDash()
    {
        DashSource.Stop();
    }
}
