using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class TextElement {
    public string tag;
    public GameObject[] gameObjects;
}

[System.Serializable]
public class UIGroup {
    public string state;
    public GameObject[] elements;
}

public class UIManager : MonoBehaviour
{
    [Header("TextElements")]
    public TextElement[] textElements;
    [Header("UI Groups")]
    public UIGroup[] groups;

    public void UpdateUIElement(string tag, string value) {
        List<TextElement> elements = GetElementsByTag(tag);
        foreach (TextElement element in elements) {
            foreach(GameObject obj in element.gameObjects) {
                string prefix;
                if (obj.GetComponent<TextPrefix>() != null) {
                    prefix = obj.GetComponent<TextPrefix>().prefix;
                } else prefix = ""; 
                obj.GetComponent<Text>().text = prefix + value;
            }
        }
    }

     public void SetUIState(string state) {
        ToggleAll(false);

        foreach (UIGroup group in groups) {
            if (group.state == state) {
                foreach (GameObject obj in group.elements) {
                    obj.SetActive(true);
                }
            }
        }
    }

    void ToggleAll(bool state) {
        foreach (UIGroup group in groups) {
            foreach (GameObject obj in group.elements) {
                obj.SetActive(state);
            }
        }
    }
    
    List<TextElement> GetElementsByTag(string tag) {
        List<TextElement> elements = new List<TextElement>();
        foreach (TextElement element in textElements) {
            if (element.tag == tag) elements.Add(element);
        }

        return elements;
    }
}
