//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.11.2
//     from Assets/Input systems/Joystick Control.inputactions
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

public partial class @JoystickControl: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @JoystickControl()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Joystick Control"",
    ""maps"": [
        {
            ""name"": ""Car Control"",
            ""id"": ""2baab14b-df0f-45c6-881d-c334a1425469"",
            ""actions"": [
                {
                    ""name"": ""Steer"",
                    ""type"": ""Value"",
                    ""id"": ""b0cee55c-5c25-48af-938b-bf9545808acf"",
                    ""expectedControlType"": ""Stick"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Accelerate"",
                    ""type"": ""PassThrough"",
                    ""id"": ""03c74ba8-5b7c-41c8-bf15-863d7aa98c60"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Back"",
                    ""type"": ""PassThrough"",
                    ""id"": ""32c72c8a-0db5-42d5-86da-9a8f87493f05"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SteerTilt"",
                    ""type"": ""Value"",
                    ""id"": ""7cf5fc70-f2ef-4706-b246-ff471956e49d"",
                    ""expectedControlType"": ""Vector3"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Nitro"",
                    ""type"": ""PassThrough"",
                    ""id"": ""22145ad6-7f5a-4d31-90fa-6d84aa8471f5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""013dd096-4a02-4f35-8c3f-097f6fa174c9"",
                    ""path"": ""<XInputController>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Xbox controller"",
                    ""action"": ""Accelerate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7942556e-5b3c-4776-926d-c60ecb6bef73"",
                    ""path"": ""<HID::DragonRise Inc.   Generic   USB  Joystick  >/button2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PS3 Controller"",
                    ""action"": ""Accelerate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3fe86015-bc74-44d0-a03a-f9fb728fcc72"",
                    ""path"": ""<XInputController>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Xbox controller"",
                    ""action"": ""Back"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""aff48dce-0bac-4349-9f8c-902b37090acf"",
                    ""path"": ""<HID::DragonRise Inc.   Generic   USB  Joystick  >/button3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PS3 Controller"",
                    ""action"": ""Back"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""918ce142-c4b8-463e-8d03-f03e871c8a24"",
                    ""path"": ""<Accelerometer>/acceleration"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SteerTilt"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""90ad1135-7b60-431b-a913-fd6f77c4b964"",
                    ""path"": ""<XInputController>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Xbox controller"",
                    ""action"": ""Steer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e89b1784-0d65-4092-b150-68f4ea86e64c"",
                    ""path"": ""<HID::DragonRise Inc.   Generic   USB  Joystick  >/stick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PS3 Controller"",
                    ""action"": ""Steer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""0ebc2e25-0136-4387-8e12-463b070c22ec"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Steer"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""4bc71ab7-1808-4b3b-8d50-2b3458c0e8ab"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Steer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""a578b9ce-6cd9-473a-8831-af6a6004fbc9"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Steer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""bba0527e-dad1-47de-9143-f8eba580ed9c"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Steer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""39ef9565-019c-42eb-a0dd-f499b68b3eb9"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Steer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""3a285d36-873a-43b2-9efb-99416e93a6a9"",
                    ""path"": ""<HID::DragonRise Inc.   Generic   USB  Joystick  >/button6"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Nitro"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Xbox controller"",
            ""bindingGroup"": ""Xbox controller"",
            ""devices"": []
        },
        {
            ""name"": ""PS3 Controller"",
            ""bindingGroup"": ""PS3 Controller"",
            ""devices"": []
        }
    ]
}");
        // Car Control
        m_CarControl = asset.FindActionMap("Car Control", throwIfNotFound: true);
        m_CarControl_Steer = m_CarControl.FindAction("Steer", throwIfNotFound: true);
        m_CarControl_Accelerate = m_CarControl.FindAction("Accelerate", throwIfNotFound: true);
        m_CarControl_Back = m_CarControl.FindAction("Back", throwIfNotFound: true);
        m_CarControl_SteerTilt = m_CarControl.FindAction("SteerTilt", throwIfNotFound: true);
        m_CarControl_Nitro = m_CarControl.FindAction("Nitro", throwIfNotFound: true);
    }

    ~@JoystickControl()
    {
        UnityEngine.Debug.Assert(!m_CarControl.enabled, "This will cause a leak and performance issues, JoystickControl.CarControl.Disable() has not been called.");
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

    // Car Control
    private readonly InputActionMap m_CarControl;
    private List<ICarControlActions> m_CarControlActionsCallbackInterfaces = new List<ICarControlActions>();
    private readonly InputAction m_CarControl_Steer;
    private readonly InputAction m_CarControl_Accelerate;
    private readonly InputAction m_CarControl_Back;
    private readonly InputAction m_CarControl_SteerTilt;
    private readonly InputAction m_CarControl_Nitro;
    public struct CarControlActions
    {
        private @JoystickControl m_Wrapper;
        public CarControlActions(@JoystickControl wrapper) { m_Wrapper = wrapper; }
        public InputAction @Steer => m_Wrapper.m_CarControl_Steer;
        public InputAction @Accelerate => m_Wrapper.m_CarControl_Accelerate;
        public InputAction @Back => m_Wrapper.m_CarControl_Back;
        public InputAction @SteerTilt => m_Wrapper.m_CarControl_SteerTilt;
        public InputAction @Nitro => m_Wrapper.m_CarControl_Nitro;
        public InputActionMap Get() { return m_Wrapper.m_CarControl; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CarControlActions set) { return set.Get(); }
        public void AddCallbacks(ICarControlActions instance)
        {
            if (instance == null || m_Wrapper.m_CarControlActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_CarControlActionsCallbackInterfaces.Add(instance);
            @Steer.started += instance.OnSteer;
            @Steer.performed += instance.OnSteer;
            @Steer.canceled += instance.OnSteer;
            @Accelerate.started += instance.OnAccelerate;
            @Accelerate.performed += instance.OnAccelerate;
            @Accelerate.canceled += instance.OnAccelerate;
            @Back.started += instance.OnBack;
            @Back.performed += instance.OnBack;
            @Back.canceled += instance.OnBack;
            @SteerTilt.started += instance.OnSteerTilt;
            @SteerTilt.performed += instance.OnSteerTilt;
            @SteerTilt.canceled += instance.OnSteerTilt;
            @Nitro.started += instance.OnNitro;
            @Nitro.performed += instance.OnNitro;
            @Nitro.canceled += instance.OnNitro;
        }

        private void UnregisterCallbacks(ICarControlActions instance)
        {
            @Steer.started -= instance.OnSteer;
            @Steer.performed -= instance.OnSteer;
            @Steer.canceled -= instance.OnSteer;
            @Accelerate.started -= instance.OnAccelerate;
            @Accelerate.performed -= instance.OnAccelerate;
            @Accelerate.canceled -= instance.OnAccelerate;
            @Back.started -= instance.OnBack;
            @Back.performed -= instance.OnBack;
            @Back.canceled -= instance.OnBack;
            @SteerTilt.started -= instance.OnSteerTilt;
            @SteerTilt.performed -= instance.OnSteerTilt;
            @SteerTilt.canceled -= instance.OnSteerTilt;
            @Nitro.started -= instance.OnNitro;
            @Nitro.performed -= instance.OnNitro;
            @Nitro.canceled -= instance.OnNitro;
        }

        public void RemoveCallbacks(ICarControlActions instance)
        {
            if (m_Wrapper.m_CarControlActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(ICarControlActions instance)
        {
            foreach (var item in m_Wrapper.m_CarControlActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_CarControlActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public CarControlActions @CarControl => new CarControlActions(this);
    private int m_XboxcontrollerSchemeIndex = -1;
    public InputControlScheme XboxcontrollerScheme
    {
        get
        {
            if (m_XboxcontrollerSchemeIndex == -1) m_XboxcontrollerSchemeIndex = asset.FindControlSchemeIndex("Xbox controller");
            return asset.controlSchemes[m_XboxcontrollerSchemeIndex];
        }
    }
    private int m_PS3ControllerSchemeIndex = -1;
    public InputControlScheme PS3ControllerScheme
    {
        get
        {
            if (m_PS3ControllerSchemeIndex == -1) m_PS3ControllerSchemeIndex = asset.FindControlSchemeIndex("PS3 Controller");
            return asset.controlSchemes[m_PS3ControllerSchemeIndex];
        }
    }
    public interface ICarControlActions
    {
        void OnSteer(InputAction.CallbackContext context);
        void OnAccelerate(InputAction.CallbackContext context);
        void OnBack(InputAction.CallbackContext context);
        void OnSteerTilt(InputAction.CallbackContext context);
        void OnNitro(InputAction.CallbackContext context);
    }
}
