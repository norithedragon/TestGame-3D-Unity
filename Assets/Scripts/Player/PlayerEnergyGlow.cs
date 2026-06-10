using System.Collections;
using UnityEngine;

public class PlayerEnergyGlow : MonoBehaviour
{
    [Header("Glow Settings")]
    [SerializeField] private Renderer[] playerRenderers;
    [SerializeField] private Color glowColor = Color.cyan;
    [SerializeField] private float glowIntensity = 4f;
    [SerializeField] private float glowDuration = 0.8f;

    private Coroutine glowCoroutine;

    public void PlayEnergyGlow()
    {
        if (glowCoroutine != null)
        {
            StopCoroutine(glowCoroutine);
        }

        glowCoroutine = StartCoroutine(EnergyGlowRoutine());
    }

    private IEnumerator EnergyGlowRoutine()
    {
        SetGlow(true);

        yield return new WaitForSeconds(glowDuration);

        SetGlow(false);
        glowCoroutine = null;
    }

    private void SetGlow(bool enabled)
    {
        if (playerRenderers == null)
        {
            return;
        }

        foreach (Renderer currentRenderer in playerRenderers)
        {
            if (currentRenderer == null)
            {
                continue;
            }

            foreach (Material material in currentRenderer.materials)
            {
                material.EnableKeyword("_EMISSION");

                Color emissionColor = enabled
                    ? glowColor * glowIntensity
                    : Color.black;

                material.SetColor(
                    "_EmissionColor",
                    emissionColor
                );
            }
        }
    }
}