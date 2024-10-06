using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;
using System.IO;

public class prefab_spawner : MonoBehaviour
{   
    [Serializable]
    public class Object_Info
    {
        public string obj_name;
        public Vector3 obj_pos;
        public Vector3 obj_mov;
        public float obj_mass;
        public float obj_radius;
    }

    [Header("Prefab to instantiate:")]
    [SerializeField] private GameObject prefab;
    [Header("Parent for instantiated objects:")]
    [SerializeField] private string objects_parent;
    [Space(20)]
    [Header("Object Properties:")]
    [SerializeField] private Object_Info[] object_infos;
    [Space(40)]
    [Header("Timescale:")]
    [Range(1f,100f)]
    [SerializeField] public float time_scale = 1f;
    [Header("Frames per second:")]
    [Range(1,100f)]
    [SerializeField] public float fixed_updates = 50f;
    [Space(40)]
    [Header("Scale:")]
    [SerializeField] public float scale = 1;
    [Space(40)]
    [Header("CSV import path")]
    [SerializeField] private bool import_csv;
    [SerializeField] private string csvFilePath;
    public string[,] csv_array; // csv file converted to this 2d array
    


    public void StartSim()
    {
        if(import_csv)
        {
            CSV_to_array();
            object_infos = SetObjectInfoArray(csv_array);
        }
        for(int i = 0; i < object_infos.Length; i++)
        {
            object_infos[i].obj_pos = Vector3.Scale(object_infos[i].obj_pos, new Vector3(scale,scale,scale));
            object_infos[i].obj_mov = Vector3.Scale(object_infos[i].obj_mov, new Vector3(scale,scale,scale));
            object_infos[i].obj_mass = object_infos[i].obj_mass;
            object_infos[i].obj_radius = object_infos[i].obj_radius * scale;

            Spawn(object_infos[i].obj_name
            ,object_infos[i].obj_pos
            ,object_infos[i].obj_mov
            ,object_infos[i].obj_mass
            ,object_infos[i].obj_radius);
        }
        GameObject.Find("UIHandler").GetComponent<ui>().UpdateDropDown();
    }
    void Update()
    {
        Time.timeScale = time_scale;
        Time.fixedDeltaTime = 1/fixed_updates;
    }
    private void Spawn(string name, Vector3 pos, Vector3 movement, float mass, float radius)
    {
        GameObject obj;
        Rigidbody rb;
        obj = Instantiate(prefab,pos,transform.rotation);
        rb = obj.GetComponent<Rigidbody>();
        obj.GetComponent<gravity_object>().mass = mass;
        rb.velocity = movement;
        obj.transform.SetParent(GameObject.Find(objects_parent).transform);
        obj.transform.localScale = new Vector3(radius,radius,radius);
        obj.name = name;
        obj.GetComponent<gravity_object>().scale = scale;
    }
    void CSV_to_array()
    {
        // Read the CSV file lines
        string[] lines = File.ReadAllLines(csvFilePath);

        // Get the number of rows and columns
        int rows = lines.Length;
        int cols = lines[0].Split(',').Length;

        // Initialize the 2D array based on the rows and columns count
        csv_array = new string[rows, cols];

        // Loop through each line and split it into columns
        for (int i = 0; i < rows; i++)
        {
            string[] lineData = lines[i].Split(',');

            for (int j = 0; j < lineData.Length; j++)
            {
                csv_array[i, j] = lineData[j];
            }
        }

        // Optional: Output to the console to verify content
        //Debug.Log("CSV data loaded into 2D array.");
        
    }
    public Object_Info[] SetObjectInfoArray(string[,] csv_array)
    {
        // Get the number of rows (assuming first row is headers, so start at 1)
        int rows = csv_array.GetLength(0);
        

        // Create an array of Object_Info based on the number of rows in csv_array
        Object_Info[] objectInfoArray = new Object_Info[rows - 1]; // Assuming the first row is headers

        // Loop through each row (start at 1 to skip the header row)
        for (int i = 1; i < rows; i++)
        {
            // Create a new Object_Info instance for each row
            Object_Info obj = new Object_Info();

            // Populate Object_Info properties by parsing the appropriate CSV data
            obj.obj_name = csv_array[i, 0];                          // First column: Name
            obj.obj_pos = new Vector3(                               
                float.Parse(csv_array[i, 1]),                       // Second column: Position X
                float.Parse(csv_array[i, 2]),                       // Third column: Position Y
                float.Parse(csv_array[i, 3])                        // Fourth column: Position Z
            );
            obj.obj_mov = new Vector3(
                float.Parse(csv_array[i, 4]),                       // Fifth column: Movement X
                float.Parse(csv_array[i, 5]),                       // Sixth column: Movement Y
                float.Parse(csv_array[i, 6])                        // Seventh column: Movement Z
            );
            obj.obj_mass = float.Parse(csv_array[i, 7]);             // Eighth column: Mass
            obj.obj_radius = float.Parse(csv_array[i, 8]);           // Ninth column: Radius

            // Add the object to the objectInfoArray
            objectInfoArray[i-1] = obj;  // Note: i-1 because we're skipping the first row
        }

        // Return the populated array
        return objectInfoArray;
    }
}

