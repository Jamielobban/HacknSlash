using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class DynamicGridNavigation : MonoBehaviour
{
    public Transform[] gridElements;
    private int currentIndex;
    private GameObject lastSelectedObject; // Added to store the last selected object
    ControllerManager controllerMan;
    public TMP_Text ITEMdESC;
    public PlayerControl playerControl;
    public Image itemHighlight;

    void Start()
    {
        controllerMan = FindObjectOfType<ControllerManager>();
        playerControl = FindObjectOfType<PlayerControl>();
        RefreshGridElements();
        currentIndex = 0;
    }

    void OnEnable()
    {
        currentIndex = 0;
        // Store the last selected object when the script is enabled
        lastSelectedObject = EventSystem.current.currentSelectedGameObject;
    }

    void OnDisable()
    {
        // Restore the last selected object when the script is disabled
        EventSystem.current.SetSelectedGameObject(lastSelectedObject);
    }

    void Update()
    {
        HandleNavigationInput();
    }

    void HandleNavigationInput()
    {
        if (Gamepad.current == null)
        {
            Debug.LogWarning("No gamepad found.");
            return;
        }

        Vector2 dpadInput = Gamepad.current.dpad.ReadValue();

        if (Gamepad.current.dpad.right.wasPressedThisFrame && dpadInput.x > 0.5f)
        {
            // Right
            currentIndex = (currentIndex + 1) % gridElements.Length;
        }
        else if (Gamepad.current.dpad.left.wasPressedThisFrame && dpadInput.x < -0.5f)
        {
            // Left
            currentIndex = (currentIndex - 1 + gridElements.Length) % gridElements.Length;
        }

        if (Gamepad.current.dpad.up.wasPressedThisFrame && dpadInput.y > 0.5f)
        {
            // Up
            int rowSize = 4; // Assuming 5 elements per row
            currentIndex = (currentIndex - rowSize + gridElements.Length) % gridElements.Length;
        }
        else if (Gamepad.current.dpad.down.wasPressedThisFrame && dpadInput.y < -0.5f)
        {
            // Down
            int rowSize = 4; // Assuming 5 elements per row
            currentIndex = (currentIndex + rowSize) % gridElements.Length;
        }

        EventSystem.current.SetSelectedGameObject(gridElements[currentIndex].gameObject);

        string itemName = gridElements[currentIndex].name;
        string formattedName = itemName.Substring(3).Replace("Panel", "").Trim();

        ITEMdESC.text = playerControl.GetItemDescription(formattedName);
        itemHighlight.transform.position = gridElements[currentIndex].gameObject.transform.position;
    }

    void RefreshGridElements()
    {
        // Assuming your grid elements are direct children of the GridLayoutGroup.
        gridElements = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            gridElements[i] = transform.GetChild(i);
        }
    }

    // Call this method whenever the grid content changes dynamically.
    void OnGridContentChanged()
    {
        RefreshGridElements();
        currentIndex = Mathf.Clamp(currentIndex, 0, gridElements.Length - 1);
        EventSystem.current.SetSelectedGameObject(gridElements[currentIndex].gameObject);
    }
}