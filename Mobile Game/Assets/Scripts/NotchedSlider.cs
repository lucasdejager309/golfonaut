using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotchedSlider : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] int maxValue;
    [SerializeField] int currentValue;

    [Header("Appeareance")]
    [SerializeField] float padding;
    [SerializeField] Sprite barGraphic;
    [SerializeField] Color activeColor;
    [SerializeField] Color inactiveColor;
    
    List<Image> bars = new List<Image>();

    void Start() {
        SetMaxValue(maxValue);
        SetValue(currentValue);
    }

    public void SetMaxValue(int newValue) {
        maxValue = newValue;

        foreach (Image bar in bars) {
            Destroy(bar.gameObject);
        }
        bars.Clear();

        float step = (this.GetComponent<RectTransform>().rect.width-padding*(maxValue+1))/maxValue;
        for (int i = 0; i < maxValue; i++) {

            GameObject barObj = new GameObject("bar");
            
            RectTransform rect = barObj.AddComponent<RectTransform>();
            rect.SetParent(this.transform);
            rect.anchorMin = rect.anchorMax = new Vector2(0, 0.5f);
            
            Vector2 position = new Vector2((step*i+step/2),0);
            position.x += padding*(i+1);
            rect.anchoredPosition = position;

            rect.localScale = new Vector3(1,1,1);
            rect.sizeDelta = new Vector2(step, this.GetComponent<RectTransform>().sizeDelta.y-padding*2);
            
            Image image = barObj.AddComponent<Image>();
            image.type = Image.Type.Sliced;
            image.raycastTarget = false;
            image.sprite = barGraphic;
            image.color = inactiveColor;

            bars.Add(image);
        }
    }

    public void SetValue(int newValue) {
        currentValue = newValue;
        
        foreach (Image bar in bars) {
            bar.color = inactiveColor;
        }

        for (int i = 0; i < newValue; i++) {
            bars[i].color = activeColor;
        }
    }
}
