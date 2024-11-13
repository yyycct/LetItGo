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
            AnimationPlayEvent playEvent = AnimationController.instance.GetNextOnList();
            if (playEvent != null)
            {
                animation = playEvent.animation;
                lockLayer = playEvent.lockLayer;
                AnimationController.instance.StartCoroutine(WaitLayers(playEvent.layers));

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
        IEnumerator WaitLayers(int[] layers)
        {
            yield return new WaitForSeconds(stateInfo.length - crossfade);

            if (cancel) yield break;
            AnimatorBrain target = animator.GetComponent<AnimatorBrain>();
            foreach (int l in layers)
            {
                target.SetLocked(false, l);
                target.Play(animation, l, lockLayer, false, crossfade);
            }
            
            
        }
    }
}