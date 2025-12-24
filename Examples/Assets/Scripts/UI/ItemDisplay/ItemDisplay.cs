#nullable enable
using System.Collections.Generic;
using UnityEngine.UIElements;


[UxmlElement]
public partial class ItemDisplay : VisualElement
{
	//Elements
	private List<ItemSlot> _slots = new List<ItemSlot>();

	//Variables
	private int _activeSlotCount = 0;
	public int SlotCount { get { return _slots.Count; } }



	//Methods
	public void AddItemInstance(ItemInstance? itemInstance)
	{
		ItemSlot itemSlot = GetOrAddItemSlot();
		itemSlot.SetItemInstance(itemInstance);
		itemSlot.Show(); //Extension method

		_activeSlotCount++;
	}

	public void AddItem(Item? item, string? text = null)
	{
		ItemSlot itemSlot = GetOrAddItemSlot();
		itemSlot.SetItem(item, text);
		itemSlot.Show(); //Extension method
		
		_activeSlotCount++;
	}

	public void ClearDisplay()
	{
		foreach (ItemSlot slot in _slots)
		{
			slot.ClearItemSlot();
			slot.Hide(); //Extension method
		}

		_activeSlotCount = 0;
	}

	public ItemSlot GetOrAddItemSlot()
	{
		if (_activeSlotCount >= SlotCount)
		{
			return AddItemSlot();
		}
		else
		{
			return _slots[_activeSlotCount];
		}
	}

	public ItemSlot AddItemSlot()
	{
		ItemSlot newSlot = new ItemSlot();

		_slots.Add(newSlot);
		Add(newSlot);

		return newSlot;
	}



	//Initialization
	public ItemDisplay()
	{
		AddToClassList("item-display");
		pickingMode = PickingMode.Ignore;
	}
}