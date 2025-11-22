using UnityEngine;
using System.Collections;
using System.Threading.Tasks;


public class ActionAnimation : MonoBehaviour
{
    protected Animator _animator;
    protected virtual string _animationName => "AnimationName";

    public string AnimationName { get; private set; }

    protected void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public async Task PlayAnimation()
    {
        _animator.Play(_animationName);

        await Task.Yield();

        AnimatorStateInfo info = _animator.GetCurrentAnimatorStateInfo(0);

        Debug.Log("Playing " + _animator + " animation for " + info.length + "s");

        await Awaitable.WaitForSecondsAsync(info.length);
    }
}