using UnityEngine;

public class Eclipse : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private bool hasDied = false;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    public void PlayDeath()
    {
        if (hasDied) return;
        hasDied = true;

        if (animator != null)
            animator.SetTrigger("DeathTrig");
    }
}