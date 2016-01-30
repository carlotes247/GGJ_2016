using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Serialization;

public class WiimoteInputModule : PointerInputModule {

    private Vector2 m_LastWiimotePosition;
    private Vector2 m_WiimotePosition;

    [SerializeField]
    private float m_InputActionsPerSecond = 10;

    [SerializeField]
    private float m_RepeatDelay = 0.5f;

    /// <summary>
    /// (Field) Forces the module to be active if true
    /// </summary>
    [SerializeField]   
    private bool m_ForceModuleActive;

    /// <summary>
    /// (Property) Forces the module to be active if true
    /// </summary>
    public bool forceModuleActive
    {
        get { return m_ForceModuleActive; }
        set { m_ForceModuleActive = value; }
    }

    public float inputActionsPerSecond
    {
        get { return m_InputActionsPerSecond; }
        set { m_InputActionsPerSecond = value; }
    }

    public float repeatDelay
    {
        get { return m_RepeatDelay; }
        set { m_RepeatDelay = value; }
    }

    /// <summary>
    /// The pointer class to work with
    /// </summary>
    private PointerEventData wiimotePointer;

    /// <summary>
    /// Update the internal state of the Module. Called on all modules before process is sent to the one activated module.
    /// (Called even if the module should not be active)
    /// </summary>
    public override void UpdateModule()
    {
        // We populate the variables of the wiimote this frame
        //Toolbox.Instance.GameManager.InputController.WiimoteInput.WiimoteInputLogic();

        m_LastWiimotePosition = m_WiimotePosition;
        m_WiimotePosition = Toolbox.Instance.GameManager.InputController.WiimoteInput.CursorPosition;
    }

    /// <summary>
    /// If any of the conditions is true, the module is supported in the game
    /// </summary>
    /// <returns> True if any of the conditions is true</returns>
    public override bool IsModuleSupported()
    {
        /* If the module is force to be active 
        OR the mouse is present AND InputManager.InputType is Wiimote
        returns true
        */
        return m_ForceModuleActive || Input.mousePresent && Toolbox.Instance.GameManager.InputController.InputType == InputController.TypeOfInput.WiiMote;
    }

    /// <summary>
    /// If the module should be activated (Apparently, the function only executes if the Module is active)
    /// </summary>
    /// <returns></returns>
    public override bool ShouldActivateModule()
    {
        
        // If ShouldActivateModule is false in the base, we return false
        if (!base.ShouldActivateModule())
            return false;

        // We populate the variables of the wiimote this frame
        Toolbox.Instance.GameManager.InputController.WiimoteInput.WiimoteInputLogic();

        // We save the value of ForceModuleActive in shouldActivate 
        var shouldActivate = m_ForceModuleActive;
        
        // |= is the OR assignment operator ( x |= y ) is ( x = x | y )
        // If the position of the wiimote has moved, we save true in shouldActivate, if not the value of m_ForceModuleActive stays
        shouldActivate |= (m_WiimotePosition - m_LastWiimotePosition).sqrMagnitude > 0.0f;
        // If the buttonB of the wiimote has been pressed, we save true in shouldActivate, if not the value of m_ForceModuleActive stays
        shouldActivate |= Toolbox.Instance.GameManager.InputController.WiimoteInput.ButtonB;
        
        // We return whatever the value is from the checks in the function
        return shouldActivate;
    }

    /// <summary>
    /// Called when the module is activated. Override this if you want custom code to execute when you activate your module
    /// </summary>
    public override void ActivateModule()
    {
        // We populate the variables of the wiimote this frame
        Toolbox.Instance.GameManager.InputController.WiimoteInput.WiimoteInputLogic();

        base.ActivateModule();
        m_WiimotePosition = Toolbox.Instance.GameManager.InputController.WiimoteInput.CursorPosition;
        m_LastWiimotePosition = Toolbox.Instance.GameManager.InputController.WiimoteInput.CursorPosition;

        var toSelect = eventSystem.currentSelectedGameObject;
        if (toSelect == null)
            toSelect = eventSystem.firstSelectedGameObject;

        eventSystem.SetSelectedGameObject(toSelect, GetBaseEventData());
    }

    /// <summary>
    /// Called when the module is deactivated. Override this if you want custom code to execute when you deactivate your module.
    /// </summary>
    public override void DeactivateModule()
    {
        base.DeactivateModule();
        ClearSelection();
    }

