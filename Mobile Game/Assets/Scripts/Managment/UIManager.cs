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

    public void SetOpacity(string state, float opacity, bool fade = false, float fadeSpeed = 0f) {

        foreach (GameObject obj in GetGroup(state).elements) {
            Image[] images = obj.GetComponentsInChildren<Image>();
            foreach(Image image in images) {
                if (fade) {
                    StartCoroutine(FadeImage(image, opacity, fadeSpeed));
                } else {
                    image.color = new Color(image.color.r, image.color.g, image.color.b, opacity/100f);
                }
            }

            Text[] texts = obj.GetComponentsInChildren<Text>();
            foreach (Text text in texts) {
                if (fade) {
                    StartCoroutine(FadeText(text, opacity, fadeSpeed));
                } else {
                    text.color = new Color(text.color.r, text.color.g, text.color.b, opacity/100f);
                }
            }
        }
    }

    IEnumerator FadeImage(Image image, float opacity, float fadeSpeed) {
        float timeElapsed = 0;
        while (timeElapsed < fadeSpeed) {
            timeElapsed += Time.deltaTime;
            image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.Lerp(image.color.a, opacity/100f, timeElapsed/fadeSpeed));
            yield return null;
        }
    }

    IEnumerator FadeText(Text text, float opacity, float fadeSpeed) {
        float timeElapsed = 0;
        while (timeElapsed < fadeSpeed) {
            timeElapsed += Time.deltaTime;
            text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.Lerp(text.color.a, opacity/100f, timeElapsed/fadeSpeed));
            yield return null;
        }
    }

    void ToggleAll(bool state) {
        foreach (UIGroup group in groups) {
            foreach (GameObject obj in group.elements) {
                obj.SetActive(state);
            }
        }
    }

    UIGroup GetGroup(string state) {
        foreach (UIGroup group in groups) {
            if (group.state == state) {
                return group;
            }
        }

        return null;
    }
    
    List<TextElement> GetElementsByTag(string tag) {
        List<TextElement> elements = new List<TextElement>();
        foreach (TextElement element in textElements) {
            if (element.tag == tag) elements.Add(element);
        }

        return elements;
    }
}
