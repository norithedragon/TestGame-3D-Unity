using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 3;

    [Header("Damage Protection")]
    [SerializeField] private float invulnerabilityDuration = 1f;

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

        StartCoroutine(InvulnerabilityRoutine());
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

    private IEnumerator InvulnerabilityRoutine()
    {
        isInvulnerable = true;

        yield return new WaitForSeconds(
            invulnerabilityDuration
        );

        isInvulnerable = false;
    }

    private void Die()
    {
        if (gameManager != null)
        {
            gameManager.LoseGame();
        }
    }
}