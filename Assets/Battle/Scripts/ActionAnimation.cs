using UnityEngine;

public class ActionAnimation : MonoBehaviour
{
    protected Animator _animator;
    protected virtual string _animationName => "AnimationName";

    public string AnimationName { get; private set; }

    protected void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlayAnimation()
    {
        _animator.Play(_animationName);
    }
}