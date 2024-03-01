using Cinemachine;
using UnityEngine;

public class PlayerCombatLockFeel : MonoBehaviour
{
    [SerializeField] private LayerMask _targetLayers;
    [SerializeField] private Transform _enemyTargetLocator;
    [SerializeField] private Animator _cinemachineAnimator;

    [Header("Settings")]
    [SerializeField] private bool zeroVert_Look;
    [SerializeField] private float _areaZone = 10; 
    [SerializeField] private float lookAtSmoothing = 2;
    [SerializeField] private float maxNoticeAngle = 60; // Angle degree to see forwd
    [SerializeField] private float _canvasScale = 0.1f; //Canvas scale

    private Transform cam;
    private float currentYOffset;
    private Vector3 pos;

    [SerializeField] private CinemachineVirtualCamera _camera;

    public GameObject selectedEnemy;

    public bool isLockedOn = false;
    [SerializeField] private Transform lockOnCanvas;
    public GameObject lookAtEnemy;

    private void Awake()
    {
        cam = Camera.main.transform;
        lockOnCanvas.gameObject.SetActive(false);
    }


    private void Update()
    {
        if(isLockedOn)
        {
            if(!TargetOnRange(selectedEnemy.transform.position))
            {
                ResetTarget();
            }
            LookAtTarget();
        }
    }

    private void LookAtTarget()
    {
        if (selectedEnemy == null)
        {
            ResetTarget();
            return;
        }
        Vector3 dir = (selectedEnemy.transform.position + transform.position) * 0.5f;
        lookAtEnemy.transform.position = dir;
        lockOnCanvas.position = selectedEnemy.transform.position;
        lockOnCanvas.localScale = Vector3.one * ((cam.position - selectedEnemy.transform.position).magnitude * _canvasScale);
    }


    private void ResetTarget()
    {
        lockOnCanvas.gameObject.SetActive(false);
        selectedEnemy = null;
        _cinemachineAnimator.Play("FollowCamera");
        isLockedOn = false;
    }

    public void SetLockCombat()
    {
        if (isLockedOn)
        {
            ResetTarget();
        }
        else
        {
            selectedEnemy = OnLockOn();

            if(!selectedEnemy)
            {
                ResetTarget();
                return;
            }
            lockOnCanvas.gameObject.SetActive(true);
            _cinemachineAnimator.Play("TargetCamera");
            isLockedOn = true;
        }
        _camera.LookAt = selectedEnemy.transform;

    }

    private GameObject OnLockOn()
    {
        GameObject nearEnemy = CheckNearEnemyByAngle();
        if (nearEnemy != null)
        {
            return nearEnemy;
        }
        return null;
    }

    private GameObject CheckNearEnemyByDistance()
    {
        Collider[] nearbyTargets = Physics.OverlapSphere(transform.position, _areaZone * .75f, _targetLayers);
        float nearestDistance = 50f;
        GameObject closestTarget = null;
        if (nearbyTargets.Length <= 0)
        {
            return null;
        }

        foreach (var target in nearbyTargets)
        {
            float dist = Vector3.Distance(transform.position, target.transform.position);
            if(dist < nearestDistance)
            {
                nearestDistance = dist;
                closestTarget = target.gameObject;
            }
        }
        if(!closestTarget)
        { 
            return null;
        }
        return closestTarget.gameObject;
    }

    private GameObject CheckNearEnemyByAngle()
    {
        Collider[] nearbyTargets = Physics.OverlapSphere(transform.position, _areaZone, _targetLayers);
        float closestAngle = maxNoticeAngle;
        Transform closestTarget = null;

        if (nearbyTargets.Length <= 0)
        {
            return null;
        }
        // Check the near target by angle
        foreach (var target in nearbyTargets)
        {
            Vector3 dir = target.transform.position - cam.position;
            dir.y = 0;
            float _ang = Vector3.Angle(cam.forward, dir);

            if (_ang < closestAngle)
            {
                closestTarget = target.transform;
                closestAngle = _ang;
            }
        }
        if (!closestTarget)
        {
            return null;
        }
        float height = closestTarget.GetComponent<CapsuleCollider>().height * closestTarget.localScale.y;
        float halfHeight = (height * 0.5f) * 0.5f;
        currentYOffset = height - halfHeight;

        if (zeroVert_Look && currentYOffset > 1.6f && currentYOffset < 1.6f * 3)
        {
            currentYOffset = 1.6f;
        }
        Vector3 tarPos = closestTarget.position + new Vector3(0, currentYOffset, 0);
        if (IsBlocked(tarPos))
        {
            return null;
        }
        return closestTarget.gameObject;
    }
    private bool TargetOnRange(Vector3 targetPos)
    {
        float dist = (transform.position - targetPos).magnitude;

        if (dist * 0.5f > _areaZone)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    private bool IsBlocked(Vector3 t)
    {
        RaycastHit _hit;
        if (Physics.Linecast(transform.position + Vector3.up * 0.5f, t, out _hit))
        {
            if (!_hit.transform.CompareTag("Enemy"))
            {
                return true;
            }
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _areaZone);
    }

    public float GetDistanceToSelectedEnemy()
    {
        if(selectedEnemy != null)
        {
            return Mathf.Abs(Vector3.Distance(transform.position, selectedEnemy.transform.position));
        }
        return -1;
    }

}
