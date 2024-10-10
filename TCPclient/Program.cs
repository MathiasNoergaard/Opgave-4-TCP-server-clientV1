using System.Net.Sockets;

string WriteAndReadToServer(StreamWriter writer, StreamReader reader)
{
    writer.WriteLine(Console.ReadLine());
    writer.Flush();
    string message = reader.ReadLine();
    Console.WriteLine(message);
    return message;
}

Console.WriteLine("Starting TCP Client");

TcpClient socket = new();
socket.Connect("127.0.0.1", 13000); //connect to localhost
NetworkStream stream = socket.GetStream();
StreamReader reader = new(stream);
StreamWriter writer = new(stream);
Console.WriteLine("Connected to the TCP server");

string message = null;
//receive initial message from server
message = reader.ReadLine();
Console.WriteLine(message);

//send the command to the server
do
{
    message = WriteAndReadToServer(writer, reader);
} while (message == "Command not recognized please try again");

//send the values to the server
string value1;
string value2;
do
{
    Console.WriteLine("Enter first value");
    value1 = Console.ReadLine();
    Console.WriteLine("Enter second value");
    value2 = Console.ReadLine();
    writer.WriteLine($"{value1.Trim()} {value2.Trim()}");
    writer.Flush();
    message = reader.ReadLine();
    Console.WriteLine(message);
} while (message == "One or both values are invalid, please enter two numbers");

socket.Close();
Console.WriteLine("TCP client closed");