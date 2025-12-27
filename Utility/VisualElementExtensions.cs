#nullable enable
using UnityEngine;
using UnityEngine.UIElements;


public static class VisualElementExtensions
{
	/*VISUAL ELEMENT*/
	public static void Show(this VisualElement element) { element.style.display = DisplayStyle.Flex; }
	public static void Hide(this VisualElement element) { element.style.display = DisplayStyle.None; }
	public static void SetDisplay(this VisualElement element, bool shouldDisplay) { element.style.display = shouldDisplay ? DisplayStyle.Flex : DisplayStyle.None; }

	public static void SetTransparentAndShow(this VisualElement element)
	{
		element.style.opacity = 0f;
		element.style.display = DisplayStyle.Flex;
	}

	public static void SetTransparentAndHide(this VisualElement element)
	{
		element.style.opacity = 0f;
		element.style.display = DisplayStyle.None;
	}

	public static void SetOpaqueAndShow(this VisualElement element)
	{
		element.style.opacity = 1f;
		element.style.display = DisplayStyle.Flex;
	}

	public static void SetOpaqueAndHide(this VisualElement element)
	{
		element.style.opacity = 1f;
		element.style.display = DisplayStyle.None;
	}

	public static void SetTranslate(this VisualElement element, float x, float y)
	{
		element.style.translate = new StyleTranslate(new Translate(x, y));
	}

	public static void SetTranslate(this VisualElement element, Vector2 vector2)
	{
		element.style.translate = new StyleTranslate(new Translate(vector2.x, vector2.y));
	}
}