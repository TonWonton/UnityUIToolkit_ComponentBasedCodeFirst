#nullable enable
using UnityEngine;
using UnityEngine.UIElements;


/// <summary>
/// Base class for <c>UI</c> and <c>UISingleton</c> that has the <c>_uiDocument</c> <c>UIDocument</c> and <c>_root</c> <c>VisualElement</c>. Defines the initialization methods.
/// </summary>
public abstract class BaseUI : MonoBehaviour
{
	//Variables
	protected UIDocument _uiDocument = null!;
	protected VisualElement _root = null!;

	public UIDocument UIDocument { get { return _uiDocument; } }
	public VisualElement Root { get { return _root; } }



	//Initialization
	protected abstract void RegisterEvents();
	protected abstract void UnregisterEvents();
	protected abstract void Initialize();
	protected abstract void GetComponents();
}