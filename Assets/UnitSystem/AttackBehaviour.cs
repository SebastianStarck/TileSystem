using UnityEngine;
namespace UnitSystem
{
    public class AttackBehaviour : StateMachineBehaviour
    {
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.transform.parent.GetComponent<Unit>().OnAttackAnimationFinish();
        }
    }
}
