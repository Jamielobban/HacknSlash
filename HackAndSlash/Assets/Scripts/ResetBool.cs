using UnityEngine;

public class ResetBool : StateMachineBehaviour
{
    public string isInteractingBool;
    public bool isInteractingStatus;       
    public string isAirAttackingBool;
    public bool isAirAttackingStatus;    
    public string isAttackingBool;
    public bool isAttackingStatus;  
    

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(isInteractingBool, isInteractingStatus);
        animator.SetBool(isAirAttackingBool, isAirAttackingStatus);
        animator.SetBool(isAttackingBool, isAttackingStatus);
    }

}
