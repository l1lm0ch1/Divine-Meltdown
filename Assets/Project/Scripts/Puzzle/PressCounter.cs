using UnityEngine;
using UnityEngine.Playables;

public class PressCounter : MonoBehaviour
{
    [SerializeField] private PlayableDirector director;
    [SerializeField] private PressurePlate[] plates;

    private bool gateOpen = false;

    private void Start()
    {
        director.Pause();
        director.enabled = false;
    }

    private void Update()
    {
        if (gateOpen)
            return;

        foreach (PressurePlate plate in plates)
        {
            if (plate == null || !plate.isPushedDown)
                return;
        }

        OpenGate();
    }

    private void OpenGate()
    {
        gateOpen = true;
        director.enabled = true;
        director.Play();
    }
}