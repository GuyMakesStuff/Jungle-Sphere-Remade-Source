using JungleSphereRemake.Managers;
using JungleSphereRemake.Gameplay;
using UnityEngine;

namespace JungleSphereRemake.Gameplay.Environmentals
{
    public class Tree : MovingObject
    {
        protected override void OnGetsOffScreen()
        {
            base.OnGetsOffScreen();
            SpawnManager.Instance.SpawnTree(260f);
            Destroy(gameObject);
        }
    }
}