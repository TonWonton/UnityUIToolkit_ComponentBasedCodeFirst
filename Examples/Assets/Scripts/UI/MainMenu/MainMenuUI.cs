#nullable enable
using UnityEngine;


public class MainMenuUI : UISingleton<MainMenuUI>
{
	//Variables
	//Inherited protected UIDocument _uiDocument;
	//Inherited protected VisualElement _root;
	//Inherited public UIDocument UIDocument { get { return _uiDocument; } }
	//Inherited public VisualElement Root { get { return _root; } }

	//Components
	private MainMenuButtonsController _mainMenuButtonsController = null!; //Not marked as nullable since GetComponents() is called in Awake()



	//Methods
	protected override void RegisterEvents()
	{

	}

	protected override void UnregisterEvents()
	{

	}

	protected override void Initialize()
	{
		//Initialize components with _root
		_mainMenuButtonsController.Initialize(_root);
	}

	protected override void GetComponents()
	{
		//Get components from GameObject
		GameObject currentGameObject = gameObject;

		//Option 1: add components to GameObject in UnityEditor and use GetComponent<>()
		_mainMenuButtonsController = currentGameObject.GetComponent<MainMenuButtonsController>();

		//Option 2: add components to GameObject in code using AddComponent<>()
		//_mainMenuButtonsController = currentGameObject.AddComponent<MainMenuButtonsController>();
	}
}