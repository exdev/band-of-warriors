// ======================================================================================
// File         : BtnCallCharSelect.cs
// Author       : Wang Nan 
// Last Change  : 10/31/2011 | 16:20:54 PM | Monday,October
// Description  : 
// ======================================================================================

///////////////////////////////////////////////////////////////////////////////
// usings
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

///////////////////////////////////////////////////////////////////////////////
//
///////////////////////////////////////////////////////////////////////////////

public class BtnCallCharSelect : MonoBehaviour {

    ///////////////////////////////////////////////////////////////////////////////
    // serialize properties
    ///////////////////////////////////////////////////////////////////////////////
    public exUIPanel charSelectPanel;

    ///////////////////////////////////////////////////////////////////////////////
    //
    ///////////////////////////////////////////////////////////////////////////////

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

	void Awake () {
        exUIButton uiButton = GetComponent<exUIButton>();
        uiButton.OnButtonPress += OnButtonPress;
	}

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void OnButtonPress () {
        Time.timeScale = 0;
        charSelectPanel.transform.position 
            = new Vector3 ( Camera.main.transform.position.x , 0, charSelectPanel.transform.position.z );
        charSelectPanel.enabled = true;
    }
}


