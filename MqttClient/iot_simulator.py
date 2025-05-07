import paho.mqtt.client as mqtt
import json
import time
import random
from datetime import datetime, timezone

# Connect to Mosquitto broker running locally
client = mqtt.Client()
client.connect("host.docker.internal", 1883, 60)

while True:
    payload = {
        "SensorType": "Temperature",  # You can hardcode it or randomly switch types if needed
        "Value": round(random.uniform(20.0, 30.0), 2),
        "Timestamp": datetime.now(timezone.utc).isoformat()  # UTC ISO8601 format
    }
    client.publish("greenhouse/sensors", json.dumps(payload))
    print(f"Published: {payload}")
    time.sleep(5)
