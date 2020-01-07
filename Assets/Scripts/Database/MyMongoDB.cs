using System;
using System.Collections.Generic;
using MongoDB.Driver;
using UnityEngine;

class MyMongoDB{
    private const string MONGO_URI = "mongodb+srv://worker:JsRqd9Cxf8Qcyiks@fe-cluster-jee7v.azure.mongodb.net/test?retryWrites=true&w=majority";
    private const string DATABASE_NAME = "FE";
    private MongoClient client;
    private IMongoDatabase db;

    private IMongoCollection<User> userCollection;
    private IMongoCollection<Score> scoreCollection;
    private bool init = false;

    public void Initialize()
    {
        if (!init)
        {
            Debug.Log("Initialize MongoDB");
            client = new MongoClient(MONGO_URI);
            db = client.GetDatabase(DATABASE_NAME);

            userCollection = db.GetCollection<User>("accounts");
            scoreCollection = db.GetCollection<Score>("scores");
            init = true;
        }
    }


    public List<User> AllUsers() 
    {
        Initialize();
        return userCollection.Find(user => true).ToList();
    }

    public List<Score> AllScores() {
        Initialize();
        return scoreCollection.Find(score => true).ToList();
    }

    public List<Score> FindMiniGamesScores(String minigame) {
        return scoreCollection.Find(score => score.miniGameName == minigame).ToList();
    }

    public User FindUserName(String username)
    {
        User player = new User();
        Initialize();
        try
        {
            player = userCollection.Find(user => user.username.Equals(username)).SingleOrDefault();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        return player;
    }

    public User FindUser(String u, String p)
    {
        User player = new User();
        Initialize();
        try
        {
            player = userCollection.Find(user => user.username.Equals(u) && user.password.Equals(p)).SingleOrDefault();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }   
        return player;
    }

    public bool FindScore(Score s)
    {
        Initialize();
        bool success = false;
        try
        {
            scoreCollection.Find(score => score.Equals(s)).SingleOrDefault();
            success = true;
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        return success;
    }

    public bool InsertUser(User newUser) 
    {
        Initialize();
        bool success = false;
        try
        {
            userCollection.InsertOne(newUser);
            success = true;
        } catch (Exception e) {
            Debug.Log(e);
        }
        return success;
    }

    public bool InsertScore(Score newScore)
    {
        Initialize();
        bool success = false;
        try
        {
            scoreCollection.InsertOne(newScore);
            success = true;
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        return success;
    }


    public bool ChangePassword(User newUser)
    {
        Initialize();
        bool success = false;
        try
        {
            userCollection.UpdateOne(Builders<User>.Filter.Eq("_id", newUser._id), Builders<User>.Update
                .Set(user => user.password, newUser.password)
                .Set(user => user.presetFlag, newUser.presetFlag));
            success = true;
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        return success;
    }
}

