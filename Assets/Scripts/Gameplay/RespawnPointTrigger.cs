using UnityEngine;

public class RespawnPointTrigger : MonoBehaviour
{
    public Transform respawnPoint;
    public bool disableAfterUse = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        var ph = other.GetComponentInParent<PlayerHealth>();
        if (ph == null){
            return;} 


        ph.SetRespawnPoint(respawnPoint);
         Debug.Log("Respawn set to: " + respawnPoint.name);

        if (disableAfterUse)
            gameObject.SetActive(false);
    }
}
