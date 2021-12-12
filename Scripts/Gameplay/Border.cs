using System.Collections;
using JungleSphereRemake.Managers;
using UnityEngine;

namespace JungleSphereRemake.Gameplay
{
    public class Border : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            if (other.collider.tag == "Player")
            {
                GameManager.Instance.Damage(other.collider.GetComponent<Player>());
            }
        }
    }
}