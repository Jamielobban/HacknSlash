using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using TMPro;

[Serializable]
public struct EnemiesToKill
{
    public GameObject parent;
    public List<GameObject> enemiesToKill;
}

public abstract class EventMap : Interactive, IInteractable
{
    public EventsManager manager;
    [SerializeField] private GameObject objectiveMarker;        
    [SerializeField] private Transform timers;
    public float timeToActivate, timeToRestart;
    protected Enums.EventState _currentEventState;

    public List<EnemiesToKill> roundsOfEnemies = new List<EnemiesToKill>();
    protected int currentRound = 0;
    public float timer = 0;
    
    
    public Enums.EventState CurrentEventState => _currentEventState;
    
    [SerializeField] protected ForceFieldController forceField;
    [SerializeField] private AudioSource noiseSound;
    private bool scaling = false;
    protected List<BoxCollider> tangentColliders = new List<BoxCollider>();
    private List<TextMeshProUGUI> timersText;

    public void Interact()
    {
        if (!canInteract) return;
        StartEvent();
    }
    
    protected virtual void Start()
    {
        timer = timeToActivate;
        timeToRestart = timeToActivate * 0.5f;
        forceField.SetSpeed(0f);
         CreateTangentColliders(forceField.GetComponentInChildren<SphereCollider>(), 40);
        _currentEventState = Enums.EventState.INACTIVE;
        timersText = timers.GetComponentsInChildren<TextMeshProUGUI>().ToList();
    }

    protected virtual void Update() 
    { 
        HandleTimer();
    }

    private void HandleTimer()
    {
        if(timer <= 0)
        {            
            if (timers.localScale.x > 0 && !scaling) 
            {
                timers.DOScale(0, 0.5f);
                scaling = true;
                Invoke(nameof(StopScale), 0.5f);
            }
            
        }
        else
        {
            if (timers.localScale.x <= 0 && !scaling)
            {
                timers.DOScale(1, 0.5f);
                scaling = true;
                Invoke(nameof(StopScale), 0.5f);
            }
            
            timer -= Time.deltaTime;

            if(timer < 0)
                timer = 0;

            int minutos = ((int)(timer / 60));
            int segundos = ((int)(timer % 60));

            foreach (TextMeshProUGUI tmp in timersText)
                tmp.text = minutos.ToString("00") + " : " + segundos.ToString("00");
        }        

        canInteract = _currentEventState == Enums.EventState.INACTIVE && timer <= 0;

        if(canInteract && !objectiveMarker.activeSelf)
        {
            objectiveMarker.SetActive(true);
        }
    }
    protected virtual void StartEvent()
    {
        GetComponent<Collider>().enabled = false;
        
        AudioManager.Instance.PlayFx(Enums.Effects.Evento);
        AudioManager.Instance.PlayMusic(Enums.Music.EpicTheme);
        
        ManagerEnemies.Instance.StartEvent(); //Clears all the current enemies and sets isInEvent
        
        forceField.SetSpeed(0.15f);
        
        foreach (Collider col in tangentColliders)
        {
            col.isTrigger = false;
        }
        _currentEventState = Enums.EventState.PLAYING;
        
        objectiveMarker.SetActive(false);
        canInteract = false;
        FindObjectOfType<PlayerCollision>().canInteract = false;
    }
    protected virtual void RestartEvent()
    {
        FindObjectOfType<CanvasAnnouncements>()?.ShowEventDefeated(); 
        AudioManager.Instance.PlayFx(Enums.Effects.FailEvent);
        GetComponent<Collider>().enabled = true;
        timer = timeToRestart;
        objectiveMarker.SetActive(true);
        ManagerEnemies.Instance.EndEvent();
        AudioManager.Instance.PlayMusic(Enums.Music.MainTheme);
        forceField.SetSpeed(-0.2f);
        foreach (BoxCollider col in tangentColliders)
        {
            col.isTrigger = true;
        }
        _currentEventState = Enums.EventState.INACTIVE;
    }
    protected virtual void FinishEvent()
    {
        manager.SetCurrentCompletedEvents();
        ManagerEnemies.Instance.EndEvent();
        AudioManager.Instance.PlayMusic(Enums.Music.MainTheme);
        AudioManager.Instance.PlayFx(Enums.Effects.SuccessEvent);

        FindObjectOfType<CanvasAnnouncements>()?.ShowEventCompleted();
        forceField.SetSpeed(-0.2f);        
        foreach (BoxCollider col in tangentColliders)
        {
            col.isTrigger = true;
        }
        _currentEventState = Enums.EventState.FINISHED;
        noiseSound.enabled = false;
        foreach(Material mat in normalMats)
            mat.DOFloat(-0.4f, "_EmissiveMapForce", 0.5f);
    }
    
    protected virtual void NextRound() => currentRound++;
    void StopScale() { scaling = false; }
    private void CreateTangentColliders(SphereCollider sphereCollider, int numberOfColliders) => StartCoroutine(CreateTangentCollidersCoroutine(sphereCollider, numberOfColliders));
    
    protected bool AllEnemiesDefeated() => roundsOfEnemies[currentRound].enemiesToKill.Count <= 0;

    public void RemoveEnemy(GameObject enemy)
    {
        if (roundsOfEnemies[currentRound].enemiesToKill.Contains(enemy))
        {
            roundsOfEnemies[currentRound].enemiesToKill.Remove(enemy);
        }
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

    protected virtual void StartSpawningEnemies()
    {
        foreach (var enemy in roundsOfEnemies[currentRound].enemiesToKill)
        {
            enemy.GetComponent<EnemyBase>().OnSpawnEnemy();
        }
    }
}
