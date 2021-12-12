using System.Collections;
using JungleSphereRemake.Audio;
using JungleSphereRemake.Managers;
using UnityEngine;

namespace JungleSphereRemake.Gameplay
{
    [RequireComponent(typeof(Rigidbody))]
    public class Player : MonoBehaviour
    {
        [Header("Movement")]
        public float MovementSpeed;
        public float JumpHeight;
        public LayerMask GroundLayer;
        Rigidbody Body;
        float X;
        bool IsGrounded;

        [Header("Invincibillity")]
        public float FlashRate;
        float FlashTimer;
        MeshRenderer MeshRenderer;
        public float InvincibellityTime;
        public static bool Invincible;

        [Header("Cube Mode")]
        public bool Cube;
        MeshFilter meshFilter;
        public Mesh SphereMesh;
        public Mesh CubeMesh;
        SphereCollider sphereCollider;
        BoxCollider boxCollider;

        [Header("Other")]
        public GameObject HitParticles;
        public bool Real;

        // Start is called before the first frame update
        void Start()
        {
            Invincible = false;
            Body = GetComponent<Rigidbody>();
            Body.constraints = RigidbodyConstraints.FreezePositionZ;
            MeshRenderer = GetComponent<MeshRenderer>();
            meshFilter = GetComponent<MeshFilter>();
            sphereCollider = GetComponent<SphereCollider>();
            boxCollider = GetComponent<BoxCollider>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Real)
            {
                X = Input.GetAxis("Horizontal") * MovementSpeed;
                IsGrounded = Physics.Raycast(transform.position, Vector3.down, 0.55f, GroundLayer);

                if (Input.GetKeyDown(KeyCode.Space) && IsGrounded)
                {
                    if (GameManager.CanMove)
                    {
                        IsGrounded = false;
                        AudioManager.Instance.InteractWithSFX("Jump", SoundEffectBehaviour.Play);
                        Body.AddForce(0, JumpHeight * 100f, 0);
                    }
                }

                if (Invincible)
                {
                    FlashTimer -= Time.deltaTime;
                    if (FlashTimer <= 0f)
                    {
                        MeshRenderer.enabled = !MeshRenderer.enabled;
                        FlashTimer = FlashRate;
                    }
                }
            }

            if(ProgressManager.IsInstanced)
            {
                Cube = ProgressManager.Instance.Config.CubeMode;
            }
            meshFilter.sharedMesh = (Cube) ? CubeMesh : SphereMesh;
            sphereCollider.enabled = !Cube;
            boxCollider.enabled = Cube;
        }

        void FixedUpdate()
        {
            if (GameManager.CanMove && Real)
            {
                Body.AddForce(X, 0, 0);
            }
        }

        public void SetInvincible()
        {
            if(!Invincible)
            {
                StartCoroutine(setInvincible());
            }
        }
        IEnumerator setInvincible()
        {
            Invincible = true;
            FlashTimer = 0;
            yield return new WaitForSeconds(InvincibellityTime);
            Invincible = false;
            MeshRenderer.enabled = true;
        }

        public void LaunchToTent()
        {
            Body.constraints = RigidbodyConstraints.None;
            Body.velocity = Vector3.zero;
            Body.AddForce(-transform.position.x * 400f, 0, GameManager.Instance.ForwardSpeed * 1000f);
        }

        public void SpawnHitParticles()
        {
            Destroy(Instantiate(HitParticles, transform.position, Quaternion.identity), 5f);
        }
    }
}