using UnityEngine;

public class NewText : MonoBehaviour
{
    public GameObject oldText;
    public GameObject newText;
    public float activationXPosition;
    private Transform playerPosition;
    private bool hasActivated = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerPosition = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasActivated && !oldText.activeSelf && playerPosition.position.x > activationXPosition)
        {
            hasActivated = true;
            newText.SetActive(true);
        }
    }
}
