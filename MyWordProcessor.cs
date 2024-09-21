using System.Text;
using System.Text.RegularExpressions;

namespace MyWordProcessing
{
	internal class MyWordProcessor : IDisposable
	{
		protected StringBuilder sb;
		public MyWordProcessor()
		{
			sb = new StringBuilder();
			Clear();
		}
		public void Dispose()
		{
			Clear();
			GC.SuppressFinalize(this);
		}
		public void Clear() => sb.Clear();
		public string Content { get => sb.ToString(); }
		public bool ReadFile(string filePath)
		{
			bool ret = false;
			string s = filePath.Trim();

			if (!string.IsNullOrEmpty(s))
				if (File.Exists(filePath))
				{
					Clear();
					sb.Append(File.ReadAllText(filePath));
					if (string.IsNullOrEmpty(Content.Trim()))
						Clear();
					else
						ret = true;
				}
			return ret;
		}

		public Dictionary<string, int> WordCount(string text)
		{
			if (String.IsNullOrEmpty(text.Trim())) return null;

			char[] separators = { ' ', '\n', '\r', '.', ',', ';', ':', '?', '!' };
			string[] words = text.Split(separators, StringSplitOptions.RemoveEmptyEntries);

			var dict = new Dictionary<string, int>();

			foreach (string word in words)
			{
				string lowercaseWord = word.ToLower();

				if (dict.ContainsKey(lowercaseWord))
					dict[lowercaseWord]++;
				else
					dict[lowercaseWord] = 1;
			}

			return dict;
		}
		public Dictionary<string, int> WordCount() => WordCount(Content);

		public Dictionary<string, int> TopN(string text, int n = 0)
		{
			var dict = WordCount(text);

			if (dict == null)
				return null;
			else if (n == 0)
				return dict.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
			else
				return dict.OrderByDescending(x => x.Value).Take(n).ToDictionary(x => x.Key, x => x.Value);
		}
		public Dictionary<string, int> TopN(int n = 0) => TopN(Content, n);

		public Dictionary<string, int> RandomWordsN(string text, int n = 0)
		{
			var dict = WordCount(text);

			if (dict == null) return null;

			if (n < 1) n = 1;
			if (n > dict.Count) n = 5;

			// Create a random number generator
			Random random = new Random();

			if (n == 0)
				return dict.OrderBy(x => random.Next()).ToDictionary(x => x.Key, x => x.Value);
			else
				return dict.OrderBy(x => random.Next()).Take(n).ToDictionary(x => x.Key, x => x.Value);
		}
		public Dictionary<string, int> RandomWordsN(int n = 0) => RandomWordsN(Content, n);

		public string RemoveHtmlTag(string text)
		{
			if (String.IsNullOrEmpty(text.Trim())) return "";

			// Regular expression to match HTML tags
			string html_tag_pattern = @"<[^>]*>";

			// Split the HTML string into tags and text
			string[] parts = Regex.Split(text, html_tag_pattern);

			StringBuilder s = new StringBuilder();
			int i = 0;

			foreach (string part in parts)
			{
				if (string.IsNullOrWhiteSpace(part))
					i++;
				else if (Regex.IsMatch(part, html_tag_pattern))
					i++;
				else
					s.Append(part + " ");
			}

			return s.ToString();
		}
		public string RemoveHtmlTag() => RemoveHtmlTag(Content);

	}


}