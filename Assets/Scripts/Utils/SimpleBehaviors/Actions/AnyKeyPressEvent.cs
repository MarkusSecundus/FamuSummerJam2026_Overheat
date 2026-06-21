using MarkusSecundus.Utils.Extensions;
using MarkusSecundus.Utils.Input;
using MarkusSecundus.Utils.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace MarkusSecundus.Utils.Behaviors.Actions
{
    public class AnyKeyPressEvent : MonoBehaviour
    {
        public KeyInputSource InputSource;

        public UnityEvent OnAnyKeyDown;

        void Update()
        {
            if (InputSource)
            {
                if (InputSource.IsAnyKeyDown) OnAnyKeyDown?.Invoke();
            }
            else
            {
                if (Keyboard.current.IsNotNil() && Keyboard.current.anyKey.wasPressedThisFrame) OnAnyKeyDown?.Invoke();
                //if (UnityEngine.Input.anyKeyDown) OnAnyKeyDown?.Invoke();
            }
        }
    }
}
