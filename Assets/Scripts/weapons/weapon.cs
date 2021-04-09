using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//base weapon class
//equipable by enemies and player
//probably has a rigidbody and collidor when dropped
//when equiped disable rigidbody and collider things.
//not sure how orientation will work exactly, but I want the equipper to decide how to orient the weapon, such that the same weapon can be used in a variety of positions on different shaped enemies.
//e.g. a behemoth will carry a sword differently than a humanoid.

//still not sure if game logic and display logic are tied here.
// especially once we tie in animation

//weapons are throwable

public abstract class weapon : MonoBehaviour {
    public bool automatic; //TODO currently does nothing. I imagine we may have to refactor some logic such that holding down the mouse0 will let us continually fire. Possibly check in player for the weapon information to see if it's automatic or not.
    public int baseDamage;
    public int shootRate;
    public int swingRate;
    public GameObject firingPosition;
    public GameObject equiper;
    private int _shootInterval;
    protected int shootInterval {
        get { return _shootInterval; }
        set {
            _shootInterval = value;
            if(_shootInterval <= 0) 
            {
                _shootInterval = 0; // force zero as min value
            }
        }
    }
    private int _swingInterval;
    protected int swingInterval{
        get { return _swingInterval; }
        set {
            _swingInterval = value;
            if(_swingInterval <= 0) 
            {
                _swingInterval = 0; // force zero as min value
            }
        }
    }

    protected void Start() {
        shootInterval = shootRate;
        swingInterval = swingRate;
        automatic = false;
    }

    protected void Update() {

    }

    protected void FixedUpdate() {
        shootInterval--;
        swingInterval--;

    }

    public abstract bool Shoot(Vector2 heldPosition);

    public abstract bool Swing(Vector2 heldPosition);

}