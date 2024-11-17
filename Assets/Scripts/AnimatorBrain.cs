
using UnityEngine;
using System;

public class AnimatorBrain : MonoBehaviour
{
    private readonly static int[] animations =
    {
        Animator.StringToHash("Sit Idle1"),
        Animator.StringToHash("Sit Idle2"),
        Animator.StringToHash("Stand Up"),
        Animator.StringToHash("Clapping"),
        Animator.StringToHash("Playing Dumb"),
        Animator.StringToHash("Stretch"),
        Animator.StringToHash("Disbelief"),
        Animator.StringToHash("Crouching"),
        Animator.StringToHash("Drinking"),
        Animator.StringToHash("Cheering"),
        Animator.StringToHash("Yawn"),
        Animator.StringToHash("Make Statement"),
    };

    private Animator animator;
    public Animations[] currentAnimation;
    private bool[] layerLocked;
    private Action<int> DefaultAnimation;

    protected void Initialize(int layers, Animations startingAnimation, Animator animator, Action<int> DefaultAnimation)
    {
        layerLocked = new bool[layers];
        currentAnimation = new Animations[layers];
        this.animator = animator;
        this.DefaultAnimation = DefaultAnimation;

        for (int i = 0; i < layers; i++)
        {
            layerLocked[i] = false;
            currentAnimation[i] = startingAnimation;
        }
    }

    public Animations GetCurrentAnimation(int layer)
    {
        return currentAnimation[layer];
    }

    public void SetLocked(bool lockLayer, int layer)
    {
        layerLocked[layer] = lockLayer;
    }

    public void Play(Animations animation, int layer, bool lockLayer, bool bypassLock, float crossfade = 0.2f)
    {
        Debug.Log("Attemp to Play " + animation);
        if (animation == Animations.NONE)
        {
            DefaultAnimation(layer);
            return;
        }

        if (layerLocked[layer] && !bypassLock) return;
        layerLocked[layer] = lockLayer;

        if (bypassLock)
            foreach (var item in animator.GetBehaviours<OnExit>())
                if (item.layerIndex == layer)
                    item.cancel = true;

        if (currentAnimation[layer] == animation) return;

        currentAnimation[layer] = animation;
        Debug.Log("Play " + animation);
        animator.CrossFade(animations[(int)currentAnimation[layer]], crossfade, layer);
    }
    public bool isPlaying(int layer)
    {
        return animator.GetCurrentAnimatorStateInfo(layer).normalizedTime < 1;
    }
}

public enum Animations
{
    SIT_IDLE1,
    SIT_IDLE2,
    STAND_UP,
    CLAPPING,
    PLAYING_DUMB,
    STRETCH,
    DISBELIEF,
    CROUCHING,
    DRINKING,
    CHEERING,
    YAWN,
    MAKE_STATEMENT,
    NONE
}
public class AnimationPlayEvent
{
    public Animations animation;
    public int[] layers;
    public bool lockLayer;
    public bool bypassLayer;

    public AnimationPlayEvent(Animations ani, int[] _layers, bool _lockLayer, bool _bypassLayer)
    {
        animation = ani;
        layers = _layers;
        lockLayer = _lockLayer;
        bypassLayer = _bypassLayer;
    }

}