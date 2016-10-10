namespace DPA_Musicsheets.Utility
{
    public struct TimeSignature
    {
        public int A { get; set; }
        public int B { get; set; }

        public TimeSignature(int a, int b)
        {
            A = a;
            B = b;
        }

        public override string ToString()
        {
            return $"{A}/{B}";
        }
    }
}
