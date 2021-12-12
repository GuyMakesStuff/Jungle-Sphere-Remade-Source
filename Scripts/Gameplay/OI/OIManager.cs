using JungleSphereRemake.Managers;
using JungleSphereRemake.Audio;
using UnityEngine;

namespace JungleSphereRemake.Gameplay.OI
{
    public class OIManager : Manager<OIManager>
    {
        [Space]
        public GameObject OIPrefab;
        public float SpawnDelay;
        float SpawnTimer;

        // Start is called before the first frame update
        void Start()
        {
            Init(this);
            SpawnTimer = SpawnDelay;
            AudioManager.Instance.InteractWithAllSFX(SoundEffectBehaviour.Stop);
            AudioManager.Instance.SetMusicVolume(20f);
            AudioManager.Instance.SetMusicTrack("oi");
        }

        // Update is called once per frame
        void Update()
        {
            SpawnTimer -= Time.deltaTime;
            if(SpawnTimer <= 0f)
            {
                SpawnTimer = SpawnDelay;
                Instantiate(OIPrefab, transform.position, Random.rotation);
            }
        }
    }
}