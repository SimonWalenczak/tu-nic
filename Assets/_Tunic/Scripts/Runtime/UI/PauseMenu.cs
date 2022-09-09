using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

namespace Tunic
{
	public class PauseMenu : MonoBehaviour
	{
		[SerializeField] InputActionReference menuInput;
		[SerializeField] Button resumebutton,tutoButton,quitButton;
		[SerializeField] TutoMenu tuto;
	    
		// Awake is called when the script instance is being loaded.
		protected void Awake()
		{
			menuInput.action.Enable();
			menuInput.action.performed+=ToggleMenu;
			resumebutton.onClick.AddListener(Resume);
			tutoButton.onClick.AddListener(Tuto);
			quitButton.onClick.AddListener(MainMenu);
			gameObject.SetActive(false);
		}
		
		// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
		protected void Start()
		{
			tuto.gameObject.SetActive(false);
		}
		
		
		void ToggleMenu(InputAction.CallbackContext c){
			bool isOn=gameObject.active;
			gameObject.SetActive(!isOn);
			Time.timeScale=isOn?1:0;
		}
	    
		void Resume(){
			Debug.Log	("Resume");
			gameObject.SetActive(false);
			Time.timeScale=1;
		}
	    
		void Tuto(){
			Debug.Log	("Tuto");
			tuto	.gameObject.SetActive(true);
			resumebutton.transform.parent.gameObject.SetActive(false);
		}
	    
		void MainMenu(){
			Debug.Log	("MainMenu");
			SceneManager.LoadScene(0);
			Time.timeScale=1;
		}
		// Sent to all game objects before the application is quit.
		protected void OnApplicationQuit()
		{
			menuInput.action.performed-=ToggleMenu;
		}
	}
}
