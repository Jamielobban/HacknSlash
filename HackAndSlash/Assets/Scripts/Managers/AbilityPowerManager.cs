using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Enums;

public class AbilityPowerManager : MonoBehaviour
{

    private static AbilityPowerManager _instance;
    public static AbilityPowerManager instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<AbilityPowerManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("Ability Power Manager");
                    go.AddComponent<AbilityPowerManager>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
    }
    private ItemManager _itemManager;
    public GameObject itemChoice;
    private Inventory inventory;
    private PlayerControl player;
    public GameObject levelUpEffects;

    private Item item1;
    private Item item2;
    private Item item3;

    private Dictionary<Enums.RarityType, Color> rarityColorMap;
    public bool menuActive = false;
    [Header("Buttons")]
    [Space(15)]
    public Button option1Button;
    public Button option2Button;
    public Button option3Button;

    [Header("Item 1")]
    [Space(15)]
    public Image item1Image;
    public TMP_Text item1Name;
    public TMP_Text item1Description;


    [Header("Item 2")]
    [Space(15)]
    public Image item2Image;
    public TMP_Text item2Name;
    public TMP_Text item2Description;

    [Header("Item 2")]
    [Space(15)]
    public Image item3Image;
    public TMP_Text item3Name;
    public TMP_Text item3Description;

    public List<GameObject> itemRarityEffect = new List<GameObject>();
    private GameObject currentButton1Rarity = null, currentButton2Rarity = null, currentButton3Rarity;

    public bool isOpen;
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
        //comboCount = 0;

        _itemManager = GetComponent<ItemManager>();
        player = FindObjectOfType<PlayerControl>();
        inventory = FindObjectOfType<Inventory>();

        InitializeRarityColorMap();
        SetUpControllerNavigation();

        option1Button.onClick.AddListener(OnOption1Clicked);
        option2Button.onClick.AddListener(OnOption2Clicked);
        option3Button.onClick.AddListener(OnOption3Clicked);

    }
    private void Start()
    {
        isOpen = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (itemChoice.activeSelf)
            {
                itemChoice.SetActive(false);
            }
            else
            {
                ShowNewOptions();
            }
        }
        if(isOpen && EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(option1Button.gameObject);
        }
    }



    public void OnOption1Clicked()
    {
        ChooseItem(item1);
        DisablePickItems();
    }

    public void OnOption2Clicked()
    {
        ChooseItem(item2);
        DisablePickItems();
    }
    public void OnOption3Clicked()
    {
        ChooseItem(item3);
        DisablePickItems();
    }

    private void DisablePickItems()
    {
        itemChoice.SetActive(false);
        FindObjectOfType<PlayerCollision>().ClearInteractables();
    }

    private void ChooseItem(Item chosenItem)
    {
        isOpen = false;
        // Add the chosen item to the player's inventory and perform any other actions
        //PlaySound select item
        AudioManager.Instance.PlayFx(Effects.SelectItem);
        AddItemHere(player, chosenItem);
        inventory.AddItem(chosenItem, player.GetItemStacks(chosenItem.GiveName()));
        inventory.RefreshInventory();
        chosenItem.OnItemPickup(player);
       // player.CallItemOnPickup(chosenItem.data.id);

        Destroy(currentButton1Rarity);
        Destroy(currentButton2Rarity);
        Destroy(currentButton3Rarity);

        StartCoroutine(SetActiveFalseCouroutine(itemChoice, 0.3f));

        GameManager.Instance.UnPauseMenuGame();
        Invoke("DesactivarMenu", 0.1f);
        EventSystem.current.SetSelectedGameObject(null);
    }

    void DesactivarMenu()
    {
        menuActive = false;
    }

    private IEnumerator SetActiveFalseCouroutine(GameObject wow, float delay)
    {
        option1Button.GetComponent<Button>().enabled = false;
        option2Button.GetComponent<Button>().enabled = false;
        option3Button.GetComponent<Button>().enabled = false;
        yield return new WaitForSeconds(delay);
        yield return new WaitForSeconds(delay);
        wow.SetActive(false);
    }

    public void ShowNewOptions()
    {
        StartCoroutine(Activef());

        AudioManager.Instance.PlayFx(Effects.OpenItemsToPickEpic);
        option1Button.GetComponent<Animator>().SetTrigger("Normal");
        option2Button.GetComponent<Animator>().SetTrigger("Normal");
        option3Button.GetComponent<Animator>().SetTrigger("Normal");

        levelUpEffects.SetActive(true);

        itemChoice.SetActive(true);
        isOpen = true;
        menuActive = true;

        option1Button.GetComponent<Button>().enabled = true;
        option2Button.GetComponent<Button>().enabled = true;
        option3Button.GetComponent<Button>().enabled = true;

        EventSystem.current.SetSelectedGameObject(option1Button.gameObject);

        // Generate new items for options
        if (_itemManager != null)
        {
            item1 = _itemManager.GetRandomItem();
            do
            {
                item2 = _itemManager.GetRandomItem();
            } while (CheckItemEqual(item1, item2));

            do
            {
                item3 = _itemManager.GetRandomItem();
            } while (CheckItemEqual(item1, item3) || CheckItemEqual(item2, item3));
        }

        item1Image.sprite = item1.GiveSprite();
        item1Name.text = item1.GiveName();
        item1Description.text = item1.GiveDescription();


        item2Image.sprite = item2.GiveSprite();
        item2Name.text = item2.GiveName();
        item2Description.text = item2.GiveDescription();


        item3Image.sprite = item3.GiveSprite();
        item3Name.text = item3.GiveName();
        item3Description.text = item3.GiveDescription();


        currentButton1Rarity = Instantiate(itemRarityEffect[(int)item1.GetRarity()], option1Button.transform);
        currentButton2Rarity = Instantiate(itemRarityEffect[(int)item2.GetRarity()], option2Button.transform);
        currentButton3Rarity = Instantiate(itemRarityEffect[(int)item3.GetRarity()], option3Button.transform);
        currentButton1Rarity.transform.localPosition = Vector3.zero;
        currentButton2Rarity.transform.localPosition = Vector3.zero;
        currentButton3Rarity.transform.localPosition = Vector3.zero;
        GameManager.Instance.PauseMenuGame();
    }

    private IEnumerator Activef()
    {
        option1Button.GetComponent<Button>().interactable = false;
        option2Button.GetComponent<Button>().interactable = false;
        option3Button.GetComponent<Button>().interactable = false;
        yield return new WaitForSecondsRealtime(.75f);
        option1Button.GetComponent<Button>().interactable = true;
        option2Button.GetComponent<Button>().interactable = true;
        option3Button.GetComponent<Button>().interactable = true;
    }
    private bool CheckItemEqual(Item item1, Item item2)
    {
        if(item1.GiveName() == item2.GiveName())
        {
            if(item1.GetRarity() == item2.GetRarity())
            {
                return true;
            }
            return false;
        }
        return false;
    }

    private void InitializeRarityColorMap()
    {
        rarityColorMap = new Dictionary<Enums.RarityType, Color>
        {
            { RarityType.Uncommon, new Color(0.75f, 0.75f, 0.75f) },    // White
            { RarityType.Common, new Color(0f, 1f, 0f) },    // Green
            { RarityType.Rare, new Color(0.5f, 0f, 1f) }, // Purple
            { RarityType.Legendary, new Color(1f, 0f, 0f) }       // Red
        };
    }

    public void AddItemHere(PlayerControl player, Item item)
    {
        foreach (ItemList i in player.items)
        {
            if (i.item.GiveName() == item.GiveName())
            {
                i.stacks++;
                return;
            }
        }
        player.items.Add(new ItemList(item, 1));
        inventory.AddItem(item, 1);
    }

    // Decay the slider value
    private void DecaySlider(ref Slider slider, float decayRate)
    {
        slider.value = Mathf.Clamp01(slider.value - decayRate * Time.deltaTime);
    }

    void DesaparecerCombo()
    {
        //Combo = false;
        //ComboHabilitiCout = comboCount;
    }

    private void SetUpControllerNavigation()
    {
        Navigation nav1 = option1Button.navigation;
        nav1.mode = Navigation.Mode.Explicit;
        nav1.selectOnRight = option2Button;

        Navigation nav2 = option2Button.navigation;
        nav2.mode = Navigation.Mode.Explicit;
        nav2.selectOnLeft = option1Button;
        nav2.selectOnRight = option3Button;


        Navigation nav3 = option3Button.navigation;
        nav3.mode = Navigation.Mode.Explicit;
        nav3.selectOnLeft = option2Button;

        option1Button.navigation = nav1;
        option2Button.navigation = nav2;
        option3Button.navigation = nav3;
    }

    // Function to increase the combo count
    public void IncreaseCombo()
    {
        //comboCount++;
        //comboNumber.text = comboCount.ToString();
        //comboSlider.value = comboSlider.maxValue;

        //slider1AddRate += 0.005f;

        //if (slider1.value <= slider1.maxValue)
        //{
        //    slider1.value += 0.05f;
        //}
    }

}