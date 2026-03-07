using UnityEngine;
using UnityEngine.UI;

public class StarManager : MonoBehaviour
{
    public int StarCount;
    public Text StarText;

    private void Start()
    {
        UpdateUI();
    }

    public void AddStar()
    {
        StarCount++;
        UpdateUI();
        Debug.Log("Star added. Count = " + StarCount);
    }

    public void UpdateUI()
    {
        if (StarText != null)
        {
            StarText.text = StarCount.ToString();
        }
    }
}