using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania
{
    public class PlayerInput : MonoBehaviour
    {

        // Update is called once per frame
        void Update()
        {
            DashPressed();
            SprintHeld();
            JumpPressed();
            JumpHeld();

        }

        public virtual bool CrouchHeld()
        {
            if (Input.GetButton("Crouch"))
            {
                return true;
            }

            return false;
        }

        public virtual bool DashPressed()
        {
            if (Input.GetButtonDown("Dash"))
            {
                return true;
            }
            else
            { return false; }
        }
        public virtual bool SprintHeld()
        {
            if (Input.GetButton("Run"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual bool JumpPressed()
        {
            if (Input.GetButtonDown("Jump"))
            {
                return true;
            }

            else
                return false;
        }

        public virtual bool JumpHeld()
        {
            if (Input.GetButton("Jump"))
            {
                return true;
            }

            else
                return false;
        }

    }
}
