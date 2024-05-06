using DG.Tweening;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.XInput;
using UnityEngine.InputSystem;


public class Chest : Interactive, IInteractable
{
    private Animator _anim;
    private Collider _collider;
    public bool canBeUnlocked = false;
    public GameObject particles;
    public GameObject particle;
    public bool isUnlocked = false;
    public MMFeedbacks chestOpenedSound;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _collider = GetComponent<Collider>();
        _collider.enabled = false;
        SetCanInteract(false);

        var gamepad = Gamepad.current;
        interactButtonImage.sprite = gamepad is DualShockGamepad ? psInteractSprite : xboxInteractSprite;
    }

    private void Update()
    {
        if (canBeUnlocked)
        {
            _anim.SetBool("openChest", true);            
            canBeUnlocked = false;
        }
    }
    public void PlayChestSound()
    {
        chestOpenedSound.PlayFeedbacks();

    }
    public void Interact()
    {
        SetCanInteract(false);
        GetItem();
        _collider.enabled = false;
    }

    public void EnableCollider() {_collider.enabled = true; SetCanInteract(true); }
    public void EnableParticle() => particles.SetActive(true);

    public void GetItem()
    {
        particle.SetActive(false);
        particles.SetActive(false);
        ItemsLootBoxManager.Instance.ShowNewOptions();
        isUnlocked = true;
    }
    

}
