using Newtonsoft.Json;

namespace GlobalModels;

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
}