using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public GameObject[] backgrounds;
    [SerializeField]
    SectionManager sectionManager;
    [SerializeField]
    private GameObject currentBackground;
    // Start is called before the first frame update
    void Start()
    {
        sectionManager =FindObjectOfType<SectionManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void ChangeBackground()
    {
        GameObject newBackGround;
        if((sectionManager.sectionCount/sectionManager.maxSections) == 0)
        {
            newBackGround = backgrounds[0];
        }
        else if((sectionManager.sectionCount / sectionManager.maxSections) == 1)
        {
            newBackGround = backgrounds[1];
        }
        else
        {
            newBackGround = backgrounds[2];
        }
        currentBackground = Instantiate(newBackGround);
    }
}
