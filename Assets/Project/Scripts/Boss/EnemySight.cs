using System.Collections;
using TMPro;
using UnityEngine;

public class EnemySight : MonoBehaviour
{
    [SerializeField] private QTEManager qteManager;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private GameObject defeatScreen;
    [SerializeField] private TMP_Text defeatText;
    [SerializeField] private Transform player;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private KnotInteraction knotInteraction;
    [SerializeField] private float viewAngle = 45f;
    [SerializeField] private float disableTurnDuration = 2f;
    [SerializeField] private float qteDuration = 2.5f;
    [SerializeField] private string defeatMessage = "You have been defeated!";

    public bool playerDead = false;

    private BossAI bossAI;
    private Animator bossAnimator;
    private bool playerInSight;
    private Coroutine qteCoroutine;

    private void Start()
    {
        playerInSight = false;
        bossAI = GetComponent<BossAI>();
        bossAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        CheckPlayerVisibility();
    }

    private void CheckPlayerVisibility()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        bool inAngle = Vector3.Angle(transform.forward, directionToPlayer) <= viewAngle / 2;
        bool blocked = Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstacleMask);

        if (inAngle && !blocked)
        {
            if (!playerInSight)
            {
                playerInSight = true;
                qteManager.ShowQTE();
                bossAI.DisableTurning(disableTurnDuration);
                qteCoroutine = StartCoroutine(TriggerQuickTimeEvent());
            }
        }
        else
        {
            if (playerInSight)
            {
                playerInSight = false;
                qteManager.HideQTE();

                if (qteCoroutine != null)
                {
                    StopCoroutine(qteCoroutine);
                    qteCoroutine = null;
                }
            }
        }
    }

    private IEnumerator TriggerQuickTimeEvent()
    {
        float timer = 0f;
        bool success = false;

        while (timer < qteDuration && playerInSight)
        {
            timer += Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.F))
            {
                success = true;
                break;
            }

            yield return null;
        }

        if (success)
        {
            BlindEnemy();
        }
        else if (!knotInteraction.GetKnotInfo())
        {
            bossAnimator.SetBool("isAttacking", true);

            if (playerController.health > 0)
                playerController.health -= 1;

            if (playerController.health == 0)
            {
                playerAnimator.SetBool("isDying", true);
                playerController.BlockMovement();
                Cursor.lockState = CursorLockMode.None;
                playerDead = true;
                defeatText.text = defeatMessage;
                StartCoroutine(UIFade.FadeIn(defeatScreen));
            }
        }

        qteManager.HideQTE();
    }

    private void BlindEnemy()
    {
        if (bossAI != null)
        {
            bossAI.Stun();
            playerAnimator.SetBool("isAttacking", true);
        }

        StartCoroutine(DisableBlindingLight());
    }

    private IEnumerator DisableBlindingLight()
    {
        yield return new WaitForSeconds(1f);
        playerAnimator.SetBool("isAttacking", false);
    }
}