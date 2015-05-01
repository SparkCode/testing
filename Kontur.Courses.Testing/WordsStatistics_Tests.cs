using System;
using System.Collections.Generic;
using System.Linq;
using Kontur.Courses.Testing.Implementations;
using NUnit.Framework;

namespace Kontur.Courses.Testing
{

	public class WordsStatistics_Tests
	{
		public Func<IWordsStatistics> createStat = () => new WordsStatistics_CorrectImplementation(); // меняется на разные реализации при запуске exe
		public IWordsStatistics stat;

		[SetUp]
		public void SetUp()
		{
			stat = createStat();
		}

		[Test]
		public void no_stats_if_no_words()
		{
			CollectionAssert.IsEmpty(stat.GetStatistics());
		}

		[Test]
		public void same_word_twice()
		{
			stat.AddWord("xxx");
			stat.AddWord("xxx");
			CollectionAssert.AreEqual(new[] { Tuple.Create(2, "xxx") }, stat.GetStatistics());
		}

		[Test]
		public void single_word()
		{
			stat.AddWord("hello");
			CollectionAssert.AreEqual(new[] { Tuple.Create(1, "hello") }, stat.GetStatistics());
		}

		[Test]
		public void two_same_words_one_other()
		{
			stat.AddWord("hello");
			stat.AddWord("world");
			stat.AddWord("world");
			CollectionAssert.AreEqual(new[] { Tuple.Create(2, "world"), Tuple.Create(1, "hello") }, stat.GetStatistics());
		}

	    [Test]
	    public void order_words_in_case_equal_frequency()
	    {
	        stat.AddWord("b");
            stat.AddWord("a");
            stat.AddWord("c");
            CollectionAssert.AreEqual(new[] {Tuple.Create(1, "a"), Tuple.Create(1, "b"), Tuple.Create(1, "c")}, stat.GetStatistics());
	    }

	    [Test]
	    public void word_in_various_register()
	    {
            stat.AddWord("Aba");
            stat.AddWord("aBA");
            stat.AddWord("aba");
            CollectionAssert.AreEqual(new[] {Tuple.Create(3, "aba")}, stat.GetStatistics());
	    }

        [Test]
	    public void word_in_case_length_more_10()
	    {
            var baseWord = new string(Enumerable.Repeat('a', 10).ToArray());
            stat.AddWord(baseWord + 'a');
            CollectionAssert.AreEqual(new[] {Tuple.Create(1, baseWord)}, stat.GetStatistics());
	    }

	    [Test]
        public void words_in_case_length_more_10_and_difference_in_11_position()
	    {
	        var baseWord = new string(Enumerable.Repeat('a', 10).ToArray());

            stat.AddWord(baseWord + 'a');
            stat.AddWord(baseWord + 'b');

            CollectionAssert.AreEqual(new[] { Tuple.Create(2, baseWord)}, stat.GetStatistics());
	    }

	    [Test]
        public void words_in_case_german_chars_in_various_register()
	    {
            stat.AddWord("Ä");
            stat.AddWord("ä");

            CollectionAssert.AreEqual(new[] {Tuple.Create(2, "ä")}, stat.GetStatistics());
	    }

	    [Test]
        public void two_another_IWordsStatistics()
        {
            stat.AddWord("aba");

            var stat2 = createStat();

            CollectionAssert.AreEqual(new[] { Tuple.Create(1, "aba") }, stat.GetStatistics());
        }

	    [Test]
	    public void null_instead_string()
	    {
	        stat.AddWord(null);

            Assert.AreEqual(0, stat.GetStatistics().Count());
	    }

        [Test]
        public void empty_string()
        {
            stat.AddWord("");

            Assert.AreEqual(0, stat.GetStatistics().Count());
        }
	}
}