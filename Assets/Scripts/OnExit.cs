using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnExit : StateMachineBehaviour
{
    [SerializeField] private Animations animation;
    [SerializeField] private bool sameWithFullBody;
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
            animation = AnimationController.instance.GetCurrentAnimation(0);
        }
        if (AnimationController.instance.StartPlayingQueue)
        {
            if (AnimationController.instance.GetListCount() <= 0)
            {
                AnimationController.instance.StartCoroutine(FinishQueue());
            }
            else
            {
                AnimationController.instance.StartCoroutine(ContinueQueue());
            }
            
        }
        
        else
        {
            AnimationController.instance.StartCoroutine(Wait());
        }
       

        IEnumerator Wait()
        {
            yield return new WaitForSeconds(stateInfo.length - crossfade);

            if (cancel) yield break;

            AnimatorBrain target = animator.GetComponent<AnimatorBrain>();
            target.SetLocked(false, layerIndex);
            target.Play(animation, layerIndex, lockLayer, false, crossfade);
        }
        IEnumerator ContinueQueue()
        {
            yield return new WaitForSeconds(stateInfo.length - crossfade);
            AnimatorBrain target = animator.GetComponent<AnimatorBrain>();
            target.SetLocked(false, layerIndex);
            AnimationController.instance.PlayQueue();
        }
        IEnumerator FinishQueue()
        {
            yield return new WaitForSeconds(stateInfo.length - crossfade);
            AnimatorBrain target = animator.GetComponent<AnimatorBrain>();
            target.SetLocked(false, layerIndex);
            target.Play(animation, layerIndex, lockLayer, false, crossfade);
            AnimationController.instance.FinishedQueue();
        }
    }
}