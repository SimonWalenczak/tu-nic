using System.Collections;
using System.Collections.Generic;
using UnityEngine;
	using UnityEngine.InputSystem;

namespace Tunic
{
	public class NewBehaviourScript : MonoBehaviour
    {
	    public InputActionReference i;
	    
	    // This function is called when the object becomes enabled and active.
	    protected void OnEnable()
	    {
	    	i.action.Enable();
	    }
	    
	    // This function is called when the behaviour becomes disabled () or inactive.
	    protected void OnDisable()
	    {
	    	i.action.Disable();
	    }
	    
	    // Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	    protected void Start()
	    {
	    	i.action.performed+=Look;
	    }
	    
	    private void Look(InputAction.CallbackContext c){
	    	Debug.Log(c);
	    }
    }
}
