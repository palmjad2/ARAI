using Microsoft.MixedReality.Toolkit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Measurement_Finder : MonoBehaviour
{
    // Declare all global variables
    private Transform rPoint1;
    private Transform rPoint2;
    private Transform bPoint1;
    private Transform bPoint2;
    private Single[] Distances;

    public float rdistanceX;
    public float rdistanceY;
    public float rdistanceZ;
    public float bdistanceX;
    public float bdistanceY;
    public float bdistanceZ;
    public float hypotenuse;

    public GameObject redText;
    public GameObject blueText;
    public GameObject fileText;

    // To change the length of a gameobject
    public GameObject IndexFinger;

    public string sFilePath;
    
    // Start is called before the first frame update
    void Start()
    {
        
        // Find Sphere objects in Unity
        rPoint1 = GameObject.Find("rPoint_1").transform;
        rPoint2 = GameObject.Find("rPoint_2").transform;
        bPoint1 = GameObject.Find("bPoint_1").transform;
        bPoint2 = GameObject.Find("bPoint_2").transform;


        //string sCurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
        //string sFile = System.IO.Path.Combine(sCurrentDirectory, @"..\..\Documents\Measurements");
        //sFilePath = Path.GetFullPath(sFile);
        
    }

    // Update is called once per frame 
    void Update()
    {


    }
    public void RedMeasure()
    {


        Vector3 rdelta = rPoint2.transform.position - rPoint1.transform.position; // Calculate the distance as a difference of position 

        // Finds the distance relative to each axis
        rdistanceX = Mathf.Abs(rdelta.x);
        rdistanceY = Mathf.Abs(rdelta.y);
        rdistanceZ = Mathf.Abs(rdelta.z);

        float distanceXZ = CalculateDistanceXZ(rPoint1.transform.position, rPoint2.transform.position);
        Debug.Log(distanceXZ * 100);
        // Calculate the total arm length with respect to the x and z axes
        // hypotenuse = (Single)Math.Sqrt(Math.Pow(rdistanceX, 2) + Math.Pow(rdistanceY, 2)); Method does not work

        //Destroy(redText);
        redText.GetComponent<TextMeshPro>().text = " " + (Math.Round(distanceXZ, 3) * 100) + " Centimeters";
    }
    public void BlueMeasure()
    {
        Vector3 bdelta = bPoint2.transform.position - bPoint1.transform.position; // Calculate the distance as a difference of position
                                                                                  // 
                                                                                  // Finds the distance relative to each axis
        bdistanceX = Mathf.Abs(bdelta.x);
        bdistanceY = Mathf.Abs(bdelta.y);
        bdistanceZ = Mathf.Abs(bdelta.z);
        double bdispy = bdistanceY * 16;
        // Begin list of hypotenuse and y distance for blue spheres
        List<Single> Distances_List = new List<Single>() { hypotenuse, bdistanceY };
        Distances = Distances_List.ToArray();  // Convert from list to array

        //Destroy(blueText);
        blueText.GetComponent<TextMeshPro>().text = " " + (Math.Round(bdistanceY, 3) * 100) + " Centimeters";

        Vector3 newScale = IndexFinger.transform.localScale;
        newScale.y = (float)bdispy;
        IndexFinger.transform.localScale = newScale;

    }




    public void SaveCSV()
    {
        //fileText.GetComponent<TextMeshPro>().text += sFilePath;
        // File Path to save the distances
        string filePath = "U:Users\\DefaultAccount\\AppData\\Local\\Documents\\Measurements";

        // Try and catch to alert user if there was any trouble saving the .csv file
        try
        {
            using (StreamWriter sw = new StreamWriter(filePath)) // Use streamwriter to create the file
            {
                for (int i = 0; i < Distances.Length; i++)
                {
                    // Write each element of the array followed by a comma
                    sw.Write(Distances[i]);

                    // Add a comma after each element except the last one
                    if (i < Distances.Length - 1)
                    {
                        sw.Write(",");
                    }
                }
            }

            Debug.Log("Array data has been successfully saved to the .csv file.");
        }
        catch (Exception ex) // Added if there is a problem saving the csv file
        {
            Debug.Log("Error occurred: " + ex.Message);
        }

    }

    float CalculateDistanceXZ(Vector3 position1, Vector3 position2)
    {
        // Calculates the distance and disregards the y-component
        Vector2 position1XZ = new Vector2(position1.x, position1.z);
        Vector2 position2XZ = new Vector2(position2.x, position2.z);

        // Calculate the distance in XZ plane
        return Vector2.Distance(position1XZ, position2XZ);

    }

}

// To do:

// Email prosthetic practices
// Find file path on hololens
// Constrain red parts so that they each stay 90 degrees from parent empty object
// Dotted line -- low priority
