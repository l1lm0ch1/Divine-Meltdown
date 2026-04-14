using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using TMPro;
using Unity.Cinemachine;

public class StartUpCutscene : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private CinemachineCamera externalCamera;
    [SerializeField] private TMP_Text dialogText;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private GameObject dialogPanel;
    [SerializeField] private Button[] optionButtons;
    [SerializeField] private PlayableDirector playableDirector;
    [SerializeField] private string speakerName = "Odin";
    [SerializeField] private string cutsceneId = "StartUpCutscene";
    [SerializeField] private int optionsAtLineIndex = 0;

    [TextArea(3, 10)]
    [SerializeField] private string[] dialogLines;

    private Queue<string> dialogQueue;
    private bool isDialogActive;
    private bool isOptionSelected;
    private int currentLineIndex;

    private void Start()
    {
        nameText.text = speakerName;
        dialogQueue = new Queue<string>();
        dialogPanel.SetActive(false);

        if (GameManager.Instance.IsCutsceneWatched(cutsceneId))
        {
            externalCamera.Priority.Value = 0;
            player.SetActive(true);
            return;
        }

        externalCamera.Priority.Value = 11;
        player.SetActive(false);
        StartCoroutine(PlayCutsceneAndStartDialog());
    }

    private void Update()
    {
        if (isDialogActive && !isOptionSelected && Input.GetKeyDown(KeyCode.E))
            DisplayNextDialog();
    }

    private IEnumerator PlayCutsceneAndStartDialog()
    {
        playableDirector.Play();

        while (playableDirector.state == PlayState.Playing)
            yield return null;

        dialogPanel.SetActive(true);
        StartDialog();
    }

    private void StartDialog()
    {
        isDialogActive = true;
        isOptionSelected = false;
        currentLineIndex = 0;

        foreach (string line in dialogLines)
            dialogQueue.Enqueue(line);

        DisplayNextDialog();
    }

    private void DisplayNextDialog()
    {
        if (dialogQueue.Count == 0)
        {
            EndDialog();
            return;
        }

        string line = dialogQueue.Dequeue();
        dialogText.text = line;

        if (currentLineIndex == optionsAtLineIndex)
            ShowOptions();
        else
            HideOptions();

        currentLineIndex++;
    }

    private void ShowOptions()
    {
        isOptionSelected = true;

        foreach (Button button in optionButtons)
        {
            button.gameObject.SetActive(true);
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnOptionSelected);
        }
    }

    private void HideOptions()
    {
        isOptionSelected = false;

        foreach (Button button in optionButtons)
        {
            button.onClick.RemoveAllListeners();
            button.gameObject.SetActive(false);
        }
    }

    private void OnOptionSelected()
    {
        HideOptions();
        DisplayNextDialog();
    }

    private void EndDialog()
    {
        isDialogActive = false;
        dialogPanel.SetActive(false);
        externalCamera.Priority.Value = 0;
        player.SetActive(true);
        GameManager.Instance.WatchCutscene(cutsceneId);
    }
}