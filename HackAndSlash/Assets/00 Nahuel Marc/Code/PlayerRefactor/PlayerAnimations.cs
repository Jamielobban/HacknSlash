using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator _anim;

    private PlayerManager _player;

    public Animator GetAnimator
    {
        get { return _anim; }
    }

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _player = transform.parent.GetComponent<PlayerManager>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        //Si player.movement.IsSprinting
        _anim.SetFloat(Constants.ANIM_VAR_SPEED, 2f);
        //Get dir

    }

    public void PlayTargetAnimation(string targetAnimation, bool isInteracting)
    {
        _anim.SetBool("isInteracting", isInteracting);
        _anim.CrossFade(targetAnimation, 0.2f);
    }
}
