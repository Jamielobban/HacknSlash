using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.XInput;
using UnityEngine.InputSystem;

public class SlotComboUi : MonoBehaviour
{
    [SerializeField] Image buttonImage;
    [SerializeField] TextMeshProUGUI triggerText;
    [SerializeField] string[] comboText;
    [SerializeField] Sprite[] sprites;
    // Start is called before the first frame update
    void Start()
    {
        var gamepad = Gamepad.current;
        triggerText.text = gamepad is DualShockGamepad ? comboText[0] : comboText[1];
        buttonImage.sprite = gamepad is DualShockGamepad ? sprites[0] : sprites[1];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
