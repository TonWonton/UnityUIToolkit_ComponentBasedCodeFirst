# UnityUIToolkit_ComponentBasedCodeFirst

## Description
Scripts for the Unity UI Toolkit, for creating component based UI (mostly) code first/programmatically. Where the top level `UI : BaseUI` or `UISingleton<T> : BaseUI where T : UISingleton<T>` defines and initializes all the `UIController` components, and the `UIController` creates and handles the logic for the UI elements.

## Disclaimer
Not tested in/for production, bugs, etc. This approach or code might also be a bad/ineffective/weird way to do UI. Not sure. I just thought examples for the Unity UI toolkit was (maybe?) lacking and that it might come in handy for someone. All code and info in this repository is provided "as is" without warranty of any kind and I am not responsible for any damages or problems, etc. that might come from using the info and code. See the license for more information.

## Installation
1. Download the files from the repository
2. Copy the `Assets` folder into the Unity project

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

	//3. Add event registration/unregistration
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

```
3. Add the `UIController` component on the `GameObject` as a component and as a field to a class deriving from `UI` or `UISingleton<T>` and then use `GetComponents()` and `Initialize()` to get and initialize the `UIController`
```csharp
/*IN UI OR UISINGLETON*/
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

- Check the `Examples` folder for e.g. custom element examples etc.


