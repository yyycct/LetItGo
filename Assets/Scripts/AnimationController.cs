using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationController : AnimatorBrain
{
    private Animator _animator;
    public static AnimationController instance;
    public bool _sitDown;
    public bool _standUp;
    public bool _clap;
    public bool _playDumb;
    public GameObject gasParticleSmelly;
    public GameObject gasParticleAverage;
    public GameObject gasParticleLoud;
    private const int FULLBODY = 0;
    private const int UPPERBODY = 1;
    private Queue<AnimationPlayEvent> listOfAnimationToPlay;
    public bool StartPlayingQueue = false;
    public UnityEvent OnQueueFinished;
    private int fartCardIndex = 0;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        Initialize(2, Animations.SIT_IDLE1, _animator, DefaultAnimation);
        listOfAnimationToPlay = new Queue<AnimationPlayEvent>();
        if(OnQueueFinished == null)
        {
            OnQueueFinished = new UnityEvent();
        }
/*        addToList(Animations.STANDUP, new int[] { 0, 1 }, false, false);
        addToList(Animations.CLAPPING, new int[] { 1 }, false, false);*/
    }

    // Update is called once per frame
    void Update()
    {
        if (_sitDown)
        {
            sitDown();
            _sitDown = false;
        }
        if (_standUp)
        {
            standUp();
            _standUp = false;
        }
        if (_clap)
        {
            clap();
            _clap = false;
        }
        if (_playDumb)
        {
            playDumb();
            _playDumb = false;
        }

    }
    private void standUp()
    {
            Play(Animations.STAND_UP, FULLBODY, false, false);
            Play(Animations.STAND_UP, UPPERBODY, false, false);
    }
    private void sitDown()
    {
            Play(Animations.SIT_IDLE1, FULLBODY, false, false);
            Play(Animations.SIT_IDLE1, UPPERBODY, false, false);

    }
    private void clap()
    {
            Play(Animations.CLAPPING, UPPERBODY, false, true);
    }
    private void stretch()
    {
             Play(Animations.STRETCH, UPPERBODY, false, true);
    }
    private void playDumb()
    {
        Play(Animations.PLAYING_DUMB, UPPERBODY, false, true);
    }
    public int GetListCount() { return listOfAnimationToPlay.Count; }
    public void addToList(Animations animations, int[] layers, bool lockLayer, bool bypassLock)
    {
        Debug.Log("Added animation" + animations);
        AnimationPlayEvent newEvent = new AnimationPlayEvent(animations, layers, lockLayer, bypassLock);
        listOfAnimationToPlay.Enqueue(newEvent);
    }
    public void StartActionAnimation(FartCard fartCardPlayed, int _fartCardIndex){
        StartPlayingQueue = true;
        fartCardIndex = _fartCardIndex;
        StartCoroutine(TurnToPlayFart(fartCardPlayed.fartType));
        PlayQueue();
    }

    private void fartAnimation(FartType type){
        switch (type)
        {
            case FartType.Smelly:
                //Instantiate(gasParticleSmelly);
                gasParticleSmelly.GetComponent<ParticleSystem>().Play();
                break;
            case FartType.Average:
                gasParticleAverage.GetComponent<ParticleSystem>().Play();
                break;
            case FartType.Loud:
                gasParticleLoud.GetComponent<ParticleSystem>().Play();
                break;
        }
    }

    IEnumerator TurnToPlayFart(FartType fartType){
        while(fartCardIndex>0){
            yield return null;
        }
        fartAnimation(fartType);
    }
    public AnimationPlayEvent GetNextOnList()
    {
        Debug.Log("GetNextOnList");
        return listOfAnimationToPlay.Dequeue();
    }

    public void PlayQueue()
    {
        Debug.Log("PlayQueue");
        if (listOfAnimationToPlay.Count <= 0)
        {
            return;
        }
        AnimationPlayEvent playEvent = GetNextOnList();
        fartCardIndex = 0;
        foreach (int l in playEvent.layers)
        {
            Play(playEvent.animation, l, playEvent.lockLayer, playEvent.bypassLayer);
        }

    }
    public void FinishedQueue()
    {
        StartPlayingQueue = false;
        OnQueueFinished.Invoke();
        Debug.Log("Invoke");
    }
    public void SelfReferencedEventCallback()
    {
        Debug.Log("TimedEvent.SelfReferencedEventCallback is called", this);
    }
    void DefaultAnimation(int layer)
    {
        sitDown();
    }
    bool AnimatorIsPlaying(int index) { return _animator.GetCurrentAnimatorStateInfo(index).normalizedTime < 1; }
}


    