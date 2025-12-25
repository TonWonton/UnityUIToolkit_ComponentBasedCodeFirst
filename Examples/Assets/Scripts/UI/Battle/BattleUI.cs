#nullable enable
using UnityEngine;


public class BattleUI : UISingleton<BattleUI>
{
	//Components
	private BattleStartBannerController _battleStartBannerController = null!;
	private UsedAttackBannerController _usedAttackBannerController = null!;
	private RoundCounterController _roundCounterController = null!;


	//Event handling
	private void OnBattleEnded()
	{
		GameObject.Destroy(gameObject);
	}


	//Initialization
	protected override void RegisterEvents()
	{
		Battle.AfterEnded += OnBattleEnded;
	}

	protected override void UnregisterEvents()
	{
		Battle.AfterEnded -= OnBattleEnded;
	}

	protected override void Initialize()
	{
		_battleStartBannerController.Initialize(_root);
		_usedAttackBannerController.Initialize(_root);
		_roundCounterController.Initialize(_root);

	}

	protected override void GetComponents()
	{
		GameObject currentGameObject = gameObject;

		_battleStartBannerController = currentGameObject.GetComponent<BattleStartBannerController>();
		_usedAttackBannerController = currentGameObject.GetComponent<UsedAttackBannerController>();
		_roundCounterController = currentGameObject.GetComponent<RoundCounterController>();
	}
}