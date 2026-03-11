using System.Collections;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebSharp.Services
{
    public interface IFileService
    {
        ArrayData Load();
    }


    /// <summary>
    /// Контейнер для работы с массивом
    /// </summary>
    public class ArrayData
    {
        public object[] Data { get; set; }

        public ArrayData Last { get; set; }

        public ArrayData(IEnumerable objects, ArrayData info)
        {
            Data = objects.Cast<object>().ToArray();
            Last = info;
        }

        public void Dispose()
        {

        }
    }


    /// <summary>
    /// Service for work with json file
    /// </summary>
    public class FileService : IFileService
    {
        ReaderWriterLock locker = new ReaderWriterLock();
        public ArrayData Load()
        {
            ArrayData loadedData = null;
            try
            {
                locker.AcquireWriterLock(-1);
                var content = File.ReadAllBytesAsync("digits.json");
                loadedData = new ArrayData(JsonSerializer.Deserialize<int[]>(content.Result), default(ArrayData));
                locker.ReleaseWriterLock();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("FileService::Load", ex);
            }

            return loadedData;
        }

        public ArrayData ArrayCache;
        private async void UpdateCache()
        {
            var content = File.OpenRead("digits.json");
            ArrayCache = new ArrayData(JsonSerializer.DeserializeAsync<int[]>(content).Result, default(ArrayData));
        }

        public async Task<double> GetQuantille(double tau)
        {
            UpdateCache();
            return MathNet.Numerics.Statistics.Statistics.Quantile(ArrayCache.Data.Select(x => Convert.ToDouble(x)), tau);
        }
    }
}
