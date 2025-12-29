#nullable enable
using UnityEngine.UIElements;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;


public interface IAutoUpdatingLocalizedUI
{
	//Methods
	public abstract void OnSelectedLocaleChanged(Locale? locale);

	//Automatically called ~1 frame after constructor
	public void OnAttachToPanelIAutoUpdatingLocalizedUI(AttachToPanelEvent evt)
	{
		LocalizationSettings.SelectedLocaleChanged += OnSelectedLocaleChanged;
	}

	//Automatically called at end of lifespan when removed
	public void OnDetachFromPanelIAutoUpdatingLocalizedUI(DetachFromPanelEvent evt)
	{
		LocalizationSettings.SelectedLocaleChanged -= OnSelectedLocaleChanged;
	}

	//Call in constructor
	public void RegisterPanelEventsIAutoUpdatingLocalizedUI()
	{
		if (this is VisualElement visualElement)
		{
			visualElement.RegisterCallbackOnce<AttachToPanelEvent>(OnAttachToPanelIAutoUpdatingLocalizedUI);
			visualElement.RegisterCallbackOnce<DetachFromPanelEvent>(OnDetachFromPanelIAutoUpdatingLocalizedUI);
		}
	}

	/*
	Template

	//IAutoUpdatingLocalizedUI
	public void OnSelectedLocaleChanged(Locale locale)
	{

	}

	//Register event callbacks, call in constructor
	if (this is IAutoUpdatingLocalizedUI autoUpdatingLocalizedUI) { autoUpdatingLocalizedUI.RegisterPanelEventsIAutoUpdatingLocalizedUI(); }
	else { Debug.LogWarning("[IAutoUpdatingLocalizedUI] Tried to register panel events but class " + this.GetType().ToString() + " does not implement IAutoUpdatingLocalizedUI"); }
	
	*/
}