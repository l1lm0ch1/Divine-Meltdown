using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CreditsManager : MonoBehaviour
{
    [SerializeField] private PlayableDirector playableDirector;
    [SerializeField] private string nextSceneName = "StartUpMenu";

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;

        if (playableDirector == null)
            playableDirector = GetComponent<PlayableDirector>();

        StartCoroutine(WaitForCreditsEnd());
    }

    private IEnumerator WaitForCreditsEnd()
    {
        yield return new WaitUntil(() => playableDirector.state != PlayState.Playing);
        SceneManager.LoadScene(nextSceneName);
    }
}