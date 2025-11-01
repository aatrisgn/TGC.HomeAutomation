#include <DHT.h>
#include <Arduino.h>
#include <ESP8266WiFi.h>
#include <ESP8266HTTPClient.h>
#include <WiFiClientSecureBearSSL.h>

#define wifi_ssid ""
#define wifi_password ""

String serverName = "";

#define DHTTYPE DHT22
#define DHTPIN 14 //G5 on ESP8266

WiFiClient espClient;
std::unique_ptr<BearSSL::WiFiClientSecure> client(new BearSSL::WiFiClientSecure);
DHT dht(DHTPIN, DHTTYPE);

String deviceId = "";
String deviceApiKey = "";

long lastMsg = 0;
float temp = 0.0;
float hum = 0.0;
String macAddress = "";

void setup() {
  Serial.begin(115200);
  dht.begin();
  setup_wifi();
}

void setup_wifi() {
  delay(10);
  // We start by connecting to a WiFi network
  Serial.println();
  Serial.print("Connecting to ");
  Serial.println(wifi_ssid);
  WiFi.begin(wifi_ssid, wifi_password);

  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }

  Serial.println("");
  Serial.println("WiFi connected");
  Serial.println("IP address: ");
  Serial.println(WiFi.localIP());

  macAddress = WiFi.macAddress();
  Serial.println(macAddress);

  // Ignore SSL certificate validation
  client->setInsecure();
}

void sendHttpRequest(String dataType, float dataValue){
  HTTPClient http;

  String serverPath = serverName + "/measure/inside";
  // Your Domain name with URL path or IP address with path
  http.begin(*client, serverPath.c_str());
  // Specify content-type header
  http.addHeader("Content-Type", "application/json");
  http.addHeader("x-device-id", deviceId);
  http.addHeader("x-device-api-key", deviceApiKey);

  String macAddress = WiFi.macAddress();
  Serial.println(macAddress);
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
}

void loop() {
  delay(10000);

  long now = millis();
  
  if (WiFi.status() == WL_CONNECTED) {
    if (now - lastMsg > 2000) {
      // Send HTTP POST request
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

    } else {
      Serial.println("WiFi Disconnected");
    }
  }
}