const express = require('express');
const {connectDb, getDb} = require('./db');
const {ObjectId} = require('mongodb');

const app = express();
app.use(express.json());

//#region Checking DB connection

app.get('/health', async (req, res) => {
    try{
        const db = getDb();
        await db.command({ ping: 1 });
        res.json({ status: 'Ok', service: "DataService", mongo: 'Connected' });
    }
    catch{
        res.status(500).json({ status: 'Error', service: "DataService", mongo: 'Not Connected' });
    }
});

//#endregion Checking DB connection

//#region Post

app.post("/readings", async (req, res) => {
    const reading = req.body;

    if(reading.utc == null){
        return res.status(400).json({ error: "Missing 'Utc' field in the request body." });
    }

    const db = getDb();
    const collection = db.collection('readings');

    const doc = {
        ...reading,
        createdAt: new Date() 
    };
    
    const result = await collection.insertOne(doc);

    res.status(201).json({id: result.insertedId, ...doc});
});

//#endregion Post

//#region Get

app.get("/readings", async (req, res) => {

    const db = getDb();
    const collection = db.collection('readings');

    const items= await collection.find().sort({ utc: -1 }).toArray();

    res.json(items);
});

app.get("/readings/:id",async (req, res) => {
    const db = getDb();
    const collection = db.collection('readings');

    try{
        const item = await collection.findOne({_id: new ObjectId(req.params.id)});
        if(!item)
            return res.status(404).json({error: "Not Found"});

        res.json(item);
    }
    catch{
        res.status(400).json({error: "Invalid ID format"});
    }
});

//#endregion Get

const PORT = process.env.PORT || 5001;

connectDb()
.then(() => {
    app.listen(PORT, () =>{
        console.log(`Data Service is running on port ${PORT}`);
    });
})
.catch((err) => {
    console.error("Failed to connect to MongoDB:", err);
    process.exit(1);
});

app.listen(PORT, "0.0.0.0", () => {
  console.log(`DataService running on port ${PORT}`);
});

