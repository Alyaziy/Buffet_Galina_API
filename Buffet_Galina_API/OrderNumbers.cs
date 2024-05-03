public class OrderNumbers
{
    int lastNumber;
    DateTime last;
    string filename = "OrderNumbers.bin";

    public OrderNumbers()
    {
        if (File.Exists(filename))
            lastNumber = BitConverter.ToInt32(File.ReadAllBytes(filename));
        else
            lastNumber = 0;
        last = DateTime.Now;
    }

    internal string GetNextNumber()
    {
        if (DateTime.Now.DayOfWeek != last.DayOfWeek)
            lastNumber = 0;
        last = DateTime.Now;
        ++lastNumber;
        File.WriteAllBytes(filename, BitConverter.GetBytes(lastNumber));
        return $"{lastNumber}";
    }
}