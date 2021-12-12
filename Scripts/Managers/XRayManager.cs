using UnityEngine.EventSystems;
using JungleSphereRemake.Audio;
using UnityEngine;

namespace JungleSphereRemake.Managers
{
    public class XRayManager : Manager<XRayManager>
    {
        [Space]
        public bool XRay;
        public GameObject XRayOverlay;
        public GameObject[] XRayObjects;
        public EventSystem eventSystem;

        // Start is called before the first frame update
        void Start()
        {
            Init(this);
        }

        // Update is called once per frame
        void Update()
        {
            PPManager.Instance.XRayEnabled = XRay;
            foreach (GameObject XROBJ in XRayObjects)
            {
                XROBJ.SetActive(XRay);
            }
            XRayOverlay.SetActive(!PPManager.Instance.Enabled && XRay);
            AudioManager.Instance.InteractWithSFXOneShot("X-Ray", (XRay) ? SoundEffectBehaviour.Play : SoundEffectBehaviour.Stop);
            AudioManager.Instance.InteractWithMusic((XRay) ? SoundEffectBehaviour.Pause : SoundEffectBehaviour.Resume);
            eventSystem.enabled = !XRay;
        }
    }
}