import java.util.Scanner;

public class WeatherRecord {

    private String date;
    private double temperature;
    private double humidity;
    private double rainfall;

    // Constructor
    public WeatherRecord(String date, double temperature, double humidity, double rainfall) {
        this.date = date;
        this.temperature = temperature;
        this.humidity = humidity;
        this.rainfall = rainfall;
    }

    // Getters and Setters
    public String getDate() {
        return date;
    }

    public double getTemperature() {
        return temperature;
    }

    public double getHumidity() {
        return humidity;
    }

    public double getRainfall() {
        return rainfall;
    }

    // Display the weather record
    public void displayRecord() {
        System.out.println("Date: " + date + ", Temperature: " + temperature + "°C, Humidity: " + humidity + "%, Rainfall: " + rainfall + "mm");
    }
}
