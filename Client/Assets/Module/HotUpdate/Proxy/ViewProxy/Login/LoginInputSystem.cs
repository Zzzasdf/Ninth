//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.8.1
//     from Assets/Module/HotUpdate/Proxy/ViewProxy/Login/LoginInputSystem.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine;

namespace Ninth.HotUpdate
{
    public partial class @LoginInputSystem: IInputActionCollection2, IDisposable
    {
        public InputActionAsset asset { get; }
        public @LoginInputSystem()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""LoginInputSystem"",
    ""maps"": [
        {
            ""name"": ""LoginView"",
            ""id"": ""9ae0fde4-2d13-4ce3-a530-1ce677991fdb"",
            ""actions"": [
                {
                    ""name"": ""Any"",
                    ""type"": ""Button"",
                    ""id"": ""f3f6baca-bcc9-4100-9557-21e79afedf57"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""8e5a70f6-d650-4efa-9c5d-33ddd3bc2538"",
                    ""path"": ""*/{Submit}"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Any"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // LoginView
            m_LoginView = asset.FindActionMap("LoginView", throwIfNotFound: true);
            m_LoginView_Any = m_LoginView.FindAction("Any", throwIfNotFound: true);
        }

        ~@LoginInputSystem()
        {
            Debug.Assert(!m_LoginView.enabled, "This will cause a leak and performance issues, LoginInputSystem.LoginView.Disable() has not been called.");
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(asset);
        }

        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enable()
        {
            asset.Enable();
        }

        public void Disable()
        {
            asset.Disable();
        }

        public IEnumerable<InputBinding> bindings => asset.bindings;

        public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
        {
            return asset.FindAction(actionNameOrId, throwIfNotFound);
        }

        public int FindBinding(InputBinding bindingMask, out InputAction action)
        {
            return asset.FindBinding(bindingMask, out action);
        }

        // LoginView
        private readonly InputActionMap m_LoginView;
        private List<ILoginViewActions> m_LoginViewActionsCallbackInterfaces = new List<ILoginViewActions>();
        private readonly InputAction m_LoginView_Any;
        public struct LoginViewActions
        {
            private @LoginInputSystem m_Wrapper;
            public LoginViewActions(@LoginInputSystem wrapper) { m_Wrapper = wrapper; }
            public InputAction @Any => m_Wrapper.m_LoginView_Any;
            public InputActionMap Get() { return m_Wrapper.m_LoginView; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(LoginViewActions set) { return set.Get(); }
            public void AddCallbacks(ILoginViewActions instance)
            {
                if (instance == null || m_Wrapper.m_LoginViewActionsCallbackInterfaces.Contains(instance)) return;
                m_Wrapper.m_LoginViewActionsCallbackInterfaces.Add(instance);
                @Any.started += instance.OnAny;
                @Any.performed += instance.OnAny;
                @Any.canceled += instance.OnAny;
            }

            private void UnregisterCallbacks(ILoginViewActions instance)
            {
                @Any.started -= instance.OnAny;
                @Any.performed -= instance.OnAny;
                @Any.canceled -= instance.OnAny;
            }

            public void RemoveCallbacks(ILoginViewActions instance)
            {
                if (m_Wrapper.m_LoginViewActionsCallbackInterfaces.Remove(instance))
                    UnregisterCallbacks(instance);
            }

            public void SetCallbacks(ILoginViewActions instance)
            {
                foreach (var item in m_Wrapper.m_LoginViewActionsCallbackInterfaces)
                    UnregisterCallbacks(item);
                m_Wrapper.m_LoginViewActionsCallbackInterfaces.Clear();
                AddCallbacks(instance);
            }
        }
        public LoginViewActions @LoginView => new LoginViewActions(this);
        public interface ILoginViewActions
        {
            void OnAny(InputAction.CallbackContext context);
        }
    }
}
