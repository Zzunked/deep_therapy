using UnityEngine;

public class ActionAnimation : MonoBehaviour
{
    protected Animator _animator;
    protected virtual string _animationName => "AnimationName";

    protected void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlayAnimation()
    {
        _animator.Play(_animationName);
    }
}