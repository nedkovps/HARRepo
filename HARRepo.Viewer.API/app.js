const express = require('express');
const app = express();
const cors = require('cors');
const webconfig = require('webconfig');
const { BlobServiceClient, StorageSharedKeyCredential } = require('@azure/storage-blob');
const jwt = require('express-jwt');
const jwks = require('jwks-rsa');

const port = process.env.PORT || 8080;

let settings = null;
const loadSettings = async () => {
    const config = await webconfig
        .compile({
            sources: [
                './Web.config',
            ]
        });
    settings = config.appSettings;
}
loadSettings().then(() => {
    const jwtCheck = jwt({
        secret: jwks.expressJwtSecret({
            cache: true,
            rateLimit: true,
            jwksUri: settings.TokenUrl
        }),
        audience: settings.HARRepoAPIAudience,
        issuer: settings.TokenIssuer,
        algorithms: ['RS256']
    });

    var corsOptions = {
        origin: settings.AllowedOrigin,
        optionsSuccessStatus: 200 // some legacy browsers (IE11, various SmartTVs) choke on 204
    }

    app.use(cors(corsOptions));
    app.use(jwtCheck);

    app.get('/HAR', async (req, res) => {
        try {
            const account = process.env.StorageAccountName || settings.StorageAccountName;
            const accountKey = process.env.StorageAccountKey || settings.StorageAccountKey;

            const credential = new StorageSharedKeyCredential(account, accountKey);
            const client = new BlobServiceClient(process.env.BlobUrl || settings.BlobUrl, credential);

            const containerName = process.env.BlobContainerName || settings.BlobContainerName;
            const containerClient = client.getContainerClient(containerName);

            const blobClient = containerClient.getBlobClient(req.query.id);
            const HARResponse = await blobClient.download(0);
            const HARContent = (await streamToBuffer(HARResponse.readableStreamBody)).toString();

            res.set({
                'Content-Type': 'application/json'
            });
            res.send(HARContent);
        }
        catch (error) {
            res.send(error);
        }
    });

    app.listen(port);
});

const streamToBuffer = async (readableStream) => {
    return new Promise((resolve, reject) => {
        const chunks = [];
        readableStream.on("data", (data) => {
            chunks.push(data instanceof Buffer ? data : Buffer.from(data));
        });
        readableStream.on("end", () => {
            resolve(Buffer.concat(chunks));
        });
        readableStream.on("error", reject);
    });
}