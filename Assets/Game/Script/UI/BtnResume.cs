// ======================================================================================
// File         : BtnResume.cs
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

public class BtnResume : MonoBehaviour {

    ///////////////////////////////////////////////////////////////////////////////
    // serialize properties
    ///////////////////////////////////////////////////////////////////////////////
    public exUIPanel pausePanel;

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
        Time.timeScale = 1.0f;
        pausePanel.transform.position 
            = new Vector3 ( pausePanel.transform.position.x, 1500.0f, pausePanel.transform.position.z );
        pausePanel.enabled = false;
        Game.instance.AcceptInput(true);
    }
}

