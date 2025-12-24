#nullable enable
using UnityEngine;
using UnityEngine.UIElements;


/// <summary>
/// Top level singleton UI class that has the <c>_root</c> <c>VisualElement</c> and initializes <c>UIController</c>s.
/// </summary>
public abstract class UISingleton<T> : BaseUI where T : UISingleton<T>
{
	//Instance
	public static T? Instance { get; protected set; }



	//Initialization
	private void OnEnable()
	{
		_root = _uiDocument.rootVisualElement;
		_root.pickingMode = PickingMode.Ignore;

		Initialize();
		RegisterEvents();
	}

	private void OnDisable()
	{
		UnregisterEvents();
	}

	private void OnDestroy()
	{
		if (Instance == (T)this)
		{
			Instance = null;
		}
	}

	private void Awake()
	{
		_uiDocument = gameObject.GetComponent<UIDocument>();
		GetComponents();

		if (Instance == null)
		{
			Instance = (T)this;
		}
		else
		{
			GameObject.Destroy(gameObject);
		}
	}



	/* Template

	//Variables
	//Inherited protected UIDocument _uiDocument;
	//Inherited protected VisualElement _root;
	//Inherited public UIDocument UIDocument { get { return _uiDocument; } }
	//Inherited public VisualElement Root { get { return _root; } }

	//Components

	
	
	//Initialization
	protected override void RegisterEvents()
	{

	}

	protected override void UnregisterEvents()
	{

	}

	protected override void Initialize()
	{

	}

	protected override void GetComponents()
	{

	}
	
	
	*/
}
