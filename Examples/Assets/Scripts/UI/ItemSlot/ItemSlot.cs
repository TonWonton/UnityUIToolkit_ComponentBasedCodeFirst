#nullable enable
using UnityEngine;
using UnityEngine.UIElements;


[UxmlElement]
public partial class ItemSlot : VisualElement
{
	//Elements
	protected Image _frame;
	protected Image _image;
	protected Label _label;


	//Methods
	public void SetImage(Texture2D? image) { _image.image = image; }
	public void SetLabelDisplay(bool shouldDisplay) { _label.style.display = shouldDisplay ? DisplayStyle.Flex : DisplayStyle.None; }
	public void SetText(string text) { _label.text = text; }
	
	public void SetItemInstance(ItemInstance? itemInstance)
	{
		if (itemInstance != null)
		{
			_frame.SetItemSlotRarityFrameFromItemRarity(itemInstance.Item.ItemRarity); //Extension method
			_image.image = itemInstance.Item.Icon;
			_label.text = itemInstance.stackQuantity.ToString();
		}
		else
		{
			_frame.image = null;
			_image.image = null;
			_label.text = string.Empty;
		}
	}

	public void SetItem(Item? item, string? text = null)
	{
		if (item != null)
		{
			_frame.SetItemSlotRarityFrameFromItemRarity(item.ItemRarity); //Extension method
			_label.text = text == null ? string.Empty : text;
			_image.image = item.Icon;
		}
		else
		{
			_frame.image = null;
			_image.image = null;
			_label.text = string.Empty;
		}
	}

	public void ClearItemSlot()
	{
		_image.image = null;
		_frame.image = null;
		_label.text = string.Empty;
	}

	public ItemSlot()
	{
		AddToClassList("item-slot");
		style.backgroundImage = null; //Reference to Texture2D
		pickingMode = PickingMode.Ignore;

		_frame = new Image();
		_frame.AddToClassList("item-slot__frame");
		_frame.pickingMode = PickingMode.Ignore;

		_image = new Image();
		_image.AddToClassList("item-slot__icon");
		_image.pickingMode = PickingMode.Ignore;

		_label = new Label();
		_label.AddToClassList("item-slot__label");
		_label.pickingMode = PickingMode.Ignore;

		Add(_frame);
		Add(_image);
		Add(_label);
	}
}