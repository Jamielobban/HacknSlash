using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimations : MonoBehaviour
{
    private Animator _anim;
    public Animator Animator => _anim;
    protected virtual void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        
    }

    public virtual void PlayTargetAnimation(string targetAnimation, bool isInteracting)
    {
        _anim.SetBool("isInteracting", isInteracting);
        _anim.CrossFade(targetAnimation, 0.2f);
    }
}
