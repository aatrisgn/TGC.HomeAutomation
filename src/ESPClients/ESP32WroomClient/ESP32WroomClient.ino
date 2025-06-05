#include <WiFi.h>
#include <HTTPClient.h>
#include <DHT.h>

#define wifi_ssid ""
#define wifi_password ""

String serverName = "http://api.homeautomation.dev.tgcportal.com/api";

#define DHTTYPE DHT22
#define DHTPIN 14  // GPIO14 (D14 WROOM)

WiFiClient espClient;
DHT dht(DHTPIN, DHTTYPE);

long lastMsg = 0;
float temp = 0.0;
float hum = 0.0;

void setup() {
  Serial.begin(9600);
  dht.begin();
  setup_wifi();
}

void setup_wifi() {
  delay(10);
  Serial.println();
  Serial.print("Connecting to ");
  Serial.println(wifi_ssid);

  WiFi.begin(wifi_ssid, wifi_password);

  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }

  Serial.println("\nWiFi connected");
  Serial.print("IP address: ");
  Serial.println(WiFi.localIP());

  Serial.print("MAC Address: ");
  Serial.println(WiFi.macAddress());
}

void sendHttpRequest(String dataType, float dataValue) {
  if (WiFi.status() == WL_CONNECTED) {
    HTTPClient http;
    WiFiClient client;

    String serverPath = serverName + "/measure/inside";
    http.begin(client, serverPath.c_str());
    http.addHeader("Content-Type", "application/json");

    String macAddress = WiFi.macAddress();
    String httpRequestData = "{\"dataValue\":" + String(dataValue) + ", \"type\": \"" + dataType + "\", \"macAddress\": \"" + macAddress + "\"}";
    Serial.println(httpRequestData);

    int httpResponse = http.POST(httpRequestData);

    if (httpResponse > 0) {
      Serial.print("HTTP Response code: ");
      Serial.println(httpResponse);
      String payload = http.getString();
      Serial.println(payload);
    } else {
      Serial.print("Error code: ");
      Serial.println(httpResponse);
    }
    http.end();
  } else {
    Serial.println("WiFi Disconnected");
  }
}

void loop() {
  delay(10000);

  long now = millis();

  if (now - lastMsg > 2000) {
    lastMsg = now;

    float newTemp = dht.readTemperature();
    float newHum = dht.readHumidity();

    if (isnan(newTemp) || isnan(newHum)) {
      Serial.println("Failed to read from DHT sensor!");
      return;
    }

    temp = newTemp;
    hum = newHum;

    Serial.print("Current temperature: ");
    Serial.println(temp);

    Serial.print("Current humidity: ");
    Serial.println(hum);

    sendHttpRequest("temperature", temp);
    sendHttpRequest("humidity", hum);
  }
}
