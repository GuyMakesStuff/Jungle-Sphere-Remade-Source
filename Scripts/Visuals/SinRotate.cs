using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JungleSphereRemake.Visuals
{
    public class SinRotate : MonoBehaviour
    {
        public enum Axis
        {
            X,
            Y,
            Z
        }
        public Axis axis;
        public float SinRange;
        public float SinSpeed;

        // Update is called once per frame
        void Update()
        {
            float Sin = Mathf.Sin(Time.time * SinSpeed) * SinRange;
            switch (axis)
            {
                case Axis.X:
                    transform.localRotation = Quaternion.Euler(Sin, 0, 0);
                    break;
                case Axis.Y:
                    transform.localRotation = Quaternion.Euler(0, Sin, 0);
                    break;
                case Axis.Z:
                    transform.localRotation = Quaternion.Euler(0, 180f, Sin);
                    break;
            }
        }
    }
}