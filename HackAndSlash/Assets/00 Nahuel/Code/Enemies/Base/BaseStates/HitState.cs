using UnityEngine;

public class HitState : EnemyState
{
    private float _timerCurve;
    private float _force;
    public override void EnterState(Enemy enemy)
    {
        base.EnterState(enemy);
        _enemy.movements.HitStopEffect();
        if (!_enemy.onAir) 
        { 
            _enemy.animations.PlayTargetAnimation("" + _enemy.currentHitEffect.animationHit, true);
            _enemy.currentFeedbacksEffect.PlayFeedbacks();
        }
        _timerCurve = Time.time;
        _enemy.movements.GetRigidBody().isKinematic = false;

        this.Wait(_enemy.animations.Animator.GetCurrentAnimatorClipInfo(0).Length, () =>
        {
            _enemy.movements.ResetHit();
            enemy.events.Following();
        });
    }
    RaycastHit _hit;
    Vector3 _targetPostion;
    
    public override void UpdateState(Enemy enemy)
    {
        _force = _enemy.currentHitEffect.animationCurve.Evaluate(Time.time - _timerCurve);
        Vector3 dir = Vector3.zero;
        if (_enemy._player.currentComboAttacks.combo == PlayerControl.ComboAtaques.HoldQuadrat || _enemy._player.currentComboAttacks.combo == PlayerControl.ComboAtaques.HoldTriangle)
        {
            dir = _enemy._player.transform.forward;

        }
        else
        {
            dir = (transform.position - _enemy._player.transform.position); 

        }         
        dir.y = 0;
        _enemy.movements.GetRigidBody().AddForce(dir.normalized * _force * _enemy.movements.hitForce * Time.deltaTime, ForceMode.Force);
        if(Physics.Raycast(transform.position, Vector3.down, out _hit, 50f, LayerMask.GetMask("Ground")))
        {
            _targetPostion = transform.position;
            _targetPostion.y = _hit.point.y;
            transform.position = Vector3.Lerp(transform.position, _targetPostion, Time.deltaTime / 0.1f);
            transform.position = _targetPostion;
        }
    }

    public override void ExitState(Enemy enemy)
    {
        base.ExitState(enemy);
        _enemy.movements.GetRigidBody().isKinematic = true;

        _timerCurve = 0;
    }
}
