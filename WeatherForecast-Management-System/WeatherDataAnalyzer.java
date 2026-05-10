import java.util.Scanner;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public class WeatherDataAnalyzer {
    private WeatherLinkedList weatherData;

    // Constructor
    public WeatherDataAnalyzer() {
        weatherData = new WeatherLinkedList();
    }

    // Show menu for user interaction
    public void showMenu() {
        Scanner scanner = new Scanner(System.in);

        while (true) {
            try {
                System.out.println("\nWeather Data Analyzer - Menu:");
                System.out.println("1. Add Weather Data");
                System.out.println("2. View Weather Data");
                System.out.println("3. Find Hottest and Coldest Days");
                System.out.println("4. Calculate Average Temperature");
                System.out.println("5. Average Rainfall");
                System.out.println("6. Search Weather Data by Date");
                System.out.println("7. Delete Weather Data by Date");
                System.out.println("8. Search Weather Data by Temperature");
                System.out.println("9. Search Weather Data by Humidity");
                System.out.println("10. Exit");
                System.out.print("Enter your choice: ");

                int choice = scanner.nextInt();
                scanner.nextLine();  // Consume newline

                switch (choice) {
                    case 1:
                        addWeatherData(scanner);
                        break;
                    case 2:
                        weatherData.viewWeatherData();
                        break;
                    case 3:
                        weatherData.findHottestAndColdestDays();
                        break;
                    case 4:
                        weatherData.calculateAverageTemperature();
                        break;
                    case 5:
                        weatherData.rainfallAnalysis();
                        break;
                    case 6:
                        searchWeatherData(scanner);
                        break;
                    case 7:
                        deleteWeatherData(scanner);
                        break;
                    case 8:
                        searchByTemperature(scanner);
                        break;
                    case 9:
                        searchByHumidity(scanner);
                        break;
                    case 10:
                        System.out.println("Exiting program...");
                        return;
                    default:
                        System.out.println("Invalid choice. Please try again.");
                }
            } catch (Exception e) {
                System.out.println("Error: " + e.getMessage());
                scanner.nextLine(); // Clear the buffer
            }
        }
    }



    // Method to add weather data with exception handling for input validation
    private void addWeatherData(Scanner scanner) {
        try {
            System.out.print("Enter Date (YYYY-MM-DD): ");
            String date = getValidDateInput(scanner);

            System.out.print("Enter Temperature (°C): ");
            double temp = getValidTemperatureInput(scanner);

            System.out.print("Enter Humidity (%): ");
            double humidity = getValidHumidityInput(scanner);

            System.out.print("Enter Rainfall (mm): ");
            double rainfall = getValidRainfallInput(scanner);

            weatherData.addWeatherData(new WeatherRecord(date, temp, humidity, rainfall));
        } catch (NumberFormatException e) {
            System.out.println("Invalid number format. Please enter a valid number.");
        } catch (Exception e) {
            System.out.println("Error: " + e.getMessage());
        }
    }

    // Method to validate temperature input (with C symbol and not exceeding 56.7°C)
    private double getValidTemperatureInput(Scanner scanner) throws Exception {
        while (true) {
            String input = scanner.nextLine();
            if (input.endsWith("C")) {
                try {
                    double temp = Double.parseDouble(input.substring(0, input.length() - 1));
                    if (temp <= 56.7) {
                        return temp;
                    } else {
                        System.out.println("Temperature cannot exceed 56.7°C. Please enter a valid temperature.");
                    }
                } catch (NumberFormatException e) {
                    System.out.println("Invalid temperature input. Please enter a valid number followed by 'C'.");
                }
            } else {
                System.out.println("Temperature must be followed by 'C'. Please try again.");
            }
        }
    }

    // Method to get a valid double input with exception handling
    private double getValidDoubleInput(Scanner scanner) {
        while (true) {
            try {
                return Double.parseDouble(scanner.nextLine());
            } catch (NumberFormatException e) {
                System.out.println("Invalid input! Please enter a valid number.");
            }
        }
    }

    // Method to validate date input in YYYY-MM-DD format
    private String getValidDateInput(Scanner scanner) throws Exception {
        while (true) {
            String date = scanner.nextLine();
            // Regular expression to match date in YYYY-MM-DD format
            String regex = "^(\\d{4})-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])$";
            Pattern pattern = Pattern.compile(regex);
            Matcher matcher = pattern.matcher(date);
            if (matcher.matches()) {
                int month = Integer.parseInt(matcher.group(2));
                int day = Integer.parseInt(matcher.group(3));

                // Validate days based on month
                if (isValidDayForMonth(month, day)) {
                    return date;
                } else {
                    System.out.println("Invalid day for the given month. Please enter a valid date.");
                }
            } else {
                System.out.println("Invalid date format! Please enter the date in YYYY-MM-DD format.");
            }
        }
    }

    // Method to check if the day is valid for the given month
    private boolean isValidDayForMonth(int month, int day) {
        switch (month) {
            case 2: // February
                return day <= 29; // For simplicity, consider 29 days for February
            case 4: case 6: case 9: case 11: // April, June, September, November
                return day <= 30;
            default: // January, March, May, July, August, October, December
                return day <= 31;
        }
    }

    // Method to validate humidity input (with % symbol)
    private double getValidHumidityInput(Scanner scanner) throws Exception {
        while (true) {
            String input = scanner.nextLine();
            if (input.endsWith("%")) {
                try {
                    return Double.parseDouble(input.substring(0, input.length() - 1));
                } catch (NumberFormatException e) {
                    System.out.println("Invalid humidity input. Please enter a valid number followed by '%'.");
                }
            } else {
                System.out.println("Humidity must be followed by a '%' symbol. Please try again.");
            }
        }
    }

    // Method to validate rainfall input (with mm suffix)
    private double getValidRainfallInput(Scanner scanner) throws Exception {
        while (true) {
            String input = scanner.nextLine();
            if (input.endsWith("mm")) {
                try {
                    return Double.parseDouble(input.substring(0, input.length() - 2));
                } catch (NumberFormatException e) {
                    System.out.println("Invalid rainfall input. Please enter a valid number followed by 'mm'.");
                }
            } else {
                System.out.println("Rainfall must be followed by 'mm'. Please try again.");
            }
        }
    }

    // Method to search weather data by date with exception handling
    private void searchWeatherData(Scanner scanner) {
        try {
            System.out.print("Enter Date to Search (YYYY-MM-DD): ");
            String searchDate = scanner.nextLine();
            weatherData.searchWeatherDataByDate(searchDate);
        } catch (Exception e) {
            System.out.println("Error: " + e.getMessage());
        }
    }
    // Method to handle deletion of weather data by date
    private void deleteWeatherData(Scanner scanner) {
        System.out.print("Enter Date to Delete (YYYY-MM-DD): ");
        String dateToDelete = scanner.nextLine();
        weatherData.deleteWeatherDataByDate(dateToDelete);
    }
    // Method to search weather data by temperature
    private void searchByTemperature(Scanner scanner) {
        System.out.print("Enter Temperature to Search (°C): ");
        double temperature = getValidDoubleInput(scanner);  // Make sure input is a valid number
        weatherData.searchWeatherDataByTemperature(temperature);
    }

    // Method to search weather data by humidity
    private void searchByHumidity(Scanner scanner) {
        System.out.print("Enter Humidity to Search (%): ");
        double humidity = getValidDoubleInput(scanner);  // Make sure input is a valid number
        weatherData.searchWeatherDataByHumidity(humidity);
    }


}
