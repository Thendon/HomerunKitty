using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class InputManagerSystem :  MonoBehaviour, DefaultInput.ICharacterActions
{
	DefaultInput input;

	public string controlSchemeOverride = null;
	bool autoScheme => controlSchemeOverride == null;
	public List<InputDevice> devices = new List<InputDevice>();

	public Vector2 move { private set; get; } = Vector2.zero;
	public Vector2 aim { private set; get; } = Vector2.zero;
	public bool mouseAim => currentScheme == input.KeyboardAndMouseScheme;
	public bool jump { private set; get; } = false;

	InputControlScheme currentScheme;

	public void Init()
	{
		if (!autoScheme)
		{
			foreach (var scheme in input.controlSchemes)
			{
				if (scheme.name == controlSchemeOverride)
				{
					currentScheme = scheme;
					break;
				}
			}
		}
	}

	public void AddDevice(InputDevice device)
    {
		devices.Add(device);
    }

	protected void Awake()
	{
		input = new DefaultInput();
		input.Character.SetCallbacks(this);
		Init();
	}

	protected void OnDestroy()
	{
	}

	bool isMyDevice(InputDevice device)
    {
		//use all devices
		if (devices.Count < 0)
			return true;

		return devices.Contains(device);
    }

	void DefaultInput.ICharacterActions.OnMove(InputAction.CallbackContext context) //=> move = context.ReadValue<Vector2>();
	{
		if (isMyDevice(context.control.device))
			return;

		var bindingGroups = context.action.GetBindingForControl(context.control).Value.groups;
		
		if (autoScheme)
        {
			foreach (var scheme in input.controlSchemes)
			{
				if (bindingGroups.Contains(scheme.bindingGroup))
				{
					currentScheme = scheme;
					break;
				}
			}
		}

		move = context.ReadValue<Vector2>();
	}
	void DefaultInput.ICharacterActions.OnAim(InputAction.CallbackContext context)
	{
		if (isMyDevice(context.control.device))
			return;
		aim = context.ReadValue<Vector2>();
	}
	void DefaultInput.ICharacterActions.OnLook(InputAction.CallbackContext context)
	{
		if (isMyDevice(context.control.device))
			return;
		aim = context.ReadValue<Vector2>();
	}

	public void OnJump(InputAction.CallbackContext context)
	{
		if (isMyDevice(context.control.device))
			return;
		jump = context.ReadValue<bool>();
	}

	protected void OnEnable()
	{
		input.Enable();
	}

	protected void OnDisable()
	{
		input.Disable();
	}
}
