using UnityEngine;
using UnityEngine.UI;

public class StarManager : MonoBehaviour
{
    public int StarCount;
    public Text StarText;

    void Update()
    {
        StarText.text = StarCount.ToString();
    }
}