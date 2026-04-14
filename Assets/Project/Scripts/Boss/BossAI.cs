using System.Collections;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    [SerializeField] private float lookAroundInterval = 3f;
    [SerializeField] private float rotationDuration = 1f;
    [SerializeField] private float stunDuration = 3f;
    [SerializeField] private KnotInteraction knotInteraction;
    [SerializeField] private EnemySight enemySight;

    private bool isTurningDisabled = false;
    private bool isStunned = false;
    private Coroutine lookAroundCoroutine;

    private void Start()
    {
        StartCoroutine(StartLookAroundAfterDelay(9.6f));
    }

    private void Update()
    {
        if (knotInteraction.GetKnotInfo() || enemySight.playerDead)
        {
            if (lookAroundCoroutine != null)
            {
                StopCoroutine(lookAroundCoroutine);
                lookAroundCoroutine = null;
            }
        }
    }

    private IEnumerator StartLookAroundAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        lookAroundCoroutine = StartCoroutine(LookAround());
    }

    private IEnumerator LookAround()
    {
        while (true)
        {
            if (!isStunned && !isTurningDisabled)
            {
                Quaternion targetRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                yield return RotateOverTime(targetRotation, rotationDuration);
            }

            yield return new WaitForSeconds(lookAroundInterval);
        }
    }

    private IEnumerator RotateOverTime(Quaternion targetRotation, float duration)
    {
        float timeElapsed = 0f;
        Quaternion initialRotation = transform.rotation;

        while (timeElapsed < duration)
        {
            transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;
    }

    public void Stun()
    {
        if (lookAroundCoroutine != null)
            StopCoroutine(lookAroundCoroutine);

        isStunned = true;
        StartCoroutine(StunDuration());
    }

    private IEnumerator StunDuration()
    {
        yield return new WaitForSeconds(stunDuration);
        isStunned = false;
        lookAroundCoroutine = StartCoroutine(LookAround());
    }

    public void DisableTurning(float duration)
    {
        if (knotInteraction.GetKnotInfo())
            return;

        if (lookAroundCoroutine != null)
            StopCoroutine(lookAroundCoroutine);

        isTurningDisabled = true;
        StartCoroutine(DisableTurningDuration(duration));
    }

    private IEnumerator DisableTurningDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        isTurningDisabled = false;
        lookAroundCoroutine = StartCoroutine(LookAround());
    }
}