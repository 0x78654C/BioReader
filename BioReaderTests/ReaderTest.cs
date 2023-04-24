
namespace BioReaderTests
{
    public class Tests
    {
        [Test]
        public void Read_half_chars_from_words_single_letter()
        {
            string data = "Hello";
            string outPut = "Hel";
            Assert.That(outPut, Is.EqualTo(string.Join(" ", reader.GetHalfChars(data).Values)));
        }


        [Test]
        public void Read_half_chars_from_words_empty()
        {
            string data = string.Empty;
            string outPut = string.Empty;
            Assert.That(outPut, Is.EqualTo(string.Join(" ", reader.GetHalfChars(data).Values)));
        }
    }
}