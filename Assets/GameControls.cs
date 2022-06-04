//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/GameControls.inputactions
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

public partial class @GameControls : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @GameControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""GameControls"",
    ""maps"": [
        {
            ""name"": ""GameControl"",
            ""id"": ""c81a5427-af84-4b13-88ae-6a131e12d8f8"",
            ""actions"": [
                {
                    ""name"": ""Attack"",
                    ""type"": ""Value"",
                    ""id"": ""6fab4e8e-403d-4828-82e5-a860aeceb045"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""0340906f-fd2b-4e9c-9d44-ec30cb289000"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""ecaa84ab-288b-4841-a180-25a82fe3a1aa"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyBoard"",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c450768a-3850-446c-b5f5-ccfd72aeedb6"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyBoard"",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f50f9728-6bfd-43ed-a812-be3467c6a438"",
                    ""path"": ""<XInputController>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": ""AxisDeadzone"",
                    ""groups"": ""xBox"",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cc9d7d65-9bab-440f-99c8-1d1ef7d57f2f"",
                    ""path"": ""<HID::USB Gamepad >/button2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""SuilyGamePad"",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""bfa990cb-fc13-41d4-b9ea-c21543fbb21d"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""815e1396-3680-40d0-b79a-eb09b4278c01"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyBoard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""43812b93-30af-4e84-a699-14c33eb9955a"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyBoard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""b40f0586-f5a4-4ab1-bfd5-3bafdc2fcfb9"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyBoard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""43272eb4-529c-46ed-a7a6-04f6a295ec97"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyBoard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""1881bd49-caf1-48da-a1f9-9dbc7f0543e0"",
                    ""path"": ""<XInputController>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""xBox"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""3eceff4f-cb27-4b0a-af1c-438418ea154a"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""d3726a16-42b4-4806-9c56-a6d4c40378d9"",
                    ""path"": ""<HID::USB Gamepad >/stick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""SuilyGamePad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""f3d44114-e876-4802-8049-d5e9240b5ee4"",
                    ""path"": ""<HID::USB Gamepad >/stick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""SuilyGamePad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""fc7b86e3-e22e-482d-b7eb-4188f6e7c1e7"",
                    ""path"": ""<HID::USB Gamepad >/stick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""SuilyGamePad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""f57f14cf-4f2c-40f4-a9e3-5e6beb3927a3"",
                    ""path"": ""<HID::USB Gamepad >/stick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""SuilyGamePad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""KeyBoard"",
            ""bindingGroup"": ""KeyBoard"",
            ""devices"": []
        },
        {
            ""name"": ""xBox"",
            ""bindingGroup"": ""xBox"",
            ""devices"": []
        },
        {
            ""name"": ""SuilyGamePad"",
            ""bindingGroup"": ""SuilyGamePad"",
            ""devices"": []
        }
    ]
}");
        // GameControl
        m_GameControl = asset.FindActionMap("GameControl", throwIfNotFound: true);
        m_GameControl_Attack = m_GameControl.FindAction("Attack", throwIfNotFound: true);
        m_GameControl_Movement = m_GameControl.FindAction("Movement", throwIfNotFound: true);
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

    // GameControl
    private readonly InputActionMap m_GameControl;
    private IGameControlActions m_GameControlActionsCallbackInterface;
    private readonly InputAction m_GameControl_Attack;
    private readonly InputAction m_GameControl_Movement;
    public struct GameControlActions
    {
        private @GameControls m_Wrapper;
        public GameControlActions(@GameControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Attack => m_Wrapper.m_GameControl_Attack;
        public InputAction @Movement => m_Wrapper.m_GameControl_Movement;
        public InputActionMap Get() { return m_Wrapper.m_GameControl; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameControlActions set) { return set.Get(); }
        public void SetCallbacks(IGameControlActions instance)
        {
            if (m_Wrapper.m_GameControlActionsCallbackInterface != null)
            {
                @Attack.started -= m_Wrapper.m_GameControlActionsCallbackInterface.OnAttack;
                @Attack.performed -= m_Wrapper.m_GameControlActionsCallbackInterface.OnAttack;
                @Attack.canceled -= m_Wrapper.m_GameControlActionsCallbackInterface.OnAttack;
                @Movement.started -= m_Wrapper.m_GameControlActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_GameControlActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_GameControlActionsCallbackInterface.OnMovement;
            }
            m_Wrapper.m_GameControlActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Attack.started += instance.OnAttack;
                @Attack.performed += instance.OnAttack;
                @Attack.canceled += instance.OnAttack;
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
            }
        }
    }
    public GameControlActions @GameControl => new GameControlActions(this);
    private int m_KeyBoardSchemeIndex = -1;
    public InputControlScheme KeyBoardScheme
    {
        get
        {
            if (m_KeyBoardSchemeIndex == -1) m_KeyBoardSchemeIndex = asset.FindControlSchemeIndex("KeyBoard");
            return asset.controlSchemes[m_KeyBoardSchemeIndex];
        }
    }
    private int m_xBoxSchemeIndex = -1;
    public InputControlScheme xBoxScheme
    {
        get
        {
            if (m_xBoxSchemeIndex == -1) m_xBoxSchemeIndex = asset.FindControlSchemeIndex("xBox");
            return asset.controlSchemes[m_xBoxSchemeIndex];
        }
    }
    private int m_SuilyGamePadSchemeIndex = -1;
    public InputControlScheme SuilyGamePadScheme
    {
        get
        {
            if (m_SuilyGamePadSchemeIndex == -1) m_SuilyGamePadSchemeIndex = asset.FindControlSchemeIndex("SuilyGamePad");
            return asset.controlSchemes[m_SuilyGamePadSchemeIndex];
        }
    }
    public interface IGameControlActions
    {
        void OnAttack(InputAction.CallbackContext context);
        void OnMovement(InputAction.CallbackContext context);
    }
}