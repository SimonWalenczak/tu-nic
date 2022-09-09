using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

namespace Tunic
{
    public class TutoMenu : MonoBehaviour
    {
	    [SerializeField] Button returnButton,nextPageButton,previousPageButton;
	    
	    GameObject[] pages;
	    int maxPages;
	    int currentPage;
	    protected void Awake()
	    {
	    	returnButton.onClick.AddListener(Return);
	    	nextPageButton.onClick.AddListener(NextPage);
	    	previousPageButton.onClick.AddListener(PreviousPage);
	    	maxPages=transform.childCount-3;
	    	var children = new List<GameObject>();
	    	foreach (Transform child in transform)
	    	{
	    		if (child.gameObject==returnButton.gameObject)continue;
	    		if (child.gameObject==nextPageButton.gameObject)continue;
	    		if (child.gameObject==previousPageButton.gameObject)continue;
	    		children.Add(child.gameObject	);
	    	}
	    	pages= children.ToArray();
	    	UpdateButtons();
	    }
	    
	    void Return(){
	    	gameObject.SetActive(false);
	    }
	    
	    void NextPage(){
	    	currentPage++;
	    	UpdateButtons();
	    }
	    
	    void PreviousPage(){
		    currentPage--;
		    UpdateButtons();
	    }
	    
	    void UpdateButtons(){
		    nextPageButton.gameObject.SetActive(currentPage>=maxPages);
		    previousPageButton.gameObject.SetActive(currentPage>0);
	    }
    }
}
