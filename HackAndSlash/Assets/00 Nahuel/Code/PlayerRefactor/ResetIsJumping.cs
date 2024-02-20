using UnityEngine;

public class ResetIsJumping : StateMachineBehaviour
{
    public string isLandedBool;
    public bool isLandedStatus;

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isJumping", false);
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
      //  animator.SetBool(isLandedBool, isLandedStatus);
    }


}
