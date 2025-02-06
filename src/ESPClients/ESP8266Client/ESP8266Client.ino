#include <ESP8266WiFi.h>
#include <DHT.h>
#include <ESP8266HTTPClient.h>

#define wifi_ssid ""
#define wifi_password ""

String serverName = "http://10.95.140.147:5298/api";

#define DHTTYPE DHT22
#define DHTPIN 14

WiFiClient espClient;
DHT dht(DHTPIN, DHTTYPE, 11);  // 11 works fine for ESP8266

long lastMsg = 0;
float temp = 0.0;
float hum = 0.0;
float diff = 1.0;

void setup() {
  Serial.begin(9600);
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
}

bool checkBound(float newValue, float prevValue, float maxDiff) {
  return !isnan(newValue) && (newValue < prevValue - maxDiff || newValue > prevValue + maxDiff);
}

void sendHttpRequest(String dataType, float dataValue){
  HTTPClient http;
  WiFiClient client;

  String serverPath = serverName + "/" + dataType + "/inside";
  // Your Domain name with URL path or IP address with path
  http.begin(client, serverPath.c_str());
  // Specify content-type header
  http.addHeader("Content-Type", "application/json");

  String macAddress = WiFi.macAddress();
  Serial.println(macAddress);
  String httpRequestData = "{\"humidity\":" + String(dataValue) + ", \"sensorId\": \"" + macAddress + "\"}";
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

      Serial.print("Current temperature:");
      Serial.println(String(temp).c_str());
      sendHttpRequest("temperature", temp);

      Serial.print("Current humidity:");
      Serial.println(String(hum).c_str());
      sendHttpRequest("humidity", hum);

    } else {
      Serial.println("WiFi Disconnected");
    }
  }
}