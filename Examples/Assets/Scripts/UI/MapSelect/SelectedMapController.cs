#nullable enable


public class SelectedMapController : UIController
{
	//Elements
	private SelectedMap? _selectedMap;


	//Event handling
	private void OnShouldChangeMapSelectDisplay(bool shouldDisplay)
	{
		if (shouldDisplay) { _selectedMap?.Show(); }
		else { _selectedMap?.Hide(); }
	}

	private void OnMapOptionSelected(MapOption? mapOption)
	{
		if (_selectedMap != null)
		{
			_selectedMap.SelectMapOption(mapOption);
			_selectedMap.DisplayMapOption(mapOption);
		}
	}


	//Initialization
	protected override void CreateElements()
	{
		if (_parentElement != null)
		{
			_selectedMap = new SelectedMap();
			_parentElement.Add(_selectedMap);
		}
	}

	protected override void RemoveElements()
	{
		if (_selectedMap != null) { _selectedMap.RemoveFromHierarchy(); }

		_selectedMap = null;
	}

	protected override void RegisterEvents()
	{
		MapSelect.ShouldChangeMapSelectDisplay += OnShouldChangeMapSelectDisplay;
		MapSelect.MapOptionSelected += OnMapOptionSelected;
	}

	protected override void UnregisterEvents()
	{
		MapSelect.ShouldChangeMapSelectDisplay -= OnShouldChangeMapSelectDisplay;
		MapSelect.MapOptionSelected -= OnMapOptionSelected;
	}
}