using UnityEngine;
using System.Collections;

public class EnemyCollider : MonoBehaviour {
	
	private Enemy controller;
	
	void Awake() {
		controller = transform.parent.GetComponent<Enemy>();
	}

    /*
	void OnTriggerEnter (Collider other) {
        //check if the collide is from left or right 
        bool isEnemyOnLeft;
        if (transform.position.x < other.transform.position.x) {
            isEnemyOnLeft = true;
        } else {
            isEnemyOnLeft = false;
        }

        //when collide with player body, send message to player collider.
		if (other.tag == "player" && transform.root.gameObject.tag != "itemCarrier") {
			other.GetComponent<PlayerCollider>().TouchedEnemy(isEnemyOnLeft, 
                                                              controller.attackPower);

		}
        //when collide with player weapon, push back and trigger damage.
		if (other.tag == "player_weapon") {
            Vector2 collisionPos = new Vector2((this.transform.position.x + 
                                                other.transform.position.x)/2, 
                                               (this.transform.position.y +
                                                other.transform.position.y)/2);
			other.SendMessage("AttackEnemy",collisionPos);
			controller.SendMessage("OnDamaged", isEnemyOnLeft);
		}
	}*/

    void OnCollisionEnter(Collision _collision) {
        //check if the collide is from left or right 
        bool isEnemyOnLeft;
        ContactPoint contact = _collision.contacts[0];
        if (transform.position.x < contact.point.x) {
            isEnemyOnLeft = true;
        } else {
            isEnemyOnLeft = false;
        }

        //when collide with player body, send message to player collider.
		if (_collision.collider.tag == "player" && transform.root.gameObject.tag != "itemCarrier") {
			_collision.collider.GetComponent<PlayerCollider>().TouchedEnemy(isEnemyOnLeft, 
                                                              controller.attackPower);

		}
        //when collide with player weapon, push back and trigger damage.
		if (_collision.collider.tag == "player_weapon") {
            Vector2 collisionPos = new Vector2(contact.point.x, contact.point.y);
			_collision.collider.SendMessage("AttackEnemy",collisionPos);
			controller.SendMessage("OnDamaged", isEnemyOnLeft);
		}
    }
	
	void LateUpdate () {
		//hack: force collision z index
		transform.position = new Vector3(transform.position.x, transform.position.y, 200);
		
	}
	
	
}
