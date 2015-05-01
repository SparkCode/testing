using System;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Kontur.Courses.Testing.Patterns.Specifications
{
	public class MarkdownProcessor
	{
		public string Render(string input)
		{
            var emReplacer = new Regex(@"([^\w\\]|^)_(.*?[^\\])_(\W|$)");
            var strongReplacer = new Regex(@"([^\w\\]|^)__(.*?[^\\])__(\W|$)");
            input = strongReplacer.Replace(input,
                match => match.Groups[1].Value +
                    "<strong>" + match.Groups[2].Value + "</strong>" +
                    match.Groups[3].Value);
            input = emReplacer.Replace(input,
                match => match.Groups[1].Value +
                    "<em>" + match.Groups[2].Value + "</em>" +
                    match.Groups[3].Value);
            input = input.Replace(@"\_", "_");
            return input;
		}
	}

	[TestFixture]
	public class MarkdownProcessor_should
	{
		private readonly MarkdownProcessor md = new MarkdownProcessor();

        [TestCase("pam", Result = "pam", TestName = "Not text change without underline")]
        [TestCase("_pam_", Result = "<em>pam</em>", TestName = "Mapping em tag")]
        [TestCase("__pam__", Result = "<strong>pam</strong>", TestName = "Mapping strong tag")]
        [TestCase("_aba __pam__ aba_", Result = "<em>aba <strong>pam</strong> aba</em>", TestName = "Mapping strong tag within em tag")]
        [TestCase("aba_aba_aba__a__aba_12_3", Result = "aba_aba_aba__a__aba_12_3", TestName = "Within letter or mumber underline no mapping")]
        [TestCase("__aba _aba", Result = "__aba _aba", TestName = "Not paired underline no mapping")]
        [TestCase(@"\_aba\_", Result = @"_aba_", TestName = "One paired underline with escape siquence no mapping")]
        [TestCase("___aba___,", Result = "<em><strong>___aba___,</em></strong>")]

	    public string Reorganizations_Tags(string input)
        {
            return md.Render(input);
        }
	}
}
