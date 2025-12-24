#nullable enable
using UnityEngine;
using UnityEngine.UIElements;


public class MainMenuButtonsController : UIController
{
	//Variables
	//Inherited protected VisualElement? _parentElement;
	//Inherited public VisualElement? ParentElement { get { return _parentElement; } }

	//Elements
	private MainMenuButtons? _mainMenuButtons;



	//Methods
	public void Show() { if (_mainMenuButtons != null) { _mainMenuButtons.style.display = DisplayStyle.Flex; } }
	public void Hide() { if (_mainMenuButtons != null) { _mainMenuButtons.style.display = DisplayStyle.None; } }



	//Event handling
	private void OnStartGameButtonClicked()
	{
		//Handle Start Game button click
		Debug.Log("[MainMenuButtonsController] Start Game button clicked");
	}

	private void OnSettingsButtonClicked()
	{
		//Handle Settings button click
		Debug.Log("[MainMenuButtonsController] Settings button clicked");
	}

	private void OnExitGameButtonClicked()
	{
		//Handle Exit Game button click
		Debug.Log("[MainMenuButtonsController] Exit Game button clicked");
	}



	//Initialization
	protected override void CreateElements()
	{
		if (_parentElement != null)
		{
			//Create elements and add to parent element
			_mainMenuButtons = new MainMenuButtons();
			_parentElement.Add(_mainMenuButtons);
		}
	}

	protected override void RemoveElements()
	{
		if (_parentElement != null)
		{
			//Remove elements from parent element
			if (_mainMenuButtons != null) { _parentElement.Remove(_mainMenuButtons); }
		}

		//Set references to null
		_mainMenuButtons = null;
	}

	protected override void RegisterEvents()
	{
		//Register events. MainMenuButtons events are static, no null check needed
		MainMenuButtons.StartGameButtonClicked += OnStartGameButtonClicked;
		MainMenuButtons.SettingsButtonClicked += OnSettingsButtonClicked;
		MainMenuButtons.ExitGameButtonClicked += OnExitGameButtonClicked;
	}

	protected override void UnregisterEvents()
	{
		//Unregister events. MainMenuButtons events are static, no null check needed
		MainMenuButtons.StartGameButtonClicked -= OnStartGameButtonClicked;
		MainMenuButtons.SettingsButtonClicked -= OnSettingsButtonClicked;
		MainMenuButtons.ExitGameButtonClicked -= OnExitGameButtonClicked;
	}
}