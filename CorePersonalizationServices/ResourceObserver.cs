namespace CorePersonalizationServices;

public static class ResourceObserver
{

	public static string ReadFullFile(string path)
	{
		//get current path
		string currentDirectory = System.IO.Directory.GetCurrentDirectory();

		string file = System.IO.File.ReadAllText(path);
		return file;
	}
}
