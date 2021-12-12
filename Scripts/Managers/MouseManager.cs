using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JungleSphereRemake.Managers
{
    public class MouseManager : Manager<MouseManager>
    {
        [Space]
        public bool CanHideMouse;
        public bool MouseVisible;

        // Start is called before the first frame update
        void Start()
        {
            Init(this);
        }

        // Update is called once per frame
        void Update()
        {
            if (CanHideMouse)
            {
                Cursor.lockState = (MouseVisible) ? CursorLockMode.None : CursorLockMode.Locked;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
            }
            Cursor.visible = !(Cursor.lockState == CursorLockMode.Locked);
        }
    }
}