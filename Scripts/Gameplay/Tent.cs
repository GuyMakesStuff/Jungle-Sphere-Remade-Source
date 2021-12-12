using JungleSphereRemake.Managers;
using JungleSphereRemake.Audio;
using UnityEngine;

namespace JungleSphereRemake.Gameplay
{
    public class Tent : MovingObject
    {
        [Space]
        public ParticleSystem Confetti;

        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player" && GameManager.CanMove)
            {
                other.GetComponent<Player>().LaunchToTent();
                Confetti.Emit(100);
                AudioManager.Instance.InteractWithSFX("Confetti", SoundEffectBehaviour.Play);
                GameManager.Instance.EndGame();
            }
        }
    }
}