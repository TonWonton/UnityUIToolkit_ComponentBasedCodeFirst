#nullable enable


public class RoundCounterController : UIController
{
	//Elements
	private RoundCounter? _roundCounter;


	//Event handling
	private void OnTurnStarted()
	{
		Battle? battle = Battle.Instance;
		if (_roundCounter != null && battle != null)
		{
			_roundCounter.SetRound(battle.Round);
		}
	}

	private void OnBattleStarted()
	{
		Battle? battle = Battle.Instance;
		if (_roundCounter != null && battle != null)
		{
			_roundCounter.SetRound(battle.Round);
			_roundCounter.Show();
		}
	}


	//Initialization
	protected override void CreateElements()
	{
		if (_parentElement != null)
		{
			_roundCounter = new RoundCounter();
			_roundCounter.Hide();
			_parentElement.Add(_roundCounter);
		}
	}

	protected override void RemoveElements()
	{
		if (_roundCounter != null) { _roundCounter.RemoveFromHierarchy(); }

		_roundCounter = null;
	}

	protected override void RegisterEvents()
	{
		Battle.Started += OnBattleStarted;
		Battle.TurnStarted += OnTurnStarted;
	}

	protected override void UnregisterEvents()
	{
		Battle.Started -= OnBattleStarted;
		Battle.TurnStarted -= OnTurnStarted;
	}
}