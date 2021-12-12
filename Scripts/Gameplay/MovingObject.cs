using System.Collections;
using JungleSphereRemake.Managers;
using UnityEngine;

namespace JungleSphereRemake.Gameplay
{
    public class MovingObject : MonoBehaviour
    {
        [HideInInspector]
        public float BaseZPos;
        [HideInInspector]
        public float PrevZPos;
        public float SpeedMultiplier;

        void Start()
        {
            BaseZPos = transform.localPosition.z;
            PrevZPos = GameManager.ZPos;
        }

        // Update is called once per frame
        void Update()
        {
            float ZPos = BaseZPos - (GameManager.ZPos - PrevZPos) * SpeedMultiplier;
            transform.localPosition = new Vector3(transform.position.x, transform.position.y, ZPos);

            if (transform.position.z < SpawnManager.Instance.TreeStartPos)
            {
                OnGetsOffScreen();
            }
        }

        protected virtual void OnGetsOffScreen()
        {
            BaseZPos = transform.localPosition.z;
            PrevZPos = GameManager.ZPos;
        }
    }
}