using UnityEngine;
using UnityEngine.Serialization;

public class Chest : MonoBehaviour, IInteractable
{
    private Animator _anim;
    private Collider _collider;
    public bool canBeUnlocked = false;
    public GameObject particles;
    public GameObject particle;
    public bool isUnlocked = false;
    
    private void Awake()
    {        
        _anim = GetComponent<Animator>();
        _collider = GetComponent<Collider>();
        _collider.enabled = false;
    }

    private void Update()
    {
        if(canBeUnlocked)
        {
            _anim.SetBool("openChest", true);
            
            canBeUnlocked = false;
        }
    }
    public void PlayChestSound() 
    {
        AudioManager.Instance.PlayFx(Enums.Effects.ChestOpen);

    }
    public void Interact()
    {
        GetItem();
        _collider.enabled = false;
    }

    public void EnableCollider() => _collider.enabled = true;
    public void EnableParticle() => particles.SetActive(true);

    public void GetItem()
    {
        particle.SetActive(false);
        particles.SetActive(false);
        ItemsLootBoxManager.Instance.ShowNewOptions();
        isUnlocked = true;

    }

}
