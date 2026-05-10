public class WeatherLinkedList {
    private Node head;  // Head node of the list

    // Constructor to initialize the list
    public WeatherLinkedList() {
        head = null;
    }

    // Method to add a weather record to the linked list
    public void addWeatherData(WeatherRecord record) {
        Node newNode = new Node(record);  // Create a new node
        if (head == null) {
            head = newNode;  // If list is empty, set the new node as the head
        } else {
            Node current = head;
            while (current.next != null) {
                current = current.next;  // Traverse to the last node
            }
            current.next = newNode;  // Add the new node at the end of the list
        }
    }
    // Method to delete a specific weather record by date
    public void deleteWeatherDataByDate(String date) {
        if (head == null) {
            System.out.println("No weather data available.");
            return;
        }

        // Special case: If the record to be deleted is the head
        if (head.data.getDate().equals(date)) {
            head = head.next;  // Move head to the next node
            System.out.println("Weather record for date " + date + " has been deleted.");
            return;
        }

        Node current = head;
        Node previous = null;

        // Traverse the list to find the record with the given date
        while (current != null) {
            if (current.data.getDate().equals(date)) {
                previous.next = current.next;  // Skip the current node
                System.out.println("Weather record for date " + date + " has been deleted.");
                return;
            }
            previous = current;
            current = current.next;
        }

        // If we reached here, no matching record was found
        System.out.println("No data found for the given date.");
    }


    // Method to display all weather records in the list
    public void viewWeatherData() {
        if (head == null) {
            System.out.println("No weather data available.");
            return;
        }
        Node current = head;
        while (current != null) {
            current.data.displayRecord();  // Print the data in each node
            current = current.next;       // Move to the next node
        }
    }

    // Method to find the hottest and coldest days
    public void findHottestAndColdestDays() {
        if (head == null) {
            System.out.println("No data available.");
            return;
        }
        Node current = head;
        WeatherRecord hottest = current.data;
        WeatherRecord coldest = current.data;

        while (current != null) {
            if (current.data.getTemperature() > hottest.getTemperature()) {
                hottest = current.data;
            }
            if (current.data.getTemperature() < coldest.getTemperature()) {
                coldest = current.data;
            }
            current = current.next;
        }
        System.out.println("Hottest day: ");
        hottest.displayRecord();
        System.out.println("Coldest day: ");
        coldest.displayRecord();
    }

    // Method to calculate average temperature
    public void calculateAverageTemperature() {
        if (head == null) {
            System.out.println("No data available.");
            return;
        }
        Node current = head;
        double sum = 0;
        int count = 0;
        while (current != null) {
            sum += current.data.getTemperature();
            count++;
            current = current.next;
        }
        double average = sum / count;
        System.out.println("Average Temperature: " + average + "°C");
    }

    // Method to calculate total and average rainfall
    public void rainfallAnalysis() {
        if (head == null) {
            System.out.println("No data available.");
            return;
        }
        Node current = head;
        double totalRainfall = 0;
        int count = 0;
        while (current != null) {
            totalRainfall += current.data.getRainfall();
            count++;
            current = current.next;
        }
        double averageRainfall = totalRainfall / count;
        System.out.println("Total Rainfall: " + totalRainfall + "mm");
        System.out.println("Average Rainfall: " + averageRainfall + "mm");
    }

    // Method to search for weather data by date
    public void searchWeatherDataByDate(String date) {
        if (head == null) {
            System.out.println("No weather data available.");
            return;
        }
        Node current = head;
        boolean found = false;
        while (current != null) {
            if (current.data.getDate().equals(date)) {
                current.data.displayRecord();
                found = true;
                break;
            }
            current = current.next;
        }
        if (!found) {
            System.out.println("No data found for the given date.");
        }
    }
    // Method to search for weather data by temperature
    public void searchWeatherDataByTemperature(double temperature) {
        if (head == null) {
            System.out.println("No weather data available.");
            return;
        }

        Node current = head;
        boolean found = false;
        while (current != null) {
            if (current.data.getTemperature() == temperature) {
                current.data.displayRecord();
                found = true;
            }
            current = current.next;
        }

        if (!found) {
            System.out.println("No data found for the given temperature.");
        }
    }

    // Method to search for weather data by humidity
    public void searchWeatherDataByHumidity(double humidity) {
        if (head == null) {
            System.out.println("No weather data available.");
            return;
        }

        Node current = head;
        boolean found = false;
        while (current != null) {
            if (current.data.getHumidity() == humidity) {
                current.data.displayRecord();
                found = true;
            }
            current = current.next;
        }

        if (!found) {
            System.out.println("No data found for the given humidity.");
        }
    }

}
