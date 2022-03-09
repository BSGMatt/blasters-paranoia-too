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

    private Vector2 mousePos;

    /// <summary>
    /// Whether the weapon can fire or not. 
    /// </summary>
    protected bool canFire;

    /// <summary>
    /// The amount of ammo the weapon currently has. 
    /// </summary>
    protected int ammo;

    /// <summary>
    /// The angle at which the weapon is being held at. 
    /// </summary>
    protected float angle;

    protected Coroutine reloading;

    /// <summary>
    /// Angle the weapon based on where the mouse is on screen. 
    /// </summary>
    protected void AimWithMouse() {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        Vector2 lookDir = mousePos - rb.position;

        angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
    }

    /// <summary>
    /// Angles the weapon such that it points to the current target. 
    /// </summary>
    protected void AimAtTarget() {
        Vector2 lookDir = (Vector2)target.position - rb.position;

        angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
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
        pellet.GetComponent<Pellet>().damage = card.dmgPerPellet;
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
    /// Fire a weapon. A weapon can a fire a weapon is differnt ways, so this function is left 
    /// for the children to implement. 
    /// </summary>
    protected abstract void Fire();
}
