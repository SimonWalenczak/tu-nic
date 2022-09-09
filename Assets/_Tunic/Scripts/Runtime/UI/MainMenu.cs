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
	    [SerializeField] TutoMenu tuto;
	    
	    // Awake is called when the script instance is being loaded.
	    protected void Awake()
	    {
	    	playbutton.onClick.AddListener(Play);
	    	tutoButton.onClick.AddListener(Tuto);
	    	quitButton.onClick.AddListener(Quit);
	    }
	    
	    protected void Start()
	    {
		    tuto	.gameObject.SetActive(false);
	    }
	    
	    void Play(){
	    	Debug.Log	("Play");
	    	SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	    }
	    
	    void Tuto(){
	    	Debug.Log	("Tuto");
	    	tuto	.gameObject.SetActive(true);
	    	playbutton.transform.parent.gameObject.SetActive(false);
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
