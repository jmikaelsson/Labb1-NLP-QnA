using System;
using Azure;
using Azure.AI.Language.QuestionAnswering;
using Microsoft.Extensions.Configuration;


internal class Program
{
    private static string translatorEndpoint = "https://api.cognitive.microsofttranslator.com";
    private static string cogSvcKey;
    private static string cogSvcRegion;
    private static void Main(string[] args)
    {

        // This example requires environment variables named "LANGUAGE_KEY" and "LANGUAGE_ENDPOINT"
        string projectName = "Labb1";
        string deploymentName = "production";
        Uri endpoint = new Uri("https://josefinqna.cognitiveservices.azure.com/");
        AzureKeyCredential credential = new AzureKeyCredential("a9aaccbbdf794d7894be7c97c8509c79");
        

        //string question = "How can i learn more about microsoft certifications";
        QuestionAnsweringClient client = new QuestionAnsweringClient(endpoint, credential);
        QuestionAnsweringProject project = new QuestionAnsweringProject(projectName, deploymentName);


        string text1 = "I solemnly swear that I am up to no good";

        int windowWidth = Console.WindowWidth;
        int windowHeight = Console.WindowHeight;

        int centeredX1 = (windowWidth - text1.Length) / 2;
        int centeredY1 = windowHeight / 2;

        Console.Clear();
        Console.SetCursorPosition(centeredX1 > 0 ? centeredX1 : 0, centeredY1);
        Console.WriteLine(text1);

        Thread.Sleep(3000);

        Console.Clear();

        string text2 = "Messrs Moony, Wormtail, Padfoot, and Prongs\r\nPurveyors of Knowledge and Insight\r\nare proud to present\r\nTHE ULTIMATE Q&A GUIDE";

        string[] lines = text2.Split(new[] { "\r\n" }, StringSplitOptions.None);

        int maxLineLength = 0;
        foreach (string line in lines)
        {
            if (line.Length > maxLineLength)
            {
                maxLineLength = line.Length;
            }
        }

        int centeredX2 = (windowWidth - maxLineLength) / 2;
        int centeredY2 = (windowHeight - lines.Length) / 2;

        Console.Clear();

        // Calculate the starting Y position for vertical centering
        int startY = centeredY2;

        foreach (string line in lines)
        {
            Console.SetCursorPosition(centeredX2 > 0 ? centeredX2 : 0, startY);
            Console.WriteLine(line);
            startY++;
        }

        Thread.Sleep(3000);

        Console.Clear();
        Console.WriteLine("Ask me anyting!");
        Console.WriteLine("To end, just say Mischief Managed");
        Console.WriteLine("*---*---*---*---*");
        while (true)
        {
            Console.Write("Q: ");
            string question = Console.ReadLine();
            if (question.ToLower() == "mischief managed")
            {
                break;
            }
            try
            {
                Response<AnswersResult> response = client.GetAnswers(question, project);
                foreach (KnowledgeBaseAnswer answer in response.Value.Answers)
                {
                    Console.WriteLine($"A:{answer.Answer}");
                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");
            }
        }
    }
}