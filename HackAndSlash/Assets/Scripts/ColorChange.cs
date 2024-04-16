using System;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange : MonoBehaviour
{
    public List<SkinnedMeshRenderer> skinnedMeshes = new List<SkinnedMeshRenderer>();
    private List<Material> _materials = new List<Material>();

    private void Awake()
    {
        SetSkinnedMaterials();
    }

    public void ChangeToRed()
    {
        Color redColor = new Color(255, 0, 0, 255 * 5.5f);
        foreach (var mat in _materials)
        {
            mat.SetColor("_BaseColor", redColor);
        }
    }

    public void ChangeToWhite()
    {
        Color colorWhite = new Color(255, 255, 255, 255 * 5.5f);
        foreach (var mat in _materials)
        {
            mat.SetColor("_BaseColor", colorWhite);
        }
    }
    
    private void SetSkinnedMaterials()
    {
        if (skinnedMeshes.Count > 0)
        {
            for (int i = 0; i < skinnedMeshes.Count; i++)
            {
                for (int j = 0; j < skinnedMeshes[i].materials.Length; j++)
                {
                    _materials.Add(skinnedMeshes[i].materials[j]);
                }
            }
        }
    }
    
}
