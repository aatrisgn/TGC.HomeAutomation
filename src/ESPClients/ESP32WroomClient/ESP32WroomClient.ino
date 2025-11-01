#include <WiFi.h>
#include <HTTPClient.h>
#include <DHT.h>
#include <WiFiClientSecure.h>

#define wifi_ssid ""
#define wifi_password ""

#define DHTTYPE DHT22
#define DHTPIN 14  // GPIO14 (D14 WROOM)

WiFiClient espClient;
HTTPClient http;

WiFiClientSecure *client = new WiFiClientSecure;
DHT dht(DHTPIN, DHTTYPE);

const char * serverUri = "";
String macAddress = "";
String deviceId = "";
String deviceApiKey = "";
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
  macAddress = WiFi.macAddress();
  Serial.println(macAddress);

  client->setInsecure();
}

void sendHttpRequest(String dataType, float dataValue) {
  if (WiFi.status() == WL_CONNECTED) {

    http.begin(serverUri);
    http.addHeader("Content-Type", "application/json");
    http.addHeader("x-device-id", deviceId);
    http.addHeader("x-device-api-key", deviceApiKey);

    String httpRequestData = "{\"dataValue\":" + String(dataValue) + ", \"type\": \"" + dataType + "\", \"macAddress\": \"" + macAddress + "\"}";
    Serial.println(httpRequestData);

    int httpResponse = http.POST(httpRequestData);

    if (httpResponse > 0) {
      Serial.print("HTTP Response code: ");
      Serial.println(httpResponse);
      Serial.println(http.getString());
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

  Serial.print("Free heap: ");
  Serial.println(ESP.getFreeHeap());

  float newTemp = dht.readTemperature();
  float newHum = dht.readHumidity();

  if (isnan(newTemp) || isnan(newHum)) {
    Serial.println("Failed to read from DHT sensor!");
    return;
  }

  Serial.print("Current temperature: ");
  Serial.println(newTemp);

  Serial.print("Current humidity: ");
  Serial.println(newHum);

  sendHttpRequest("temperature", newTemp);
  sendHttpRequest("humidity", newHum);
}
