const express = require('express');
const net = require('net');
const bodyParser = require('body-parser');
const swaggerUi = require('swagger-ui-express');
const swaggerJsdoc = require('swagger-jsdoc');

const app = express();
const HTTP_PORT = 3000; // Express API
const TCP_PORT = 4000; // TCP socket server

app.use(bodyParser.json());

let sockets = []; // Store all connected TCP clients

// 1. Create TCP server (YOU are the server)
const tcpServer = net.createServer((socket) => {
    console.log('New TCP client connected');

    sockets.push(socket);
    socket.write('Welcome to the TCP server!\n');
    socket.on('data', (data) => {
        console.log('Received from TCP client:', data.toString());
    });

    socket.on('end', () => {
        console.log('TCP client disconnected');
        sockets = sockets.filter(s => s !== socket);
    });

    socket.on('error', (err) => {
        console.error('TCP Socket error:', err);
        sockets = sockets.filter(s => s !== socket);
    });
});

tcpServer.listen(TCP_PORT, () => {
    console.log(`TCP Server listening on port ${TCP_PORT}`);
});

// 2. Express route to send message to all connected TCP clients
app.post('/send', (req, res) => {
    const { message } = req.body;

    if (sockets.length === 0) {
        return res.status(500).send('No TCP clients connected.');
    }

    sockets.forEach(socket => {
        socket.write(message);
    });

    console.log('Sent message to all TCP clients:', message);
    res.send('Message sent to all connected TCP clients');
});

// 3. Swagger setup
const swaggerOptions = {
    definition: {
        openapi: '3.0.0',
        info: {
            title: 'TCP Server Control API',
            version: '1.0.0',
        },
    },
    apis: ['./main.js'], // adjust to your file path if needed
};

const swaggerSpec = swaggerJsdoc(swaggerOptions);
app.use('/api-docs', swaggerUi.serve, swaggerUi.setup(swaggerSpec));

/**
 * @swagger
 * /send:
 *   post:
 *     summary: Send a message to all connected TCP clients
 *     requestBody:
 *       required: true
 *       content:
 *         application/json:
 *           schema:
 *             type: object
 *             properties:
 *               message:
 *                 type: string
 *                 example: "Hello TCP Client!"
 *     responses:
 *       200:
 *         description: Message sent successfully
 */

app.listen(HTTP_PORT, () => {
    console.log(`Express server running on http://localhost:${HTTP_PORT}`);
    console.log(`Swagger docs available at http://localhost:${HTTP_PORT}/api-docs`);
});
