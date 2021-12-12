using UnityEngine.SceneManagement;
using System.Collections.Generic;
using JungleSphereRemake.Audio;
using JungleSphereRemake.IO;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace JungleSphereRemake.Managers
{
    public class MenuManager : Manager<MenuManager>
    {
        [Header("Start")]
        public TMP_Dropdown DiffDropdown;
        public Toggle EndlessToggle;

        [Header("Settings")]
        public Slider MusicSlider;
        public Slider SFXSlider;
        public TMP_Dropdown QualDropdown;
        public TMP_Dropdown ResDropdown;
        Resolution[] Resolutions;
        public Toggle FSToggle;
        public Toggle PPToggle;
        public Toggle MLToggle;
        public Toggle CubeToggle;
        [System.Serializable]
        public class Settings : SaveFile
        {
            [Space]
            public float MusicVol;
            public float SFXVol;
            public int QualLevel;
            public int ResIndex;
            public bool FS;
            public bool PP;
            public bool ML;
            public bool IsCube;
        }
        public Settings settings;

        void Start()
        {
            Init(this);

            AudioManager.Instance.SetMusicTrack("Menu");

            MouseManager.Instance.MouseVisible = true;

            DiffDropdown.ClearOptions();
            List<string> Options = new List<string>();
            for (int D = 0; D < 3; D++)
            {
                if(D <= ProgressManager.Instance.progress.LevelAt)
                {
                    Options.Add(ProgressManager.Instance.DifficultyModes[D].Name);
                }
            }
            DiffDropdown.AddOptions(Options);
            EndlessToggle.gameObject.SetActive(ProgressManager.Instance.progress.LevelAt == 3);

            Settings LoadedSettings = Saver.Load(settings) as Settings;
            int CurResIndex = 0;
            InitRes(out CurResIndex);
            if(LoadedSettings != null)
            {
                settings = LoadedSettings;
            }
            else
            {
                settings.MusicVol = 0f;
                settings.SFXVol = 0f;
                settings.QualLevel = QualitySettings.GetQualityLevel();
                settings.ResIndex = CurResIndex;
                settings.FS = Screen.fullScreen;
                settings.PP = PPManager.Instance.Enabled;
                settings.ML = MouseManager.Instance.CanHideMouse;
                settings.IsCube = false;
            }
            MusicSlider.value = settings.MusicVol;
            SFXSlider.value = settings.SFXVol;
            QualDropdown.value = settings.QualLevel;
            ResDropdown.value = settings.ResIndex;
            FSToggle.isOn = settings.FS;
            PPToggle.isOn = settings.PP;
            MLToggle.isOn = settings.ML;
            CubeToggle.isOn = settings.IsCube;
        }
        void InitRes(out int CurResIndex)
        {
            ResDropdown.onValueChanged.AddListener(new UnityAction<int>(UpdateRes));
            FSToggle.onValueChanged.AddListener(new UnityAction<bool>(UpdateFS));

            Resolutions = Screen.resolutions;
            ResDropdown.ClearOptions();
            List<string> Res2String = new List<string>();
            int curResIndex = 0;
            Resolution CurRes = Screen.currentResolution;
            for (int R = 0; R < Resolutions.Length; R++)
            {
                Resolution Res = Resolutions[R];
                string String = Res.width + "x" + Res.height;
                Res2String.Add(String);
                if(Res.width == CurRes.width && Res.height == CurRes.height)
                {
                    curResIndex = R;
                }
            }
            ResDropdown.AddOptions(Res2String);
            CurResIndex = curResIndex;
        }

        void Update()
        {
            ProgressManager.Instance.Config.DiffIndex = DiffDropdown.value;
            ProgressManager.Instance.Config.Endless = EndlessToggle.isOn;

            AudioManager.Instance.SetMusicVolume(MusicSlider.value);
            settings.MusicVol = MusicSlider.value;
            AudioManager.Instance.SetSFXVolume(SFXSlider.value);
            settings.SFXVol = SFXSlider.value;
            QualitySettings.SetQualityLevel(QualDropdown.value);
            settings.QualLevel = QualDropdown.value;
            settings.ResIndex = ResDropdown.value;
            settings.FS = FSToggle.isOn;
            PPManager.Instance.Enabled = PPToggle.isOn;
            settings.PP = PPToggle.isOn;
            MouseManager.Instance.CanHideMouse = MLToggle.isOn;
            settings.ML = MLToggle.isOn;
            ProgressManager.Instance.Config.CubeMode = CubeToggle.isOn;
            settings.IsCube = CubeToggle.isOn;
            settings.Save();
        }

        void UpdateRes(int Value)
        {
            UpdateScreen();
        }
        void UpdateFS(bool Value)
        {
            UpdateScreen();
        }
        void UpdateScreen()
        {
            Resolution Res = Resolutions[ResDropdown.value];
            Screen.SetResolution(Res.width, Res.height, FSToggle.isOn);
        }

        public void ResetProgress()
        {
            ProgressManager.Instance.progress.HIScore = 0f;
            ProgressManager.Instance.progress.LevelAt = 0;
            AudioManager.Instance.InteractWithSFX("Reset Save", SoundEffectBehaviour.Play);
            LoadScene(SceneManager.GetActiveScene().name);
        }

        public void LoadScene(string SceneName)
        {
            SceneManager.LoadSceneAsync(SceneName);
        }
        public void QuitGame()
        {
            QuitManager.Instance.Quit();
        }
    }
}