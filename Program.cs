using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


var currentPath = Environment.CurrentDirectory;

var paragraph_file = Path.Combine(currentPath, @"..\..\..\paragraph.htm");
var keywords_file = Path.Combine(currentPath, @"..\..\..\keywords.txt");

var wp = new MyWordProcessing.MyWordProcessor();

var paragraph = wp.ReadFile(paragraph_file) ? wp.Content : "";
var keywords = wp.RandomWordsN(5); //   wp.TopN(5);

if (keywords == null || paragraph == "")
{
	Console.WriteLine("error: paragraph or keyword not found.");
	return;
}

// -----------

Console.WriteLine("keywords:\n" + String.Join(", ", keywords.Keys) + "\n");

int i = 10;
Console.WriteLine($"Top {i}:");

var top = wp.TopN(i);
foreach (var word in top)
	Console.WriteLine($"{word.Key}={word.Value}");


// var keyword_array = keywords.Split(separator).Select(s => s.Trim()).ToArray();
var keyword_array = keywords.Keys.ToArray();
var replacedParagraph = ReplaceKeywords(paragraph, keyword_array);

Console.WriteLine("\nOriginal paragraph:\n" + paragraph);
Console.WriteLine("\nReplaced paragraph:\n" + replacedParagraph);
Console.WriteLine("\nNo HTML paragraph:\n" + wp.RemoveHtmlTag());
Console.WriteLine("\nDone !!!");
string ReplaceKeywords(string paragraph, string[] keywords)
{
	foreach (string keyword in keywords)
	{
		paragraph = paragraph.Replace(keyword, $"<strong>{keyword}</strong>");
	}
	return paragraph;
}




