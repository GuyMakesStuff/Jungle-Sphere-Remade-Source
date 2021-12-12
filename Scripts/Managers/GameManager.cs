using JungleSphereRemake.Gameplay;
using TMPro;
using UnityEngine.SceneManagement;
using JungleSphereRemake.Visuals;
using JungleSphereRemake.Audio;
using UnityEngine;

namespace JungleSphereRemake.Managers
{
    public class GameManager : Manager<GameManager>
    {
        CameraShake cameraShake;
        [Header("Difficulty")]
        public float ForwardSpeed;
        public float MaxForwardSpeed;
        public float DiffIncreaseAmount;
        public static float ZPos;
        public float TrackEnd;
        public bool IsEndless;
        public GameObject TentPrefab;
        public static bool TentSpawned;
        public static bool CanMove;

        [Header("UI")]
        public TMP_Text ScoreCounter;
        public TMP_Text HIScoreCounter;
        [HideInInspector]
        public float HIScore;
        public TMP_Text HealthCounter;
        bool HIBeat;
        public GameObject NewHIText;
        public int Health;

        [Header("Mini Menus")]
        public GameObject PauseMenu;
        public GameObject GameOverMenu;
        public GameObject WinMenu;
        public TMP_Text WinMessage;
        public static bool IsPaused;
        public static bool CanPause;

        // Start is called before the first frame update
        void Start()
        {
            Init(this);
            ZPos = 0f;
            cameraShake = FindObjectOfType<CameraShake>();
            CanMove = true;
            CanPause = true;
            TentSpawned = false;
            HIScore = ProgressManager.Instance.progress.HIScore;
            AudioManager.Instance.SetMusicTrack("Main");

            if (ProgressManager.IsInstanced)
            {
                IsEndless = ProgressManager.Instance.Config.Endless;
                ProgressManager.Difficulty difficulty = ProgressManager.Instance.DifficultyModes[ProgressManager.Instance.Config.DiffIndex];
                TrackEnd = difficulty.ScoreGoal;
                Health = difficulty.Lives;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if(ForwardSpeed < MaxForwardSpeed)
            {
                ForwardSpeed += DiffIncreaseAmount * Time.deltaTime;
            }

            if (CanMove)
            {
                ZPos += ForwardSpeed * Time.deltaTime;
            }

            if(ZPos > HIScore)
            {
                HIScore = ZPos;
                if(!HIBeat)
                {
                    HIBeat = true;
                    AudioManager.Instance.InteractWithSFX("New HI", SoundEffectBehaviour.Play);
                    NewHIText.SetActive(true);
                }
            }
            ScoreCounter.text = "Score:" + ZPos.ToString("000");
            HIScoreCounter.text = "High Score:" + HIScore.ToString("000");
            HealthCounter.text = "Health:" + Health.ToString();

            if(ZPos >= TrackEnd && !IsEndless)
            {
                if(!TentSpawned)
                {
                    TentSpawned = true;
                    Instantiate(TentPrefab, new Vector3(0, 7, 260f + -SpawnManager.Instance.TreeStartPos), TentPrefab.transform.rotation);
                }
            }

            if(Input.GetKeyDown(KeyCode.Escape) && CanPause)
            {
                IsPaused = !IsPaused;
                PlaySelectSound();
            }
            Time.timeScale = (!IsPaused) ? 1f : 0f;
            PauseMenu.SetActive(IsPaused);
            MouseManager.Instance.MouseVisible = (MouseManager.Instance.CanHideMouse && (IsPaused || !CanPause));
            AudioManager.Instance.InteractWithAllSFX((IsPaused) ? SoundEffectBehaviour.Pause : SoundEffectBehaviour.Resume);
            AudioManager.Instance.InteractWithMusic((IsPaused) ? SoundEffectBehaviour.Pause : SoundEffectBehaviour.Resume);

            ProgressManager.Instance.progress.HIScore = HIScore;
        }

        public void Damage(Player player)
        {
            if(!Player.Invincible)
            {
                Health--;
                player.SpawnHitParticles();
                if(Health == 0)
                {
                    Destroy(player.gameObject);
                    CanMove = false;
                    CanPause = false;
                    cameraShake.Shake(10f);
                    PPManager.Instance.ShowDeathFX();
                    GameOverMenu.SetActive(true);
                    AudioManager.Instance.MuteMusic();
                    AudioManager.Instance.InteractWithSFX("Die", SoundEffectBehaviour.Play);
                }
                else
                {
                    player.SetInvincible();
                    cameraShake.Shake(5f);
                    PPManager.Instance.ShowHitFX();
                    AudioManager.Instance.InteractWithSFX("Hit", SoundEffectBehaviour.Play);
                }
            }
        }
        
        public void EndGame()
        {
            CanMove = false;
            CanPause = false;
            cameraShake.Freeze();
            AudioManager.Instance.InteractWithSFX("Win", SoundEffectBehaviour.Play);
            WinMenu.SetActive(true);
            int NewLevel = ProgressManager.Instance.progress.LevelAt + 1;
            if(ProgressManager.Instance.progress.LevelAt < NewLevel && NewLevel < 4f)
            {
                string NewMode = (NewLevel != 3) ? ProgressManager.Instance.DifficultyModes[NewLevel].Name : "Endless";
                WinMessage.text = "Unlocked " + NewMode + " Mode!";
                ProgressManager.Instance.progress.LevelAt = NewLevel;
            }
            else
            {
                WinMessage.text = "";
            }
        }

        public void Menu()
        {
            IsPaused = false;
            PPManager.Instance.ResetHitFX();
            SceneManager.LoadSceneAsync("Menu");
        }
        public void Retry()
        {
            IsPaused = false;
            PPManager.Instance.ResetHitFX();
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        }
        public void QuitGame()
        {
            QuitManager.Instance.Quit();
        }
    }
}