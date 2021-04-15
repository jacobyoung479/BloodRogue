using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : entity
{
    public float moveSpeed = 5f;

    public Rigidbody2D rb;
    
    //This is going to be currentweapon player is holding that can be swapped
    public GameObject weapon;

    private Vector2 lookDir;
    public GameObject handPosition;
    private Vector2 oldLookDir;
    private Vector2 newLookDir;
    private Quaternion lookRotation;
    public float armlength;

    public GameObject bloodgun;
    public GameObject ability;
    public GameObject consumable;
    public int ammoChargeCountTotal = 10;
    private int _ammoChargeCountCurrent;
    private int ammoChargeCountCurrent {
        get { return _ammoChargeCountCurrent; }
        set {
            _ammoChargeCountCurrent = value;
            if(_ammoChargeCountCurrent <= 0) 
            {
                _ammoChargeCountCurrent = 0; // force zero as min value
            }
        }

    }
    

    // Start is called before the first frame update
    new void Start()
    {
       
        //Instantiate()
        //this is only so player can instantiate a copy when needed.
        base.Start();
        ammoChargeCountCurrent = ammoChargeCountTotal;
        bloodgun.SetActive(false);
        Debug.Log(weapon.transform.localScale);
        //health = 6;

    }

    // Update is called once per frame
    new void Update()
    {

    }

    void FixedUpdate()
    {
       
    }

    public void RequestMove(Vector2 movement) {
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    public void RequestPlayerAngle(Vector2 mousePos) {
        float angle = Utilities.AngleBetweenTwoPoints(mousePos, rb.position);
        newLookDir = (mousePos - rb.position).normalized;
        Vector3 newPosition = rb.position + newLookDir * armlength;
        handPosition.GetComponent<Transform>().position = newPosition;
        handPosition.GetComponent<Transform>().rotation = Quaternion.Euler (new Vector3(0f,0f,angle));
    }

    public void RequestShootWeapon() {
        if (ammoChargeCountCurrent > 0) {
            if (weapon.GetComponent<weapon>().Shoot(handPosition.GetComponent<Transform>().position)) {
                SubtractAmmoCount(weapon.GetComponent<weapon>().ammoCostPerShot);
                Debug.Log("shot weapon");
            } else {
                Debug.Log("weapon cooldown");
            }
        } else {
            Debug.Log("Out of ammo");
        }

        
    }

    public void RequestSwingWeapon() {
        weapon.GetComponent<weapon>().Swing(handPosition.GetComponent<Transform>().position);
    }

    private void GetWeapon(GameObject newWeapon) {
        Debug.Log(newWeapon);
        GameObject createdWeapon = InitializeWeapon(newWeapon);
        SwitchWeapon(createdWeapon);
    }

    private GameObject InitializeWeapon(GameObject newWeapon) {
        GameObject createdWeapon = Instantiate(newWeapon, weapon.transform.position, weapon.transform.rotation);
        createdWeapon.GetComponent<weapon>().equipper = this.gameObject;
        return createdWeapon;
    }

    private void SwitchWeapon(GameObject createdWeapon) {
        DestroyImmediate(weapon);
        weapon = createdWeapon;
        createdWeapon.transform.parent = handPosition.transform;
        //weapon.transform.parent = handPosition.transform;
        Vector3 calc = Utilities.MatrixMultiplication(this.transform.localScale, handPosition.transform.localScale);
        createdWeapon.transform.localScale = Utilities.MatrixMultiplication(createdWeapon.transform.localScale, calc);
        Debug.Log(calc);
    }

    private void SubtractAmmoCount(int ammoCost){
        ammoChargeCountCurrent -= ammoCost;
        //userInterface.GetComponent<UI>().updateAmmo(currAmmoCount);
        Debug.Log("current ammo: " + ammoChargeCountCurrent);
    }

    public void ResetAmmoToTotal() {
        ammoChargeCountCurrent = ammoChargeCountTotal;
        Debug.Log("Reset! current ammo: " + ammoChargeCountCurrent);
    }

    public void IncreaseAmmoTotal() {
        ammoChargeCountTotal += 1;
        ammoChargeCountCurrent += 1;//maybe remove this. only weird case, is if you have 0 ammo, and pick this up, it gives you an extra shot. It might be good for split second choices/increasing combos
    }

}