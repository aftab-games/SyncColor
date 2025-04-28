using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

public class UISystem : MonoBehaviour
{
    [Inject] private ColorStateHandler _colorHandler;

    void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        var buttons = root.Query<Button>("color-button").ToList();

        for (int i = 0; i < buttons.Count; i++)
        {
            int index = i;
            buttons[i].style.backgroundColor = new StyleColor(_colorHandler.GetColorByIndex(index));
            buttons[i].clicked += () => _colorHandler.SetColor(index);
        }
    }
}
