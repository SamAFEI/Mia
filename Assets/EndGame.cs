using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    public Flowchart StartDialog;
    public WaveBase wave11;
    public bool IsActived;
    // Start is called before the first frame update
    void Start()
    {
        wave11 = GetComponent<WaveBase>();
    }

    // Update is called once per frame
    void Update()
    {
        if (wave11.IsEnded && !IsActived) 
        {
            IsActived = true;
            StartDialog.ExecuteBlock("End");
        }
    }
}
