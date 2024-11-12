using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : AnimatorBrain
{
    private Animator _animator;
    public bool _sitDown;
    public bool _standUp;
    public bool _clap;
    private const int FULLBODY = 0;
    private const int UPPERBODY = 1;
    public enum AnimationFullBodyState
    {
        Sitting,
        Standing,
        Walking
    }
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        Initialize(2, Animations.SITIDLE1, _animator, DefaultAnimation);
    }

    // Update is called once per frame
    void Update()
    {
        if (_sitDown)
        {
            sitDown();
        }
        if (_standUp)
        {
            standUp();
        }
        if (_clap)
        {
            clap();
        }
    }
    private void standUp()
    {
            Play(Animations.STANDUP, FULLBODY, false, false);
            Play(Animations.STANDUP, UPPERBODY, false, false);
    }
    private void sitDown()
    {
            Play(Animations.SITIDLE1, FULLBODY, false, false);
            Play(Animations.SITIDLE1, UPPERBODY, false, false);

    }
    private void clap()
    {
            Play(Animations.CLIPPING, UPPERBODY, false, false);
    }
    void DefaultAnimation(int layer)
    {

    }
    bool AnimatorIsPlaying(int index) { return _animator.GetCurrentAnimatorStateInfo(index).normalizedTime < 1; }
}
    