using JungleSphereRemake.IO;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace JungleSphereRemake.Managers
{
    public class ProgressManager : Manager<ProgressManager>
    {
        [System.Serializable]
        public class Difficulty
        {
            public string Name;
            public float ScoreGoal;
            public int Lives;
        }
        public Difficulty[] DifficultyModes;

        public class GameConfig
        {
            public int DiffIndex;
            public bool Endless;
            public bool CubeMode;
        }
        public GameConfig Config;

        [System.Serializable]
        public class Progress : SaveFile
        {
            [Space]
            public float HIScore;
            public int LevelAt;
        }
        public Progress progress;

        // Start is called before the first frame update
        void Start()
        {
            Init(this);
            Progress Loaded = Saver.Load(progress) as Progress;
            if(Loaded != null) { progress = Loaded; }
            Config = new GameConfig();
            SceneManager.LoadSceneAsync("Menu");
        }

        // Update is called once per frame
        void Update()
        {
            progress.Save();
        }
    }
}