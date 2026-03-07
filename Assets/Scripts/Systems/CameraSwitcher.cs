using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera monsterCamera;
    public Camera playerCamera;
    public Transform player;

    public float monsterCamSpeed = 2f;

    private bool usingPlayerCamera = false;

    private void Start()
    {
        playerCamera.enabled = false;
    }

    void Update()
    {
        Vector3 viewPos = monsterCamera.WorldToViewportPoint(player.position);
        float x = viewPos.x;

        float targetY = player.position.y;

        float smoothY = Mathf.Lerp(
            monsterCamera.transform.position.y,
            targetY,
            5f * Time.deltaTime
        );

        monsterCamera.transform.position = new Vector3(
            monsterCamera.transform.position.x,
            smoothY,
            monsterCamera.transform.position.z
        );

        if (!usingPlayerCamera)
        {
            // monsterkameran stannar
            if (x < 0.45f)
            {
                return;
            }

            // monsterkameran rör sig
            if (x >= 0.45f && x <= 0.70f)
            {
                monsterCamera.transform.Translate(Vector3.right * monsterCamSpeed * Time.deltaTime);
            }

            // byt kamera
            if (x > 0.70f)
            {
                usingPlayerCamera = true;
                monsterCamera.enabled = false;
                playerCamera.enabled = true;
            }
        }
        else
        {
            // byt tillbaka
            if (x <= 0.70f)
            {
                usingPlayerCamera = false;
                monsterCamera.enabled = true;
                playerCamera.enabled = false;
            }
        }
    }
}
