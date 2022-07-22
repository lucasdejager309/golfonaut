using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    Vector2 worldPos;
    Vector2 startPos;
    Vector2 currentPos;

    bool trackShot;

    Camera cam;
    PlayerScript player;
    
    void Start() {
        cam = Camera.main;
        player = GameObject.FindObjectOfType<PlayerScript>();
        
    }

    public void GetInput() {
        if (Input.touchCount > 0) {
            for (int i = 0; i < Input.touchCount; i++) {
                Touch touch = Input.GetTouch(i);
                worldPos = cam.ScreenToWorldPoint(touch.position);

                if (!IsPointerOverUIObject()) {
                    switch (touch.phase) {
                        case TouchPhase.Began:
                            Began();
                            break;
                        case TouchPhase.Moved:
                            Moved();
                            break;
                        case TouchPhase.Stationary:
                            Stationary();
                            break;
                        case TouchPhase.Ended:
                            Ended();
                            break;
                        default:
                            break;
                    }
                }
                
            }
        }
    }

    void Began() {
        FindObjectOfType<UIManager>().SetOpacity("IN_GAME", 20f, true, 0.5f);

        startPos = worldPos;
        currentPos = worldPos;
    }

    void Moved() {
        currentPos = worldPos;
        player.DrawLine(startPos, currentPos);
    }

    void Stationary() {
        player.DrawLine(startPos, currentPos);
    }

    void Ended() {
        FindObjectOfType<UIManager>().SetOpacity("IN_GAME", 100f, true, 0.5f);

        currentPos = worldPos;
        Vector2 inputVector = currentPos - startPos;
        player.gameManager.Move(-inputVector);
        player.HideLine();
    }

    //NOT MY CODE: https://answers.unity.com/questions/1115464/ispointerovergameobject-not-working-with-touch-inp.html
    private bool IsPointerOverUIObject() {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
