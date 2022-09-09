using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Linq;

namespace Tunic
{
    public class TutoMenu : MonoBehaviour
    {
	    [SerializeField] Button returnButton,nextPageButton,previousPageButton;
	    
	    GameObject[] pages;
	    GameObject currentPageGO;
	    int maxPages;
	    int currentPage;
	    protected void Awake()
	    {
	    	returnButton.onClick.AddListener(Return);
	    	nextPageButton.onClick.AddListener(NextPage);
	    	previousPageButton.onClick.AddListener(PreviousPage);
	    	maxPages=transform.childCount-4;
	    	var children = new List<GameObject>();
	    	foreach (Transform child in transform)
	    	{
	    		if (child.gameObject==returnButton.gameObject)continue;
	    		if (child.gameObject==nextPageButton.gameObject)continue;
	    		if (child.gameObject==previousPageButton.gameObject)continue;
	    		children.Add(child.gameObject	);
	    		child.gameObject.SetActive(false);
	    	}
	    	pages= children.ToArray();
	    	UpdateButtons();
	    }
	    
	    void Return(){
	    	gameObject.SetActive(false);
	    	transform.parent.GetChild(1).gameObject.SetActive(true);
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
		    if(currentPageGO!=null)currentPageGO.SetActive(false);
	    	currentPageGO = pages[currentPage];
		    currentPageGO.SetActive(true	);
		    nextPageButton.gameObject.SetActive(currentPage<maxPages);
		    previousPageButton.gameObject.SetActive(currentPage>0);
		    
	    }
    }
}
