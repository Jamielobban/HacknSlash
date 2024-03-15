using MoreMountains.Feedbacks;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Hit Data", menuName = "Enemies/Hits")]
public class OnHitEffect : ScriptableObject
{
    public string animationHit;
    public AnimationCurve animationCurve;
    public MMFeedbacks feedbackHit;
    public float delayHit;
    //public string animationMid;
    //public AnimationCurve animCurveMid;
    //public MMFeedback feedbackMid;
    //public float delayMid;
    //public string animationStrong;
    //public AnimationCurve animvCurveStrong;
    //public MMFeedback feedbackStrong;
    //public float delayStrong;


}
