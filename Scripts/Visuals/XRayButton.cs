using JungleSphereRemake.Managers;
using JungleSphereRemake.Audio;
using UnityEngine;

namespace JungleSphereRemake.Visuals
{
    public class XRayButton : MonoBehaviour
    {
        private void OnMouseDown()
        {
            AudioManager.Instance.InteractWithSFX("Switch", SoundEffectBehaviour.Play);
            XRayManager.Instance.XRay = !XRayManager.Instance.XRay;
        }
    }
}