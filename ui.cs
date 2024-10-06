using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ui : MonoBehaviour
{
    [SerializeField] private GameObject prefab_obj;
    [SerializeField] private GameObject parent_obj;
    [Space]
    [SerializeField] private Slider distance_slider;
    [SerializeField] private Slider time_slider;
    [SerializeField] private Slider fps_slider;
    [Space]
    [SerializeField] private TMP_Dropdown orbit_drop_down;
    [Space]
    [SerializeField] private TMP_InputField g_input_field;

    public static bool playing = false;
    // Start is called before the first frame update
    void Start()
    {
        distance_slider.value = prefab_obj.GetComponent<prefab_spawner>().scale;
        time_slider.value = prefab_obj.GetComponent<prefab_spawner>().time_scale;
        fps_slider.value = prefab_obj.GetComponent<prefab_spawner>().fixed_updates;

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DistanceSlider()
    {
        prefab_obj.GetComponent<prefab_spawner>().scale = distance_slider.value;
    }
    public void TimeSlider()
    {
        prefab_obj.GetComponent<prefab_spawner>().time_scale = time_slider.value;
    }
    public void FPSSlider()
    {
        prefab_obj.GetComponent<prefab_spawner>().fixed_updates = fps_slider.value;
    }
    public void PlayPauseButton()
    {
        if (playing == false)
        {
            prefab_obj.SetActive(true);
            playing = true;
            prefab_obj.GetComponent<prefab_spawner>().StartSim();
        } else
        {
            prefab_obj.SetActive(false);
            playing = false;
            foreach (Transform child in parent_obj.transform)
            {
                // Destroy each child game object
                GameObject.Destroy(child.gameObject);
            }

        }
    }
    public void OrbitDropDown()
    {
        //print(orbit_drop_down.value);
        
        for (int i = 0; i < parent_obj.transform.childCount; i++)
        {
            Transform child = parent_obj.transform.GetChild(i);

            // The current index is 'i', no need for GetSiblingIndex
            if(i == orbit_drop_down.value)
            {
                
                this_camera.target = child;
            }
        }
        
       
    }
    public void UpdateDropDown()
    {
        orbit_drop_down.ClearOptions();
        foreach (Transform child in parent_obj.transform)
        {
            orbit_drop_down.AddOptions(new List<string> {child.name});
            
        }
        g_input_field.text = gravity_object.g.ToString();
    }
    public void GChanged()
    {
        gravity_object.g = float.Parse(g_input_field.text);
    }
}
