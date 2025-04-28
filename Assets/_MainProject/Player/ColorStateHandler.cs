using UnityEngine;
using System;

public class ColorStateHandler : MonoBehaviour
{
    [SerializeField] private Color[] availableColors;
    public event Action<Color> OnColorChanged;

    public int CurrentIndex { get; private set; }
    public Color CurrentColor => availableColors[CurrentIndex];

    public void SetColor(int index)
    {
        CurrentIndex = Mathf.Clamp(index, 0, availableColors.Length - 1);
        OnColorChanged?.Invoke(CurrentColor);
    }

    public Color GetColorByIndex(int i) => availableColors[i];
}
