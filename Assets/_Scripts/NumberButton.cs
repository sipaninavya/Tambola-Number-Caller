using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NumberButton : MonoBehaviour
{
    private int number;
    private TambolaManager manager;
    private Image bg;
    private TextMeshProUGUI label;
    private Button btn;

    public void Init(int num, TambolaManager mgr)
    {
        number = num;
        manager = mgr;

        bg = GetComponent<Image>();
        label = GetComponentInChildren<TextMeshProUGUI>();
        btn = GetComponent<Button>();

        label.text = num.ToString();

        // Wire up click
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => manager.OnNumberCellClicked(number));
    }

    public void SetCalled(Color bgColor, Color textColor)
    {
        bg.color = bgColor;
        label.color = textColor;
    }

    public void SetDefault(Color bgColor, Color textColor)
    {
        bg.color = bgColor;
        label.color = textColor;
    }
}