using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBarFillAmount : MonoBehaviour
{
    private Image imageLoading;

    private void OnEnable()
    {
        imageLoading = GetComponent<Image>();
        imageLoading.fillAmount = 0;
    }
}
