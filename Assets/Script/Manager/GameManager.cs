using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject Player { get; private set; }
    public MiaController Mia { get; private set; }
    public bool IsDie { get; private set; }
    public bool IsPaused;
    public static string LoadSceneName;

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

    private void Start()
    {
        Player = GameObject.FindWithTag("Player");
        if (Player != null)
        { Mia = Player.GetComponent<MiaController>(); }
    }

    private void Update()
    {
        Instance.IsDie = Player != null ? Mia.CurrentHP <= 0 : false;
    }

    /// <summary>
    /// Get 朝Player方向向量
    /// </summary>
    /// <param name="_position"></param>
    /// <returns> Vector2 方向 </returns>
    public Vector2 GetPlayerDirection(Vector2 _position)
    {
        if (Player == null) { return Vector2.zero; }
        Vector2 _source = new Vector2(_position.x, 0);
        Vector2 _target = new Vector2(Player.transform.position.x, 0);
        return _target - _source;
    }

    /// <summary>
    /// Get 與Player的距離
    /// </summary>
    /// <param name="_position"></param>
    /// <returns></returns>
    public float GetPlayerDistance(Vector2 _position)
    {
        if (Player == null) { return 0; }
        return GetPlayerDirection(_position).magnitude;

    }

    public void PausedGame(bool paused)
    {
        IsPaused = paused;
        /*if (IsPaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }*/
    }

    public void RestarScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void LoadScene(string _sceneName)
    {
        LoadSceneName = _sceneName;
        SceneManager.LoadScene("LoadingScene");
    }
}
