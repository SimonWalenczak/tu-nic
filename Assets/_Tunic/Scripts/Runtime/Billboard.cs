using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tunic
{
    public class Billboard : MonoBehaviour
	{
		Transform cam;
        void Start()
        {
        	cam = Camera.main.transform;
        }

        void Update()
        {
        	transform.LookAt(cam);
        }
    }
}
