using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Defective.JSON;

[SerializeField]
public struct PointOfInterest
{
    //Variable declaration
    public string title;
    public VideoClip video;
    public AudioClip podcast;
    public Sprite[] illustrations;


    //Constructor (not necessary, but helpful)
    public PointOfInterest(string _title, string videoPath, string podcastPath, string illustrationPath)
    {
        this.title = _title;
        this.video = Resources.Load<VideoClip>(videoPath);
        this.illustrations = Resources.LoadAll<Sprite>(illustrationPath);
        this.podcast = Resources.Load<AudioClip>(podcastPath);
    }
}

[SerializeField]

struct Location
{
    //Variable declaration
    public string title;
    public string description;
    public Sprite image;
    public List<PointOfInterest> sites;

    //Constructor (not necessary, but helpful)
    public Location(string _title, string _description, string imagePath, List<PointOfInterest> _sites)
    {
        this.title = _title;
        this.description = _description;
        this.image = Resources.Load<Sprite>(imagePath);
        this.sites = _sites;
    }
}
public class SiteDatabase : MonoBehaviour
{
    public TextAsset jsonDatabase;
    public int selectedLocationIndex = 0;
    private List<Location> locations;


    // Start is called before the first frame update
    void Start()
    {
        
        JSONObject jsonObjects = new JSONObject(jsonDatabase.text);
        locations = new List<Location>();
        jsonObjects.GetField("sites", sites =>
        {
            for(int i = 0; i < sites.count; i++)
            {
                //              Debug.Log("site:" + sites[i].GetField("title").stringValue);
                string title = sites[i].GetField("title").stringValue;
                string description = sites[i].GetField("description").stringValue;
                string imagePath = sites[i].GetField("imagepath").stringValue;
                JSONObject landmarks = sites[i].GetField("landmarks");
                List<PointOfInterest> pois = new List<PointOfInterest>();

                for(int j = 0; j < landmarks.count; j++)
                {
                    string landmark_title = landmarks[j].GetField("title").stringValue;
                    string landmark_video = landmarks[j].GetField("title").stringValue;
                    string landmark_podcast = landmarks[j].GetField("title").stringValue;
                    string landmark_thumbnail = landmarks[j].GetField("imagepath").stringValue;
                    string landmark_illustration_path = landmarks[j].GetField("illustrations").stringValue;
                    PointOfInterest newLandmark = new PointOfInterest(landmark_title, landmark_video, landmark_podcast, landmark_illustration_path);
                    pois.Add(newLandmark);
                }

                //                Debug.Log("title: " + newSite.title);
                Debug.Log("Title: " + title);
                Debug.Log("Description: " + description);
                Debug.Log("Path: " + imagePath);
                Debug.Log("Landmark Count: " + pois.Count);
                Location site = new Location(title, description, imagePath, pois);
                locations.Add(site);
            }
        });
//        sites = JsonUtility.FromJson<Location[]>(Res);
        Debug.Log("LOCATIONS FOUND: " + locations.Count);
       
    }

    public void PopulateList(Transform t, GameObject prefab, GameObject titleObject)
    {
        if(prefab.GetComponent<SiteListing>())
        {
            for (int i = 0; i < locations.Count; i++)
            {
                GameObject go = Instantiate(prefab, t);
                go.GetComponent<SiteListing>().title.text = locations[i].title;
                go.GetComponent<SiteListing>().description.text = locations[i].description;
                go.GetComponent<SiteListing>().index = i;
                //TODO: Update JSON to have actual images selected
                //go.GetComponent<SiteListing>().thumbnail.sprite = locations[i].image;


            }
        }
        else if (prefab.GetComponent<Landmark>())
        {
            if (titleObject.GetComponent<Populator>())
            {
                titleObject.GetComponent<Populator>().headerTitle.text = locations[selectedLocationIndex].title;

            }

            for (int i = 0; i < locations[selectedLocationIndex].sites.Count; i++)
            {
                GameObject go = Instantiate(prefab, t);
                go.GetComponent<Landmark>().title.text = locations[selectedLocationIndex].sites[i].title;
                go.GetComponent<Landmark>().description.text = locations[selectedLocationIndex].sites[i].title;
                go.GetComponent<Landmark>().index = i;
            }
        }
    }
    
}