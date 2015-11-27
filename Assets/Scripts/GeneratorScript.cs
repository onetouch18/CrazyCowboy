using UnityEngine;
using System.Collections.Generic;

public class GeneratorScript : MonoBehaviour {

    public GameObject[] availableRooms;
    public List<GameObject> currentRooms;

    public GameObject[] avalaibleObjects;
    public List<GameObject> objects;

    public float objectsMinDistance = 5.0f;
    public float objectsMaxDistance = 10.0f;

    public float objectsMinY = -1.4f;
    public float objectsMaxY = 1.4f;

    int screenWidthInPoints;
    int counter = 0;
    string randomTag;
    int starsWave = 0;
    int obstaclesWave = 0;

	void Start ()
    {
        float height = 2.0f * Camera.main.orthographicSize;
        screenWidthInPoints = (int)(height * Camera.main.aspect);
	}

    void FixedUpdate()
    {
        GenerateRoomIfRequired();
        GenerateObjectsIfRequired();
    }

    void AddRoom(float farhtestRoomEndX)
    {
        int randomRommIndex = Random.Range(0, availableRooms.Length);

        GameObject room = (GameObject)Instantiate(availableRooms[randomRommIndex]);

        float roomWidth = room.transform.FindChild("floor").localScale.x;

        float roomCenter = farhtestRoomEndX + roomWidth * 0.5f;

        room.transform.position = new Vector3(roomCenter, 0, 0);

        currentRooms.Add(room);
    }

    void AddObject(float lastObjectX)
    {
        GameObject obj;
        float objectPositionX = lastObjectX + Random.Range(objectsMinDistance, objectsMaxDistance);

        string[] taggedObjects = new string[2] { "Stars", "Obstacle" };
        if(counter == 0)
            randomTag = taggedObjects[Random.Range(0,2)];

        if (starsWave == 1)
        {
            randomTag = "Obstacles";
            starsWave = 0;
        }
        else if (obstaclesWave >= Random.Range(1, 3))
        {
            randomTag = "Stars";
            obstaclesWave = 0;
        }

            if (randomTag == "Stars")
            {
                obj = (GameObject)Instantiate(avalaibleObjects[Random.Range(3, 6)]);
                obj.transform.position = new Vector3(objectPositionX, Random.Range(objectsMinY, objectsMaxY), 0);
                counter++;
                if (counter >= Random.Range(1, 4))
                {
                    counter = 0;
                    starsWave++;
                }
            }
            else
            {
                obj = (GameObject)Instantiate(avalaibleObjects[Random.Range(0, 3)]);
                obj.transform.position = new Vector3(objectPositionX, obj.transform.position.y, 0);
                counter++;
                if (counter >= Random.Range(3, 5))
                {
                    counter = 0;
                    obstaclesWave++;
                }
            }
        objects.Add(obj);
    }

    void GenerateRoomIfRequired()
    {
        List<GameObject> roomToRemove = new List<GameObject>();

        bool addRooms = true;

        float playerX = transform.position.x;

        float removeRoomX = playerX - screenWidthInPoints;

        float addRoomX = playerX + screenWidthInPoints;

        float farthestRoomEndX = 0;

        foreach(var room in currentRooms)
        {
            float roomWidth = room.transform.FindChild("floor").localScale.x;
            float roomStartX = room.transform.position.x - (roomWidth * 0.5f);
            float roomEndX = roomStartX + roomWidth;

            if (roomStartX > addRoomX)
                addRooms = false;

            if (roomEndX < removeRoomX)
                roomToRemove.Add(room);

            farthestRoomEndX = Mathf.Max(farthestRoomEndX, roomEndX);
        }

        foreach(var room in roomToRemove)
        {
            currentRooms.Remove(room);
            Destroy(room);
        }

        if (addRooms)
            AddRoom(farthestRoomEndX);
    }

    void GenerateObjectsIfRequired()
    {
        float playerX = transform.position.x;
        float removeObjectsX = playerX - screenWidthInPoints;
        float addObjectsX = playerX + screenWidthInPoints;
        float farthestObjectX = 0;

        List<GameObject> objectsToRemove = new List<GameObject>();

        foreach (var obj in objects)
        {
            float objX = obj.transform.position.x;

            farthestObjectX = Mathf.Max(farthestObjectX, objX);

            if (objX < removeObjectsX)
                objectsToRemove.Add(obj);
        }

        foreach (var obj in objectsToRemove)
        {
            objects.Remove(obj);
            Destroy(obj);
        }

        if (farthestObjectX < addObjectsX)
            AddObject(farthestObjectX);
    }
}
