using System.Collections;
using JungleSphereRemake.Managers;
using UnityEngine;

namespace JungleSphereRemake.Gameplay
{
    public class Obstacle : MovingObject
    {
        protected override void OnGetsOffScreen()
        {
            base.OnGetsOffScreen();
            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player")
            {
                GameManager.Instance.Damage(other.GetComponent<Player>());
            }
        }
    }
}