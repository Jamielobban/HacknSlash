using System.Collections;
using UnityEngine;
using UnityHFSM;

public class MeleeAttackBoss : MeleeAttack
{
    public string animationRightSide, animationLeftSide, animationFrontSide;
    public float rotationTime;
    public override void OnAttack(State<Enums.EnemyStates, Enums.StateEvent> state)
    {
        //_enemyBase.transform.LookAt(_enemyBase.target.transform.position);
        StartCoroutine(RotateOverTime());
        Use();
    }
    protected override void PlayAttackAnimation()
    {
        Vector3 dirToPlayer = (_enemyBase.target.transform.position - _enemyBase.transform.position).normalized;

        float angle = Vector3.Angle(transform.forward, dirToPlayer);

        if (angle <= 45f)
        {
            _animator.CrossFade(animationFrontSide, 0.2f);
        }
        else if (angle > 45f && angle <= 135f)
        {
            _animator.CrossFade(animationRightSide, 0.2f);
        }
        else
        {
            _animator.CrossFade(animationLeftSide, 0.2f);
        }

        Invoke(nameof(WaitForAnimation), 0.22f);
    }

    private IEnumerator RotateOverTime()
    {
        float elapsedTime = 0f;
        Quaternion initialRotation = _enemyBase.transform.rotation;

        Vector3 directionToTarget = _enemyBase.target.transform.position - transform.position;
        directionToTarget.Normalize();

        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        targetRotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);

        StartCoroutine(RotateOverTime());

        while (elapsedTime < rotationTime)
        {
            float t = elapsedTime / rotationTime;

            _enemyBase.transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, t);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        _enemyBase.transform.rotation = targetRotation;

    }

}
