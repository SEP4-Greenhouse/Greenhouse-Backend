require('dotenv').config();
const mqtt = require('mqtt');
const axios = require('axios');

const options = {
  username: process.env.MQTT_USERNAME,
  password: process.env.MQTT_PASSWORD,
};

const client = mqtt.connect(process.env.MQTT_BROKER_URL, options);

const topics = [
  'greenhouse/sensor/temperature',
  'greenhouse/sensor/humidity',
  'greenhouse/sensor/moisture',
];

client.on('connect', () => {
  console.log('Connected to MQTT broker');
  client.subscribe(topics, (err) => {
    if (err) {
      console.error('Subscription error:', err);
    } else {
      console.log('Subscribed to topics:', topics);
    }
  });
});

client.on('message', async (topic, message) => {
  const msg = message.toString();
  console.log(`[MQTT] Received on ${topic}:`, msg);

  try {
    const payload = JSON.parse(msg);
    console.log(`[DEBUG] Parsed payload:`, payload);

    await axios.post(`${process.env.API_BASE_URL}/reading`, payload);
    console.log('Forwarded to API:', payload);
  } catch (err) {
    console.error('[ERROR] Failed to process MQTT message:', err.message);
  }
});