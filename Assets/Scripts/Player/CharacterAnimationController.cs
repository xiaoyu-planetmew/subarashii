using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    public bool loop;
    public AnimationEventArrays[] animations;

    public static CharacterAnimationController Instance;
    private Animator anim;
    private float changeEventTime;
    private float timer = 0;
    [SerializeField] private int nowEventIndex;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        nowEventIndex = 0;
        changeEventTime = Random.Range(animations[nowEventIndex].randomTime[0], animations[nowEventIndex].randomTime[1] + 1);
        PlayAnimation();

    }

    // Update is called once per frame
    void Update()
    {
        if (!loop) return;

        timer += Time.deltaTime;
        if (timer > changeEventTime)
        {
            if(animations[nowEventIndex].randomTime[1] != 0)  //如果随机时间都是0，则只在调用nowIdex时播放一次
            {
                PlayAnimation();
            }

            timer = 0;
            changeEventTime = Random.Range(animations[nowEventIndex].randomTime[0], animations[nowEventIndex].randomTime[1] + 1);
        }
    }

    public void ChangeAnimationEvent(AnimationEventType changeAnimsType)
    {
        for(int i = 0; i< animations.Length;i++)
        {
            if (animations[i].animsType == changeAnimsType)
            {
                nowEventIndex = i;
                PlayAnimation();
            }
        }
    }

    public void PlayAnimation()
    {
        int randomAnimIndex = Random.Range(0, animations[nowEventIndex].animationEvents.Length);
        //Debug.Log(gameObject.name + "'s Animation is " + animations[nowEventIndex].animationEvents[randomAnimIndex]);
        anim.SetTrigger(animations[nowEventIndex].animationEvents[randomAnimIndex]);
    }

}

[System.Serializable]
public class AnimationEventArrays
{
    public AnimationEventType animsType;
    public Vector2 randomTime = new Vector2(5f, 9f);
    public string[] animationEvents;

    public string this[int index]
    {
        get
        {
            return animationEvents[index];
        }
    }
    public AnimationEventArrays()
    {
        this.animationEvents = new string[4];
    }
    public AnimationEventArrays(int index)
    {
        this.animationEvents = new string[index];
    }
}

public enum AnimationEventType
{
    Idle,
    Good,
    Miss,
    Absorb_Left,
    Absorb_Right,
    GameOver,
    Talk,
    Charge,
    Smash,
    Guard,
}
