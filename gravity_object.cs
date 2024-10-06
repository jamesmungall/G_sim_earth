using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gravity_object : MonoBehaviour
{
    static public float g = 6.67430e-11f;
    [NonSerialized] public float scale;

    private Vector3 start_pos;
    private Vector3 start_velocity;
    [NonSerialized] public float mass;
    private float radius;

    private Rigidbody rb;
    private TrailRenderer tr;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<TrailRenderer>();
        tr.startWidth = 0.2f * scale;
        tr.endWidth = 0;

        GetSelfValues();

        g *= scale*scale*scale;
    }
    void FixedUpdate()
    {
        ApplyGravity();
        //Debug.Log(this.name + " pos: " + rb.velocity);
    }
    private void GetSelfValues()
    {
        start_pos = transform.position;
        start_velocity = rb.velocity;
        radius = transform.localScale.x;

        //print(transform.localPosition);
    }
    private void ApplyGravity()
    {
        
        foreach (Transform child in transform.parent)
        {
            if(child.name != gameObject.name)
            {
                GameObject other_obj = GameObject.Find(child.name);
                float other_mass = other_obj.GetComponent<gravity_object>().mass;
                Vector3 force_dir = Vector3.Normalize(new Vector3(transform.position.x - other_obj.transform.position.x,transform.position.y - other_obj.transform.position.y,transform.position.z - other_obj.transform.position.z));

                float separation = Vector3.Distance(transform.position,other_obj.transform.position);
                
                

                float force;
                force = g * mass * other_mass /(separation * separation);

                //print(this.name + rb.velocity);

                //print("Name: " + this.name + " Mass: " + mass + " OtherMass: " + other_mass + " Seperation: " + separation + " Force: " + force);
                Vector3 directional_force = new Vector3(-force_dir.x*force, -force_dir.y*force, -force_dir.z*force);
                
                Vector3 current_acceleration = directional_force / mass;
                Vector3 current_velocity = current_acceleration * Time.fixedDeltaTime;
                rb.velocity += current_velocity;
                //rb.AddForce(directional_force);
                if(this.name == "earth"){
                    /*print("g " + g.GetType().ToString() +g);
                    print("mass " +my_mass.GetType().ToString() +my_mass);
                    print("othermass " +other_mass.GetType().ToString() +other_mass);
                    print("separation " +separation.GetType().ToString() +separation);
                    print("force " +force_double.GetType().ToString() +force);*/
                }
                
                
            }
        }
    }
}
