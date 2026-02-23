using UnityEngine;
using TMPro;
using System.Collections;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;

    private int index;

    private Player playerMovements;
    void Start()
    {
        playerMovements = GameObject.Find("bruh").GetComponent<Player>();

        if (textComponent == null)
        {
            Debug.LogError("Dialogue: textComponent is not assigned.");
            enabled = false;
            return;
        }

        if (lines == null || lines.Length == 0)
        {
            Debug.LogWarning("Dialogue: no lines assigned.");
            enabled = false;
            return;
        }

        textComponent.text = string.Empty;
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        playerMovements.enabled = false;

        if (Input.GetMouseButtonDown(0))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            playerMovements.enabled = true;
            gameObject.SetActive(false);
        }
    }
}
