using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimationController : AnimatorBrain
{
    private Animator _animator;
    public static NPCAnimationController instance;
    private const int FULLBODY = 0;
    private const int UPPERBODY = 1;
    private void Awake()
    {
        instance = this;
    }

    private void sitDown()
    {
        Play(Animations.SIT_IDLE1, FULLBODY, false, false);
        Play(Animations.SIT_IDLE1, UPPERBODY, false, false);

    }
    void DefaultAnimation(int layer)
    {
        sitDown();
    }
}

