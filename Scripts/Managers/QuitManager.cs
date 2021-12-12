using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JungleSphereRemake.Managers
{
    public class QuitManager : Manager<QuitManager>
    {
        // Start is called before the first frame update
        void Start()
        {
            Init(this);
        }

        public void Quit()
        {
            Application.Quit();
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
    }
}