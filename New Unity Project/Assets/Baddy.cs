using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Baddy : MonoBehaviour
{
    //gonna be commenting a lot so thats good
    //this object will eventually be a prefab and this will apply to all baddies, with variations. Like infantry and tanks still will have same pathfinding shit
    public Animation anim;
    public AnimationClip path;

    public int health;
    public int attackRange;

    public int baddyPhase=0;
    GameObject[] listofbuildings;
    List<GameObject> found = new List<GameObject>();
    public float anim_time;

    public enum PHASE
    {
        FOLLOWPATH,
        DESTROYBUILDINGS,
    }


    private void Update() {

        //keeping track of animation time and therefore where the fuck they are on the 'board'


        switch (baddyPhase) {
            case (int)PHASE.FOLLOWPATH:
                //steps
                //check if any buildings are in range
                //if they are, go to DESTROYALLBUILDINGS with a list of all the buildings within range
                //also get the current time the animation is running, save it to paste over when it comes back from DESTROY, and stop animation
                //the current time of the animation is by counting the time the object has been in FOLLOWPATH, since there isnt a built in way whoops

                /*
                since time commplicated, gonna work another day, here some links ig
                https://docs.unity3d.com/ScriptReference/Animator.Play.html
                https://www.reddit.com/r/Unity2D/comments/5vh3s0/starting_an_animation_on_a_certain_frame/
                http://esotericsoftware.com/forum/Unity-Need-to-set-a-animation-to-a-specific-frame-4714
                https://docs.unity3d.com/ScriptReference/AnimationState-time.html bruh this might literally be it lmao
                */
                //keep time good
                anim_time += Time.deltaTime;


                //find buildings. Keep this to like every 30 frames, not everyone too memory taxing
                GameObject[] listofbuildings = GameObject.FindGameObjectsWithTag("Building");
                found.Clear();
                for(int i=0; i < listofbuildings.Length; i++) {
                    float distance = Vector3.Distance(listofbuildings[i].transform.position, transform.position);
                    if(distance <= attackRange) {
                        found.Add(listofbuildings[i]);
                    }
                }
                if(found.Count !=0) {   //found buildings in range!
                    baddyPhase = (int)PHASE.DESTROYBUILDINGS;
                }

                break;
            case (int)PHASE.DESTROYBUILDINGS:
                //idea is that when they find a building, they will stop in their tracks and deal damage to it in intervals
                //attack buildings in order of found. Found contains all the buildings it needs to destroy b4 it moves again

                if (found.Count == 0) {   //all buildings destroyed
                    baddyPhase = (int)PHASE.FOLLOWPATH;
                    //set the animation time to anim_time
                }
                break;
        }
    }



    //some common things enemy will do. might as well make some methods
    void TakeDamage(int dmg) {
        health -= dmg;
    }
    void Die() {
        //some fancy effects at some point, maybe particle system.
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected() {   
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }



}


//little confused about the 2d camera but ok