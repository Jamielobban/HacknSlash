using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class CamaraCollision : MonoBehaviour
{

    [SerializeField]private float _minDistance = 1;
    [SerializeField] private float _maxDistance = 5;
    [SerializeField] private float _smoothness = 10;
    private float _distance;

    private Vector3 _direccion;

    void Start()
    {
        _direccion = transform.localPosition.normalized;
        _distance = transform.localPosition.magnitude;
    }

    void Update()
    {
        Vector3 posCamara = transform.parent.TransformPoint(_direccion * _maxDistance);

        RaycastHit hit;
        if(Physics.Linecast(transform.parent.position, posCamara, out hit))
        {
            _distance = Mathf.Clamp(hit.distance * 0.85f, _minDistance, _maxDistance);
        }
        else
        {
            _distance = _maxDistance;
        }
        transform.localPosition = Vector3.Lerp(transform.localPosition, _direccion * _distance, _smoothness * Time.deltaTime);
    }
}
