using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TambolaManager : MonoBehaviour
{
    [Header("Grid")]
    public Transform gridContent;           // The GridContent transform
    public GameObject numberButtonPrefab;   // Your NumberButton prefab

    [Header("Right Panel UI")]
    public TextMeshProUGUI currentNumberText;  // Text inside the circle
    public GameObject messagePanel;            // "All numbers generated" panel
    public TextMeshProUGUI messageText;

    [Header("History Panel")]
    public GameObject historyPanel;
    public GameObject ConfirmReset;
    public TextMeshProUGUI historyText;

    [Header("Colors")]
    public Color defaultColor = new Color(0.56f, 0.93f, 0.56f);  // light green
    public Color calledColor = new Color(0.85f, 0.15f, 0.15f);  // red
    public Color defaultTextColor = Color.black;
    public Color calledTextColor = Color.white;

    //  internals 
    private bool canCheat = false;
    private bool[] isGenerated = new bool[91];   // index 1-90
    private List<int> history = new List<int>();
    private NumberButton[] numberButtons = new NumberButton[91];

    void Start()
    {
        BuildGrid();
        ResetGame();
    }

    // ?? Build the 90-button grid ???????????????????????????????
    void BuildGrid()
    {
        for (int i = 1; i <= 90; i++)
        {
            GameObject btn = Instantiate(numberButtonPrefab, gridContent);
            btn.name = "Btn_" + i;

            NumberButton nb = btn.GetComponent<NumberButton>();
            if (nb == null) nb = btn.AddComponent<NumberButton>();

            nb.Init(i, this);
            numberButtons[i] = nb;
        }
    }

    //  Called by Generate button 
    public void OnGenerateClicked()
    {
        List<int> available = new List<int>();
        for (int i = 1; i <= 90; i++)
            if (!isGenerated[i]) available.Add(i);

        if (available.Count == 0)
        {
            ShowMessage("Cannot generate number,\nAll numbers are generated!");
            return;
        }

        int number = available[Random.Range(0, available.Count)];
        CallNumber(number);
    }

    // ?? Called by clicking a specific number cell ??????????????
    public void OnNumberCellClicked(int number)
    {
        if (canCheat)
        {
            if (isGenerated[number]) return;   // already called, ignore
            CallNumber(number);
        }
    }

    // ?? Core: mark a number as called ?????????????????????????
    void CallNumber(int number)
    {
        isGenerated[number] = true;
        history.Add(number);

        // Update circle display
        currentNumberText.text = number.ToString();

        // Update grid cell color
        numberButtons[number].SetCalled(calledColor, calledTextColor);

        // Hide any lingering message
        if (messagePanel.activeSelf) messagePanel.SetActive(false);
    }

    //  Reset button 
    public void OnConfirmResetClicked()
    {
        ResetGame();
    }

    void ResetGame()
    {
        for (int i = 1; i <= 90; i++)
        {
            isGenerated[i] = false;
            if (numberButtons[i] != null)
                numberButtons[i].SetDefault(defaultColor, defaultTextColor);
        }

        history.Clear();
        currentNumberText.text = "-";
        messagePanel.SetActive(false);
        historyPanel.SetActive(false);
        ConfirmReset.SetActive(false);
    }

    // History button
    public void OnHistoryClicked()
    {
        if (history.Count == 0)
        {
            historyText.text = "No numbers generated yet.";
        }
        else
        {
            historyText.text = string.Join("  ->  ", history);
        }
        historyPanel.SetActive(true);
    }

    public void OnResetClick()
    {
        historyPanel.SetActive(false);
        ConfirmReset.SetActive(true);
    }
    public void OnCloseResetClick()
    {
        ConfirmReset.SetActive(false);
    }

    public void OnCheatClicked()
    {
        canCheat = true;
    }

    public void OnHistoryCloseClicked()
    {
        historyPanel.SetActive(false);
    }

    // ?? Message helper ?????????????????????????????????????????
    void ShowMessage(string msg)
    {
        messageText.text = msg;
        messagePanel.SetActive(true);
    }
}