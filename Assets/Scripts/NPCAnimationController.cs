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
    private void Start()
    {
        _animator = GetComponent<Animator>();
        Initialize(2, Animations.SIT_IDLE1, _animator, DefaultAnimation);
    }
    public void sitDown()
    {
        Play(Animations.SIT_IDLE1, FULLBODY, false, false);
        Play(Animations.SIT_IDLE1, UPPERBODY, false, false);

    }
    public void clap()
    {
        Play(Animations.CLAPPING, UPPERBODY, false, false);
    }

    public void disbelief()
    {
        Play(Animations.DISBELIEF, UPPERBODY, false, false);
    }


    void DefaultAnimation(int layer)
    {
        sitDown();
    }
}

