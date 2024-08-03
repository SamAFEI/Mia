using Live2D.Cubism.Core;
using UnityEngine;

public class UI_Live2D : MonoBehaviour
{
    public static UI_Live2D Instance { get; private set; }

    public GameObject Characters;
    public GameObject Mia;
    public GameObject Elise;
    public GameObject Stella;

    public Animator MiaAnim { get; private set; }
    public Animator EliseAnim { get; private set; }
    public Animator StellaAnim { get; private set; }
    public CubismModel MiaModel { get; private set; }
    public CubismModel EliseModel { get; private set; }
    public CubismModel StellaModel { get; private set; }

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

        MiaAnim = Mia.GetComponent<Animator>();
        EliseAnim = Elise.GetComponent<Animator>();
        StellaAnim = Stella.GetComponent<Animator>();

        MiaModel = Mia.FindCubismModel();
        EliseModel = Elise.FindCubismModel();
        StellaModel = Stella.FindCubismModel();
    }

    private void Start()
    {
        //Characters.SetActive(false);
    }

    private void LateUpdate()
    {
        EliseModel.Parameters.FindById("Break1").Value = -30;
        EliseModel.Parameters.FindById("Hair1").Value = 30;
        StellaModel.Parameters.FindById("Display").Value = 0;
    }

    public void SetCharactersActive(bool _value)
    {
        Characters.SetActive(_value);
    }

    public void SetAct1()
    {
        Mia.SetActive(true);
        Elise.SetActive(true);
        Stella.SetActive(true);
    }
    public void PlayMiaAnim(string anim)
    {
        MiaAnim.Play(anim);
    }
    public void PlayEliseAnim(string anim)
    {
        EliseAnim.Play(anim);
    }
    public void PlayStellaAnim(string anim)
    {
        StellaAnim.Play(anim);
    }
}
