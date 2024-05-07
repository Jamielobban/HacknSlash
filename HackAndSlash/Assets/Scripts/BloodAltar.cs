using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodAltar : Interactive
{
    [SerializeField] MeshRenderer materialRender;
    Material bloodMaterial;
    // Start is called before the first frame update
    void Start()
    {
        bloodMaterial = materialRender.material;
        materialRender.material = new Material(bloodMaterial);
        bloodMaterial = materialRender.material;
        CanNotInteract();
        SetMaterialDepth(0);
    }

    public void CanNotInteract() => SetCanInteract(false);
    public void CanInteract() => SetCanInteract(true);

    public void SetMaterialDepth(float fullPerOne) => bloodMaterial.SetFloat("Vector1_D183151F", 4 * fullPerOne);
    public void BloodConsumed() { bloodMaterial.SetFloat("Vector1_D183151F", 0); CanNotInteract(); }       
}
