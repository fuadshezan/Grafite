using Newtonsoft.Json;

namespace ChildHealthBot.Models;

public class PromptContent
{
    [JsonProperty("role")]
    public string Role { get; set; }

    [JsonProperty("parts")]
    public List<PromptContentPart> Parts { get; set; } = new();
	
	
}

public class PromptContentPart
{
    [JsonProperty("text")]
    public string Text { get; set; }
}

public class PromptModel
{
    [JsonProperty("contents")]
    public List<PromptContent> Contents { get; set; } = new();
	public List<item> safetySettings { get; } = new List<item>
		{
			new item
			{
				category = "HARM_CATEGORY_DANGEROUS_CONTENT",
				threshold = "BLOCK_ONLY_HIGH"
			},
			new item
			{
				category = "HARM_CATEGORY_HARASSMENT",
				threshold = "BLOCK_ONLY_HIGH"
			},
			new item
			{
				category = "HARM_CATEGORY_HATE_SPEECH",
				threshold = "BLOCK_ONLY_HIGH"
			},
			new item
			{
				category = "HARM_CATEGORY_SEXUALLY_EXPLICIT",
				threshold = "BLOCK_ONLY_HIGH"
			}
		};
}

public class item
{
	public string category { get; set; }
	public string threshold { get; set; }
}


