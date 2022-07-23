namespace BioReaderTests
{
    public class Tests
    {
        [Test]
        public void Read_half_chars_from_words_fourOrMore_letters()
        {
            string data = "Hello world everyone result";
            string outPut = "Hel wor ever res";
            Br.Reader reader = new Br.Reader();
            reader.Data = data;
            Assert.That(outPut, Is.EqualTo(string.Join(" ", reader.GetHalfChars())));
        }

        [Test]
        public void Read_half_chars_from_words_three_letters()
        {
            string data = "new set ov";
            string outPut = "n s o";
            Br.Reader reader = new Br.Reader();
            reader.Data = data;
            Assert.That(outPut, Is.EqualTo(string.Join(" ", reader.GetHalfChars())));
        }


        [Test]
        public void Read_half_chars_from_words_uppercase()
        {
            string data = "NEWFire SET Ov";
            string outPut = "NEWF S O";
            Br.Reader reader = new Br.Reader();
            reader.Data = data;
            Assert.That(outPut, Is.EqualTo(string.Join(" ", reader.GetHalfChars())));
        }

        [Test]
        public void Read_half_chars_from_words_two_letters()
        {
            string data = "ne st ov";
            string outPut = "n s o";
            Br.Reader reader = new Br.Reader();
            reader.Data = data;
            Assert.That(outPut, Is.EqualTo(string.Join(" ", reader.GetHalfChars())));
        }


        [Test]
        public void Read_half_chars_from_words_one_letters()
        {
            string data = "n s o";
            string outPut = "n s o";
            Br.Reader reader = new Br.Reader();
            reader.Data = data;
            Assert.That(outPut, Is.EqualTo(string.Join(" ", reader.GetHalfChars())));
        }

        [Test]
        public void Read_half_chars_from_words_doubleSpace()
        {
            string data = "new  state  of";
            string outPut = "n  sta  o";
            Br.Reader reader = new Br.Reader();
            reader.Data = data;
            Assert.That(outPut, Is.EqualTo(string.Join(" ", reader.GetHalfChars())));
        }


        [Test]
        public void Read_half_chars_from_words_empty()
        {
            string data = string.Empty;
            string outPut = string.Empty;
            Br.Reader reader = new Br.Reader();
            reader.Data = data;
            Assert.That(outPut, Is.EqualTo(string.Join(" ", reader.GetHalfChars())));
        }
    }
}