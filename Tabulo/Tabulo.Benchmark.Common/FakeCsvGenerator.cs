using System.Globalization;

namespace Tabulo.Benchmark;
public class FakeCsvGenerator
{
    public string GenerateTempFile()
    {
        var tempFile = Path.Combine(Path.GetTempPath(), $"products_{Guid.NewGuid()}.csv");
        var random = new Random();

        using var writer = new StreamWriter(tempFile);
        writer.WriteLine("ProductId,ProductName,ProductPrice,InStockFlag,CreatedAt");

        for (int i = 1; i <= 5000; i++)
        {
            var id = i;
            var name = $"Product {i}";
            var price = (decimal)(random.NextDouble() * 1000);
            var inStock = random.Next(0, 2) == 1 ? "true" : "false";
            var date = DateTime.Now.AddDays(-random.Next(0, 365)).ToString("yyyy-MM-dd HH:mm:ss");

            writer.WriteLine($"{id},{name},{price.ToString(CultureInfo.InvariantCulture)},{inStock},{date}");
        }

        return tempFile;
    }

    public void DeleteTempFile(string filePath)
    {
        if (File.Exists(filePath))
            File.Delete(filePath);
    }
}