// See https://aka.ms/new-console-template for more information
using HitachiTask;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;

string senderEmail = "";
string receiverEmail = "";
string password = "";
string fileNamePath = "";

questions(ref senderEmail, ref receiverEmail, ref password, ref fileNamePath);

try
{
    MailMessage mailMessage = new MailMessage();
    mailMessage.From = new MailAddress(senderEmail);
    mailMessage.Subject = "Weather Conditions Criteria";
    mailMessage.To.Add(new MailAddress(receiverEmail));
    mailMessage.Body = "<html><body>This email contains a file with Weather Conditions for a rocket launch.</body></html>";
    mailMessage.IsBodyHtml = true;

    var client = new SmtpClient("smtp.abv.bg")
    {
        Port = 465,
        UseDefaultCredentials = false,
        Credentials = new NetworkCredential(senderEmail, password),
        EnableSsl = true,
        DeliveryMethod = SmtpDeliveryMethod.Network
        //Timeout = 10000 // set the timeout value to 10 seconds
    };

    Attachment attachment = new Attachment(fileNamePath);
    mailMessage.Attachments.Add(attachment);

    client.Send(mailMessage);

    Console.WriteLine("Email sent successfully.");
}
catch (SmtpException ex)
{
    Console.WriteLine("An error occurred while sending the email: " + ex.Message);
}

