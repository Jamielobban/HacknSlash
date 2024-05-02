using DG.Tweening;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Chest : MonoBehaviour, IInteractable
{
    private Animator _anim;
    private Collider _collider;
    public bool canBeUnlocked = false;
    public GameObject particles;
    public GameObject particle;
    public bool isUnlocked = false;
    public MMFeedbacks chestOpenedSound;
    [SerializeField] Image interactCross;

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
            DOVirtual.Float(0, 1, 0.5f, (alpha) => { interactCross.color = new Color(interactCross.color.r, interactCross.color.g, interactCross.color.b, alpha); });
            
            canBeUnlocked = false;
        }
    }
    public void PlayChestSound() 
    {
       chestOpenedSound.PlayFeedbacks();

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
