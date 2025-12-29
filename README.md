# UnityUIToolkit_ComponentBasedCodeFirst

## Description
Scripts for the Unity UI Toolkit, for creating component based UI (mostly) code first/programmatically. Where the top level `UI : BaseUI` or `UISingleton<T> : BaseUI where T : UISingleton<T>` defines and initializes all the `UIController` components, and the `UIController` creates and handles the logic for the UI elements.

## Disclaimer
Not tested in/for production, bugs, etc. This approach or code might also be a bad/ineffective/weird way to do UI. Not sure. I just thought examples for the Unity UI toolkit was (maybe?) lacking and that it might come in handy for someone. All code and info in this repository is provided "as is" without warranty of any kind and I am not responsible for any damages or problems, etc. that might come from using the info and code. See the license for more information.

## Table of contents
- [Installation](#installation)
- [How it works](#how-it-works)
- [Suggested usage](#suggested-usage)
- [Benefits of this approach](#benefits-of-this-approach)
- [Alternative approaches](#alternative-approaches)
- [Notes](#notes)

## Installation
1. Download the files from the repository
2. Copy the `Assets` folder into the Unity project
3. OPTIONAL: copy scripts in the `Utility` folder into the Unity project anywhere inside the Unity project's `Asset` folder
    - NOTE: the `IAutoUpdatingLocalizedUI` script has the Unity Localization package as a dependency

## How it works
### Structure
There are 4 main scripts/classes. They are all abstract base classes that are intended to be inherited from and create your own classes with.
1. `BaseUI` is the base class for `UI` and `UISingleton<T>`
2. `UI` and `UISingleton<T>` is the base class for "top level" UI (e.g. `MainMenuUI`, `DungeonUI`) which define, intialize, and is composed of different `UIController` components
3. `UIController` is the base class for the UI components, and defines and creates the elements and handles the functionality and logic for the elements

### Flow
#### Initialization
1. The "top level" `UI` or `UISingleton<T>` gets the `UIDocument`, `_root` `VisualElement`, and `UIController` components in `Awake()` and `OnEnable()`
2. It then uses `_root` to call `Initialize(_root)` on the `UIController` components and registers it's own events
3. The `UIController.Initialize(_root)` accepts the `_root` parentElement and then calls `CreateElements()` and then `RegisterEvents()`

#### End of lifespan
4. On the end of the UI lifespan in `OnDisable()` (when it is destroyed or disabled) it calls `RemoveElements()` on the `UIController`, and `UnregisterEvents()` on everything.
    - Each component has `OnDisable()` it on itself, it doesn't propagate from `UI`/`UISingleton<T>` -> `UIController`

## Suggested usage
1. Create a `GameObject` in a scene
2. Add the `UIDocument` component to the `GameObject`
3. Add a Panel Settings Asset to the `Panel Settings` field and a `.uxml` UI Document file to the `Source Asset` field
    - The `.uxml` UI Document should contain the file path to any used `.uss` style sheet
4. Create and add a "top level" `UI` or `UISingleton<T>` component to the `GameObject` (copy template in `UI.cs` or `UISingleton.cs`)
    - E.g. `MainMenuUI : UISingleton<T>` or `CharacterUI : UI`
5. Create and add `UIController` components to the `GameObject` and initialize them in the `UI` or `UISingleton<T>` component (copy template in `UIController.cs`)
    - E.g. `HealthbarController : UIController` or `PauseMenuController : UIController`
```xml
<?xml version="1.0" encoding="utf-8"?>
<ui:UXML xmlns:ui="UnityEngine.UIElements" xmlns:uie="UnityEditor.UIElements" editor-extension-mode="False">


    <!-- Add stylesheets -->
    <Style src="project://database/Assets/UI/MainMenu/MainMenuUI.uss" />


</ui:UXML>
```

### Creating/adding components
1. Create a new class that inherits from `UIController` and implement the `UIController` abstract methods in the new class (copy the template in `UIController.cs`)
2. Add `UnityEngine.UIElements` (UI Toolkit/`VisualElement` elements) as fields to the class and fill out the methods
```csharp
/*IN UICONTROLLER*/
using UnityEngine.UIElements;

public class ButtonController : UIController
{
	//1. Add elements
	private Button? _button;

	//2. Create elements
	protected override void CreateElements()
	{
		if (_parentElement != null)
		{
			_button = new Button(); //Create elements
			_button.AddToClassList("button-style"); //Add to style class list if needed
			_parentElement.Add(_button); //Add elements to _parentElement
		}
	}

	//3. Remove elements
	protected override void RemoveElements()
	{
		//Remove elements from parent element
		if (_button != null) { _button.RemoveFromHierarchy(); }

		//Set references to null
		_button = null;
	}

	//4. Add event registration
	protected override void RegisterEvents()
	{
		if (_button != null)
		{
			_button.clicked += OnClicked;
		}
	}

	//5. Add event unregistration
	protected override void UnregisterEvents()
	{
		if (_button != null)
		{
			_button.clicked -= OnClicked;
		}
	}

	private void OnClicked()
	{
		Debug.Log("Button clicked");
	}
}

```
3. Add the `UIController` component to the `GameObject` as a component, and as a field on a class deriving from `UI` or `UISingleton<T>` and then use `GetComponents()` and `Initialize()` to get and initialize the `UIController`
```csharp
/*IN UI OR UISINGLETON*/
public class ButtonUI : UI
{
	//Inherited protected VisualElement _root;

	//1. Add components
	private ButtonController _buttonController = null!; //Not marked as nullable since GetComponents() is called in Awake()

	//2. Get components from GameObject
	protected override void GetComponents()
	{
		GameObject currentGameObject = gameObject;

		//Option 1: add components to GameObject in UnityEditor and use GetComponent<>()
		_buttonController = currentGameObject.GetComponent<ButtonController>();

		//Option 2: add components to GameObject in code using AddComponent<>()
		_buttonController = currentGameObject.AddComponent<ButtonController>();
	}

	//3. Initialize components with _root
	protected override void Initialize()
	{
		_buttonController.Initialize(_root);
	}
}
```

#### Using `GetComponent<>()`
- Allows for `[SerializeField]` on the `UIController` (e.g. for tweening, transitions, animations, etc.)
- Getting components should be faster than adding
- Have to use UnityEditor and add component on `GameObject`

#### Using `AddComponent<>()`
- Don't have to use UnityEditor

### Changing element style
1. Call `VisualElement` `.AddToClassList("style-class-name")`
2. Add any used `.uss` files to the `.uxml` UI Document
```xml
<?xml version="1.0" encoding="utf-8"?>
<ui:UXML xmlns:ui="UnityEngine.UIElements" xmlns:uie="UnityEditor.UIElements" editor-extension-mode="False">


    <!-- Add stylesheets -->
    <Style src="project://database/Assets/UI/Global.uss" />
    <Style src="project://database/Assets/UI/Buttons.uss" />
    <Style src="project://database/Assets/UI/MainMenu/MainMenuUI.uss" />


</ui:UXML>
```
3. Add/make changes to the `.uss` file(s)
    - Works without recompilation but the UI will rebuild and call `BaseUI.Initialize()`. If needed add logic to e.g. the `UIController` that e.g. sets itself up again with previous state

- Style sheets can also be added through code with references to the style sheet

## Benefits of this approach
Keep in mind the disclaimer above. There are probably better ways to handle the UI etc. or some of the things and examples listed below might be wrong or misleading.

### Code first/programmatic approach
- Most of the work (if not all of it) is done with IDE/text editors
- Strongly typed, no magic strings (except for style sheets e.g. `AddToClassList("style-class-name")` and adding the style sheet to the `.uxml` file)
- Suggested usage only has to use the UnityEditor at the start to:
  1. Create a `GameObject`
  2. Add the `UIDocument` component to the `GameObject`
  3. Assign the `Panel Settings` and `Source Asset` fields
  4. Add the UI script(s)
- The rest can be done through IDE/text editors

### Modularity and reusability
- The `GameObject` and/or `UI`/`UISingleton<T>` can be split into different groups
  - E.g. split into groups based on scene or logical part of the UI
- Each `GameObject` and/or `UI`/`UISingleton<T>` can be made into a prefab and instantiated when needed, e.g:
  - Dynamic UI composed of the different `GameObject` and/or `UI`/`UISingleton<T>` parts/groups
  - Instantiate UI only when needed, e.g. when a scene changes or when a battle starts
  - World space UI for characters, using the same prefab
- Each `UIController` component can be reused
  - E.g. in an RPG, a `CharacterSheetController : UIController` or `ShopWindowController : UIController` can be reused
- Custom elements can be reused
  - E.g. `ItemSlot : VisualElement`, `Healthbar : VisualElement`

### Automated lifecycle and memory management
- The `UI`/`UISingleton<T>` and `UIController` automatically create the elements and register events, then later removes the elements and unregisters events when needed, just by filling out the methods
- Can add/call/assign `DontDestroyOnLoad()` to create global/persistent UI, loading screen/transitions, etc.
- No memory leaks, but garbage is still generated unless you use pooling etc. when the `GameObject` is destroyed
- Included `ICallbackUI` can automatically register and unregister events for custom UI Toolkit/`VisualElement` elements
  - E.g. if you have a custom `MainMenuButtons` `VisualElement` that has 3 buttons
```csharp
[UxmlElement]
public partial class MainMenuButtons : VisualElement, ICallbackUI
{
	//Elements
	private Button _startGameButton;
	private Button _settingsButton;
	private Button _exitGameButton;

	//ICallbackUI
	//Automatically called ~1 frame after constructor
	public void RegisterCallbacks()
	{
		_startGameButton.clicked += OnStartGameButtonClicked;
		_settingsButton.clicked += OnSettingsButtonClicked;
		_exitGameButton.clicked += OnExitGameButtonClicked;
	}

	//Automatically called at end of lifespan when removed
	public void UnregisterCallbacks()
	{
		_startGameButton.clicked -= OnStartGameButtonClicked;
		_settingsButton.clicked -= OnSettingsButtonClicked;
		_exitGameButton.clicked -= OnExitGameButtonClicked;
	}

	//1. Register button click events with ICallbackUI in constructor
	public MainMenuButtons()
	{
		if (this is ICallbackUI callbackUI) { callbackUI.RegisterPanelEventsICallbackUI(); }
		else { Debug.LogWarning("[ICallbackUI] Tried to register panel events but class " + this.GetType().ToString() + " does not implement ICallbackUI"); }
	}
}
```
### Hot reload/live editing of layout
- When changing the `.uss` style file the layout will rebuild and the UI will remove the previous elements and then create and add the new ones
  - Prevents issues such as duplicate UI being created when changing the layout

### Separation of concerns and flexible architecture
- The top level `UI`/`UISingleton<T>` defines and initializes the needed `UIController`s, and can be coded to handle or do whatever:
```csharp
public class MainMenuUI : UISingleton<MainMenuUI>
{
	//Respond to MainMenuStateChanged event
	private void OnMainMenuStateChanged(MainMenuState newMainMenuState)
	{
		if (newMainMenuState == MainMenuState.SettingsMenu)
		{
			//If MainMenuState is MainMenuState.SettingsMenu
			_mainMenuButtonsController.Hide();
			_settingsMenuController.Show();
		}
	}



	//Handle it's own state
	public MainMenuUIState MainMenuUIState { get; private set; }
	public static Action<MainMenuUIState>? MainMenuUIStateChanged;

	public void SetMainMenuUIState(MainMenuUIState newMainMenuUIState)
	{
		if (this.MainMenuUIState != newMainMenuUIState)
		{
			//Set state
			this.MainMenuUIState = newMainMenuUIState;

			//If MainMenuUIState is MainMenuUIState.SettingsMenu
			if (newMainMenuUIState == MainMenuUIState.SettingsMenu)
			{
				_mainMenuButtonsController.Hide();
				_settingsMenuController.Show();
			}

			//Invoke event
			MainMenuUIStateChanged?.Invoke(newMainMenuUIState);
		}
	}



	//Direct call
	public void ShowSettingsMenu()
	{
		_settingsMenuController.Show();
	}



	//Invoke event
	private bool _settingsMenuDisplay = false;
	public static Action<bool>? ShouldChangeSettingsMenuDisplay;

	public void ToggleSettingsMenuDisplay()
	{
		_settingsMenuDisplay = !_settingsMenuDisplay;
		ShouldChangeSettingsMenuDisplay?.Invoke(_settingsMenuDisplay);
	}
}
```
- The `UIController` can be made to only be concerned about itself and it's elements, making it self contained and reusable
```csharp
public class SelectedMapController : UIController
{
	//Elements
	private SelectedMap? _selectedMap;

	//Event handling
	//SelectedMapController does not know and isn't concerned about anything else, it just responds to events
	private void OnShouldChangeMapSelectDisplay(bool shouldDisplay)
	{
		if (shouldDisplay) { _selectedMap?.Show(); }
		else { _selectedMap?.Hide(); }
	}

	private void OnMapOptionSelected(MapOption? mapOption)
	{
		if (_selectedMap != null)
		{
			_selectedMap.DisplayMapOption(mapOption);
		}
	}
}
```
- Elements usually only accept and displays data from the `UIController`, depending on the use case
```csharp
[UxmlElement]
public partial class CharacterInfo : VisualElement
{
	//Elements
	private Healthbar _healthbar;
	private StatDisplay _statDisplay;

	//UIController subscribes to e.g. CharacterClicked or TurnStartedCharacter events (or any event with a `Character`). Then the UIController calls `SetCharacter(Character? character)` on the element
	//The element can then be added to any `UIController` that processes and/or subscribes to an event with a `Character`
	public void SetCharacter(Character? character)
	{
		if (character != null)
		{
			_healthbar.SetFillPercentHundred(character.health.PercentHundred);
			_statDisplay.DisplayCharacterStats(character);

			_healthbar.Show();
			_statDisplay.Show();
		}
		else
		{
			_healthbar.Hide();
			_statDisplay.Hide();
		}
	}
}

```

## Alternative approaches
### Encapsulate functionality and logic in the element itself
Forgo the `UI`/`UISingleton<T>` and/or `UIController` and have the elements themselves handle the logic. Makes elements that have logic more advanced than just accepting and displaying information usable without an `UIController`.
- E.g. for building UI with `.uxml` or the UI Builder, or making the element more reusable with different `UIController` while avoiding code duplication.

Using the previous `MainMenuButtons` example:
```csharp
[UxmlElement]
public partial class MainMenuButtons : VisualElement, ICallbackUI
{
	//Elements
	private Button _startGameButton;
	private Button _settingsButton;
	private Button _exitGameButton;

	//Event handling
	//Call methods directly instead of relying on controller, events, or other components
	private void OnStartGameButtonClicked()
	{
		//"Action" method intended for UI interaction that checks e.g. game state
		GameManager.StartGameClickedAction();
	}

	private void OnSettingsButtonClicked()
	{
		//"Request" method that checks if the settings menu can/should be displayed
		SettingsManager.RequestShowSettingsMenu();
	}

	private void OnExitGameButtonClicked()
	{
		//"Request" method that checks if game can be exited 
		GameManager.RequestExitGame();
	}

	//ICallbackUI
	//Automatically called ~1 frame after constructor
	public void RegisterCallbacks()
	{
		_startGameButton.clicked += OnStartGameButtonClicked;
		_settingsButton.clicked += OnSettingsButtonClicked;
		_exitGameButton.clicked += OnExitGameButtonClicked;
	}

	//Automatically called at end of lifespan when removed
	public void UnregisterCallbacks()
	{
		_startGameButton.clicked -= OnStartGameButtonClicked;
		_settingsButton.clicked -= OnSettingsButtonClicked;
		_exitGameButton.clicked -= OnExitGameButtonClicked;
	}

	//Initialization
	public MainMenuButtons()
	{
		//Register button click events with ICallbackUI in constructor
		if (this is ICallbackUI callbackUI) { callbackUI.RegisterPanelEventsICallbackUI(); }
		else { Debug.LogWarning("[ICallbackUI] Tried to register panel events but class " + this.GetType().ToString() + " does not implement ICallbackUI"); }
	}
}
```

## Notes

- Check the examples above and `Examples` folder for e.g. UI, UIController, custom element examples etc.