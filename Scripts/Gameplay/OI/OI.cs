using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JungleSphereRemake.Gameplay.OI
{
    [RequireComponent(typeof(Rigidbody))]
    public class OI : MonoBehaviour
    {
        Rigidbody Body;
        MeshRenderer meshRenderer;
        Material Mat;
        public float Speed;
        public float RotSpeed;

        // Start is called before the first frame update
        void Start()
        {
            Body = GetComponent<Rigidbody>();
            Body.useGravity = false;

            meshRenderer = GetComponent<MeshRenderer>();
            Mat = new Material(meshRenderer.sharedMaterial.shader);
            Mat.CopyPropertiesFromMaterial(meshRenderer.sharedMaterial);
            Mat.name = "oioioioioioioioiioioi";
            Mat.color = new Color(Random.value, Random.value, Random.value, 1f);
            meshRenderer.sharedMaterial = Mat;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            Body.AddForce(Random.insideUnitSphere * Speed);
            Body.AddTorque(Random.insideUnitSphere * (RotSpeed * 100f));
        }
    }
}