return;
static void questions(ref string senderEmail, ref string receiverEmail, ref string password, ref string fileNamePath)
{

    String line;
    int count;
    bool correctSyntEmail = true;
    bool correctFileName = true;

    Console.Write("Type your email: ");
    senderEmail = Console.ReadLine();

    if (!senderEmail.Contains('@'))
    {
        Console.Write("Please include '@' in the email \nType your email: ");
        senderEmail = Console.ReadLine();
        correctSyntEmail = false;
    }

    while (!correctSyntEmail)
    {
        if (!senderEmail.Contains('@'))
        {
            Console.Write("Please include '@' in the email \nType your email: ");
            senderEmail = Console.ReadLine();
        }
        else
        {
            correctSyntEmail = true;
        }
    }

    correctSyntEmail = true;

    Console.Write("Type your password: ");
    password = Console.ReadLine();
    Console.Write("Type receiver email: ");
    receiverEmail = Console.ReadLine();

    if (!receiverEmail.Contains('@'))
    {
        Console.Write("Please include '@' in the email \nType receiver email: ");
        receiverEmail = Console.ReadLine();
        correctSyntEmail = false;
    }

    while (!correctSyntEmail)
    {

        if (!receiverEmail.Contains('@'))
        {
            Console.Write("Please include '@' in the email \nType email: ");
            receiverEmail = Console.ReadLine();
        }
        else
        {
            correctSyntEmail = true;
        }
    }

    WeatherParameterData daysData = new WeatherParameterData();
    WeatherParameterData temperatureData = new WeatherParameterData();
    WeatherParameterData windData = new WeatherParameterData();
    WeatherParameterData humidityData = new WeatherParameterData();
    WeatherParameterData precipitationData = new WeatherParameterData();
    WeatherParameterData lightningData = new WeatherParameterData();
    WeatherParameterData cloudsData = new WeatherParameterData();

    Console.Write("Type file input: ");
    fileNamePath = Console.ReadLine();

    if (!fileNamePath.Contains("WeatherForecast.csv") || !fileNamePath.Contains("WeatherForecast"))
    {
        Console.WriteLine("Please enter correct file name!");
        fileNamePath = Console.ReadLine();
        correctFileName = false;
    }

    while(!correctFileName)
    {
        if (!fileNamePath.Contains("WeatherForecast.csv") || !fileNamePath.Contains("WeatherForecast"))
        {
            Console.WriteLine("Please enter correct file name!");
            fileNamePath = Console.ReadLine();
        }
        else
        {
            correctFileName = true;
        }
    }

    try
    {
        StreamReader sr = new StreamReader(fileNamePath);
        // Create a new StreamWriter object
        if (!File.Exists("WeatherReport.csv"))
        {
            // Create the file if it doesn't exist
            File.Create("WeatherReport.csv").Close();
        }
        StreamWriter sw = new StreamWriter("WeatherReport.csv", false);

                // Read past the first line of the file, which contains the column headings
                line = sr.ReadLine();
                line = sr.ReadLine(); // read the second line (temperature values)
               // Console.Write("LINE:" +line);
                    string[] temperatureValues = line.Split(';');

                    // loop through the temperature values and print them out
                    for (int i = 1; i < temperatureValues.Length; i++) // start from index 1 to skip the empty value before the first temperature
                    {
                        if (int.TryParse(temperatureValues[i], out int temperature) && temperature >= 2 && temperature <= 31)
                        {
                            // Console.WriteLine($"Day {i}: {temperature} C");
                            // Calculate average
                                temperatureData.AverageValue += temperature;
                                temperatureData.MedianValues.Add(temperature);
                            // Calculate maximum
                                temperatureData.MaxValue = Math.Max(temperatureData.MaxValue, temperature);
                            // Calculate minimum
                                temperatureData.MinValue = Math.Min(temperatureData.MinValue, temperature);
                                sw.WriteLine($"Day {i}: {temperature} C");
                        }
                    }
                            temperatureData.AverageValue /= (temperatureValues.Length - 1); // Divide by the number of data points to get average
                            temperatureData.MedianValues.Sort();

                    count = temperatureData.MedianValues.Count;
                    if (count % 2 == 0)
                    {
                        // If the count is even, average the two middle values
                        int middle1 = count / 2 - 1;
                        int middle2 = count / 2;
                        temperatureData.MedianValue = (temperatureData.MedianValues[middle1] + temperatureData.MedianValues[middle2]) / 2;
                    }
                    else
                    {
                        // If the count is odd, take the middle value
                        int middle = count / 2;
                        temperatureData.MedianValue = temperatureData.MedianValues[middle];
                    }

sw.WriteLine("Temp Math.max:" + temperatureData.MaxValue + "Temp Math.min:" + temperatureData.MinValue + "Temp Average:" + temperatureData.AverageValue + "Temp Median value:" + temperatureData.MedianValue);

        line = sr.ReadLine();
                            string[] windValues = line.Split(';');

                            for (int i = 1; i < windValues.Length; i++)
                            {
                            if(int.TryParse(windValues[i], out int windValue) && windValue <= 10)
                                {

                                    windData.AverageValue += windValue;

                                    windData.MedianValues.Add(windValue);    

                                    windData.MaxValue = Math.Max(windData.MaxValue, windValue);

                                    windData.MinValue = Math.Min(windData.MinValue, windValue);

                                    sw.WriteLine($"Day {i}: {windValue} (m/s)");
            }
                            }
                                windData.AverageValue /= (windValues.Length - 1);
                                windData.MedianValues.Sort();

                    count = windData.MedianValues.Count;
                    if (count % 2 == 0)
                    {
                        // If the count is even, average the two middle values
                        int middle1 = count / 2 - 1;
                        int middle2 = count / 2;
                        windData.MedianValue = (windData.MedianValues[middle1] + windData.MedianValues[middle2]) / 2;
                    }
                    else
                    {
                        // If the count is odd, take the middle value
                        int middle = count / 2;
                        windData.MedianValue = windData.MedianValues[middle];
                    }

sw.WriteLine("Wind Math.max:" + windData.MaxValue + "Wind Math.min:" + windData.MinValue + "Wind Average:" + windData.AverageValue + "wind Median value:" + windData.MedianValue);


                            line = sr.ReadLine();
                            string[] humidityValues = line.Split(';');

                            for (int i = 1; i < humidityValues.Length; i++)
                            {
                                if (int.TryParse(humidityValues[i], out int humidityValue) && humidityValue < 60)
                                {

                                        humidityData.AverageValue += humidityValue;

                                        humidityData.MedianValues.Add(humidityValue);

                                        humidityData.MaxValue = Math.Max(humidityData.MaxValue, humidityValue);

                                        humidityData.MinValue = Math.Min(humidityData.MinValue, humidityValue);

                                        sw.WriteLine($"Day {i}: {humidityValue} (%)");
                                }
                            }
                                humidityData.AverageValue /= (humidityValues.Length - 1);
                                humidityData.MedianValues.Sort();

                        count = humidityData.MedianValues.Count;
                        if (count % 2 == 0)
                        {
                            // If the count is even, average the two middle values
                            int middle1 = count / 2 - 1;
                            int middle2 = count / 2;
                            windData.MedianValue = (humidityData.MedianValues[middle1] + humidityData.MedianValues[middle2]) / 2;
                        }
                        else
                        {
                            // If the count is odd, take the middle value
                            int middle = count / 2;
                            humidityData.MedianValue = humidityData.MedianValues[middle];
                        }

                        // The median value is stored in the MedianValue property of the WeatherPatameterData object
sw.WriteLine("Humidity Math.max:" + humidityData.MaxValue + "Humidity Math.min:" + humidityData.MinValue + "Humidity Average:" + humidityData.AverageValue + "Humidity Median value:" + humidityData.MedianValue);

                            line = sr.ReadLine(); 
                            string[] precipitationValues = line.Split(';');

                            for (int i = 1; i < precipitationValues.Length; i++)
                            {
                                if (int.TryParse(precipitationValues[i], out int precipitationValue) && precipitationValue == 0)
                                {              

                                        precipitationData.AverageValue += precipitationValue;

                                        precipitationData.MedianValues.Add(precipitationValue);

                                        precipitationData.MaxValue = Math.Max(humidityData.MaxValue, precipitationValue);

                                        precipitationData.MinValue = Math.Min(humidityData.MinValue, precipitationValue);

                                        sw.WriteLine($"Day {i}: {precipitationValue} (%)");
                                }
                            }
                                precipitationData.AverageValue /= (humidityValues.Length - 1);
                                precipitationData.MedianValues.Sort();

                count = precipitationData.MedianValues.Count;
                if (count % 2 == 0)
                {
                    // If the count is even, average the two middle values
                    int middle1 = count / 2 - 1;
                    int middle2 = count / 2;
                    precipitationData.MedianValue = (precipitationData.MedianValues[middle1] + precipitationData.MedianValues[middle2]) / 2;
                }
                else
                {
                    // If the count is odd, take the middle value
                    int middle = count / 2;
                    precipitationData.MedianValue = precipitationData.MedianValues[middle];
                }

sw.WriteLine("Precipitation Math.max:" + precipitationData.MaxValue + "Precipitation Math.min:" + precipitationData.MinValue + "Precipitation Average:" + precipitationData.AverageValue + "Precipitation Median value:" + precipitationData.MedianValue);

                            line = sr.ReadLine(); 
                            string[] lightningValues = line.Split(';');

                            for (int i = 1; i < lightningValues.Length; i++)
                            {
                                if (lightningValues[i].Contains("No"))
                                {
                                sw.WriteLine($"Day {i}: {lightningValues[i]} lightnings");
                                }
                            }

                            line = sr.ReadLine();
                            string[] cloudsValues = line.Split(';');

                            for (int i = 1; i < cloudsValues.Length; i++)
                            {
                            if (!cloudsValues[i].Contains("Cumulus") && !cloudsValues[i].Contains("Nimbus"))
                            {
                                sw.WriteLine($"Day {i}: {cloudsValues[i]} clouds");
                            }
                            }
        //close the file
        sr.Close();

    }
    catch (FileNotFoundException)
    {
        Console.WriteLine("File not found!");
        Environment.Exit(0);
    }
}