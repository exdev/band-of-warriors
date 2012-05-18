// ======================================================================================
// File         : exDebugHelper.cs
// Author       : Wu Jie 
// Last Change  : 06/05/2011 | 11:08:21 AM | Sunday,June
// Description  : 
// ======================================================================================

///////////////////////////////////////////////////////////////////////////////
// usings
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

///////////////////////////////////////////////////////////////////////////////
// defines
///////////////////////////////////////////////////////////////////////////////

// ------------------------------------------------------------------ 
// Desc: 
// ------------------------------------------------------------------ 

public class exDebugHelper : MonoBehaviour {

    ///////////////////////////////////////////////////////////////////////////////
    // static
    ///////////////////////////////////////////////////////////////////////////////

    // static instance
    private static exDebugHelper instance = null;

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public static void ScreenPrint ( string _text ) {
        if ( instance.showScreenPrint_ ) {
            instance.txtPrint.text = instance.txtPrint.text + _text + "\n"; 
        }
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public static void ScreenPrint ( Vector2 _pos, string _text ) {
        if ( instance.showScreenDebugText ) {
            exSpriteFont debugText = instance.debugTextPool.Request<exSpriteFont>();

            Vector2 screenPos = debugText.renderCamera.WorldToScreenPoint(_pos);
            exScreenPosition screenPosition = debugText.GetComponent<exScreenPosition>();
            screenPosition.x = screenPos.x;
            screenPosition.y = Screen.height - screenPos.y;

            debugText.text = _text;
            debugText.enabled = true;
        }
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public static void ScreenLog ( string _text ) {
        instance.logs.Add(_text);
        if ( instance.logs.Count > instance.logCount ) {
            instance.logs.RemoveAt(0);
        }
        instance.updateLogText = true;
    }

    ///////////////////////////////////////////////////////////////////////////////
    // serialized
    ///////////////////////////////////////////////////////////////////////////////

    public exSpriteFont txtPrint;
    public exSpriteFont txtFPS;
    public exSpriteFont txtLog;
    public exGameObjectPool debugTextPool = new exGameObjectPool();

    [SerializeField] protected bool showFps_ = true;
    public bool showFps {
        get { return showFps_; }
        set {
            if ( showFps_ != value ) {
                showFps_ = value;
                if ( txtFPS != null )
                    txtFPS.enabled = showFps_;
            }
        }
    }

    [SerializeField] protected bool showScreenPrint_ = true;
    public bool showScreenPrint {
        get { return showScreenPrint_; }
        set {
            if ( showScreenPrint_ != value ) {
                showScreenPrint_ = value;
                if ( txtPrint != null )
                    txtPrint.enabled = showScreenPrint_;
            }
        }
    }

    [SerializeField] protected bool showScreenLog_ = true;
    public bool showScreenLog {
        get { return showScreenLog_; }
        set {
            if ( showScreenLog_ != value ) {
                showScreenLog_ = value;
                if ( txtLog != null ) 
                    txtLog.enabled = showScreenLog_;
            }
        }
    }

    public bool showScreenDebugText = false;
    public int logCount = 10;

    ///////////////////////////////////////////////////////////////////////////////
    // non-serialized
    ///////////////////////////////////////////////////////////////////////////////

    [System.NonSerialized] public List<string> logs = new List<string>();
    [System.NonSerialized] public bool updateLogText = false; 
    private int frames = 0;
    private float fps = 0.0f;
    private float lastInterval = 0.0f;

    ///////////////////////////////////////////////////////////////////////////////
    // functions
    ///////////////////////////////////////////////////////////////////////////////

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void Awake () {
        if ( instance == null )
            instance = this;

        txtPrint.text = "";
        txtFPS.text = "";
        txtLog.text = "";

        if ( showScreenDebugText ) {
            debugTextPool.Init();
            for ( int i = 0; i < debugTextPool.initData.Length; ++i ) {
                GameObject textGO = debugTextPool.initData[i];
                textGO.transform.parent = transform;
                textGO.transform.localPosition = Vector3.zero;
                textGO.GetComponent<exSpriteFont>().enabled = false;
            }
        }
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void Start () {
        InvokeRepeating("UpdateFPS", 0.0f, 1.0f );
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void Update () {
        // count fps
        ++frames;

        // update log
        UpdateLog ();

        // NOTE: the OnGUI call multiple times in one frame, so we just clear text here.
        StartCoroutine ( CleanDebugText() );
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void UpdateFPS () {
        float timeNow = Time.realtimeSinceStartup;
        fps = frames / (timeNow - lastInterval);
        frames = 0;
        lastInterval = timeNow;
        txtFPS.text = "fps: " + fps.ToString("f2");
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    IEnumerator CleanDebugText () {
        yield return new WaitForEndOfFrame();
        txtPrint.text = "";

        if ( showScreenDebugText ) {
            debugTextPool.Reset();
            for ( int i = 0; i < debugTextPool.initData.Length; ++i ) {
                GameObject textGO = debugTextPool.initData[i];
                textGO.GetComponent<exSpriteFont>().enabled = false;
            }
        }
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void UpdateLog () {
        if ( updateLogText ) {
            string text = "";
            foreach ( string l in logs ) {
                text = text + l + "\n";
            }
            txtLog.text = text;
            updateLogText = false;
        }
    }
}
