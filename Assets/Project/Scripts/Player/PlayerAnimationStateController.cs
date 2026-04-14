using UnityEngine;

public class PlayerAnimationStateController : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("isJogging", false);
            animator.SetBool("isJumping", false);
        }

        else if(Input.GetKeyDown(KeyCode.W) && Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.A) && Input.GetKeyDown(KeyCode.LeftShift) ||
                Input.GetKeyDown(KeyCode.S) && Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.D) && Input.GetKeyDown(KeyCode.LeftShift))
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isJogging", true);
            animator.SetBool("isJumping", false);
        }

        else if(Input.GetKey("space"))
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isJogging", false);
            animator.SetBool("isJumping", true);
        }

        else if(Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D))
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isJogging", false);
            animator.SetBool("isJumping", false);
        }
    }
}
