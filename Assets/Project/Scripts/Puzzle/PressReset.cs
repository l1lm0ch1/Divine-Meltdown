using UnityEngine;

public class PressReset : MonoBehaviour
{
    [SerializeField] private PressurePlate[] plates;
    [SerializeField] private int[] correctSequence;

    private int currentStep = 0;

    private void OnEnable()
    {
        PressurePlate.OnButtonPushed += CheckButtonSequence;
    }

    private void OnDisable()
    {
        PressurePlate.OnButtonPushed -= CheckButtonSequence;
    }

    public void CheckButtonSequence()
    {
        if (currentStep >= correctSequence.Length)
            return;

        int expectedPlateIndex = correctSequence[currentStep] - 1;

        for (int i = 0; i < plates.Length; i++)
        {
            if (plates[i] == null)
                continue;

            bool shouldBePressed = i <= expectedPlateIndex && plates[correctSequence[i] - 1].isPushedDown;

            if (i == expectedPlateIndex && !plates[i].isPushedDown)
            {
                ResetSequence();
                return;
            }
        }

        currentStep++;

        if (currentStep == correctSequence.Length)
        {
            currentStep = 0;
        }
    }

    private void ResetSequence()
    {
        currentStep = 0;
        ResetAllPlates();
    }

    private void ResetAllPlates()
    {
        foreach (PressurePlate plate in plates)
        {
            if (plate != null)
                plate.Reset();
        }
    }
}