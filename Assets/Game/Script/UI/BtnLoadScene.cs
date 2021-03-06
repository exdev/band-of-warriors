// ======================================================================================
// File         : BtnLoadScene.cs
// Author       : Wu Jie 
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

public class BtnLoadScene : MonoBehaviour {

    ///////////////////////////////////////////////////////////////////////////////
    // serialize properties
    ///////////////////////////////////////////////////////////////////////////////

	public string ip4SceneName = "";

    ///////////////////////////////////////////////////////////////////////////////
    //
    ///////////////////////////////////////////////////////////////////////////////

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

	void Awake () {
        exUIButton uiButton = GetComponent<exUIButton>();
        uiButton.OnButtonRelease += OnButtonRelease;
	}

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void OnButtonRelease () {
        Application.LoadLevel(ip4SceneName);
    }
}
