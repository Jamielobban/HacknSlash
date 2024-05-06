using MoreMountains.Feedbacks;
using UnityEngine;
using UnityHFSM;

public class InvokeAttack : BaseEnemyAttack
{
    public GameObject invokeEffect;
    public GameObject _spawner;
    public MMFeedbacks invokeSpawnSound;


    protected override void Awake()
    {
        base.Awake();
    }
    public void OnInvoke(State<Enums.EnemyStates, Enums.StateEvent> state)
    {
        _enemyBase.transform.LookAt(_enemyBase.target.transform.position);
        _enemyBase._currentTime = 0f;
        Ray ray = new Ray(_enemyBase.transform.position, Vector3.down);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, 25f, LayerMask.GetMask("Ground")))
        {
            Vector3 hitPoint = hitInfo.point;
            Quaternion groundRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            Quaternion desiredRotation = Quaternion.Euler(-90f, 0f, 0f); 
            Quaternion finalRotation = groundRotation * desiredRotation;
            GameObject nuevoPlano = Instantiate(invokeEffect, hitPoint, finalRotation);
        }
        Use();
    }

    protected override void SetVisualEffects()
    {
        base.SetVisualEffects();
        invokeSpawnSound.PlayFeedbacks();
    }

    protected override void AttackAction()
    {
        base.AttackAction();
        GameObject go = Instantiate(_spawner);
        go.transform.SetParent(gameObject.transform);
        go.transform.localPosition = Vector3.zero;
        go.transform.localScale = Vector3.one;

    }


}
