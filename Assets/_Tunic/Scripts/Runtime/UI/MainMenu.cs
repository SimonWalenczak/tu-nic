using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Tunic
{
    public class MainMenu : MonoBehaviour
    {
	    [SerializeField]Button playbutton,tutoButton,quitButton;
	    
	    // Awake is called when the script instance is being loaded.
	    protected void Awake()
	    {
	    	playbutton.onClick.AddListener(Play);
	    	tutoButton.onClick.AddListener(Tuto);
	    	quitButton.onClick.AddListener(Quit);
	    }
	    
	    void Play(){
	    	Debug.Log	("Play");
	    	SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	    }
	    
	    void Tuto(){
	    	Debug.Log	("Tuto");
	    }
	    
	    void Quit(){
	    	Debug.Log	("Quit");
#if UNITY_EDITOR
		    UnityEditor.EditorApplication.isPlaying = false;
#else
		    Application.Quit();
#endif
	    }
    }
}
