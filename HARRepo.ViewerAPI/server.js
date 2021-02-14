'use strict';
var http = require('http');
var port = process.env.PORT || 1337;
var webconfig = require("webconfig");

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
loadSettings();

const { BlobServiceClient, StorageSharedKeyCredential } = require('@azure/storage-blob');
const url = require('url');

http.createServer(async (req, res) => {

    const query = url.parse(req.url, true).query;

    const account = process.env.StorageAccountName || settings.StorageAccountName;
    const accountKey = process.env.StorageAccountKey || settings.StorageAccountKey;

    const credential = new StorageSharedKeyCredential(account, accountKey);
    const client = new BlobServiceClient(process.env.BlobUrl || settings.BlobUrl, credential);

    const containerName = process.env.BlobContainerName || settings.BlobContainerName;
    const containerClient = client.getContainerClient(containerName);

    const blobClient = containerClient.getBlobClient(query.id);
    const HARResponse = await blobClient.download(0);
    const HARContent = (await streamToBuffer(HARResponse.readableStreamBody)).toString();

    res.writeHead(200, { 'Content-Type': 'application/json', "Access-Control-Allow-Origin": "*" });
    res.end(HARContent);

}).listen(port);

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