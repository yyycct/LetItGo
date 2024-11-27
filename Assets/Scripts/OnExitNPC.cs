using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnExitNPC : StateMachineBehaviour
{
    [SerializeField] private Animations animation;
    [SerializeField] private bool sameWithFullBody = true;
    [SerializeField] private bool lockLayer;
    [SerializeField] private float crossfade = 0.2f;
    [HideInInspector] public bool cancel = false;
    [HideInInspector] public int layerIndex = -1;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.layerIndex = layerIndex;
        cancel = false;
        if (sameWithFullBody)
        {
            animation = NPCAnimationController.instance.GetCurrentAnimation(0);
        }

        NPCAnimationController.instance.StartCoroutine(Wait());
        IEnumerator Wait()
        {
            yield return new WaitForSeconds(stateInfo.length - crossfade);

            if (cancel) yield break;

            AnimatorBrain target = animator.GetComponent<AnimatorBrain>();
            target.SetLocked(false, layerIndex);
            target.Play(animation, layerIndex, lockLayer, false, crossfade);
        }
    }
}
