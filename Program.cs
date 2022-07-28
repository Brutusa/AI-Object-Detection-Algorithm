using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;
using System.IO;
using System.Threading;

namespace AussieAnimalAi3._1Prototype 
{

    public class Program
    {
        
        static string statPredictionResourceId = "/subscriptions/741953b4-c318-4579-a2ac-b3039a1550bd/resourceGroups/animal-identifier-ai/providers/Microsoft.CognitiveServices/accounts/AussieAnimalAI";
        static Guid iterationId = new Guid("f3d47761-46cf-48b2-9122-6b87b39d9cfe");
        static Guid projectId = new Guid("fc37adbc-98d6-46ca-8588-92b4ec53be47");
        
        static string modelName = "AussieAnimalAIModel";




        public static void Main(string[] args)
        {

            string trainingKey = "0aeb1e65b95e41a0928ded5d15d12958";
            string trainingEndpoint = "https://aussieanimalai.cognitiveservices.azure.com/";
            string predictionKey = "7e517bc63e334e9a8ee75ae466d95606";
            string predictionEndpoint = "https://aussieanimalai-prediction.cognitiveservices.azure.com/";

            CustomVisionTrainingClient trainingApi = AuthenticateTraining(trainingEndpoint, trainingKey, predictionKey);
            CustomVisionPredictionClient predictionApi = AuthenticatePrediction(predictionEndpoint, predictionKey);

            Project project = GetProject(trainingApi);

            //CreateHostBuilder(args).Build().Run();
            TestIteration(predictionApi, project);



  

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static CustomVisionTrainingClient AuthenticateTraining(string endpoint, string trainingKey, string predictionKey)
        {
            // Create the Api, passing in the training key
            CustomVisionTrainingClient trainingApi = new CustomVisionTrainingClient(new Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.ApiKeyServiceClientCredentials(trainingKey))
            {
                Endpoint = endpoint
            };
            return trainingApi;
        }

        private static CustomVisionPredictionClient AuthenticatePrediction(string endpoint, string predictionKey)
        {
            // Create a prediction endpoint, passing in the obtained prediction key
            CustomVisionPredictionClient predictionApi = new CustomVisionPredictionClient(new Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.ApiKeyServiceClientCredentials(predictionKey))
            {
                Endpoint = endpoint
            };
            return predictionApi;
        }



        private static Project GetProject(CustomVisionTrainingClient trainingApi)
        {
            // Find the object detection domain
            var domains = trainingApi.GetDomains();
            var objDetectionDomain = domains.FirstOrDefault(d => d.Type == "ObjectDetection");

            // Re-query the iteration to get its updated status
            var iteration = trainingApi.GetIteration(projectId, iterationId);

            
            Console.WriteLine("Getting project:");
            Console.WriteLine(trainingApi.GetProject(projectId).Name);
            Console.WriteLine(trainingApi.GetProject(projectId).Description);
            return trainingApi.GetProject(projectId);


        }

        private static void TestIteration(CustomVisionPredictionClient predictionApi, Project project)
        {

            // Make a prediction against the new project
            Console.WriteLine("Making a prediction:");


           
        var imageFile = "/Users/boost/Projects/AussieAnimalAi3.1Prototype/AussieAnimalAi3.1Prototype/test.jpg";
       

            using (var stream = File.OpenRead(imageFile))
            {
        
                var result = predictionApi.DetectImage(projectId, modelName, stream);
               

                // Loop over each prediction and write out the results
                foreach (var c in result.Predictions)
                {
                  
                    Console.WriteLine($"\t Found {c.TagName}! I am {c.Probability:P1} sure!");
                   

                }
            }
            Console.Read();
        }

    }


}