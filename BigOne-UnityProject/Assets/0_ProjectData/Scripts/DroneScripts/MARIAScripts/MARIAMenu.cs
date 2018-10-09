using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MARIAMenu : MonoBehaviour {
	public enum MenuPanel {
		Main = 0,
		Scan = 1,
		MovementMode = 2,
		AudioSources = 3,
		Options = 4,
		Settings = 5,
		Controls = 6
	}

	public enum BehaviourButtons {
		Follow = 0,
		Patrol = 1
	}

	[Header("Face")]
	public GameObject faceScreen;

	[Header("Menu panels")]
	public GameObject mainMenu;
	public GameObject scanMonitor;
	public GameObject movementModeMenu;
	public GameObject audioSourcesMenu;
	public GameObject optionsMenu;
	public GameObject settingsMenu;
	public GameObject controlsMenu;
	public GameObject followText;
	public GameObject patrolText;

	[Header("Buttons")]
	public GameObject followButton;
	public GameObject selectedFollowButton;
	public GameObject patrolButton;
	public GameObject selectedPatrolButton;
	//public GameObject backButton;

	[Header("Scan Screen Elements")]
	public GameObject scanCamera;
	public Texture scanTexture;
	public Texture dummyTexture;

	[Header("Utilities")]
	public float rotationTime = 1f;

	// Events
	[Space]
	public GameEvent OnMenuSwitchOn = null;
	public GameEvent OnMenuSwitchOff = null;

	// Private
	private GameObject currentMenu;
	private bool isMenuActive = false;
	private bool isCameraActive = false;

	private Renderer myBeautifulRenderer;
	private MARIAGrabAttach grab;
	private MARIAManager mariaManager;
	private Animator animator;

	private void Start()
	{
		grab = GetComponent<MARIAGrabAttach>();
		mariaManager = GetComponent<MARIAManager>();
		scanCamera.SetActive( false );
		myBeautifulRenderer = scanMonitor.GetComponent<MeshRenderer>();
		animator = GameObject.Find( "Maria" ).GetComponent<Animator>();
	}

	private void Update()
	{
		// TEMPORARY
		if ( Input.GetKeyDown( KeyCode.Alpha1 ) )
		{
			SetScreenRotation( isMenuActive );
		}

		// TEMPORARY
		if ( Input.GetKeyDown( KeyCode.Alpha2 ) && isMenuActive )
		{
			SetCameraActivation( isCameraActive );
		}
	}

	private void SetScreenRotation( bool setToOff )
	{
		if ( setToOff && isMenuActive )
		{
			faceScreen.transform.DOLocalRotate( new Vector3( 0, 0, 0 ), rotationTime ).OnComplete( HiddenMenuCallback );
			isMenuActive = false;
			if ( OnMenuSwitchOff != null )
				OnMenuSwitchOff.Invoke();

		} 
		else if ( !setToOff && !isMenuActive )
		{
			GoToMenu( ( int ) MenuPanel.Main );
			faceScreen.transform.DOLocalRotate( new Vector3( 0, 180, 0 ), rotationTime );
			isMenuActive = true;
			if ( OnMenuSwitchOn != null )
				OnMenuSwitchOn.Invoke();
		}
	}

	void HiddenMenuCallback()
	{
		mainMenu.SetActive( false );
		scanMonitor.SetActive( false );
		movementModeMenu.SetActive( false );
		audioSourcesMenu.SetActive( false );
		optionsMenu.SetActive( false );
		settingsMenu.SetActive( false );
		controlsMenu.SetActive( false );

		SetCameraActivation( true );
	}

	private void SetCameraActivation( bool setToOff )
	{
		if ( setToOff && isCameraActive )
		{
			isCameraActive = false;

			myBeautifulRenderer.material.SetTexture( "_MainTex", dummyTexture );

			scanCamera.SetActive( false );
		} else if ( !setToOff && !isCameraActive )
		{
			isCameraActive = true;

			myBeautifulRenderer.material.SetTexture( "_MainTex", scanTexture );
			scanCamera.SetActive( true );
		}
	}

	public void SwitchMenuOn()
	{
		if ( !grab || grab.IsGrabbed )
		{
			SetScreenRotation( false );
			//backButton.SetActive(true);
		}
	}
	public void SwitchMenuOff()
	{
		SetScreenRotation( true );
		//backButton.SetActive(false);
	}

	public void GoToMenu( int menuPanelIndex )
	{
		if ( currentMenu )
		{
			if ( currentMenu == scanMonitor )
				SetCameraActivation( true );
			currentMenu.SetActive( false );
		}

		switch ( ( MenuPanel ) menuPanelIndex )
		{
			case MenuPanel.Main:
				//backButton.SetActive(true);
				currentMenu = mainMenu;
				break;
			case MenuPanel.Scan:
				//backButton.SetActive(true);
				currentMenu = scanMonitor;
				SetCameraActivation( false );
				break;
			case MenuPanel.MovementMode:
				//backButton.SetActive(true);
				currentMenu = movementModeMenu;
				InitializeMenu( mariaManager.currentMovement.GetType().ToString() );
				break;
			case MenuPanel.AudioSources:
				//backButton.SetActive(true);
				currentMenu = audioSourcesMenu;
				break;
			case MenuPanel.Options:
				//backButton.SetActive(true);
				currentMenu = optionsMenu;
				break;
			case MenuPanel.Settings:
				//backButton.SetActive(true);
				currentMenu = settingsMenu;
				break;
			case MenuPanel.Controls:
				//backButton.SetActive(true);
				currentMenu = controlsMenu;
				break;
		}

		if ( currentMenu != null )
		{
			currentMenu.SetActive( true );

			if ( ( MenuPanel ) menuPanelIndex != MenuPanel.Main )
			{
				//backButton.SetActive(true);
			} else
			{
				//backButton.SetActive(false);
			}
		}
	}

	public void InitializeMenu( string movement )
	{
		movement = movement.ToUpper();

		if ( movement == "FOLLOW" )
		{
			SwapButtons( BehaviourButtons.Follow );
		} else if ( movement == "PATROL" )
		{
			SwapButtons( BehaviourButtons.Patrol );
		}
	}

	public void SwapButtons( BehaviourButtons button )
	{
		if ( button == BehaviourButtons.Follow )
		{
			animator.ResetTrigger( "BecomesSad" );
			followText.SetActive( true );
			patrolText.SetActive( false );
			followButton.SetActive( false );
			selectedFollowButton.SetActive( true );
			patrolButton.SetActive( true );
			selectedPatrolButton.SetActive( false );
		} else if ( button == BehaviourButtons.Patrol )
		{
			animator.ResetTrigger( "BecomesHappy" );
			followText.SetActive( false );
			patrolText.SetActive( true );
			patrolButton.SetActive( false );
			selectedPatrolButton.SetActive( true );
			followButton.SetActive( true );
			selectedFollowButton.SetActive( false );
		}
	}
	public void SwapButtons( int buttonIndex )
	{
		SwapButtons( ( BehaviourButtons ) buttonIndex );
	}

	public void BackFunction()
	{
		GoToMenu( ( int ) MenuPanel.Main );
	}
}
