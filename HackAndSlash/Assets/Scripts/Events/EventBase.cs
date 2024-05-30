using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct EnemiesToKill
{
    public GameObject parent;
    public List<GameObject> enemiesToKill;
}

public abstract class EventBase : MonoBehaviour
{
    [Header("Settings: ")]
    public string eventName;
    public string eventDescription;
    public float timeToStartEvent;

    protected Enums.EventState _currentEventState;
    protected CanvasAnnouncements _announcements;

    [Header("Trap Settings: ")]
    public bool canTrap = false;
    [SerializeField] protected ForceFieldController _forceField;
    protected List<BoxCollider> _tangentColliders = new List<BoxCollider>();

    // -- Getters -- //
    public Enums.EventState CurrentEventState => _currentEventState;

    protected virtual void Awake()
    {
        _announcements = FindObjectOfType<CanvasAnnouncements>();
    }

    protected virtual void Start()
    {
        SpawnEvent();
    }

    protected virtual void Update() { }

    protected virtual void SpawnEvent()
    {
        if(canTrap)
        {
            _forceField.SetSpeed(0f);
            CreateTangentColliders(_forceField.GetComponentInChildren<SphereCollider>(), 40);
        }
        _currentEventState = Enums.EventState.INACTIVE;

        _announcements.ShowNotification(eventName, eventDescription);

        Invoke(nameof(StartEvent), timeToStartEvent);
    }

    protected virtual void StartEvent()
    {
        if (canTrap)
        {
            _forceField.SetSpeed(0.15f);

            foreach (Collider col in _tangentColliders)
            {
                col.isTrigger = false;
            }
        }

       // AudioManager.Instance.PlayFx(Enums.Effects.Evento);
       // AudioManager.Instance.PlayMusic(Enums.Music.EpicTheme);
        
        LevelManager.Instance.StartEvent(); //Clears all the current enemies and sets isInEvent
        
        _currentEventState = Enums.EventState.PLAYING;        
    }
    protected virtual void CompleteEvent()
    {
        if (canTrap)
        {
            _forceField.SetSpeed(-0.2f);
            foreach (BoxCollider col in _tangentColliders)
            {
                col.isTrigger = true;
            }
        }
        _currentEventState = Enums.EventState.FINISHED;
        LevelManager.Instance.EndEvent();
        AudioManager.Instance.PlayMusic(Enums.Music.MainTheme);
        AudioManager.Instance.PlayFx(Enums.Effects.SuccessEvent);

        _announcements.ShowEventCompleted();
        Destroy(gameObject);
    }

    protected virtual void DefeatEvent()
    {
        _currentEventState = Enums.EventState.FINISHED;
        LevelManager.Instance.EndEvent();
        AudioManager.Instance.PlayMusic(Enums.Music.MainTheme);
        AudioManager.Instance.PlayFx(Enums.Effects.FailEvent);

        _announcements.ShowEventDefeated();
        Destroy(gameObject);
    }

    protected virtual void CreateTangentColliders(SphereCollider sphereCollider, int numberOfColliders) => StartCoroutine(CreateTangentCollidersCoroutine(sphereCollider, numberOfColliders));
        
    protected virtual IEnumerator CreateTangentCollidersCoroutine(SphereCollider sphereCollider, int numberOfColliders)
    {
        // Obtener la posición del centro del Sphere Collider
        Vector3 colliderCenter = sphereCollider.bounds.center;

        // Calcular el radio del Sphere Collider
        float colliderRadius = sphereCollider.radius;

        // Crear una posición para los colliders, inicialmente centrada en el centro de la esfera
        Vector3 colliderPosition = colliderCenter;

        // Calcular el ángulo entre cada Box Collider
        float angleBetweenColliders = 360f / numberOfColliders;

        // Calcular el tamaño de los BoxColliders para cubrir el perímetro del SphereCollider
        float colliderWidth = 2f * colliderRadius * Mathf.Sin(Mathf.PI / numberOfColliders);

        for (int i = 0; i < numberOfColliders; i++)
        {
            // Calcular la posición del nuevo Box Collider
            float angle = i * angleBetweenColliders * Mathf.Deg2Rad;
            colliderPosition.x = colliderCenter.x + Mathf.Cos(angle) * colliderRadius;
            colliderPosition.z = colliderCenter.z + Mathf.Sin(angle) * colliderRadius;

            // Crear un nuevo GameObject para el Box Collider
            GameObject newColliderObject = new GameObject("TangentCollider" + i);
            newColliderObject.transform.position = colliderPosition;
            newColliderObject.transform.parent = sphereCollider.transform;

            // Agregar un Box Collider al nuevo GameObject
            BoxCollider boxCollider = newColliderObject.AddComponent<BoxCollider>();

            boxCollider.isTrigger = true;
            // Ajustar el tamaño del Box Collider para cubrir el perímetro del SphereCollider
            boxCollider.size = new Vector3(colliderWidth, 100f, 0.1f);

            // Mantener la posición en el eje Y igual al centro de la esfera
            newColliderObject.transform.position = new Vector3(colliderPosition.x, colliderCenter.y, colliderPosition.z);

            // Orientar el Box Collider hacia el centro de la esfera
            newColliderObject.transform.LookAt(colliderCenter);

            _tangentColliders.Add(boxCollider);

            // Pausa por un frame antes de continuar con el próximo Box Collider
            yield return null;
        }
    }

}
