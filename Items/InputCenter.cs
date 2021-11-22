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
    public bool spaceKeyEnter { get; set; }
    public float zoom { get; set; }
    public Vector2 look { get; set; }
    public Vector3 mousePos { get; set; }
    public Vector2 mouseScreenPos { get; set; }
    public Vector2 mouseMove { get; set; }
    public Vector2 move { get; set; }
    public bool mouseLeftKeyHold { get; set; }
    public bool mouseLeftKeyDown { get; set; }
    public bool mouseRightKeyHold { get; set; }
    public bool mouseRightKeyDown { get; set; }
    public bool mouseMiddleKeyHold { get; set; }
    public bool leftShiftKeyHold { get; set; }

    private Vector2 lookTemp;
    private Vector2 mouseScreenPosTemp;
    private Vector2 mouseMoveTemp;
    private Vector2 moveTemp;
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

        moveTemp.x = Input.GetAxisRaw("Horizontal");
        moveTemp.y = Input.GetAxisRaw("Vertical");
        move = moveTemp;

        lookTemp.x = Input.GetAxisRaw("Mouse X");
        lookTemp.y = -Input.GetAxisRaw("Mouse Y");
        look = lookTemp;
        mouseMoveTemp.x = -look.x;
        mouseMoveTemp.y = look.y;
        mouseMove = mouseMoveTemp;
        mousePos = Input.mousePosition;
        mouseScreenPosTemp.x = mousePos.x;
        mouseScreenPosTemp.y = Screen.height - mousePos.y;
        mouseScreenPos = mouseScreenPosTemp;

        if (Input.GetMouseButtonDown(0) && !mouseOnUI)
        {
            mouseLeftKeyHold = true;
            mouseLeftKeyDown = true;
        }
        else
        {
            mouseLeftKeyDown = false;
        }
        if (Input.GetMouseButtonUp(0))
        {
            mouseLeftKeyHold = false;
        }
        if (Input.GetMouseButtonDown(1) && !mouseOnUI)
        {
            mouseRightKeyHold = true;
            mouseRightKeyDown = true;
        }
        else
        {
            mouseRightKeyDown = false;
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
