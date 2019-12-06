using Godot;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ProjectTD.scripts.data {
public class FileHandler {

	private static readonly StringEnumConverter Converter = new StringEnumConverter();
	public static string loadFile(string path) {
		var file = new File();
		file.Open(path, (int)File.ModeFlags.Read);
		var content = file.GetAsText();
		file.Close();
		return content;
	}

	public static T  loadJson<T>(string path) {
		return JsonConvert.DeserializeObject<T>(loadFile(path), Converter);
	}

	public static void writeFile(string path, string content) {
		var file = new File();
		file.Open(path, (int) File.ModeFlags.Write);
		file.StoreString(content);
		file.Close();
	}
	
	public static void writeJson<T>(string path, T content) {
		writeFile(path, JsonConvert.SerializeObject(content, Converter));
	}
}
}