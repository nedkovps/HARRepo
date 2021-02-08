'use strict';
var http = require('http');
var port = process.env.PORT || 1337;

const { BlobServiceClient, StorageSharedKeyCredential } = require('@azure/storage-blob');
const url = require('url');

http.createServer(async (req, res) => {

    const query = url.parse(req.url, true).query;

    const account = process.env.ACCOUNT_NAME || "devstoreaccount1";
    const accountKey = process.env.ACCOUNT_KEY || "Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==";

    const credential = new StorageSharedKeyCredential(account, accountKey);
    const client = new BlobServiceClient('http://127.0.0.1:10000/devstoreaccount1', credential);

    const containerName = 'harstorage';
    const containerClient = client.getContainerClient(containerName);

    const blobClient = containerClient.getBlobClient(query.id);
    const HARResponse = await blobClient.download(0);
    const HARContent = (await streamToBuffer(HARResponse.readableStreamBody)).toString();

    res.writeHead(200, { 'Content-Type': 'text/plain' });
    res.end(HARContent);
}).listen(port);

async function streamToBuffer(readableStream) {
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