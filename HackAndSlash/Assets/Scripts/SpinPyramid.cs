using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class SpinPyramid : MonoBehaviour
{
    [SerializeField] float period0, period1, verticalMoveTime, verticaDistance, pyramidFragRotInSec;
    [SerializeField] Transform pyramid;
    

    


    MenuInputs menuInputs;
    bool spinning = false;
    // Start is called before the first frame update
    
    void Start()
    {       
        Vector3 startPos = this.transform.parent.position;
        this.transform.parent.DOMoveY(startPos.y + verticaDistance, verticalMoveTime).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);

        this.transform.GetChild(0).DOLocalRotate(new Vector3(0, -360, 0) + this.transform.rotation.eulerAngles, period0, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
        this.transform.GetChild(1).DOLocalRotate(new Vector3(0, 360, 0) + this.transform.rotation.eulerAngles, period1, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);

        menuInputs = new MenuInputs();
        menuInputs.UiInputs.SpinPyramidLeft.performed += SpinPyramidLeft;
        menuInputs.UiInputs.SpinPyramidRight.performed += SpinPyramidRight;        
    }

    private void OnDestroy() => menuInputs.Disable();
    
    // Update is called once per frame
    void Update()
    {

    }

    public void StartListening() => menuInputs.Enable();

    public void SpinPyramidLeft(InputAction.CallbackContext context) {
        if (spinning) return; 
        spinning = true;        
        pyramid.DOLocalRotate(new Vector3(0, -90, 0) + this.transform.rotation.eulerAngles, pyramidFragRotInSec, RotateMode.LocalAxisAdd).SetEase(Ease.InOutSine).OnComplete(() => { spinning = false; }); 
    
    }
    public void SpinPyramidRight(InputAction.CallbackContext context) { 
        if (spinning) return; 
        spinning = true;
        pyramid.DOLocalRotate(new Vector3(0, 90, 0) + this.transform.rotation.eulerAngles, pyramidFragRotInSec, RotateMode.LocalAxisAdd).SetEase(Ease.InOutSine).OnComplete(() => { spinning = false; }); 
    
    }   


}
