﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

/// <summary>
/// This class inherits from "SteamVR_TrackedController" script.
/// Button functions will be called once
/// </summary>
/// <example>
/// <code>
/// public override void OnMenuClicked(ClickedEventArgs e)
/// {
///     base.OnMenuClicked(e);
/// }
/// </code>
/// </example>
public class ControllerHandler : SteamVR_TrackedController {
		
	private GameObject gameManager;
	private ResponseGetter response_script;
	private Vector2 touchPos;
	private Vector2 unTouchPos;
    private Vector2 clickPos;

	/// <summary>
	/// This function tells game manager to generate stimulus
	/// </summary>
	/// <remarks>
	/// This function will be called once trigger has been pressed
	/// </remarks>
	public override void OnTriggerClicked(ClickedEventArgs e)
	{
		base.OnTriggerClicked(e);
		
		gameManager.GetComponent<GameManager>().AcceptSignal();	
	}
		
	
	public override void OnPadClicked(ClickedEventArgs e)
	{
        if (Experiment.InputMethod == "c")
        {
            base.OnPadClicked(e);
            clickPos.x = e.padX;
            clickPos.y = e.padY;
        }
		//response_script.GetTouchResponse(true);
	}
	
	public override void OnPadUnclicked(ClickedEventArgs e)
	{
		base.OnPadUnclicked(e);
        DeterminePadLocationClick();
        //response_script.GetTouchResponse(false);
	}
	
	/// <summary>
	/// This function records the position where user touches pad
	/// </summary>
	public override void OnPadTouched(ClickedEventArgs e)
	{
        if (Experiment.InputMethod == "s")
        {
            base.OnPadTouched(e);

            touchPos.x = e.padX;
            touchPos.y = e.padY;
        }
    }
	
	/// <summary>
	/// This function records the position where user leaves pad
	/// </summary>
	public override void OnPadUntouched(ClickedEventArgs e)
	{
		base.OnPadUntouched(e);
		
		unTouchPos.x = controllerState.rAxis0.x;
		unTouchPos.y = controllerState.rAxis0.y;
		DeterminPadDirectionSwipe();
	}

    // Use this for initialization
    protected override void Start () {
		base.Start();
		
		// Find the game manager and ResponseGetter object
		gameManager = GameObject.Find("Game Manager");	
		GameObject response_obj = GameObject.Find("ResponseModule");
		response_script = response_obj.GetComponent<ResponseGetter>();
	}
	
	// Update is called once per frame
	protected override void Update()
	{
		base.Update();	
	}
	
	
	/// <summary>
	/// This function determines the direction where user swipes the pad
	/// </summary>
	public void DeterminPadDirectionSwipe(){
		Vector2 direction = unTouchPos - touchPos;

        if (Experiment.InputAxis == "vertical")
		if( direction.y > 0.3 ){
			Debug.Log("Up");
			response_script.GetSwipeResponse(0);
		} else if( direction.y < -0.3 ){
			Debug.Log("Down");
			response_script.GetSwipeResponse(1);
		}
        if (Experiment.InputAxis == "horizontal")
        {
            if (direction.x < -0.3)
            {
                Debug.Log("User entered: Left");
                response_script.GetSwipeResponse(2);
            }
            else if (direction.x > 0.3)
            {
                Debug.Log("User entered: Right");
                response_script.GetSwipeResponse(3);
            }
        }
	}
    public void DeterminePadLocationClick()
    {
        Vector2 location = clickPos;
        if(Experiment.InputAxis == "vertical")
        {
            if (location.y < 0)
            {
                Debug.Log("User entered: Down:");
                response_script.GetClickResponse(0);
            }
            else
            {
                Debug.Log("User entered: Up");
                response_script.GetClickResponse(1);
            }
        }
        if (Experiment.InputAxis == "horizontal")
        {
            if (location.x < 0)
            {
                Debug.Log("User entered: Left");
                response_script.GetSwipeResponse(2);
            }
            else
            {
                Debug.Log("User entered: Right");
                response_script.GetSwipeResponse(3);
            }
        }
    }
	
}
