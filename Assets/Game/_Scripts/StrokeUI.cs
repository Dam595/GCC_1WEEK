using TMPro;
using UnityEngine;
public class StrokeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI strokeText;
    private int strokeCount = 0;

    public void AddStroke()
    {
        strokeCount++;
        UpdateText();
    }

    public void ResetStroke()
    {
        strokeCount = 0;
        UpdateText();
    }

    private void UpdateText()
    {
        if (strokeText != null)
            strokeText.text = "" + strokeCount;
    }
}
