#nullable enable
using UnityEngine.UIElements;


/// <summary>
/// Top level UI class that has the <c>_root</c> <c>VisualElement</c> and initializes <c>UIController</c>s.
/// </summary>
public abstract class UI : BaseUI
{
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

	private void Awake()
	{
		_uiDocument = gameObject.GetComponent<UIDocument>();
		GetComponents();
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