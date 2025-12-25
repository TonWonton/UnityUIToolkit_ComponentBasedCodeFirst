#nullable enable
using UnityEngine.UIElements;


[UxmlElement]
public partial class RoundCounter : VisualElement
{
	//Elements
	private Label _roundLabel;

	public void SetRound(int round)
	{
		_roundLabel.text = (round + 1).ToString();
	}

	public RoundCounter()
	{
		AddToClassList("round-counter");
		pickingMode = PickingMode.Ignore;

		_roundLabel = new Label();
		_roundLabel.AddToClassList("round-counter__label");
		_roundLabel.pickingMode = PickingMode.Ignore;

		Add(_roundLabel);
	}
}