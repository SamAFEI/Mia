using Fungus;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class SceneSystem : MonoBehaviour
{
    public GameObject FlyingEye;
    public GameObject Goblin;
    public GameObject Skeleton;
    public GameObject Mushroom;
    public GameObject talkUI;
    private List<Vector3> pointList = new List<Vector3>();
    private void Awake()
    {
    }
    private void Start()
    {
        AudioManager.Instance.PlayBGM();
        //GameManager.Instance.ReSetKillNum();
        pointList.Add(new Vector2(75, 75));
        pointList.Add(new Vector2(-170, 85));

        pointList.Add(new Vector2(109, 10));
        pointList.Add(new Vector2(-126, 50));

        pointList.Add(new Vector2(117, 45));
        pointList.Add(new Vector2(-180, 45));
        pointList.Add(new Vector2(-46, 55));

        //InvokeRepeating("SpwawnEnemyStart", 0, 6f);
        //InvokeRepeating("SpawnFlyingEyeStart", 0, 4f);
        //InvokeRepeating("SpawnSkeletonStart", 0, 10f);
        //InvokeRepeating("SpwawnEnemyStart", 10, 30f);
        //InvokeRepeating("SpawnFlyingEyeStart", 10, 20f);
        //InvokeRepeating("SpawnSkeletonStart", 10, 50f);
    }
    private void SpawnFlyingEyeStart()
    {
        StartCoroutine(SpawnFlyingEye());
    }
    IEnumerator SpawnFlyingEye()
    {
        if (FlyingEye == null) yield break;
        for (int i=0; i <= 2; i++)
        {
            GameObject enemy = FlyingEye;
            float playerX = GameManager.Instance.Player.transform.position.x;
            Vector2 point = pointList[0];
            if (point.x + 15f < playerX || point.x - 15f > playerX)
            {
                SpawnOne(enemy, point);
            }
            point = pointList[1];
            if (point.x + 15f < playerX || point.x - 15f > playerX)
            {
                SpawnOne(enemy, point);
            }
            yield return new WaitForSeconds(1f);
        }
    }
    private void SpawnSkeletonStart()
    {
        StartCoroutine(SpawnSkeleton());
    }
    IEnumerator SpawnSkeleton()
    {
        if (Skeleton == null) yield break;
        for (int i = 0; i <= 2; i++)
        {
            GameObject enemy = Skeleton;
            float playerX = GameManager.Instance.Player.transform.position.x;
            Vector2 point = pointList[2];
            if (point.x + 15f < playerX || point.x - 15f > playerX)
            {
                SpawnOne(enemy, point);
            }
            point = pointList[3];
            if (point.x + 15f < playerX || point.x - 15f > playerX)
            {
                SpawnOne(enemy, point);
            }
            yield return new WaitForSeconds(1f);
        }
    }
    private void SpwawnEnemyStart()
    {
        StartCoroutine(SpwawnEnemy());
    }
    IEnumerator SpwawnEnemy()
    {
        if (Goblin == null || Mushroom == null) yield break;
        for (int i = 0; i <= 5; i++)
        {
            GameObject enemy = Skeleton;
            float x;
            Vector2 point;
            float playerX = GameManager.Instance.Player.transform.position.x;
            point = pointList[4];
            x = Random.Range(0f, 2f);
            if (x < 1.6f)
                enemy = Goblin;
            else
                enemy = Mushroom;
            if (point.x + 15f < playerX || point.x - 15f > playerX)
            {
                SpawnOne(enemy, point);
            }
            point = pointList[5];
            x = Random.Range(0f, 2f);
            if (x < 1.6f)
                enemy = Goblin;
            else
                enemy = Mushroom;
            if (point.x + 15f < playerX || point.x - 15f > playerX)
            {
                SpawnOne(enemy, point);
            }
            point = pointList[6];
            x = Random.Range(0f, 2f);
            if (x < 1.6f)
                enemy = Goblin;
            else
                enemy = Mushroom;
            if (point.x + 15f < playerX || point.x - 15f > playerX)
            {
                SpawnOne(enemy, point);
            }
            yield return new WaitForSeconds(1f);
        }
    }
    private void SpawnOne(GameObject Enemy,Vector3 birthPoint)
    {
        if (Enemy == null) { return; }
        GameObject enemy = Instantiate(Enemy, birthPoint, Quaternion.identity);
        float bodySize = Random.Range(0.8f, 1.3f);
        //enemy.GetComponent<EnemyFSM>().SetBodySize(bodySize);
    }

}
