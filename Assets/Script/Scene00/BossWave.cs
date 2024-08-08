using Assets.Script.Enemy;
using Assets.Script.Manager;
using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWave : MonoBehaviour
{
    public UI_BOSSStatus bossStatus;
    public UICanvas uiCanvas;
    public List<GameObject> Enemys;
    public List<GameObject> SpawnPoints;
    public GameObject EndPoint;
    public GameObject BossObj;
    public EnemyController Boss;
    public BossWave NextWave;
    public Flowchart flowchart;
    public bool IsBossActived;
    public bool IsEnded;
    public bool IsBossLive;
    public int battlemuisc = 0;
    public bool IsEndActive;

    private void Start()
    {
        IsBossLive = true;
    }
    private void Update()
    {
        if (Boss != null && Boss.CurrentHP <= 0)
        {
            IsBossLive = false;
        }
        if (!IsBossLive)
        {
            CheckEnd();
            if (IsEnded)
            {
                bossStatus.SetUIStateActive(false);
                AudioManager.Instance.StopBGM();
                if (NextWave != null)
                {
                    NextWave.DoEvent();
                    gameObject.SetActive(false);
                }
                else if (!IsEndActive)
                {
                    IsEndActive = true;
                    flowchart.ExecuteBlock("End");
                }
            }
        }
    }
    public virtual void DoEvent()
    {
        StartCoroutine(SpawnOneWave());
        AudioManager.Instance.PlayBGM(true, battlemuisc);
    }

    protected virtual IEnumerator SpawnOneWave()
    {
        while (IsBossLive)
        {
            StartCoroutine(AudioManager.Instance.PlayStatue());
            foreach (GameObject point in LevelManager.Instance.SpawnPoints)
            {
                if (!IsBossLive) { break; }
                GameObject enemy = Enemys[Random.Range(0, Enemys.Count)];
                float offset = Random.Range(-1.0f, 1.0f);
                Vector2 vector = new Vector2(point.transform.position.x + offset, point.transform.position.y + offset);
                EffectManager.Instance.DoSpawnFX(vector);
                Instantiate(enemy, vector, Quaternion.identity);
                yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
            }
            if (!IsBossActived)
            {
                yield return new WaitForSeconds(10f);
                if (BossObj != null)
                {
                    //GameObject point = SpawnPoints[Random.Range(0, SpawnPoints.Count - 1)];
                    //float offset = Random.Range(-1f, 1f);
                    //Vector2 vector = new Vector2(point.transform.position.x + offset, point.transform.position.y + offset);
                    Vector2 vector = new Vector2(GameManager.Instance.Player.transform.position.x,
                                                    GameManager.Instance.Player.transform.position.y + 5);
                    EffectManager.Instance.DoSpawnFX(vector);
                    StartCoroutine(AudioManager.Instance.PlayStatue());
                    Boss = Instantiate(BossObj, vector, Quaternion.identity).GetComponent<EnemyController>();
                    Boss.name = BossObj.name;
                    bossStatus.boss = Boss;
                    bossStatus.SetUIStateActive(true);
                    TimerManager.Instance.SlowFrozenTime(1f);
                }
                IsBossActived = true;
            }
            yield return new WaitForSeconds(30f);
        }
    }
    protected virtual void CheckEnd()
    {
        Collider2D HitCollider = EndPoint.GetComponent<Collider2D>();
        List<Collider2D> colliders = new List<Collider2D>();
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = true;
        Physics2D.OverlapCollider(HitCollider, filter, colliders);
        bool noEnemy = false;
        foreach (var collider in colliders)
        {
            if (collider.tag == "Enemy")
            {
                if (!IsBossLive)
                {
                    collider.GetComponent<EnemyController>().Hurt(10000);
                }
                noEnemy = false;
                break;
            }
            noEnemy = true;
        }
        IsEnded = noEnemy;
    }
}
