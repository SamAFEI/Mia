using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    public TextMeshProUGUI TextBox { get; private set; }
    public Image TalkerIcon1 { get; private set; }
    public int TextIndex { get; private set; }
    public bool IsTexting { get; private set; }
    public bool IsSkipText { get; private set; }
    public TextAsset StartTextFile;
    List<string> textList = new List<string>();
    // Start is called before the first frame update
    void Awake()
    {
        TextBox = transform.Find("pnTextBox").GameObject().GetComponentInChildren<TextMeshProUGUI>();
        TextBox.text = "";
        TalkerIcon1 = GetComponent<Image>();
        GetTextFormFile(StartTextFile);
    }
    private void OnEnable()
    {
        Time.timeScale = 0f;
        StartCoroutine(SetTextUI());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (TextIndex == textList.Count)
            {
                gameObject.SetActive(false);
                Time.timeScale = 1.0f;
            }
            else if (!IsTexting && !IsSkipText)
            {
                StartCoroutine(SetTextUI());
            }
            else if (IsTexting && !IsSkipText)
            {
                IsSkipText = true;
            }
        }
    }
    void GetTextFormFile(TextAsset file)
    {
        textList.Clear();
        TextIndex = 0;
        string[] lineData = file.text.Split("\n");
        foreach (string line in lineData)
        {
            textList.Add(line);
        }
    }

    private IEnumerator SetTextUI()
    {
        IsTexting = true;
        TextBox.text = "";
        /*for (int i = 0; i < textList[TextIndex].Length; i++)
        {
            TextBox.text += textList[TextIndex][i];
            yield return new WaitForSecondsRealtime(0.1f);
        }*/
        int letter = 0;
        while (!IsSkipText && letter < textList[TextIndex].Length)
        {
            yield return new WaitForSecondsRealtime(0.1f);
            TextBox.text += textList[TextIndex][letter];
            letter++;
        }
        TextBox.text = textList[TextIndex];
        IsSkipText = false;
        IsTexting = false;
        TextIndex++;
    }
}
