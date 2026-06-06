using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Energy Cells")]
    [SerializeField] private int requiredEnergyCells = 3;

    [Header("UI")]
    [SerializeField] private TMP_Text energyCellText;

    private int collectedEnergyCells;

    private void Start()
    {
        UpdateEnergyCellUI();
    }

    public void CollectEnergyCell()
    {
        collectedEnergyCells++;
        UpdateEnergyCellUI();

        Debug.Log(
            "Energetické články: " +
            collectedEnergyCells +
            " / " +
            requiredEnergyCells
        );

        if (collectedEnergyCells >= requiredEnergyCells)
        {
            AllEnergyCellsCollected();
        }
    }

    private void UpdateEnergyCellUI()
    {
        if (energyCellText == null)
        {
            return;
        }

        energyCellText.text =
            "Energy Cells: " +
            collectedEnergyCells +
            " / " +
            requiredEnergyCells;
    }

    private void AllEnergyCellsCollected()
    {
        Debug.Log("Všechny energetické články byly sebrány!");
    }
}