using NonsensicalKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class InputCenter : MonoSingleton<InputCenter>
{
    public bool spaceKeyEnter;
    public float zoom;
    public Vector2 look;
    public Vector2 mouseMove;
    public Vector2 move;
    public bool mouseLeftKeyHold;
    public bool mouseRightKeyHold;
    public bool mouseMiddleKeyHold;
    public bool leftShiftKeyHold;

    private EventSystem eventSystem;

    private bool mouseOnUI
    {
        get
        {
            if (eventSystem == null)
            {
                return false;
            }
            else
            {
                return eventSystem.IsPointerOverGameObject();
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();

        UnityEngine.SceneManagement.SceneManager.sceneLoaded += (a1, a2) => { eventSystem = EventSystem.current; };
    }

    private void Start()
    {
        eventSystem = EventSystem.current;
    }

    private void Update()
    {
#if ENABLE_INPUT_SYSTEM

#else
        zoom = Input.GetAxisRaw("Mouse ScrollWheel");

        move.x = Input.GetAxisRaw("Horizontal");
        move.y = Input.GetAxisRaw("Vertical");

        look.x = Input.GetAxisRaw("Mouse X");
        look.y = -Input.GetAxisRaw("Mouse Y");

        mouseMove.x = -look.x;
        mouseMove.y = look.y;

        if (Input.GetMouseButtonDown(0) && !mouseOnUI)
        {
            mouseLeftKeyHold = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            mouseLeftKeyHold = false;
        }
        if (Input.GetMouseButtonDown(1) && !mouseOnUI)
        {
            mouseRightKeyHold = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            mouseRightKeyHold = false;
        }
        if (Input.GetMouseButtonDown(2) && !mouseOnUI)
        {
            mouseMiddleKeyHold = true;
        }
        if (Input.GetMouseButtonUp(2))
        {
            mouseMiddleKeyHold = false;
        }

        leftShiftKeyHold = Input.GetKey(KeyCode.LeftShift);
        spaceKeyEnter = Input.GetKey(KeyCode.Space);
#endif
    }
}
