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
    //public Slider slider1;
    //public Slider comboSlider;

    // Combo thresholds
    //private float slider1AddRate = 0.05f;
    //private float comboSliderDecayRateMultiplier = 0.05f;
    // Decay rate for sliders
    //private float decayRate = 0.00001f;

    //bool Combo = false;
    //int ComboHabilitiCout = 0;

    //public int comboCount;

    //public TMP_Text comboNumber;
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
    private Animator itemChoiceAnim;
    private Inventory inventory;
    private Item item1;
    private Item item2;
    private PlayerControl player;
    private ControllerManager controllerManager;
    public GameObject levelUpEffects;


    private Dictionary<Enums.RarityType, Color> rarityColorMap;
    public bool menuActive = false;
    [Header("Buttons")]
    [Space(15)]
    public Button option1Button;
    public Button option2Button;

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
        controllerManager = FindObjectOfType<ControllerManager>();
        option1Button.onClick.AddListener(OnOption1Clicked);
        option2Button.onClick.AddListener(OnOption2Clicked);
        InitializeRarityColorMap();
        SetUpControllerNavigation();
        itemChoiceAnim = itemChoice.GetComponent<Animator>();
    }
    private void Start()
    {
        isOpen = false;
    }
    private void Update()
    {
        //if (slider1.value >= 0.99f)
        //{
        //    ItemManager.instance.SpawnRandomItem(GameObject.FindObjectOfType<PlayerControl>().transform.position + new Vector3(0f, 5f, 0f));
        //    slider1.value = 0f;
        //}

        if (Input.GetKeyDown(KeyCode.J))
        {
            if (itemChoice.activeSelf)
            {

                itemChoice.SetActive(false);
            }
            else
            {

                //itemChoice.SetActive(true);
                ShowNewOptions();

            }
        }

        //float comboSliderDynamicDecayRate = decayRate + comboCount * comboSliderDecayRateMultiplier;
        //DecaySlider(ref comboSlider, comboSliderDynamicDecayRate);
        //if (comboSlider.value <= 0)
        //{
        //    comboCount = 0;
        //    comboSlider.gameObject.SetActive(false);
        //}
        //else
        //{
        //    comboSlider.gameObject.SetActive(true);
        //}
    }

    public void SetOptionClicked()
    {
        if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject == option1Button.gameObject)
        {
            OnOption1Clicked();
        }
        if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject == option2Button.gameObject)
        {
            OnOption2Clicked();
        }
        levelUpEffects.SetActive(false);

    }

    public void OnOption1Clicked()
    {
        //player.GetComponent<PlayerControl>().enabled = true;
        ChooseItem(item1);
        itemChoice.SetActive(false);
        FindObjectOfType<PlayerCollision>().ClearInteractables();

       // option2Button.gameObject.GetComponent<Animator>().SetTrigger("NotChosen");
    }

    public void OnOption2Clicked()
    {
        //player.GetComponent<PlayerControl>().enabled = true;
        ChooseItem(item2);
        itemChoice.SetActive(false);
        FindObjectOfType<PlayerCollision>().ClearInteractables();


        // option1Button.gameObject.GetComponent<Animator>().SetTrigger("NotChosen");
    }

    private void ChooseItem(Item chosenItem)
    {
        // Add the chosen item to the player's inventory and perform any other actions
        AddItemHere(player, chosenItem);
        inventory.AddItem(chosenItem, player.GetItemStacks(chosenItem.GiveName()));
        inventory.RefreshInventory();
        player.CallItemOnPickup(chosenItem.GetAssociatedStatType());

        StartCoroutine(SetActiveFalseCouroutine(itemChoice, 0.3f));

        GameManager.Instance.PauseGame();
        isOpen = false;
        //itemChoiceAnim.SetTrigger("Close");
        //item1 = null;
        //item2 = null;
        menuActive = false;
        EventSystem.current.SetSelectedGameObject(null);
    }

    private IEnumerator SetActiveFalseCouroutine(GameObject wow, float delay)
    {
        option1Button.GetComponent<Button>().enabled = false;
        option2Button.GetComponent<Button>().enabled = false;
        yield return new WaitForSeconds(delay);


        //itemChoiceAnim.SetTrigger("Close");


        yield return new WaitForSeconds(delay);
        wow.SetActive(false);
    }

    public void ShowNewOptions()
    {
        levelUpEffects.SetActive(true);

        itemChoice.SetActive(true);
        isOpen = true;
        menuActive = true;
        option1Button.GetComponent<Button>().enabled = true;
        option2Button.GetComponent<Button>().enabled = true;
        //player.GetComponent<PlayerInventory>().enabled = false;
        EventSystem.current.SetSelectedGameObject(option1Button.gameObject);

        GameManager.Instance.PauseGame();
        // Generate new items for options
     //   Debug.Log(_itemManager.gameObject);
        if (_itemManager != null)
        {
            item1 = _itemManager.GetRandomItem();
            if(item1 != null)
            {
                Debug.Log(item1.GiveName());
            }
            else
            {
                Debug.Log("Item 1 is null");
            }
            item2 = _itemManager.GetRandomItem();
            if (item2 != null)
            {
                Debug.Log(item2.GiveName());
            }
            else
            {
                Debug.Log("Item 2 is null");
            }

            //while (item1.GiveName() == item2.GiveName())
            //{
            //    item2 = ItemManager.instance.GetRandomItem();
            //}
        }

        item1Image.sprite = item1.GiveSprite();
        item1Name.text = item1.GiveName();
        item1Description.text = item1.GiveDescription();
        option1Button.GetComponent<Image>().color = rarityColorMap[item1.GetRarity()];


        item2Image.sprite = item2.GiveSprite();
        item2Name.text = item2.GiveName();
        item2Description.text = item2.GiveDescription();
        option2Button.GetComponent<Image>().color = rarityColorMap[item2.GetRarity()];
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
        player.items.Add(new ItemList(item, item.GiveName(), 1, item.GiveSprite(), item.GiveDescription(), item.GetAssociatedStatType(), item.GetRarity()));
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

        option1Button.navigation = nav1;
        option2Button.navigation = nav2;
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