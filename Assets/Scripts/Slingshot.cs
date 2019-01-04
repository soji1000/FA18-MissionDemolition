using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour {
    static private Slingshot S; // a
    [Header("Set in Inspector")] // a
    public GameObject prefabProjectile;
    public GameObject prefabExProjectile;
    public float velocityMult = 8f; // a
    // fields set dynamically
    [Header("Set Dynamically")] // a
    public GameObject launchPoint;
    public Vector3 launchPos; // b
    public GameObject projectile; // b
    public bool aimingMode; // b
    private int shots = 0;
    private Rigidbody projectileRigidbody; // a

    static public Vector3 LAUNCH_POS
    { // b
        get
        {
            if (S == null) return Vector3.zero;
            return S.launchPos;
        }
    }

    void Awake()
    {
        S = this;
        Transform launchPointTrans = transform.Find("LaunchPoint"); // a
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false); // b
        launchPos = launchPointTrans.position;
    }


    void OnMouseEnter()
    {
        //print("Slingshot:OnMouseEnter()");
        launchPoint.SetActive(true);
    }
    void OnMouseExit()
    {
        //print("Slingshot:OnMouseExit()");
        launchPoint.SetActive(false);
    }

    void OnMouseDown()
    {
        // The player has pressed the mouse button while over Slingshot
        aimingMode = true;
        if (shots == 3)
        {
            // Instantiate a Projectile
            projectile = Instantiate(prefabExProjectile) as GameObject;
            // Start it at the launchPoint
            projectile.transform.position = launchPos;
            // Set it to isKinematic for now
            //  projectile.GetComponent<Rigidbody>().isKinematic = true;
            projectileRigidbody = projectile.GetComponent<Rigidbody>(); // a
            projectileRigidbody.isKinematic = true;
        } else if (shots > 3 && shots % 3 == 0) {
            // Instantiate a Projectile
            projectile = Instantiate(prefabExProjectile) as GameObject;
            // Start it at the launchPoint
            projectile.transform.position = launchPos;
            // Set it to isKinematic for now
            //  projectile.GetComponent<Rigidbody>().isKinematic = true;
            projectileRigidbody = projectile.GetComponent<Rigidbody>(); // a
            projectileRigidbody.isKinematic = true;
        }
        else { 
       
        // Instantiate a Projectile
        projectile = Instantiate(prefabProjectile) as GameObject;
        // Start it at the launchPoint
        projectile.transform.position = launchPos;
        // Set it to isKinematic for now
        //  projectile.GetComponent<Rigidbody>().isKinematic = true;
        projectileRigidbody = projectile.GetComponent<Rigidbody>(); // a
        projectileRigidbody.isKinematic = true;
    }
        
    }

    void Update()
    {
        // If Slingshot is not in aimingMode, don't run this code
        if (!aimingMode) return; // b
                                 // Get the current mouse position in 2D screen coordinates
        Vector3 mousePos2D = Input.mousePosition; // c
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
        // Find the delta from the launchPos to the mousePos3D
        Vector3 mouseDelta = mousePos3D - launchPos;
        // Limit mouseDelta to the radius of the Slingshot SphereCollider // d
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if (mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }
        // Move the projectile to this new position
        Vector3 projPos = launchPos + mouseDelta;
        if (projectile != null) {
            projectile.transform.position = projPos;
        }
        if (Input.GetMouseButtonUp(0))
        { // e
          // The mouse has been released
            aimingMode = false;
            projectileRigidbody.isKinematic = false;
            projectileRigidbody.velocity = -mouseDelta * velocityMult;
            FollowCam.POI = projectile;
            projectile = null;
            MissionDemolition.ShotFired(); // a
            shots++;
            ProjectileLine.S.poi = projectile; // b
        }
    }


}
