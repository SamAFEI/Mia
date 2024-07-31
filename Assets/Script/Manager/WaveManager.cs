using System.Collections;
using UnityEngine;

namespace Assets.Script.Manager
{
    public class WaveManager : MonoBehaviour
    {
        public GameObject Goblin;
        public GameObject FlyingEye;
        public GameObject Seleton;
        public GameObject Mushroom;
        public GameObject Deather;
        public GameObject BirthPoin01;
        public GameObject BirthPoin02;

        private float intervalTime;
        private int GoblinNum = 2;
        private int FlyingEyeNum = 2;
        private int SeletonNum = 1;
        private int MushroomNum = 1;

        private void LateUpdate()
        {
            intervalTime -= Time.deltaTime;
            if (intervalTime <= 0 )
            {
                StartCoroutine(SpawnGoblin());
                intervalTime = 80f;
            }
        }

        public IEnumerator SpawnGoblin()
        {
            for (int i = 0; i < GoblinNum; i++)
            {
                Instantiate(Goblin, BirthPoin01.transform.position, Quaternion.identity);
                Instantiate(Goblin, BirthPoin02.transform.position, Quaternion.identity);
                yield return new WaitForSeconds(1f);
            }
            StartCoroutine(SpawnFlyingEye());
        }
        public IEnumerator SpawnFlyingEye()
        {
            for (int i = 0; i < FlyingEyeNum; i++)
            {
                Instantiate(FlyingEye, BirthPoin01.transform.position, Quaternion.identity);
                Instantiate(FlyingEye, BirthPoin02.transform.position, Quaternion.identity);
                yield return new WaitForSeconds(1f);
            }
            StartCoroutine(SpawnMushroom());
        }
        public IEnumerator SpawnMushroom()
        {
            for (int i = 0; i < MushroomNum; i++)
            {
                Instantiate(Mushroom, BirthPoin01.transform.position, Quaternion.identity);
                Instantiate(Mushroom, BirthPoin02.transform.position, Quaternion.identity);
                yield return new WaitForSeconds(1f);
            }
            StartCoroutine(SpawnSeleton());
        }
        public IEnumerator SpawnSeleton()
        {
            for (int i = 0; i < SeletonNum; i++)
            {
                Instantiate(Seleton, BirthPoin01.transform.position, Quaternion.identity);
                Instantiate(Seleton, BirthPoin02.transform.position, Quaternion.identity);
                yield return new WaitForSeconds(1f);
            }
        }
    }
}
