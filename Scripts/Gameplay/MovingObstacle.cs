using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JungleSphereRemake.Gameplay
{
    public class MovingObstacle : Obstacle
    {
        [Space]
        public float MovementRange;
        public float MovementSpeed;
        float T;
        public bool Rotates;
        public float RotationSpeed;
        bool RotDir;
        float RotDirChangeTimer;
        public Transform[] RotatedObjects;

        void Awake()
        {
            T = Random.value;
            RotDirChangeTimer = 1- T;
            RotDir = true;
        }

        // Update is called once per frame
        void LateUpdate()
        {
            T += Time.deltaTime * MovementSpeed;
            float L = Mathf.PingPong(T, 1f);
            float Pos = Mathf.Lerp(-MovementRange, MovementRange, L);
            RotDirChangeTimer -= MovementSpeed * Time.deltaTime;
            if(RotDirChangeTimer <= 0f)
            {
                RotDirChangeTimer = 1f;
                RotDir = !RotDir;
            }
            transform.localPosition = new Vector3(Pos, transform.position.y, transform.position.z);

            if(Rotates)
            {
                float Rot = (RotDir) ? -RotationSpeed : RotationSpeed;
                foreach (Transform RO in RotatedObjects)
                {
                    RO.Rotate(Vector3.forward, (Rot) * Time.deltaTime);
                }
            }
        }
    }
}