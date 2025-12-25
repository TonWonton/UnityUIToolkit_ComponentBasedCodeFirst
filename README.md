# UnityUIToolkit_ComponentBasedCodeFirst

## Description
Scripts for the Unity UI Toolkit, for creating component based UI (mostly) code first/programmatically. Where the top level `UI : BaseUI` or `UISingleton<T> : BaseUI where T : UISingleton<T>` defines and initializes all the `UIController` components, and the `UIController` creates and handles the logic for the UI elements.

## Disclaimer
Not tested in/for production, bugs, etc. This approach or code might also be a bad/ineffective/weird way to do UI. Not sure. I just thought examples for the Unity UI toolkit was (maybe?) lacking and that it might come in handy for someone. All code and info in this repository is provided "as is" without warranty of any kind and I am not responsible for any damages or problems, etc. that might come from using the info and code. See the license for more information.

## Installation
1. Download the files from the repository
2. Copy the `Assets` folder into the Unity project

## Benefits of this approach
Keep in mind the disclaimer above. There are probably better ways to handle the UI etc. or some of the things and examples listed below might be wrong or misleading.

### Code first/programmatic approach
- Most of the work (if not all of it) is done with IDE/text editors
- Suggested usage only has to use the UnityEditor at the start to:
  - Create a `GameObject`, add the `UIDocument` component to the `GameObject` and assign the `Panel Settings` and `Source Asset` fields
  - Add the UI script
- The rest can be done through IDE/text editors

### Modularity and reusability
- The `GameObject`s and/or `UI`/`UISingleton<T>` can be split into different groups
  - E.g. split into groups based on scene or logical part of the UI
- Each `GameObject` and/or `UI`/`UISingleton<T>` can be made into a prefab and instantiated when needed, e.g:
  - Dynamic UI composed of the different `GameObject` parts/groups
  - Instantiate UI only when needed, e.g. when a scene changes or when a battle starts
  - World space UI for characters, using the same prefab
- Each `UIComponent` can be reused
  - E.g. in an RPG, a `CharacterSheetController : UIController` or `ShopWindowController : UIController` can be reused anywhere

### Automated lifecycle and memory management
- The `UI`/`UISingleton<T>` and `UIComponent`s automatically create the elements and register events, then later removes the elements and unregisters events when needed, just by filling out the methods
- Can add/call/assign `DontDestroyOnLoad()` to create global/persistent UI, loading screen/transitions, etc.
- No memory leaks, but garbage is ofc. still generated unless you use pooling, etc. when the `GameObject` is destroyed
- Included `ICallbackUI` can automatically register and unregister events for custom UI Toolkit/`VisualElement` elements
  - E.g. if you have a custom `MainMenuButtons` `VisualElement` that has 3 buttons
```csharp
[UxmlElement]
public partial class MainMenuButtons : VisualElement
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
- The top level `UI`/`UISingleton<T>` defines and initializes the needed `UIComponent`s, and can be coded to handle or do whatever:
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

			//If MainMenuState is MainMenuState.SettingsMenu
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

## Suggested usage
1. Create a `GameObject` in a scene and add the `UIDocument` `MonoBehaviour` component to the `GameObject`
2. On the `UIDocument`, add a Panel Settings Asset to the  `Panel Settings` field and an UI Document `.uxml` file that contains the file path to any used `.uss` style sheets to the `Source Asset` field
```xml
<?xml version="1.0" encoding="utf-8"?>
<ui:UXML xmlns:ui="UnityEngine.UIElements" xmlns:uie="UnityEditor.UIElements" editor-extension-mode="False">


    <!-- Add stylesheets -->
    <Style src="project://database/Assets/UI/MainMenu/MainMenuUI.uss" />


</ui:UXML>
```
3. Create and/or add a "top level" `UI` or `UISingleton<T>` component to the same `GameObject` (e.g. `MainMenuUI : UI` or `MainMenuUI : UISingleton<MainMenuUI>`)
4. Add `UIController` components to the same `GameObject` (either in UnityEditor then call `GetComponent<>()` on `UI`/`UISingleton<T>`, or through code with `AddComponent<>()` on `UI`/`UISingleton<T>`)

### Creating/adding components
1. Create a new class that inherits from `UIController` and implement the `UIController` abstract methods in the new class (template in `UIController.cs`)
2. Add `UnityEngine.UIElements` (UI Toolkit/`VisualElement` elements) as fields to the class and create/remove the elements in `CreateElements()` and `RemoveElements()`, and register any events if needed
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
			//Create elements and add to parent element
			_button = new Button();
			_button.AddToClassList("button-style"); //Add to style class list if needed
			_parentElement.Add(_button);
		}
	}

	//3. Remove elements
	protected override void RemoveElements()
	{
		if (_parentElement != null)
		{
			//Remove elements from parent element
			if (_button != null) { _parentElement.Remove(_button); }
		}

		//Set references to null
		_button = null;
	}

	//4. Add event registration/unregistration
	protected override void RegisterEvents()
	{
		if (_button != null)
		{
			_button.clicked += OnClicked;
		}
	}

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

	//2. Get components
	protected override void GetComponents()
	{
		//Get components from GameObject
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

## Notes

- Check the examples above and `Examples` folder for e.g. UI, UIController, custom element examples etc.



