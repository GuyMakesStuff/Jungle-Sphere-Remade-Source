using JungleSphereRemake.Gameplay;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine;

namespace JungleSphereRemake.Managers
{
    [RequireComponent(typeof(PostProcessVolume))]
    public class PPManager : Manager<PPManager>
    {
        PostProcessVolume Vol;
        public PostProcessProfile Profile;
        public bool Enabled;

        [Header("Hit Effect")]
        public float BaseIntensity;
        float EndIntensity;
        public float EndIntensityMultiplier;
        public Color HitColor;
        [HideInInspector]
        [Range(0f, 1f)]
        public float HitValue;
        Vignette VignetteEffect;
        bool IsDead;

        [Header("X-Ray Effect")]
        public Color XRayColor;
        ColorGrading colorGrading;
        Grain grain;
        public bool XRayEnabled;

        // Start is called before the first frame update
        void Start()
        {
            Init(this);

            Vol = GetComponent<PostProcessVolume>();
            Vol.profile = Profile;

            VignetteEffect = Profile.GetSetting<Vignette>();
            EndIntensity = BaseIntensity * EndIntensityMultiplier;
            ResetHitFX();

            colorGrading = Profile.GetSetting<ColorGrading>();
            grain = Profile.GetSetting<Grain>();
        }

        // Update is called once per frame
        void Update()
        {
            Vol.enabled = Enabled;

            if (!IsDead)
            {
                if (FindObjectOfType<Player>() != null)
                {
                    HitValue -= Time.deltaTime / FindObjectOfType<Player>().InvincibellityTime;
                }
            }
            else
            {
                HitValue += Time.deltaTime;
            }
            HitValue = Mathf.Clamp01(HitValue);
            VignetteEffect.intensity.value = Mathf.Lerp(BaseIntensity, EndIntensity, HitValue);
            VignetteEffect.color.value = Color.Lerp(Color.black, HitColor, HitValue);

            colorGrading.colorFilter.value = (XRayEnabled) ? XRayColor : Color.white;
            grain.enabled.value = XRayEnabled;
        }

        public void ShowHitFX()
        {
            HitValue = 1f;
        }
        public void ShowDeathFX()
        {
            IsDead = true;
        }
        public void ResetHitFX()
        {
            IsDead = false;
            HitValue = 0f;
        }
    }
}