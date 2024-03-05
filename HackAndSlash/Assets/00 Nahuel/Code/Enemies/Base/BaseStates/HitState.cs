using UnityEngine;

public class HitState : EnemyState
{
    private float _timer;
    private float _timerCurve;
    private float _force;
    public override void EnterState(Enemy enemy)
    {
        base.EnterState(enemy);
        _enemy.movements.HitStopEffect();
        if (!_enemy.onAir) 
        { 
            _enemy.animations.PlayTargetAnimation("" + _enemy.currentHitEffect.animationHit, true);
            _enemy.currentHitEffect.feedbackHit.PlayFeedbacks();
        }
        _timerCurve = Time.time;
        _timer = 0;

        this.Wait(_enemy.animations.Animator.GetCurrentAnimatorClipInfo(0).Length, () =>
        {
            _enemy.movements.ResetHit();
            enemy.events.Following();
        });
        _enemy.movements.GetRigidBody().isKinematic = false;
    }
    RaycastHit _hit;
    Vector3 _targetPostion;
    
    public override void UpdateState(Enemy enemy)
    {
        _force = _enemy.currentHitEffect.animationCurve.Evaluate(Time.time - _timerCurve);
        Vector3 playerPosition = _enemy._player.transform.position;
        Vector3 dir = (transform.position - playerPosition);
        dir.y = 0;
        _enemy.movements.GetRigidBody().AddForce(dir.normalized * _force * 5000f * Time.deltaTime, ForceMode.Force);
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

        _timer = 0;
        _timerCurve = 0;
    }
}