    // Process is called once per tick
    public override void Process()
    {
        bool usedEvent = SendUpdateEventToSelectedObject();

        if (eventSystem.sendNavigationEvents)
        {
            if (!usedEvent)
            {
                SendSubmitEventToSelectedObject();
            }
        }
            ProcessWiimoteEvent();
    }

    /// <summary>
    /// Process submit keys
    /// </summary>
    /// <returns> Returns true if submit or cancel were received by selected object</returns>
    private bool SendSubmitEventToSelectedObject()
    {
        if (eventSystem.currentSelectedGameObject == null)
            return false;

        var data = GetBaseEventData();
        if (Toolbox.Instance.GameManager.InputController.WiimoteInput.ButtonB)
            ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, data, ExecuteEvents.submitHandler);

        //if (Input.GetButtonDown(m_CancelButton))
        //    ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, data, ExecuteEvents.cancelHandler);
        return data.used;
    }

    /// <summary>
    /// See if the selected object has used the event
    /// </summary>
    /// <returns>Return true if it used the event</returns>
    private bool SendUpdateEventToSelectedObject()
    {
        if (eventSystem.currentSelectedGameObject == null)
        {
            return false;
        }

        // Generates a BaseEventData that can be used by the event system
        var data = GetBaseEventData();
        ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, data, ExecuteEvents.updateSelectedHandler);
        // return if the event is used
        return data.used;
    }


    private void ProcessWiimoteEvent()
    {
        // Getting objects related to player
        GetPointerData(0, out wiimotePointer, true);

        // We update the pointer in screen coords
        //wiimotePointer.position = Toolbox.Instance.GameManager.InputController.WiimoteInput.CursorPosition;ç
        wiimotePointer.position = m_WiimotePosition;

        // Raycasting
        eventSystem.RaycastAll(wiimotePointer, this.m_RaycastResultCache);
        RaycastResult raycastResult = FindFirstRaycast(this.m_RaycastResultCache);
        wiimotePointer.pointerCurrentRaycast = raycastResult;
        // We process the press of the pointer (PointerDown, PointerUp)
        this.ProcessWiimotePress(wiimotePointer);
        // We process the move of the pointer
        this.ProcessMove(wiimotePointer);
        // We process the drag of the pointer
        this.ProcessDrag(wiimotePointer);


        wiimotePointer.clickCount = 0;
        // If cursor click...
        if ( Toolbox.Instance.GameManager.InputController.WiimoteInput.ButtonB)
        {
            // Click
            wiimotePointer.pressPosition = Toolbox.Instance.GameManager.InputController.WiimoteInput.CursorPosition;
            wiimotePointer.clickTime = Time.unscaledTime;
            wiimotePointer.pointerPressRaycast = raycastResult;

            wiimotePointer.clickCount = 1;
            wiimotePointer.eligibleForClick = true;

            // Dragging
            wiimotePointer.dragging = false;
            wiimotePointer.useDragThreshold = false;            
            
            // If there are anyobjects in front of the cursor (the ray hitted something)
            if (this.m_RaycastResultCache.Count > 0)
            {
                wiimotePointer.selectedObject = raycastResult.gameObject;
                
                // search for the control that will receive the press
                // if we can't find a press handler set the press
                // handler to be what would receive a click.
                var newPressed = ExecuteEvents.ExecuteHierarchy(wiimotePointer.selectedObject, wiimotePointer, ExecuteEvents.pointerDownHandler);
                // didnt find a press handler... search for a click handler
                if (newPressed == null)
                {
                    newPressed = ExecuteEvents.GetEventHandler<IPointerClickHandler>(wiimotePointer.selectedObject);
                }                    

                //wiimotePointer.pointerPress = ExecuteEvents.ExecuteHierarchy(raycastResult.gameObject, wiimotePointer, ExecuteEvents.submitHandler);
                wiimotePointer.pointerPress = newPressed;
                wiimotePointer.rawPointerPress = raycastResult.gameObject;

                wiimotePointer.clickTime = Time.unscaledTime;

                // save the drag handler as well
                wiimotePointer.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(wiimotePointer.selectedObject);

                if (wiimotePointer.pointerDrag != null)
                {
                    ExecuteEvents.Execute(wiimotePointer.pointerDrag, wiimotePointer, ExecuteEvents.initializePotentialDrag);
                }
            }
            else
            {
                wiimotePointer.selectedObject = null;
                wiimotePointer.pointerPress = null;
                wiimotePointer.rawPointerPress = null;
            }
        }
        // If not cursor click...
        else
        {
            wiimotePointer.clickCount = 0;
            wiimotePointer.eligibleForClick = false;
            wiimotePointer.pointerPress = null;
            wiimotePointer.rawPointerPress = null;
        }

    }

    /// <summary>
    /// Process the current WiimotePress (PointerDown, PointerUp)
    /// </summary>
    /// <param name="data"> The wiimotePointer to pass in </param>
    protected void ProcessWiimotePress(PointerEventData data)
    {
        // We already have the buttonData
        var pointerEvent = data;
        var currentOverGo = pointerEvent.pointerCurrentRaycast.gameObject;

        // PointerDown notification (pressed this frame)
        if (Toolbox.Instance.GameManager.InputController.WiimoteInput.ButtonB)
        {
            pointerEvent.eligibleForClick = true;
            pointerEvent.delta = Vector2.zero;
            pointerEvent.dragging = false;
            pointerEvent.useDragThreshold = true;
            pointerEvent.pressPosition = pointerEvent.position;
            pointerEvent.pointerPressRaycast = pointerEvent.pointerCurrentRaycast;

            DeselectIfSelectionChanged(currentOverGo, pointerEvent);

            // search for the control that will receive the press
            // if we can't find a press handler set the press
            // handler to be what would receive a click.
            var newPressed = ExecuteEvents.ExecuteHierarchy(currentOverGo, pointerEvent, ExecuteEvents.pointerDownHandler);

            // didnt find a press handler... search for a click handler
            if (newPressed == null)
                newPressed = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentOverGo);

            // Debug.Log("Pressed: " + newPressed);

            float time = Time.unscaledTime;

            if (newPressed == pointerEvent.lastPress)
            {
                var diffTime = time - pointerEvent.clickTime;
                if (diffTime < 0.3f)
                    ++pointerEvent.clickCount;
                else
                    pointerEvent.clickCount = 1;

                pointerEvent.clickTime = time;
            }
            else
            {
                pointerEvent.clickCount = 1;
            }

            pointerEvent.pointerPress = newPressed;
            pointerEvent.rawPointerPress = currentOverGo;

            pointerEvent.clickTime = time;

            // Save the drag handler as well
            pointerEvent.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(currentOverGo);

            if (pointerEvent.pointerDrag != null)
                ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.initializePotentialDrag);
        }

        // PointerUp notification (NOT pressed this frame)
        if (!Toolbox.Instance.GameManager.InputController.WiimoteInput.ButtonB)
        {
            // Debug.Log("Executing pressup on: " + pointer.pointerPress);
            ExecuteEvents.Execute(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerUpHandler);

            // Debug.Log("KeyCode: " + pointer.eventData.keyCode);

            // see if we mouse up on the same element that we clicked on...
            var pointerUpHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentOverGo);

            // PointerClick and Drop events
            if (pointerEvent.pointerPress == pointerUpHandler && pointerEvent.eligibleForClick)
            {
                ExecuteEvents.Execute(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerClickHandler);
            }
            else if (pointerEvent.pointerDrag != null && pointerEvent.dragging)
            {
                ExecuteEvents.ExecuteHierarchy(currentOverGo, pointerEvent, ExecuteEvents.dropHandler);
            }

            pointerEvent.eligibleForClick = false;
            pointerEvent.pointerPress = null;
            pointerEvent.rawPointerPress = null;

            if (pointerEvent.pointerDrag != null && pointerEvent.dragging)
                ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.endDragHandler);

            pointerEvent.dragging = false;
            pointerEvent.pointerDrag = null;

            // redo pointer enter / exit to refresh state
            // so that if we moused over somethign that ignored it before
            // due to having pressed on something else
            // it now gets it.
            if (currentOverGo != pointerEvent.pointerEnter)
            {
                HandlePointerExitAndEnter(pointerEvent, null);
                HandlePointerExitAndEnter(pointerEvent, currentOverGo);
            }
        }
    }
}
