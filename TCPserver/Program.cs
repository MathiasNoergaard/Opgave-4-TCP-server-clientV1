using System.Net;
using System.Net.Sockets;

int CommandLogic(string command, int value1, int value2)
{
    switch (command)
    {
        case "random":
            Random randomGenerator = new();
            return randomGenerator.Next(value1, value2);
        case "add":
            return value1 + value2;
        case "subtract":
            return value1 - value2;
    }
    return 0;
}

void WriteToClient(StreamWriter writer, string message)
{
    writer.WriteLine(message);
    writer.Flush();
}

void HandleClient(TcpClient client)
{
    NetworkStream stream = client.GetStream();
    StreamReader reader = new(stream);
    StreamWriter writer = new(stream);
    Console.WriteLine("Client connected");

    WriteToClient(writer, "Enter one of following commands: random add subtract");

    string command = reader.ReadLine().ToLower().Trim();

    while (!(command == "random" || command == "add" || command == "subtract"))
    {
        WriteToClient(writer, "Command not recognized please try again");
        command = reader.ReadLine().ToLower().Trim();
        Console.WriteLine($".{command}.");
    }

    WriteToClient(writer, "Enter two numbers");

    string[] values = reader.ReadLine().Split(' ');
    int value1;
    int value2;
    while (Int32.TryParse(values[0], out value1) == false ||
          Int32.TryParse(values[1], out value2) == false)
    {
        WriteToClient(writer, "One or both values are invalid, please enter two numbers");
        values = reader.ReadLine().Split(' ');
    }

    WriteToClient(writer, $"{CommandLogic(command, value1, value2)}");

    client.Close();
    Console.WriteLine("Closed connection with client");
}

Console.WriteLine("Starting TCP server");
TcpListener listener = new(IPAddress.Any, 13000);
listener.Start();
Console.WriteLine("TCP server started");

while (true)
{
    TcpClient socket = listener.AcceptTcpClient();
    Thread thread = new(() => HandleClient(socket));
    thread.Start();
}

listener.Stop();
Console.WriteLine("Server stopped");