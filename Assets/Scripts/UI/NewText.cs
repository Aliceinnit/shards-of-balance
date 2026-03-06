using UnityEngine;

public class NewText : MonoBehaviour
{
    public GameObject oldText;
    public GameObject newText;
    public float activationXPosition;
    private Transform playerPosition;
    private bool hasActivated = false;
    private Animator playerAnimator;
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
            playerAnimator = GameObject.Find("Player").GetComponentInChildren<Animator>();
            newText.SetActive(true);
        }
    }
}
