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
            //if (!enemy.movements.InRangeToChase())
            //{
            //    _enemy.movements.ResetHit();

            //    enemy.events.Idle();
            //}
            //else
            //{
                _enemy.movements.ResetHit();

                enemy.events.Following();
            //}
        });
    }
    public override void UpdateState(Enemy enemy)
    {
        _force = _enemy.currentHitEffect.animationCurve.Evaluate(Time.time - _timerCurve);
        Vector3 dir = (transform.position - _enemy._player.transform.position);
        _enemy.movements.GetRigidBody().AddForce(dir * _force * 5000f * Time.deltaTime, ForceMode.Force);
    }

    public override void ExitState(Enemy enemy)
    {
        base.ExitState(enemy);
        _timer = 0;
        _timerCurve = 0;
    }
}
