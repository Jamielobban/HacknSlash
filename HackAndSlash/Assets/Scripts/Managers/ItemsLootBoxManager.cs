using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Enums;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.XInput;
using UnityEngine.InputSystem;


public class ItemsLootBoxManager : MonoBehaviour
{
    private static ItemsLootBoxManager _instance;
    public static ItemsLootBoxManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<ItemsLootBoxManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("Items Loot Box Manager");
                    go.AddComponent<ItemsLootBoxManager>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
    }
    
    #region Settings
    [Header("Settings")]
    public int maxSpawnItems = 3;
    public GameObject itemChoice;
    public GameObject slotsGrid;
    public GameObject itemPrefab;
    public GameObject levelUpEffects;
    public GameObject[] playerCanvas;

    public List<Item> itemsToSpawn = new List<Item>();
    private List<GameObject> spawnedUIItems = new List<GameObject>();
    public List<GameObject> itemRarityEffect = new List<GameObject>();

    public bool menuActive = false;
    public bool isOpen;
    bool isPs;
    #endregion    
    
    private ItemManager _itemManager;
    private PlayerControl _player;
    
    private void Awake()
    {
        levelUpEffects.SetActive(false);
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        _itemManager = GetComponent<ItemManager>();
        _player = FindObjectOfType<PlayerControl>();

    }
    private void Start()
    {
        isOpen = false;
        var gamepad = Gamepad.current;
        isPs = gamepad is DualShockGamepad;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (itemChoice.activeSelf)
            {
                itemChoice.SetActive(false);
                ResetItemChoiceMenu();
            }
            else
            {
                ShowNewOptions();
            }
        }
        if(isOpen && EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(spawnedUIItems[0].gameObject);
        }
    }

    private void DisablePickItems()
    {
        itemChoice.SetActive(false);
        FindObjectOfType<PlayerCollision>().ClearInteractables();
    }

    public void ChooseItem(Item chosenItem, InventoryItem chosedItem)
    {
        isOpen = false;
        AudioManager.Instance.PlayFx(Effects.SelectItem);
        
        _player.inventory.AddItem(chosedItem);
        chosenItem.OnItemPickup(_player);

        if(chosenItem.data.rarityType == RarityType.Ability)
        {
            if(_itemManager.abilityItems.Contains(chosenItem))
            {
                _itemManager.abilityItems.Remove(chosenItem);
            }
        }
        
        ResetItemChoiceMenu();   

    }

    private void ResetItemChoiceMenu()
    {
        itemsToSpawn.Clear();

        foreach (var item in spawnedUIItems)
        {
            Destroy(item);
        }
        spawnedUIItems.Clear();
        
        itemChoice.SetActive(false);

        foreach (var canvas in playerCanvas)
        {
            canvas.SetActive(true);
        }

        GameManager.Instance.UnPauseMenuGame();
        Invoke("DesactivarMenu", 0.1f);
        EventSystem.current.SetSelectedGameObject(null);
    }
    private void DesactivarMenu() => menuActive = false;

    public void ShowNewOptions()
    {
        if(GameManager.Instance.Player.states == PlayerControl.States.DEATH)
        {
            return;
        }

        foreach (var canvas in playerCanvas)
        {
            canvas.SetActive(false);
        }

        AudioManager.Instance.PlayFx(Effects.OpenItemsToPickEpic);
        levelUpEffects.SetActive(true);
        itemChoice.SetActive(true);
        isOpen = true;
        menuActive = true;
        
        GenerateItems();
        InstantiatePickItemPrefabs();
        EventSystem.current.SetSelectedGameObject(spawnedUIItems[0].gameObject);
        GameManager.Instance.PauseMenuGame();
    }

    private void GenerateItems()
    {
        if (_itemManager != null)
        {
            for (int i = 0; i < maxSpawnItems; i++)
            {
                Item newItem;
                do
                {
                    newItem = _itemManager.GetRandomItem();
                } while (CheckItemAlreadyExists(itemsToSpawn, newItem));
                itemsToSpawn.Add(newItem);
            }
        }
    }
    
    private void InstantiatePickItemPrefabs()
    {
        for (int i = 0; i < maxSpawnItems; i++)
        {
            GameObject go = Instantiate(itemPrefab, slotsGrid.transform.position, Quaternion.identity);
            go.transform.SetParent(slotsGrid.transform);
            go.transform.localScale = new Vector3(1.7f, 1.7f, 1.7f);
            go.transform.localPosition = new Vector3 (go.transform.localPosition.x, go.transform.localPosition.y ,0);
            go.transform.localRotation = Quaternion.identity;

            UIItemChoice itemUIChoice = go.GetComponent<UIItemChoice>();
            itemUIChoice.item = itemsToSpawn[i];
            itemUIChoice.image.sprite = itemsToSpawn[i].GetSprite();
            itemUIChoice.itemName.text = itemsToSpawn[i].GetName();
            itemUIChoice.description.text = itemsToSpawn[i].GetDescription();
            itemUIChoice.type.text = itemsToSpawn[i].data.itemType;
            itemUIChoice.type.color = itemsToSpawn[i].data.typeColor;

            GameObject rarityEffect = Instantiate(itemRarityEffect[(int)itemsToSpawn[i].GetRarity()]);
            rarityEffect.transform.SetParent(go.transform.GetChild(0).transform);
            rarityEffect.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            rarityEffect.transform.localRotation = Quaternion.identity;
            rarityEffect.transform.localPosition = Vector3.zero;

            if (itemsToSpawn[i].GetRarity() == RarityType.Ability)
            {
                itemUIChoice.inputAbility.sprite = isPs ? itemsToSpawn[i].data.inputIcon : itemsToSpawn[i].data.inputXbox; /////////////////////////
                if(itemsToSpawn[i].data.extraInput == "L2 +")                
                    itemUIChoice.extraInputInfo.text = isPs ? "L2 +" : "LT +";                
                else
                    itemUIChoice.extraInputInfo.text = itemsToSpawn[i].data.extraInput;

                itemUIChoice.inputAbility.gameObject.SetActive(true);
                itemUIChoice.extraInputInfo.gameObject.SetActive(true);
            }
            spawnedUIItems.Add(go);
        }
    }
    
    private bool CheckItemEqual(Item item1, Item item2) => item1.data.ID == item2.data.ID;

    private bool CheckItemAlreadyExists(List<Item> itemList, Item newItem)
    {
        foreach (var item in itemList)
        {
            if (CheckItemEqual(item, newItem))
            {
                return true;
            }
        }
        return false;
    }
    
}