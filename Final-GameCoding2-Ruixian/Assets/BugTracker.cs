using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugTracker : MonoBehaviour
{

    public static BugTracker Instance;

    private Dictionary<string, int> bugCounts = new Dictionary<string, int>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            bugCounts["Beetle"] = 0;
            bugCounts["Butterfly"] = 0;
            bugCounts["Grasshopper"] = 0;
            bugCounts["Ant"] = 0;
            bugCounts["Worm"] = 0;
            bugCounts["Dragonfly"] = 0;
            bugCounts["Firefly"] = 0;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddBug(string bugName)
    {
        if (bugCounts.ContainsKey(bugName))
        {
            bugCounts[bugName]++;
        }
    }

    public Dictionary<string, int> GetAllBugCounts()
    {
        return bugCounts;
    }

}
