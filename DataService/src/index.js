const express = require('express');

const app = express();

app.use(express.json());

app.get('/health', (req, res) => {
    res.json({ status: 'Ok', service: "DataService" });
});

app.post("/readings", (req, res) => {
    const readings = req.body;

    console.log("Received readings:", readings);

    res.status(201).json(readings);
});


const PORT = 5001;

app.listen(PORT, () => {
    console.log(`Data Service is running on port ${PORT}`);
});