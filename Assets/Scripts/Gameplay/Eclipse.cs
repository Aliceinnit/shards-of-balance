using System.Collections;
using UnityEngine;

public class Eclipse : MonoBehaviour
{
    public GameObject defeatCanvas;
    public GameObject defeatButton;

    [SerializeField] private Animator animator;
    private bool hasDied = false;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    public void PlayDeath()
    {
        if (hasDied)
        {
            Debug.Log("Eclipse has already died, skipping death animation.");
            return;
        }
        hasDied = true;

        Debug.Log("eclipse death animation triggered");

        if (animator != null)
        StartCoroutine(eclipseDeath());
    }


    IEnumerator eclipseDeath()
    {
        animator.SetTrigger("DeathTrigger");
        yield return new WaitForSeconds(1.5f);
        if (defeatCanvas != null)
            defeatCanvas.SetActive(true);
        defeatButton.SetActive(false);
        Time.timeScale = 0f;
    }
}