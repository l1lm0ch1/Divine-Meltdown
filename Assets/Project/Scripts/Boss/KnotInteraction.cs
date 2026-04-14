using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using Unity.Cinemachine;

public class KnotInteraction : MonoBehaviour
{
    [SerializeField] private GameObject[] knots;
    [SerializeField] private GameObject[] chains;
    [SerializeField] private GameObject chandelier;
    [SerializeField] private GameObject player;
    [SerializeField] private CinemachineCamera eventCamera;
    [SerializeField] private GameObject uiPanel;
    [SerializeField] private PlayableDirector endDoorDirector;
    [SerializeField] private Animator bossAnimator;
    [SerializeField] private float resetCameraDelay = 3f;

    private bool[] knotsInteracted;
    private bool chandelierFell = false;

    private void Start()
    {
        knotsInteracted = new bool[knots.Length];
        uiPanel.SetActive(false);
        eventCamera.Priority.Value = 0;
        endDoorDirector.Stop();

        if (GameManager.Instance.IsBossDefeated())
        {
            chandelierFell = true;
            endDoorDirector.Play();
        }
    }

    private void Update()
    {
        if (chandelierFell)
            return;

        bool anyInRange = false;

        for (int i = 0; i < knots.Length; i++)
        {
            if (knotsInteracted[i])
                continue;

            if (Vector3.Distance(player.transform.position, knots[i].transform.position) < 5f)
            {
                anyInRange = true;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    knotsInteracted[i] = true;
                    knots[i].SetActive(false);
                    chains[i].SetActive(false);
                    CheckAllKnots();
                }
            }
        }

        uiPanel.SetActive(anyInRange);
    }

    public bool GetKnotInfo()
    {
        return chandelierFell;
    }

    private void CheckAllKnots()
    {
        foreach (bool interacted in knotsInteracted)
        {
            if (!interacted)
                return;
        }

        if (!chandelierFell)
        {
            chandelierFell = true;
            DropChandelier();
        }
    }

    private void DropChandelier()
    {
        player.SetActive(false);
        eventCamera.Priority.Value = 11;

        Rigidbody rb = chandelier.AddComponent<Rigidbody>();
        rb.useGravity = true;

        bossAnimator.SetBool("isDying", true);

        GameManager.Instance.DefeatBoss();
        StartCoroutine(ResetCameraAfterDelay());
    }

    private IEnumerator ResetCameraAfterDelay()
    {
        yield return new WaitForSeconds(resetCameraDelay);
        eventCamera.Priority.Value = 0;
        player.SetActive(true);
        endDoorDirector.Play();
    }
}