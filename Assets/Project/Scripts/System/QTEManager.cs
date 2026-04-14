using TMPro;
using UnityEngine;

public class QTEManager : MonoBehaviour
{
    [SerializeField] private GameObject qtePanel;
    [SerializeField] private TMP_Text qteText;
    [SerializeField] private KnotInteraction knotInteraction;
    [SerializeField] private string qtePromptText = "Press 'F' to Blind";

    private void Start()
    {
        qtePanel.SetActive(false);
    }

    public void ShowQTE()
    {
        if (knotInteraction.GetKnotInfo())
            return;

        qteText.text = qtePromptText;
        qtePanel.SetActive(true);
    }

    public void HideQTE()
    {
        if (knotInteraction.GetKnotInfo())
            return;

        qtePanel.SetActive(false);
    }
}