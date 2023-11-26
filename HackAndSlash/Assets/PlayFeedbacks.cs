using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayFeedbacks : MonoBehaviour
{
    private MMF_Player thisFeedback;
    public bool shouldParent;
    void Start()
    { 
        thisFeedback = this.GetComponent<MMF_Player>();
        thisFeedback.PlayFeedbacks();
            if (shouldParent)
            {
                MMF_MMSoundManagerSound soundFeedback = thisFeedback.GetFeedbackOfType<MMF_MMSoundManagerSound>();
                soundFeedback.AttachToTransform = this.transform.parent;
            }
    }
}
