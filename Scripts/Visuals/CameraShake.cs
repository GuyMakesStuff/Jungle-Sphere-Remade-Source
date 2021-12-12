using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JungleSphereRemake.Visuals
{
    [RequireComponent(typeof(Camera))]
    public class CameraShake : MonoBehaviour
    {
        public float ShakeDrainTime;
        public float ShakeRangeMultiplier;
        float ShakeIntensity;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            float MinMaxRot = ShakeIntensity * ShakeRangeMultiplier;
            float Rot = Random.Range(-MinMaxRot, MinMaxRot);
            transform.localRotation = Quaternion.Euler(5f, transform.localRotation.y, Rot);

            if (ShakeIntensity > 0f)
            {
                ShakeIntensity -= Time.deltaTime * ShakeDrainTime;
            }
        }

        public void Shake(float Amount)
        {
            ShakeIntensity = Amount;
        }
        public void Freeze()
        {
            ShakeIntensity = 0f;
        }
    }
}