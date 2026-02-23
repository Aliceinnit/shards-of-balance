using UnityEngine;
using TMPro;
using System.Collections;

public class ObstacleTrigger : MonoBehaviour
{
    public GameObject dialogueBox;      // Själva rutan (panel)
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed = 0.05f;

    private int index = 0;
    private bool isTyping = false;

    void Start()
    {
        textComponent.text = "";
        dialogueBox.SetActive(false); // Börjar gömd
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            dialogueBox.SetActive(true);

            if (index < lines.Length)
            {
                StartCoroutine(TypeLine(lines[index]));
                index++;
            }
        }
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;

        textComponent.text = string.Empty;

        foreach (char c in line)
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        isTyping = false;
    }
}
