using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



[System.Serializable]
public class UIGroup {
    public string state;
    public GameObject[] elements;
}

public class UIManager : MonoBehaviour
{
    [Header("All UIElements")]
    public GameObject[] UIElements;
    [Header("UI Groups")]
    public UIGroup[] groups;

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
        foreach (GameObject obj in UIElements) {
            obj.SetActive(state);
        }
    }
}
