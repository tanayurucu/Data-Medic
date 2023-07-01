# Data Medic

Data Medic is an open-source application designed to track the status of data flow sensors, such as MQTT, Docker, Kafka, and more. It allows you to monitor messages flowing through topics, check if Docker containers have any logs or are experiencing downtime, and provides valuable insights into the health and performance of your data processing infrastructure.

## Features

- **MQTT Sensor Tracking**: Data Medic enables you to monitor MQTT topics and track the arrival of messages in real-time. You can easily configure and manage multiple MQTT brokers and topics to ensure smooth data flow.

- **Docker Container Monitoring**: The application keeps an eye on your Docker containers, providing visibility into their status, logs, and availability. You can quickly identify any container-related issues and take necessary actions.

- **Kafka Message Tracking**: Data Medic allows you to track messages flowing through Kafka topics, ensuring the continuous flow of data in your streaming pipeline. You can view metrics, monitor partitions, and detect potential bottlenecks.

- **Open-Source and Customizable**: This project is open-source, which means you can modify and extend its functionality to fit your specific requirements. The codebase is written in .NET Core and follows a domain-driven design (DDD) architecture with CQRS (Command Query Responsibility Segregation) principles.

## Tech Stack

- .NET Core: The project is built using .NET Core, a cross-platform development framework for building modern applications.

- REST API: The application exposes a RESTful API that allows you to interact with the system, manage configurations, and retrieve data.

- Worker with Hangfire: Data Medic utilizes a worker process powered by Hangfire, a popular .NET library for background job processing. This enables efficient task scheduling and ensures the timely execution of critical monitoring tasks.

## Getting Started

To get started with Data Medic, follow these steps:

1. Clone the repository: `git clone https://github.com/your-username/data-medic.git`
2. Install the required dependencies using NuGet package manager: `dotnet restore`
3. Configure the application by updating the appropriate settings in the `appsettings.json` file.
4. Build the project: `dotnet build`
5. Run the application: `dotnet run`

For detailed instructions on how to configure and use Data Medic, please refer to the [Documentation](docs/README.md) section.

## Contributing

We welcome contributions from the community to enhance Data Medic and make it even more powerful. If you're interested in contributing, please follow our [Contribution Guidelines](CONTRIBUTING.md) for detailed instructions.

## License

Data Medic is released under the [MIT License](LICENSE), which allows you to use, modify, and distribute the software for both commercial and non-commercial purposes. Please refer to the [License](LICENSE) file for more information.

## Support

If you encounter any issues or have any questions, please feel free to open an [issue](https://github.com/your-username/data-medic/issues). Our team will be happy to assist you.
