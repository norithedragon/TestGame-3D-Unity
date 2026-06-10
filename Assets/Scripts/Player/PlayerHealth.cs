using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 3;

    [Header("Damage Protection")]
    [SerializeField] private float invulnerabilityDuration = 1f;

    [Header("Damage Flash")]
    [SerializeField] private Renderer[] playerRenderers;
    [SerializeField] private int flashCount = 4;
    [SerializeField] private float flashInterval = 0.1f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip damageSound;
    [SerializeField, Range(0f, 2f)] private float damageVolume = 1f;

    private int currentHealth;
    private bool isInvulnerable;
    private GameManager gameManager;

    private void Start()
    {
        currentHealth = maxHealth;
        gameManager = FindFirstObjectByType<GameManager>();

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (gameManager != null)
        {
            gameManager.UpdateHealthUI(
                currentHealth,
                maxHealth
            );
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvulnerable || currentHealth <= 0)
        {
            return;
        }

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        PlayDamageSound();

        if (gameManager != null)
        {
            gameManager.UpdateHealthUI(
                currentHealth,
                maxHealth
            );
        }

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        StartCoroutine(DamageFeedbackRoutine());
    }

    private IEnumerator DamageFeedbackRoutine()
    {
        isInvulnerable = true;

        for (int i = 0; i < flashCount; i++)
        {
            SetRenderersVisible(false);

            yield return new WaitForSeconds(
                flashInterval
            );

            SetRenderersVisible(true);

            yield return new WaitForSeconds(
                flashInterval
            );
        }

        float flashDuration =
            flashCount * flashInterval * 2f;

        float remainingInvulnerability =
            invulnerabilityDuration - flashDuration;

        if (remainingInvulnerability > 0f)
        {
            yield return new WaitForSeconds(
                remainingInvulnerability
            );
        }

        SetRenderersVisible(true);
        isInvulnerable = false;
    }

    private void SetRenderersVisible(bool visible)
    {
        if (playerRenderers == null)
        {
            return;
        }

        foreach (Renderer playerRenderer in playerRenderers)
        {
            if (playerRenderer != null)
            {
                playerRenderer.enabled = visible;
            }
        }
    }

    private void PlayDamageSound()
    {
        if (audioSource == null || damageSound == null)
        {
            return;
        }

        audioSource.PlayOneShot(
            damageSound,
            damageVolume
        );
    }

    private void Die()
    {
        SetRenderersVisible(true);

        if (gameManager != null)
        {
            gameManager.LoseGame();
        }
    }
}