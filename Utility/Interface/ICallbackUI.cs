#nullable enable
using UnityEngine.UIElements;


public interface ICallbackUI
{
	public abstract void RegisterCallbacks();
	public abstract void UnregisterCallbacks();


	public void OnAttachToPanelICallbackUI(AttachToPanelEvent evt)
	{
		RegisterCallbacks();
	} 
	
	public void OnDetachFromPanelICallbackUI(DetachFromPanelEvent evt)
	{
		UnregisterCallbacks();
	}

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

	//Initialization
	public void RegisterCallbacks()
	{

	}

	public void UnregisterCallbacks()
	{

	}

	//Register event callbacks
	if (this is ICallbackUI callbackUI) { callbackUI.RegisterPanelEventsICallbackUI(); }
	else { Log.Warning("[ICallbackUI] Tried to register panel events but class " + this.GetType().ToString() + " does not implement ICallbackUI"); }

	*/
}