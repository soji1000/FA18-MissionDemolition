using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour {

    static public GameObject POI; // The static point of interest // a
    [Header("Set in Inspector")]
    public float easing = 0.05f;
    public Vector2 minXY = Vector2.zero;

    [Header("Set Dynamically")]
    public float camZ; // The desired Z pos of the camera
    private bool isExplosive = false;
    private Vector3 explosiveProjectilePosition;
    private float startTime;
    void Awake()
    {
        camZ = this.transform.position.z;
    }
    void FixedUpdate()
    {
        // if there's only one line following an if, it doesn't need braces
        // if (POI == null) return; // return if there is no poi // b
        // Get the position of the poi
        // Vector3 destination = POI.transform.position;
        Vector3 destination = Vector3.zero;
        // If there is no poi, return to P:[ 0, 0, 0 ]
        if (POI == null)
        {
            if (isExplosive ) {
               
               
                destination = explosiveProjectilePosition;
                //keep the camera pointed at the last place of imapct for 4 seconds
                if (Time.time - startTime >= 4.0f){
                    isExplosive = false;
                }
               
                
            }
            else {
                destination = Vector3.zero;
            }
        }
        else
        {
            // Get the position of the poi
            destination = POI.transform.position;
            // If poi is a Projectile, check to see if it's at rest
            if (POI.tag == "Projectile")
            {
                // if it is sleeping (that is, not moving)
                if (POI.GetComponent<Rigidbody>().IsSleeping())
                {
                    // return to default view
                    
                    
                    
                        POI = null;
                    
                    // in the next update
                    return;
                }

                if (POI.layer == 8)
                {
                    //This is the explosive projectile. 
                    isExplosive = true;
                  //  Get The time that it arrived at the castle
                    startTime = Time.time;
                    //The last position of the projectile before it gets destroyed.
                    explosiveProjectilePosition = POI.transform.position;
                    
                    
                }
            }
        }

        // Limit the X & Y to minimum values
        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);
        // Interpolate from the current Camera position toward destination
        destination = Vector3.Lerp(transform.position, destination, easing);
        // Force destination.z to be camZ to keep the camera far enough away
        destination.z = camZ;
        // Set the camera to the destination
        transform.position = destination;
        // Set the orthographicSize of the Camera to keep Ground in view
        Camera.main.orthographicSize = destination.y + 10;
    }
    
}
