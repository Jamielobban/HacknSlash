using UnityEngine;
public class ResetBoolInteracting : StateMachineBehaviour
{
    public string isInteractingBool;
    public bool isInteractingStatus;  
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(isInteractingBool, isInteractingStatus);
    }
}

