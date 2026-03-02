using UnityEngine;

public class Portal : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject playerCamera;
    public GameObject successCamera;
    public GameObject failureCamera;
    public GameObject HeartsStarCount;

    private Portal_Controller portalController;
    private StarManager starManager;
    void Start()
    {
        portalController = GameObject.Find("Portal").GetComponent<Portal_Controller>();
        portalController.TogglePortal(true);
        starManager = GameObject.Find("GameManager").GetComponent<StarManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(starManager.StarCount >= 15)
            {
                playerCamera.SetActive(false);
                HeartsStarCount.SetActive(false);
                successCamera.SetActive(true);
            } else
            {
                playerCamera.SetActive(false);
                HeartsStarCount.SetActive(false);
                failureCamera.SetActive(true);
            }
        }
    }
}
