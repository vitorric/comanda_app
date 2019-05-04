using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorImage : MonoBehaviour
{
    public ColorPicker picker;

    public List<Image> ListImageAlvo;    

    private void Awake()
    {
        picker.onValueChanged.AddListener(ColorChanged);
    }

    private void OnDestroy()
    {
        picker.onValueChanged.RemoveListener(ColorChanged);
    }

    private void ColorChanged(Color newColor)
    {
        ListImageAlvo.ForEach(x => x.color = newColor);
    }
}
