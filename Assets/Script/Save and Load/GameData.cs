using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
public class GameData 
{

    public int RedSouls;
    public string closeCheckPointId;
    public SerializedDictionary<string, bool> checkpoints;
    public SerializedDictionary<string, bool> WaveBossLives;
    public bool IsStartDialog;

    public GameData()
    {
        this.RedSouls = 0;
        IsStartDialog = false;
        closeCheckPointId = string.Empty;
        checkpoints = new SerializedDictionary<string, bool>();
        WaveBossLives = new SerializedDictionary<string, bool>();
    }   
}
