using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TMPOutline : MonoBehaviour
{
    TextMeshPro textmeshPro;
    public float outline;
    public Color32 _color;


    private void Awake()
    {
        textmeshPro = GetComponent<TextMeshPro>();
        textmeshPro.outlineWidth = outline;
        textmeshPro.outlineColor = _color;
    }
}
