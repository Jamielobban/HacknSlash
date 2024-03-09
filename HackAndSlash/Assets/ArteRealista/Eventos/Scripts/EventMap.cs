using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class EventMap : Interactive, IInteractable
{
    [SerializeField] GameObject objectiveMarker;    
    [SerializeField] protected ForceFieldController forceField;
    protected List<BoxCollider>tangentColliders = new List<BoxCollider>();
    protected Enums.EventState _currentEventState;
    public Enums.EventState CurrentEventState => _currentEventState;
    protected int currentRound = 0;
    public void Interact()
    {
        if (!canInteract) return;

        StartEvent();
        objectiveMarker.SetActive(false);
        canInteract = false;
    }
    protected virtual void Start()
    {
        forceField.SetSpeed(0f);
        CreateTangentColliders(forceField.GetComponentInChildren<SphereCollider>(), 40);
        _currentEventState = Enums.EventState.INACTIVE;
    }
    protected virtual void Update()
    {

    }
    protected virtual void StartEvent()
    {
        ManagerEnemies.Instance.StartEvent();
        forceField.SetSpeed(0.15f);
        foreach (Collider col in tangentColliders)
        {
            col.isTrigger = false;
        }
        _currentEventState = Enums.EventState.PLAYING;
    }
    protected virtual void NextRound()
    {
        currentRound++;
    }
    protected virtual void FinishEvent()
    {
        ManagerEnemies.Instance.EndEvent();
        FindObjectOfType<CanvasAnnouncements>()?.ShowEventCompleted();
        forceField.SetSpeed(-0.1f);        
        foreach (BoxCollider col in tangentColliders)
        {
            col.isTrigger = true;
        }
        _currentEventState = Enums.EventState.FINISHED;
    }    
    protected void CreateTangentColliders(SphereCollider sphereCollider, int numberOfColliders)
    {
        StartCoroutine(CreateTangentCollidersCoroutine(sphereCollider, numberOfColliders));
    }

    IEnumerator CreateTangentCollidersCoroutine(SphereCollider sphereCollider, int numberOfColliders)
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

            tangentColliders.Add(boxCollider);

            // Pausa por un frame antes de continuar con el próximo Box Collider
            yield return null;
        }
    }
}
