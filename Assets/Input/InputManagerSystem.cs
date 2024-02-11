using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class InputManagerSystem :  MonoBehaviour
{
	DefaultInput input => PlayerManager.instance.input;

	public string controlSchemeOverride = null;
	bool autoScheme => controlSchemeOverride == null;
	public List<InputDevice> devices = new List<InputDevice>();

	public Vector2 move = Vector2.zero;
	public Vector2 aim = Vector2.zero;
	public bool mouseAim = false;
	public bool jump = false;
	public bool upgrade = false;

	InputControlScheme currentScheme;

	public void Init(DefaultInput input)
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

	public void UpdateScheme(InputAction.CallbackContext context)
    {
		if (autoScheme)
		{
			var bindingGroups = context.action.GetBindingForControl(context.control).Value.groups;

			foreach (var scheme in input.controlSchemes)
			{
				if (bindingGroups.Contains(scheme.bindingGroup))
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
	}

	protected void OnDestroy()
	{
	}
}
