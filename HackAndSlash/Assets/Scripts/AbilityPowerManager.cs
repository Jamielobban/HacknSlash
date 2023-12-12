using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityPowerManager : MonoBehaviour
{
    public Slider slider1;
    public Slider slider2;
    public Slider slider3;
    public Slider comboSlider;

    // Combo thresholds
    public float slider1AddRate = 0.05f;
    public float comboSliderDecayRateMultiplier = 0.05f;
    // Decay rate for sliders
    public float decayRate = 0.00001f;

    bool Combo = false;
    int ComboHabilitiCout = 0;

    public int comboCount;


    PlayerControl.ComboAtaques[] combo;


    public TMP_Text comboNumber;

    public static AbilityPowerManager instance;
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
        //comboSlider.gameObject.SetActive(false);
        comboCount = 0; 
    }
    private void Update()
    {
        if (slider1.value >= 0.99f)
        {
            ItemManager.instance.SpawnRandomItem(GameObject.FindObjectOfType<PlayerControl>().transform.position + new Vector3(0f, 5f, 0f));
            slider1.value = 0f;
        }

        //if (slider2.value <= 0.99f)
        //{
        //    DecaySlider(ref slider2, decayRate);
        //}

        //if (slider3.value <= 0.99f)
        //{
        //    DecaySlider(ref slider3, decayRate);
        //}
        // Decay combo slider with dynamic decay rate based on combo count
        float comboSliderDynamicDecayRate = decayRate + comboCount * comboSliderDecayRateMultiplier;
        DecaySlider(ref comboSlider, comboSliderDynamicDecayRate);
        if(comboSlider.value <= 0)
        {
            comboCount = 0;
            comboSlider.gameObject.SetActive(false);
        }
        else
        {
            comboSlider.gameObject.SetActive(true);
        }
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
        //else
        //{
        //    ItemManager.instance.SpawnRandomItem(GameObject.FindObjectOfType<PlayerControl>().transform.position + new Vector3(0f, 5f, 0f));
        //}
        //if (slider1.value >= slider1.maxValue)
        //{
        //    slider2.value += slider1AddRate;
        //}
        //if (slider2.value >= slider2.maxValue)
        //{
        //    slider3.value += slider1AddRate;
        //}
    }

    // Function to reset the combo count
    //public void ResetCombo()
    //{
    //    comboCount = 0;
    //    slider1AddRate = 0.05f;
    //    slider1.value = 0f;
    //    slider2.value = 0f;
    //    slider3.value = 0f;
    //    comboSlider.value = 0f;
    //}


}