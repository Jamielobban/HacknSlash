using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StructureEnemiesLife : MonoBehaviour
{
    public int enemiesToActivate;
    protected int _currentEnemeisKilled;
    public bool isActive = false;
    public GameObject particleToCharge;

    public UnityEvent<float> killedEnemyTrigger;

    protected virtual void Awake() { }

    protected virtual void Update()
    { 
        if(CanActivate())
        {
            Activate();
        }
    }


    protected virtual void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyBase>().nearStructure = this;
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyBase>().nearStructure = null;
        }
    }
    public virtual void AddKilledEnemy() { 
        _currentEnemeisKilled++;
        float perOne = (float)((float)_currentEnemeisKilled / (float)enemiesToActivate);
        killedEnemyTrigger.Invoke((float)(perOne)); 
    }
    protected virtual void Activate() => isActive = true;
    protected virtual bool CanActivate() => _currentEnemeisKilled >= enemiesToActivate && !isActive;

    public virtual void ResetStructure()
    {
        _currentEnemeisKilled = 0;
        isActive = false;
    }

}
