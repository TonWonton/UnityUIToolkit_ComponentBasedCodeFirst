#nullable enable
using UnityEngine;
using UnityEngine.UIElements;


/// <summary>
/// Creates, initializes, and manages UI elements and logic. Adds the elements to the <c>_parentElement</c> received from <c>UI</c> or <c>UISingleton</c>.
/// </summary>
public abstract class UIController : MonoBehaviour
{
	//Variables
	protected VisualElement? _parentElement;
	public VisualElement? ParentElement { get { return _parentElement; } }



	//Initialization
	protected abstract void CreateElements();
	protected abstract void RemoveElements();
	protected abstract void RegisterEvents();
	protected abstract void UnregisterEvents();

	public virtual void Initialize(VisualElement parentElement)
	{
		_parentElement = parentElement;

		CreateElements();
		RegisterEvents();
	}

	protected virtual void OnDisable()
	{
		UnregisterEvents();
		RemoveElements();
	}



	/* Template

	//Variables
	//Inherited protected VisualElement? _parentElement;
	//Inherited public VisualElement? ParentElement { get { return _parentElement; } }

	//Elements
	private ? _;



	//Methods



	//Event handling
	


	//Initialization
	protected override void CreateElements()
	{
		if (_parentElement != null)
		{

		}
	}

	protected override void RemoveElements()
	{
		if (_ != null) { _.RemoveFromHierarchy(); }

		_ = null;
	}

	protected override void RegisterEvents()
	{

	}

	protected override void UnregisterEvents()
	{

	}

	*/
}