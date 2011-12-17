// ======================================================================================
// File         : playerController.cs
// Author       : nantas 
// Last Change  : 12/17/2011 | 14:41:16 PM | Saturday,December
// Description  : 
// ======================================================================================


///////////////////////////////////////////////////////////////////////////////
// class PlayerController 
// 
// Purpose: Handle input, control player speed and acceleration
// 
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

public enum JumpState {
    Ready2Jump,
    Ground,
    InAir,
}

public enum HurtState {
    Invincible,
    Hurt,
    Hitable
}

public enum ActionState {
    Free,
    Stun,    
    Jump,
    Dash,
    AirDash,
    Recover
}


public class PlayerController: MonoBehaviour {

	public float initMoveSpeed = 120.0f;
    public float initJumpSpeed = 500.0f;

    private PlayMakerFSM playerFSM;
    [System.NonSerialized]public PlayerBase player; 
    public Vector2 velocity;


	[System.NonSerialized] public MoveDir charMoveState;
    public ActionState charActionState = ActionState.Free;
    public HurtState charHurtState = HurtState.Hitable;
    public JumpState charJumpState = JumpState.Ground;
    private ActionState lastActionState = ActionState.Free;

    void Awake () {
        playerFSM = GetComponent<PlayMakerFSM>();
        player = transform.GetComponent<PlayerBase>();
        velocity = new Vector2(0, 0);
    }
    
    void Start () {
        charMoveState = MoveDir.Stop;
        playerFSM.Fsm.Event("To_Walk");
    }

	void Update () {
		if ( Input.GetButtonDown("Right") ) {
			TurnRight();
		}
		if ( Input.GetButtonDown("Left") ) {
			TurnLeft();
		}
		if ( Input.GetButtonDown("Jump") ) {
			StartJump();
		}
            
		
        //handle Jump
        if (charJumpState == JumpState.Ready2Jump) {
            velocity.y = initJumpSpeed;
            playerFSM.Fsm.Event("To_Jump");
        }
		//handle horizontal movement
		float horizonDist = Time.deltaTime * velocity.x;
        float verticalDist = 0;
		switch (charMoveState) {
			case MoveDir.Right:
				//don't need to change
				break;
			case MoveDir.Left:
				horizonDist = -horizonDist;
				break;
			case MoveDir.Stop:
				horizonDist = 0;
				break;
			default:
				horizonDist = 0;
				break;
		}
        //handle vertical movement
        if (charJumpState == JumpState.InAir) {
            velocity.y += Game.instance.gravity;
            verticalDist = Time.deltaTime * velocity.y;
        }
		//move character
        if (charActionState != ActionState.Stun){
    		//horizontal
            if ((transform.position.x + horizonDist < Game.instance.rightBoundary.position.x) 
	    		&& (transform.position.x + horizonDist > Game.instance.leftBoundary.position.x) ) {
		    	transform.Translate (horizonDist, 0, 0, Space.World);
		    }
            //vertical
            if ( charJumpState == JumpState.InAir ) {
                transform.Translate (0, verticalDist, 0);
            }
        }
        //update air to ground state
        if (charJumpState != JumpState.Ground) {
            if ( transform.position.y <= Game.instance.groundPosY ) {
                transform.position = new Vector3 (transform.position.x, 
                                              Game.instance.groundPosY, transform.position.z);
                playerFSM.Fsm.Event("To_Walk");
            }
        }
	
	}


	public void TurnRight() {
		if (charActionState == ActionState.Free || charActionState == ActionState.Jump) {
            if (charMoveState != MoveDir.Right) {
                if (charMoveState == MoveDir.Stop) {
                    charMoveState = MoveDir.Right;
                    transform.localEulerAngles = new Vector3 (0, 0, 0);
                    velocity.x = initMoveSpeed;
                    playerFSM.Fsm.Event("To_Walk");
                } else {
	    		    charMoveState = MoveDir.Right;
                    transform.localEulerAngles = new Vector3 (0, 0, 0);
                }
		    } else {
                if (charJumpState == JumpState.Ground) {
                    //get into dash state
                    Debug.Log("dashing right!");
                    playerFSM.Fsm.Event("To_Dash");
                }
            }
        }

	}
	
	public void TurnLeft() {
        if (charActionState == ActionState.Free || charActionState == ActionState.Jump) {
            if (charMoveState != MoveDir.Left) {
                if (charMoveState == MoveDir.Stop) {
                    charMoveState = MoveDir.Left;
                    transform.localEulerAngles = new Vector3 (0, 180, 0);
                    velocity.x = initMoveSpeed;
                    playerFSM.Fsm.Event("To_Walk");
                } else {
	    		    charMoveState = MoveDir.Left;
                    transform.localEulerAngles = new Vector3 (0, 180, 0);
                }
		    } else {
                if (charJumpState == JumpState.Ground) {
                    //get into dash state
                    Debug.Log("dashing left!");
                    playerFSM.Fsm.Event("To_Dash");
                }
            }
        }
	}

    public void StartWalk() {
        velocity.x = initMoveSpeed;
        playerFSM.Fsm.Event("To_Walk");
    }

	public void StartJump() {
        if (charActionState == ActionState.Free) {
            if (charJumpState == JumpState.Ground) {
                charJumpState = JumpState.Ready2Jump;
            }
        }
	}

    public void StartDash() {
        velocity.x = player.dashSpeed;
        player.OnDashStart();
    }

    public void StopDash() {
        velocity.x = initMoveSpeed;
        player.OnDashStop();
    }

    public void OnDamagePlayer (bool _isHurtFromLeft, int _damageAmount) {
        if (charHurtState == HurtState.Hitable) {
            if ( charActionState == ActionState.Dash ) {
                StopDash();
            }
            Game.instance.OnPlayerHPChange(-_damageAmount);
            StartHurt(_isHurtFromLeft);
        }
    }	

    public void StartHurt(bool _isHurtFromLeft) {
        //playing hurt flash effect
        player.OnHurtStart();
        if (_isHurtFromLeft) {
            transform.Translate(40.0f, 0, 0);
        } else {
            transform.Translate(-40.0f, 0, 0);
        }
        lastActionState = charActionState;
        playerFSM.Fsm.Event("To_Hurt");
    }

    public void OnStunFinish() {
        //this method is independent from FSM states
        Debug.Log("stun finished.");
        charHurtState = HurtState.Invincible;
        if (lastActionState == ActionState.Free || lastActionState == ActionState.Dash
            || lastActionState == ActionState.Recover) {
            playerFSM.Fsm.Event("To_Walk");
        } else if (lastActionState == ActionState.Jump || lastActionState == ActionState.AirDash ) {
            playerFSM.Fsm.Event("To_Jump");
        }
        Invoke("OnInvincibleFinish", player.flashTime);
    }

    public void OnInvincibleFinish() {
        //this method is independent from FSM states
        charHurtState = HurtState.Hitable;
    }
        

}

