using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Energy Cells")]
    [SerializeField] private int requiredEnergyCells = 3;

    [Header("UI")]
    [SerializeField] private TMP_Text energyCellText;
    [SerializeField] private TMP_Text objectiveText;

    [Header("Exit")]
    [SerializeField] private ExitDoor exitDoor;

    private int collectedEnergyCells;

    private void Start()
    {
        collectedEnergyCells = 0;

        UpdateEnergyCellUI();
        UpdateObjectiveText("Collect all Energy Cells!");
    }

    public void CollectEnergyCell()
    {
        collectedEnergyCells++;
        UpdateEnergyCellUI();

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
            $"Energy Cells: {collectedEnergyCells} / {requiredEnergyCells}";
    }

    private void UpdateObjectiveText(string newObjective)
    {
        if (objectiveText == null)
        {
            return;
        }

        objectiveText.text = newObjective;
    }

    private void AllEnergyCellsCollected()
    {
        UpdateObjectiveText("Reach the escape zone!");

        if (exitDoor != null)
        {
            exitDoor.OpenDoor();
        }
    }
}