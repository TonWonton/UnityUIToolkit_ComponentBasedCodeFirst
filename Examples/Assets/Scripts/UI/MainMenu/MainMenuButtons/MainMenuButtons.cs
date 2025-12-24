#nullable enable
using UnityEngine.UIElements;
using System;


[UxmlElement]
public partial class MainMenuButtons : VisualElement
{
	//Elements
	private Button _startGameButton;
	private Button _settingsButton;
	private Button _exitGameButton;

	//Events
	public static event Action? StartGameButtonClicked;
	public static event Action? SettingsButtonClicked;
	public static event Action? ExitGameButtonClicked;



	//Event handling
	private void OnStartGameButtonClicked()
	{
		//Invoke event when _startGameButton is clicked and handle logic in controller
		StartGameButtonClicked?.Invoke();
	}

	private void OnSettingsButtonClicked()
	{
		//Invoke event when _settingsButton is clicked and handle logic in controller
		SettingsButtonClicked?.Invoke();
	}

	private void OnExitGameButtonClicked()
	{
		//Invoke event when _exitGameButton is clicked and handle logic in controller
		ExitGameButtonClicked?.Invoke();
	}



	//Initialization
	public MainMenuButtons()
	{
		//Add container to style class list and set container pickingMode to PickingMode.Ignore
		//to prevent container from blocking input events
		AddToClassList("main-menu-buttons");
		pickingMode = PickingMode.Ignore;

		//Create buttons
		_startGameButton = new Button();
		_settingsButton = new Button();
		_exitGameButton = new Button();

		//Add buttons to style class list and set text
		_startGameButton.AddToClassList("main-menu-buttons__button");
		_settingsButton.AddToClassList("main-menu-buttons__button");
		_exitGameButton.AddToClassList("main-menu-buttons__button");

		_startGameButton.text = "Start Game";
		_settingsButton.text = "Settings";
		_exitGameButton.text = "Exit Game";

		//Register button click events
		//This will cause a memory leak if events are not unregistered. Handle unregistration with `ICallbackUI`, controller, etc.
		_startGameButton.clicked += OnStartGameButtonClicked;
		_settingsButton.clicked += OnSettingsButtonClicked;
		_exitGameButton.clicked += OnExitGameButtonClicked;

		//Add buttons to container
		Add(_startGameButton);
		Add(_settingsButton);
		Add(_exitGameButton);
	}
}