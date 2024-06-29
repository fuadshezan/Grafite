using ChildHealthBot.Models;

using ChildHealthBot.Rsc;
using ChildHealthBot.Services;

using Newtonsoft.Json;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;

internal class Program
{
   private static BardServices BardServices;
   private static PromptModel SessionPrompt;
   

   private static bool IsNewConversation = true;

   private static void Main(string[] args)
   {
      Console.OutputEncoding = System.Text.Encoding.UTF8;

      Task t = MainAsync(args);
      t.Wait();
   }

   private static async Task MainAsync(string[] args)
   {
      await InitializeNewChat();
   }

   private static async Task InitializeNewChat()
   {
      Console.Clear();
      Console.WriteLine("           Starting New Conversation          \n");

      if (IsNewConversation)
      {
         SessionPrompt = new PromptModel();

         BardServices = new BardServices();

         IsNewConversation = false;

         await InitializeBot().ConfigureAwait(true);

         await StartChat();
      }
   }

   private static async Task InitializeBot()
   {
      bool IsBotReady = false;
      
        string p4 = @"I will give you english word as an input.You will return that word defination,Bengali meaning,different types parts of speech,Exmples,Synonyms. The response should maintain the given JSON format: 
                  {
                    'Word': '{word}', 
                    'Definition': 'The meaning of the word',
                    'Bengali': 'The Bengali meaning of the word',
                    'Examples': {
                      'Noun': 'Example sentence using the word as a noun, if applicable',
                      'Verb': 'Example sentence using the word as a verb, if applicable',
                      'Adjective': 'Example sentence using the word as an adjective, if applicable',
                      'Adverb': 'Example sentence using the word as an adverb, if applicable'
                    },
                    'Synonyms': ['List', 'of', 'synonyms','Max','Three'],
                    'Forms': {
                      'Noun': 'Meaning when used as a noun, if applicable',
                      'Verb': 'Meaning when used as a verb, if applicable',
                      'Adjective': 'Meaning when used as an adjective, if applicable',
                      'Adverb': 'Meaning when used as an adverb, if applicable'
                    }
                  }\n\n If you understand the instructions, please type 'start' to begin.";


	  try
      {
         //While Enter is Pressed Continue
         while (!IsBotReady)
         {
            var Query = p4;

            if (!string.IsNullOrEmpty(Query))
            {
                    SessionPrompt.Contents.Add(new PromptContent()
                    {
                        Role = "user",
                        Parts =
                        [
                            new() { Text = Query }
                        ],
        
                    });
            }

            Console.WriteLine("\n Thinking ...\n");

            var response = await BardServices.GetResponse(SessionPrompt).ConfigureAwait(true);

            if (!string.IsNullOrEmpty(response))
            {
               if (response != "Error" || response != "Exception")
               {
                  SessionPrompt.Contents.Add(new PromptContent()
                  {
                     Role = "model",
                     Parts =
                      [
                          new() { Text=response }
                      ],
                  });
               }

               //Convert SessionPrompt to json
               string json = JsonConvert.SerializeObject(SessionPrompt);

               //try parsing response
               try
               {
                  string extractedContent = ExtractBetweenMarkers(response, "```");
                  // Convert the string content to bytes using UTF-8 encoding
                  byte[] jsonBytes = Encoding.UTF8.GetBytes(extractedContent);

                  // Deserialize the bytes to the desired object
                  var formattedResponse = JsonConvert.DeserializeObject<JsonFormatAnsewer>(Encoding.UTF8.GetString(jsonBytes));
                  IsBotReady = true;
               }
               catch (Exception ex)
               {
                  Console.WriteLine(ex.Message);
               }
            }
         }//End
      }
      catch (Exception ex)
      {
         Console.WriteLine(ex.Message);
      }
   }

   private static async Task StartChat()
   {
      try
      {
         //While Enter is Pressed Continue
         while (!IsNewConversation)
         {
            Console.Write(@"your query:");
            string Query = Console.ReadLine() ?? "";

            if (Query == "new")
            {
               IsNewConversation = true;
               await InitializeNewChat().ConfigureAwait(true);
               continue;
            }

            if (!string.IsNullOrEmpty(Query))
            {
               SessionPrompt.Contents.Add(new PromptContent()
               {
                  Role = "user",
                  Parts =
                   [
                       new() { Text = Query }
                   ],
               });
            }

            Console.WriteLine("\n Thinking ...\n");

            var response = await BardServices.GetResponse(SessionPrompt).ConfigureAwait(true);

            if (!string.IsNullOrEmpty(response))
            {
               if (response != "Error" || response != "Exception")
               {
                  SessionPrompt.Contents.Add(new PromptContent()
                  {
                     Role = "model",
                     Parts =
                      [
                          new() { Text = response }
                      ],
                  });
               }
               //else { SessionPrompt.Contents.RemoveAt(-1); }

               Console.WriteLine("\n Model: " + response + "\n");
            }
         }//End
      }
      catch (Exception ex)
      {
         Console.WriteLine(ex.Message);
      }
   }

   public static string ReadFullFile(string path)
   {
      //get current path
      string C = System.IO.Directory.GetCurrentDirectory();

      string file = System.IO.File.ReadAllText(path);
      return file;
   }

   private static string ExtractBetweenMarkers(string input, string marker)
   {
      string pattern = $@"{marker}(.*?){marker}";

      Match match = Regex.Match(input, pattern, RegexOptions.Singleline);

      if (match.Success)
      {
         return match.Groups[1].Value;
      }

      return string.Empty;
   }





}