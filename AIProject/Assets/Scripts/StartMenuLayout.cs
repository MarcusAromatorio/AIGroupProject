using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


    /*
        As the name of this script implies, here is laid out the structure of the start menu for the game
        For the first iteration, only a button surrounded by a box exists, where pressing the button starts the game scene

        The first iteration follows the Fixed Layout format as described by the Unity documentation
    */
public class StartMenuLayout : MonoBehaviour {

    // Locations for rectangles that define GUI elements
    private Rect groupBox;
    private Rect backgroundBox;
    private Rect buttonBox;

    // Initialize variables here
    void Start()
    {
        // The box is positioned relative to the top-left of the screen (Not a visual element, but a grouping construct)
        groupBox = new Rect(Screen.width / 2.0f - 62.5f, Screen.height / 3.0f, 125.0f, 87.5f);

        // The box is positioned RELATIVELY to the group by being nested within (First visual element to draw)
        backgroundBox = new Rect(0, 0, 125.0f, 87.5f);

        // The button is within the box, also positioned relative to the group
        buttonBox = new Rect(12.5f, 37.5f, 100.0f, 25.0f);
    }

    // This method is used for GUI elements being defined and used in game
	void OnGUI()
    {
        // GUI elements are placed in groups via BeginGroup and EndGroup calls
        GUI.BeginGroup(groupBox);
        GUI.Box(backgroundBox, "AI Demo");

        // In the Unity documentation, the normal way to handle button presses is to wrap buttons in an if statement like below
        if(GUI.Button(buttonBox, "Start"))
        {
            // Code within this block only executes once the user lifts off of the button
            SceneManager.LoadScene("gameScene");
        }
        GUI.EndGroup();

    }
}
