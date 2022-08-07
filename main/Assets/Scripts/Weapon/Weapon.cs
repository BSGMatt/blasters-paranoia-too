using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public WeaponCard card;
    public Character host;
    public Rigidbody2D rb;
    public Camera cam;
    public Transform target;

    public bool isEnemy;

    /// <summary>
    /// The amount of ammo the weapon currently has. 
    /// </summary>
    public int ammo;

    private Vector2 mousePos;

    /// <summary>
    /// Whether the weapon can fire or not. 
    /// </summary>
    public bool canFire;

    /// <summary>
    /// The angle at which the weapon is being held at. 
    /// </summary>
    protected float angle;

    protected Coroutine reloading;
    protected Coroutine firing;

    /// <summary>
    /// Aims the weapon based on the mouse position, 
    /// changing the rotation and angle in the process. 
    /// </summary>
    /// <returns>the angle of the weapon's rotation.</returns>
    protected float AimWithMouse() {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        Vector2 lookDir = mousePos - rb.position;

        angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        rb.rotation = angle;

        return angle;
    }

    /// <summary>
    /// Aims the weapon at its target, changing the rotation and angle in the process. 
    /// </summary>
    /// <returns>the angle of the weapon's rotation. </returns>
    protected float AimAtTarget() {
        Vector2 lookDir = (Vector2)target.position - rb.position;

        angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        rb.rotation = angle;

        return angle;
    }

    /// <summary>
    /// Creates a pellet based on the given card and trajectory. 
    /// </summary>
    /// <param name="card"></param>
    /// <param name="trajectory"></param>
    protected void CreatePellet(WeaponCard card, float trajectory) {
        //Create the pellet object
        GameObject pellet = Instantiate(card.pelletType);
        //Ingore the collision between the pellet and the character who is holding the weapon. 
        Physics2D.IgnoreCollision(pellet.GetComponent<Collider2D>(), host.surfaceCollider);
        Physics2D.IgnoreCollision(pellet.GetComponent<Collider2D>(), host.nonSurfaceCollider);

        //Set starting position and velocity. 
        pellet.GetComponent<Pellet>().damage = (int) (card.dmgPerPellet * host.currentDmgDealtModValue);
        pellet.GetComponent<Pellet>().isEnemy = isEnemy;
        pellet.GetComponent<Pellet>().Init(new Vector2(card.pelletSpeed * Mathf.Cos(trajectory * Mathf.Deg2Rad),
            card.pelletSpeed * Mathf.Sin(trajectory * Mathf.Deg2Rad)), rb.position);
    }

    /// <summary>
    /// Puts the weapon on a reload timer.
    /// During this time, canFire is set to false. The duration of the timer is 
    /// dependant on the WeaponCard's reloadSpeed value. 
    /// </summary>
    /// <returns></returns>
    protected IEnumerator Reload() {

        Debug.Log("Beginning Reload");

        canFire = false;
        float time = 0; 

        while (time < card.reloadSpeed) {
            yield return new WaitForSeconds(card.reloadSpeed);

            time += card.reloadSpeed;
        }

        ammo = card.maxAmmo;
        canFire = true;

        reloading = null;

        Debug.Log("End Reload");

        yield return 0;
    }

    /// <summary>
    /// An update method containing the common operations that most weapons will use. 
    /// </summary>
    public void CommonUpdate() {

        //Don't do anything if the player is trying to access the shop. 
        if (FindObjectOfType<GameManager>().shopEnabled) {
            return;
        }


        //Check if the player pressed the reload key. 
        if (Input.GetKeyDown(KeyCode.R)) {
            if (reloading == null) {
                reloading = StartCoroutine(Reload());
            }
        }

        //Fire a pellet if player left-clicks. 
        if (Input.GetMouseButtonDown(0)) {
            if (canFire && ammo > 0) Fire();
        }
    }

    /// <summary>
    /// Similar to CommonUpdate(), but for weapons held by enemies. 
    /// </summary>
    public void CommonEnemyUpdate()
    {
        //Don't do anything if the player is trying to access the shop. 
        if (FindObjectOfType<GameManager>().shopEnabled)
        {
            return;
        }

        if (canFire && ammo > 0) Fire();
    }

    public void CommonFixedUpdate() {
        rb.position = host.GetRigidBody().position;

        AimWithMouse();
    }

    /// <summary>
    /// An start method containing the common operations that most weapons will use. 
    /// </summary>
    public void CommonStart() {
        cam = FindObjectOfType<CameraMan>().GetCamera();
        ammo = card.maxAmmo;
        host.currentWeapon = this;
    }

    protected void GrabHostTarget() {
        target = ((Enemy)host).aiController.target.transform;
    }

    /// <summary>
    /// Generates a random value in the range [-maxSpread, -minSpread]U[minSpread, maxSpread].
    /// Used to create spread that varies between shots. 
    /// </summary>
    /// <returns>The randomly generated spread value.</returns>
    protected float RandomSpreadValue() {
        int rand = (int)Mathf.Round(Random.value);

        if (rand == 1)
            return Random.Range(card.minSpread, card.maxSpread);
        else
            return Random.Range(-card.maxSpread, -card.minSpread);
    }

    /// <summary>
    /// Fire a weapon. A weapon can a fire a weapon is different ways, so this function is left 
    /// for the children to implement. 
    /// </summary>
    protected abstract void Fire();

    /// <summary>
    /// Coroutine to automatically fire a weapon. 
    /// </summary>
    /// <returns></returns>
    protected IEnumerator FireCoroutine()
    {
        
        while (ammo > 0)
        {
            Fire();
            yield return new WaitForSeconds(card.fireRate);
        }

        //Reload Ammo

        yield return new WaitForSeconds(card.reloadSpeed);

        ammo = card.maxAmmo;

        firing = null;

        yield return null;
    }
}
