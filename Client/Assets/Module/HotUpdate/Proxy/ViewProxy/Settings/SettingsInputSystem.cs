//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.8.1
//     from Assets/Module/HotUpdate/Proxy/ViewProxy/Settings/SettingsInputSystem.inputactions
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
    public partial class @SettingsInputSystem: IInputActionCollection2, IDisposable
    {
        public InputActionAsset asset { get; }
        public @SettingsInputSystem()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""SettingsInputSystem"",
    ""maps"": [
        {
            ""name"": ""Menu"",
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
                },
                {
                    ""name"": ""Back"",
                    ""type"": ""Button"",
                    ""id"": ""54e6724b-0b41-4c75-8da9-1f34d1cb7686"",
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
                },
                {
                    ""name"": """",
                    ""id"": ""003af6a0-1362-4678-9c48-a732eb8cb61a"",
                    ""path"": ""*/{Back}"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Back"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // Menu
            m_Menu = asset.FindActionMap("Menu", throwIfNotFound: true);
            m_Menu_Any = m_Menu.FindAction("Any", throwIfNotFound: true);
            m_Menu_Back = m_Menu.FindAction("Back", throwIfNotFound: true);
        }

        ~@SettingsInputSystem()
        {
            Debug.Assert(!m_Menu.enabled, "This will cause a leak and performance issues, SettingsInputSystem.Menu.Disable() has not been called.");
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

        // Menu
        private readonly InputActionMap m_Menu;
        private List<IMenuActions> m_MenuActionsCallbackInterfaces = new List<IMenuActions>();
        private readonly InputAction m_Menu_Any;
        private readonly InputAction m_Menu_Back;
        public struct MenuActions
        {
            private @SettingsInputSystem m_Wrapper;
            public MenuActions(@SettingsInputSystem wrapper) { m_Wrapper = wrapper; }
            public InputAction @Any => m_Wrapper.m_Menu_Any;
            public InputAction @Back => m_Wrapper.m_Menu_Back;
            public InputActionMap Get() { return m_Wrapper.m_Menu; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(MenuActions set) { return set.Get(); }
            public void AddCallbacks(IMenuActions instance)
            {
                if (instance == null || m_Wrapper.m_MenuActionsCallbackInterfaces.Contains(instance)) return;
                m_Wrapper.m_MenuActionsCallbackInterfaces.Add(instance);
                @Any.started += instance.OnAny;
                @Any.performed += instance.OnAny;
                @Any.canceled += instance.OnAny;
                @Back.started += instance.OnBack;
                @Back.performed += instance.OnBack;
                @Back.canceled += instance.OnBack;
            }

            private void UnregisterCallbacks(IMenuActions instance)
            {
                @Any.started -= instance.OnAny;
                @Any.performed -= instance.OnAny;
                @Any.canceled -= instance.OnAny;
                @Back.started -= instance.OnBack;
                @Back.performed -= instance.OnBack;
                @Back.canceled -= instance.OnBack;
            }

            public void RemoveCallbacks(IMenuActions instance)
            {
                if (m_Wrapper.m_MenuActionsCallbackInterfaces.Remove(instance))
                    UnregisterCallbacks(instance);
            }

            public void SetCallbacks(IMenuActions instance)
            {
                foreach (var item in m_Wrapper.m_MenuActionsCallbackInterfaces)
                    UnregisterCallbacks(item);
                m_Wrapper.m_MenuActionsCallbackInterfaces.Clear();
                AddCallbacks(instance);
            }
        }
        public MenuActions @Menu => new MenuActions(this);
        public interface IMenuActions
        {
            void OnAny(InputAction.CallbackContext context);
            void OnBack(InputAction.CallbackContext context);
        }
    }
}
