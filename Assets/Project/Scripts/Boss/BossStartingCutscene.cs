using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using Unity.Cinemachine;

public class BossStartingCutscene : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private CinemachineCamera externalCamera;
    [SerializeField] private PlayableDirector playableDirector;
    [SerializeField] private string cutsceneId = "BossStartingCutscene";

    private void Start()
    {
        if (GameManager.Instance.IsCutsceneWatched(cutsceneId))
        {
            externalCamera.Priority.Value = 0;
            player.SetActive(true);
            return;
        }

        externalCamera.Priority.Value = 11;
        player.SetActive(false);
        playableDirector.Play();
        StartCoroutine(WaitForCutsceneEnd());
    }

    private IEnumerator WaitForCutsceneEnd()
    {
        while (playableDirector.state == PlayState.Playing)
            yield return null;

        externalCamera.Priority.Value = 0;
        player.SetActive(true);
        GameManager.Instance.WatchCutscene(cutsceneId);
    }
}