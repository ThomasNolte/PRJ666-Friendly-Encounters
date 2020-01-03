using System;
using System.Collections.Generic;
using MongoDB.Driver;
using UnityEngine;

class MyMongoDB{
    private const string MONGO_URI = "mongodb+srv://pluu3:Hakki-yo1@fe-cluster-jee7v.azure.mongodb.net/test?retryWrites=true&w=majority";
    private const string DATABASE_NAME = "test";
    private MongoClient client;
    private IMongoDatabase db;

    public void Initialize()
    {
        Debug.Log("Initialize MongoDB");
        client = new MongoClient(MONGO_URI);
        db = client.GetDatabase(DATABASE_NAME);

        IMongoCollection<User> userCollection = db.GetCollection<User>("FE");
        List<User> userModelList = userCollection.Find(user => true).ToList();
        foreach (User u in userModelList) {
            Debug.Log(u._id);
            Debug.Log(u.username);
            Debug.Log(u.email);
            Debug.Log(u.password);
        }
    }
}
