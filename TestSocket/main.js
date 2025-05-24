const express = require("express");
const bodyParser = require("body-parser");
const swaggerUi = require("swagger-ui-express");
const swaggerJsdoc = require("swagger-jsdoc");
const tls = require("tls");
const app = express();
const HTTP_PORT = 3000;

const GODOT_SERVER_ADDRESS = "127.0.0.1"; 
const GODOT_SERVER_PORT = 5000;

app.use(bodyParser.json());

// TCP Client Setup
let client;
let connected = false;

function connectToGodot() {

  const options = {
    host: GODOT_SERVER_ADDRESS,
    port: GODOT_SERVER_PORT,
    // For self-signed certs during development, set this to false:
    rejectUnauthorized: false,
    // You can add ca: fs.readFileSync('path-to-ca.pem') if you have CA certs
  };

  client = tls.connect(options, () => {
    if (client.authorized) {
      console.log(`Connected securely to Godot server at ${GODOT_SERVER_ADDRESS}:${GODOT_SERVER_PORT}`);
    } else {
      console.warn(`Connected to Godot server but TLS NOT authorized:`, client.authorizationError);
    }
    connected = true;

    client.write("HELLO_GODOT\n");
  });

  client.on("data", (data) => {
    console.log("Received from Godot:", data.toString());
  });

  client.on("close", () => {
    console.log("Connection to Godot closed");
    connected = false;
  });

  client.on("error", (err) => {
    console.error("Godot TLS error:", err.message);
    connected = false;
  });
}

connectToGodot();

// Express Route
/**
 * @swagger
 * /send:
 *   post:
 *     summary: Send a message to the Godot server
 *     requestBody:
 *       required: true
 *       content:
 *         application/json:
 *           schema:
 *             type: object
 *             properties:
 *               message:
 *                 type: string
 *                 example: "Hello from Swagger!"
 *     responses:
 *       200:
 *         description: Message sent successfully
 *       500:
 *         description: Connection issue
 */
app.post("/send", (req, res) => {
const { message } = req.body;

  if (!client || !client.writable) {
    return res.status(500).send("Not connected or client not writable");
  }

  try {
    client.write(message + "\n", (err) => {
      if (err) {
        console.error("Error writing to Godot:", err.message);
        return res.status(500).send("Failed to send message to Godot");
      }
      console.log("Sent to Godot:", message);
      res.send("Message sent to Godot server");
    });
  } catch (err) {
    console.error("Exception while sending message:", err.message);
    return res.status(500).send("Internal server error");
  }
});

/**
 * @swagger
 * /connect:
 *   post:
 *     summary: Connect to the Godot server
 *     responses:
 *       200:
 *         description: Connected successfully
 *       500:
 *         description: Connection issue
 */
app.post("/connect", (req, res) => {
  try {
    connectToGodot();
    return res.status(200).send("OK! Connected!");
  } catch (err) {
    console.error("Exception while connecting to godot:", err.message);
    return res.status(500).send("Internal server error");
  }
});

const swaggerOptions = {
  definition: {
    openapi: "3.0.0",
    info: {
      title: "Godot Control API",
      version: "1.0.0",
      description: "API to control Godot via TCP commands",
    },
  },
  apis: ["./main.js"], 
};

const swaggerSpec = swaggerJsdoc(swaggerOptions);
app.use("/api-docs", swaggerUi.serve, swaggerUi.setup(swaggerSpec));


app.listen(HTTP_PORT, () => {
  console.log(`Express server on http://localhost:${HTTP_PORT}`);
  console.log(`Swagger docs at http://localhost:${HTTP_PORT}/api-docs`);
});
