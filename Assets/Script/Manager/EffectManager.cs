using UnityEngine;

namespace Assets.Script.Manager
{
    public class EffectManager : MonoBehaviour
    {
        public static EffectManager Instance { get; private set; }
        [SerializeField] private GameObject HitFX;
        [SerializeField] private GameObject LandFX;
        [SerializeField] private GameObject SlideFX;
        [SerializeField] private GameObject SpawnFX;
        [SerializeField] private GameObject ThreeCombosFX;
        [SerializeField] private GameObject CircleHitFX;
        [SerializeField] private GameObject ShockWaveFX;

        public GameObject CircleHitFXobj;
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
        }
        public void DoHitFX(GameObject point)
        {
            DoHitFX(point.transform.position);
        }
        public void DoHitFX(Vector3 vector)
        {
            GameObject FXObj = Instantiate(HitFX, vector, Quaternion.identity);
            Destroy(FXObj, 0.3f);
        }
        public void DoLandFX(GameObject point)
        {
            GameObject obj = Instantiate(LandFX, point.transform.position, Quaternion.identity);
            Destroy(obj, 0.3f);
        }
        public void DoSlideFX(GameObject point)
        {
            GameObject obj = Instantiate(SlideFX, point.transform.position, Quaternion.identity);
            Destroy(obj, 0.3f);
        }
        public void DoSpawnFX(Vector3 vector)
        {
            GameObject obj = Instantiate(SpawnFX, vector, Quaternion.identity);
            Destroy(obj, 1f);
        }
        public void DoThreeCombosFX(GameObject point, bool isFacingRight, int angle = 0)
        {
            if (!isFacingRight)
            {
                angle *= -1;
            }
            GameObject obj = Instantiate(ThreeCombosFX, point.transform.position, Quaternion.Euler(0, 0, angle));
            if (!isFacingRight)
            {
                obj.transform.localScale = new Vector3(-1, -1, 1);
            }
            else
            {
                obj.transform.localScale = new Vector3(1, 1, 1);
            }
            Destroy(obj, 0.3f);
        }

        public void DoCircleHitFX(GameObject point, Transform _paranet)
        {
            if (CircleHitFX == null) return;
            CircleHitFXobj = Instantiate(CircleHitFX
                                    , point.transform.position
                                    , Quaternion.identity, _paranet);
            Destroy(CircleHitFXobj, 5f);
        }
        public void DoShockWaveFX(GameObject point, Transform _paranet)
        {
            if (ShockWaveFX == null) return;
            GameObject obj = Instantiate(ShockWaveFX
                                    , point.transform.position
                                    , Quaternion.identity, _paranet);
            Destroy(obj, 0.5f);
        }

        public void DestroyCircleHitFXobj()
        {
            if (CircleHitFXobj != null)
            {
                Destroy(CircleHitFXobj);
            }
        }
    }
}
