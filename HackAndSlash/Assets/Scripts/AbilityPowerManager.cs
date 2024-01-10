using DarkPixelRPGUI.Scripts.UI.Equipment;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityPowerManager : MonoBehaviour
{
    public Slider slider1;
    public Slider comboSlider;

    // Combo thresholds
    private float slider1AddRate = 0.05f;
    private float comboSliderDecayRateMultiplier = 0.05f;
    // Decay rate for sliders
    private float decayRate = 0.00001f;

    bool Combo = false;
    int ComboHabilitiCout = 0;

    public int comboCount;

    public TMP_Text comboNumber;

    public static AbilityPowerManager instance;

    public GameObject itemChoice;
    private Animator itemChoiceAnim;
    private Inventory inventory;
    private Item item1;
    private Item item2;
    private PlayerControl player;
    private ControllerManager controllerManager;

    private Dictionary<RarityColor, Color> rarityColorMap;
    bool menuActive = false;
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
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        comboCount = 0;
        player = FindObjectOfType<PlayerControl>();
        inventory = FindObjectOfType<Inventory>();
        controllerManager = FindObjectOfType<ControllerManager>();
        option1Button.onClick.AddListener(OnOption1Clicked);
        option2Button.onClick.AddListener(OnOption2Clicked);
        InitializeRarityColorMap();
        SetUpControllerNavigation();
        itemChoiceAnim = itemChoice.GetComponent<Animator>();
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
                itemChoice.SetActive(true);
                ShowNewOptions();

            }
        }

        if (menuActive)
        {
            if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject == option1Button.gameObject)
            {
                if (controllerManager != null && controllerManager.CheckIfPressed())
                {
                    OnOption1Clicked();
                }
            }
            if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject == option2Button.gameObject)
            {
                if (controllerManager != null && controllerManager.CheckIfPressed())
                {
                    OnOption2Clicked();
                }
            }
        }

        float comboSliderDynamicDecayRate = decayRate + comboCount * comboSliderDecayRateMultiplier;
        DecaySlider(ref comboSlider, comboSliderDynamicDecayRate);
        if (comboSlider.value <= 0)
        {
            comboCount = 0;
            comboSlider.gameObject.SetActive(false);
        }
        else
        {
            comboSlider.gameObject.SetActive(true);
        }
    }

    public void OnOption1Clicked()
    {
        player.GetComponent<PlayerControl>().enabled = true;
        ChooseItem(item1);
        option2Button.gameObject.GetComponent<Animator>().SetTrigger("NotChosen");
    }

    public void OnOption2Clicked()
    {
        player.GetComponent<PlayerControl>().enabled = true;
        ChooseItem(item2);
        option1Button.gameObject.GetComponent<Animator>().SetTrigger("NotChosen");
    }

    private void ChooseItem(Item chosenItem)
    {
        // Add the chosen item to the player's inventory and perform any other actions
        AddItemHere(player, chosenItem);
        inventory.AddItem(chosenItem, player.GetItemStacks(chosenItem.GiveName()));
        inventory.RefreshInventory();
        player.CallItemOnPickup(chosenItem.GetAssociatedStatType());

        StartCoroutine(SetActiveFalseCouroutine(itemChoice, 0.3f));
        Time.timeScale = 1.0f;
        //itemChoiceAnim.SetTrigger("Close");
        item1 = null;
        item2 = null;
        menuActive = false;
        EventSystem.current.SetSelectedGameObject(null);
    }

    private IEnumerator SetActiveFalseCouroutine(GameObject wow, float delay)
    {
        option1Button.GetComponent<Button>().enabled = false;
        option2Button.GetComponent<Button>().enabled = false;
        yield return new WaitForSeconds(delay*5);
        itemChoiceAnim.SetTrigger("Close");
        yield return new WaitForSeconds(delay);
        wow.SetActive(false);
    }

    private void ShowNewOptions()
    {
        menuActive = true;
        option1Button.GetComponent<Button>().enabled = true;
        option2Button.GetComponent<Button>().enabled = true;
        player.GetComponent<PlayerControl>().enabled = false;
        EventSystem.current.SetSelectedGameObject(option1Button.gameObject);
        Time.timeScale = 0.0f;
        // Generate new items for options
        item1 = ItemManager.instance.GetRandomItem();
        item2 = ItemManager.instance.GetRandomItem();


        int colorBox1;
        int colorBox2;
        RarityColor colorShow1;
        RarityColor colorShow2;
        //option1Button.colors.normalColor.

        colorBox1 = (int)item1.GetRarity();
        colorBox2 = (int)item2.GetRarity();
        colorShow1 = (RarityColor)colorBox1;
        colorShow2 = (RarityColor)colorBox2;

        item1Image.sprite = item1.GiveSprite();
        item1Name.text = item1.GiveName();
        item1Description.text = item1.GiveDescription();
        option1Button.GetComponent<Image>().color = rarityColorMap[colorShow1];



        item2Image.sprite = item2.GiveSprite();
        item2Name.text = item2.GiveName();
        item2Description.text = item2.GiveDescription();
        option2Button.GetComponent<Image>().color = rarityColorMap[colorShow2];


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
        Combo = false;
        ComboHabilitiCout = comboCount;
    }

    private void SetUpControllerNavigation()
    {
        Navigation nav1 = option1Button.navigation;
        nav1.mode = Navigation.Mode.Explicit;
        nav1.selectOnDown = option2Button;

        Navigation nav2 = option2Button.navigation;
        nav2.mode = Navigation.Mode.Explicit;
        nav2.selectOnUp = option1Button;

        option1Button.navigation = nav1;
        option2Button.navigation = nav2;
    }

    // Function to increase the combo count
    public void IncreaseCombo()
    {
        comboCount++;
        comboNumber.text = comboCount.ToString();
        comboSlider.value = comboSlider.maxValue;

        slider1AddRate += 0.005f;

        if (slider1.value <= slider1.maxValue)
        {
            slider1.value += 0.05f;
        }
    }

    private void InitializeRarityColorMap()
    {
        rarityColorMap = new Dictionary<RarityColor, Color>
        {
            { RarityColor.White, new Color(1f, 1f, 1f) },    // White
            { RarityColor.Green, new Color(0f, 1f, 0f) },    // Green
            { RarityColor.Purple, new Color(0.5f, 0f, 1f) }, // Purple
            { RarityColor.Red, new Color(1f, 0f, 0f) }       // Red
        };
    }

    public enum RarityColor
    {
        White,
        Green,
        Purple,
        Red
    }
}