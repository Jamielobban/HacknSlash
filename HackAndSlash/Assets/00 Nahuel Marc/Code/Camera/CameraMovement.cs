using System;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Enums.CameraType type = Enums.CameraType.FreeLook;
    private Camera _cam;
    private PlayerManager _player;

    [Header("Configuration variables:")]
    [SerializeField][Range(0.1f, 2.0f)] private float _sensitivity;
    [SerializeField] private bool _invertXAxis;
    [SerializeField] private bool _invertYAxis;
    [SerializeField] private LayerMask _collisionLayers;
    [SerializeField] private float _collisionRadius;
    [SerializeField] private float _minimumCollisionOffset;
    


    public Transform lookAt;
    public Transform trueLookAt;

    #region Spherical Coordinates
    private double theta = Math.PI / 2;
    private float tTheta = 0.5f;
    private double alpha = -Math.PI / 2;
    [Serializable]
    public struct CameraSettings
    {
        [SerializeField][Range(-1.0f, 1.0f)] private float _offsetX;
        [SerializeField][Range(-1.0f, 2.0f)] private float _offsetY;
        [SerializeField][Range(0.0f, 90.0f)] private float _maxVerticalAngle;
        [SerializeField][Range(0.0f, 90.0f)] private float _minVerticalAngle;
        [SerializeField] private float _cameraDistance;
        private Vector3 _rotationOrigin;

        public void SetRotationOrigin(Vector3 o) => _rotationOrigin = o;
        public float GetCameraDistance() => _cameraDistance;
        public Vector2 GetOffset() => new Vector2(_offsetX, _offsetY);
        public Vector2 GetLimitVerticalAnglesRadians() => new Vector2(_maxVerticalAngle * (float)Math.PI / 180, _minVerticalAngle * (float)Math.PI / 180);
    }
    [SerializeField] private CameraSettings _cameraSettings;
    #endregion

    #region Camera Transitions

    private bool _inTransition;
    private CamaraState _startState;
    private CamaraState _endState;
    private float _transitionTime = 0.0f;

    private struct CamaraState
    {
        public Vector3 pos;
        public Vector3 rot;
        public Transform lookAt;
        public float time;
    }

    #endregion

    public CameraSettings GetCameraSettings() => _cameraSettings;

    private void Awake()
    {
        _player = GetComponent<PlayerManager>();

        _cam = Camera.main;

        if (type == Enums.CameraType.Locked)
        {
            _cam.transform.parent = transform;
        }
        _cameraSettings.SetRotationOrigin(transform.position + Vector3.up);
    }

    private void FixedUpdate()
    {
        OrbitSphericalCoords();
        HandleCameraCollision();
    }

    private void OrbitSphericalCoords()
    {
        if (!_inTransition)
        {
            float inputX = _player.inputs.GetDirectionRightStick().x;
            float inputY = _player.inputs.GetDirectionRightStick().y;

            inputX = (_invertXAxis) ? inputX : (-inputX);
            inputY = (_invertYAxis) ? (-inputY) : inputY;

            //Orbit camera around player
            if (inputX != 0)
            {
                alpha += inputX * _sensitivity * Time.deltaTime;
            }
            if (inputY != 0)
            {
                Vector2 limitAnglesRads = _cameraSettings.GetLimitVerticalAnglesRadians();
                float maxAngle = limitAnglesRads.x;
                float minAngle = limitAnglesRads.y;

                maxAngle = UtilsNagu.PI_MEDIO - maxAngle;
                minAngle = UtilsNagu.PI_MEDIO + minAngle;


                tTheta += inputY * _sensitivity * Time.deltaTime;
                tTheta = Mathf.Clamp(tTheta, 0, 1);
                theta = Mathf.Lerp(maxAngle, minAngle, tTheta);
            }
            float x = lookAt.transform.position.x + (float)(_cameraSettings.GetCameraDistance() * Math.Sin(theta) * Math.Cos(alpha));
            float y = lookAt.transform.position.y + (float)(_cameraSettings.GetCameraDistance() * Math.Cos(theta));
            float z = lookAt.transform.position.z + (float)(_cameraSettings.GetCameraDistance() * Math.Sin(theta) * Math.Sin(alpha));

            Vector3 newCameraPosition = new Vector3(x, y, z);
            Vector3 offsetCameraPosition = newCameraPosition + _cameraSettings.GetOffset().x * _cam.transform.right + _cameraSettings.GetOffset().y * _cam.transform.up;
            _cam.transform.position = offsetCameraPosition;

            trueLookAt.transform.position = lookAt.transform.position + +_cameraSettings.GetOffset().x * _cam.transform.right + _cameraSettings.GetOffset().y * _cam.transform.up;

            _cam.transform.LookAt(trueLookAt);
        }
        else
        {
            float t = (Time.time - _startState.time) / (_endState.time - _startState.time);
            _cam.transform.position = Vector3.Lerp(_startState.pos, _startState.pos, t);
            _cam.transform.eulerAngles = Vector3.Lerp(_startState.rot, _startState.rot, t);


            _cam.transform.LookAt(_endState.lookAt);
            if (t >= 1)
            {
                _inTransition = false;
            }
        }
    }

    public void SetCameraToOrigin()
    {
        double originTheta = UtilsNagu.PI_MEDIO;
        double originAlpha = -UtilsNagu.PI_MEDIO;

        if (!_cam) _cam = Camera.main;
        if (!trueLookAt) trueLookAt = transform.Find("LookAtTransform");

        float camDistance = _cameraSettings.GetCameraDistance();

        if (lookAt)
        {
            Vector3 newCameraPosition = lookAt.transform.position +
                                    new Vector3(camDistance * (float)(Math.Sin(originTheta) * Math.Cos(originAlpha)),
                                        camDistance * (float)(Math.Cos(originTheta)),
                                        camDistance * (float)(Math.Sin(originTheta) * Math.Sin(originAlpha)));
            Vector3 offsetCameraPosition = newCameraPosition + _cameraSettings.GetOffset().x * _cam.transform.right + _cameraSettings.GetOffset().y * _cam.transform.up;

            _cam.transform.position = offsetCameraPosition;

            trueLookAt.transform.position = lookAt.transform.position + +_cameraSettings.GetOffset().x * _cam.transform.right + _cameraSettings.GetOffset().y * _cam.transform.up;
            _cam.transform.LookAt(trueLookAt);
        }
    }

    public void TransitionTo(Vector3 finalPosition, Vector3 finalRotation, Transform finalLookAt, float duration)
    {
        _startState.pos = _cam.transform.position;
        _startState.rot = _cam.transform.rotation.eulerAngles;
        _startState.lookAt = lookAt;
        _startState.time = Time.time;

        _endState.pos = finalPosition;
        _endState.rot = finalRotation;
        _endState.lookAt = finalLookAt;
        _endState.time = _startState.time + duration;

        _transitionTime = duration;
        _inTransition = true;
    }

    //Not working
    private void HandleCameraCollision()
    {
        float targetPosition = _cameraSettings.GetCameraDistance(); 

        RaycastHit hit;
        Vector3 direction = (_cam.transform.position - lookAt.position).normalized;

        if (Physics.SphereCast(lookAt.position, _collisionRadius, direction, out hit, Mathf.Abs(targetPosition), _collisionLayers))
        {
            float distance = Vector3.Distance(lookAt.position, hit.point);
            targetPosition = Mathf.Max(_minimumCollisionOffset, targetPosition - (distance - _minimumCollisionOffset));
        }

        Vector3 cameraPositionOffset = -_cam.transform.forward * targetPosition; 
        _cam.transform.position = Vector3.Lerp(_cam.transform.position, lookAt.position + cameraPositionOffset, 0.2f);

        _cam.transform.LookAt(lookAt);
    }

}
