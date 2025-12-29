#nullable enable
using UnityEngine.UIElements;


public interface ICallbackUI
{
	//Methods
	public abstract void RegisterCallbacks();
	public abstract void UnregisterCallbacks();

	//Automatically called ~1 frame after constructor
	public void OnAttachToPanelICallbackUI(AttachToPanelEvent evt)
	{
		RegisterCallbacks();
	} 
	
	//Automatically called at end of lifespan when removed
	public void OnDetachFromPanelICallbackUI(DetachFromPanelEvent evt)
	{
		UnregisterCallbacks();
	}

	//Call in constructor
	public void RegisterPanelEventsICallbackUI()
	{
		if (this is VisualElement visualElement)
		{
			visualElement.RegisterCallbackOnce<AttachToPanelEvent>(OnAttachToPanelICallbackUI);
			visualElement.RegisterCallbackOnce<DetachFromPanelEvent>(OnDetachFromPanelICallbackUI);
		}
	}


	/*
	Template

	//ICallbackUI
	public void RegisterCallbacks()
	{

	}

	public void UnregisterCallbacks()
	{

	}

	//Register event callbacks, call in constructor
	if (this is ICallbackUI callbackUI) { callbackUI.RegisterPanelEventsICallbackUI(); }
	else { Debug.LogWarning("[ICallbackUI] Tried to register panel events but class " + this.GetType().ToString() + " does not implement ICallbackUI"); }

	*/
}