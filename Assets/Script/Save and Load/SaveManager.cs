using Assets.Script.Save_and_Load;
using System.Collections.Generic;
using System.IO.Enumeration;
using System.Linq;
using UnityEngine;


[DefaultExecutionOrder(500)]
public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    [SerializeField] private string fileName;
    private GameData gameData;
    private List<ISaveManager> saveManagers;
    private FileDataHandler dataHandler;

    [ContextMenu("Delete Save File")]
    public void DeleteSaveData()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath,fileName);
        dataHandler.Delete();
    }

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
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        saveManagers = FindAllSaveManagers();
        LoadGame();
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    private void LoadGame()
    {
        gameData = dataHandler.Load();
        if (this.gameData == null)
        {
            NewGame();
        }

        foreach (ISaveManager manager in saveManagers)
        {
            manager.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        foreach(ISaveManager manager in saveManagers)
        {
            manager.SaveData(ref gameData);
        }
        dataHandler.Save(gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<ISaveManager> FindAllSaveManagers()
    {
        IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();
        return new List<ISaveManager>(saveManagers);
    }
}
