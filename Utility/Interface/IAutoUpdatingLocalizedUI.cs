#nullable enable
using UnityEngine.UIElements;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;


public interface IAutoUpdatingLocalizedUI
{
	public abstract void OnSelectedLocaleChanged(Locale? locale);

	public void OnAttachToPanelIAutoUpdatingLocalizedUI(AttachToPanelEvent evt)
	{
		LocalizationSettings.SelectedLocaleChanged += OnSelectedLocaleChanged;
	}

	public void OnDetachFromPanelIAutoUpdatingLocalizedUI(DetachFromPanelEvent evt)
	{
		LocalizationSettings.SelectedLocaleChanged -= OnSelectedLocaleChanged;
	}

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

	public void OnSelectedLocaleChanged(Locale locale)
	{

	}

	if (this is IAutoUpdatingLocalizedUI autoUpdatingLocalizedUI) { autoUpdatingLocalizedUI.RegisterPanelEventsIAutoUpdatingLocalizedUI(); }
	else { Log.Warning("[IAutoUpdatingLocalizedUI] Tried to register panel events but class " + this.GetType().ToString() + " does not implement IAutoUpdatingLocalizedUI"); }
	
	*/
}