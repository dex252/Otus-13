using ReflectionSample.Tests.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ReflectionSample.Tests.Tests
{
    public class TimeTest
    {
        const int ITERATIONS = 100000;
        public static object[][] TimeTestData =>
          new[]
        {
                new object[] { new F(){i1 = 1, i2 = 2, i3 = 3, i4 = 4, i5 = 5 }, "{\"i1\":1,\"i2\":2,\"i3\":3,\"i4\":4,\"i5\":5}" }

        };

        /// <summary>
        /// С выводом в консоль
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="equal"></param>
        [Theory]
        [MemberData(nameof(TimeTestData))]
        public void SerializeTimeTest(F obj, string equal)
        {
            var times = new double[ITERATIONS];
            var stopwatch = new Stopwatch();

            var serialized = string.Empty;
            for (var index = 0; index < ITERATIONS; index++)
            {
                stopwatch.Start();
                serialized = Reflection.Serialize(obj);
                stopwatch.Stop();

                var elapsedTime = stopwatch.Elapsed.TotalMilliseconds;
                times[index] = elapsedTime;
                Debug.WriteLine(elapsedTime);
                stopwatch.Reset();
            }


            var average = times.Average();
            var sum = times.Sum();

            Debug.WriteLine($"Среднее время выполнения одного вызова: {average}");
            Debug.WriteLine($"Общее время выполнения для всех итераций: {sum}");
            Debug.WriteLine($"Полученная строка: {serialized}");

            stopwatch.Reset();

            var serializedNewtonsoft = string.Empty;
            for (var index = 0; index < ITERATIONS; index++)
            {
                stopwatch.Start();
                serializedNewtonsoft = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                stopwatch.Stop();

                var elapsedTime = stopwatch.Elapsed.TotalMilliseconds;
                times[index] = elapsedTime;
                Debug.WriteLine(elapsedTime);
                stopwatch.Reset();
            }

            var averageNewtonsoft = times.Average();
            var sumNewtonsoft = times.Sum();

            Debug.WriteLine($"Среднее время выполнения одного вызова: {averageNewtonsoft}");
            Debug.WriteLine($"Общее время выполнения для всех итераций: {sumNewtonsoft}");
            Debug.WriteLine($"Полученная строка: {serializedNewtonsoft}");

            Assert.Equal(serialized, serializedNewtonsoft);
        }

        /// <summary>
        /// Без вывода в консоль
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="equal"></param>
        [Theory]
        [MemberData(nameof(TimeTestData))]
        public void SerializeTimeTestWithoutConsole(F obj, string equal)
        {
            var times = new double[ITERATIONS];
            var stopwatch = new Stopwatch();

            var serialized = string.Empty;

            for (var index = 0; index < ITERATIONS; index++)
            {
                stopwatch.Start();
                serialized = Reflection.Serialize(obj);
                stopwatch.Stop();

                var elapsedTime = stopwatch.Elapsed.TotalMilliseconds;
                times[index] = elapsedTime;
                stopwatch.Reset();
            }

            var average = times.Average();
            var sum = times.Sum();

            Debug.WriteLine($"Среднее время выполнения одного вызова: {average}");
            Debug.WriteLine($"Общее время выполнения для всех итераций: {sum}");
            Debug.WriteLine($"Полученная строка: {serialized}");

            stopwatch.Reset();

            var serializedNewtonsoft = string.Empty;
            for (var index = 0; index < ITERATIONS; index++)
            {
                stopwatch.Start();
                serializedNewtonsoft = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                stopwatch.Stop();

                var elapsedTime = stopwatch.Elapsed.TotalMilliseconds;
                times[index] = elapsedTime;
                stopwatch.Reset();
            }

            var averageNewtonsoft = times.Average();
            var sumNewtonsoft = times.Sum();

            Debug.WriteLine($"Среднее время выполнения одного вызова: {averageNewtonsoft}");
            Debug.WriteLine($"Общее время выполнения для всех итераций: {sumNewtonsoft}");
            Debug.WriteLine($"Полученная строка: {serializedNewtonsoft}");

            Assert.Equal(serialized, serializedNewtonsoft);
        }

        /// <summary>
        /// Без вывода в консоль
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="equal"></param>
        [Theory]
        [MemberData(nameof(TimeTestData))]
        public void DeserializeTimeTestWithoutConsole(F obj, string equal)
        {
            var times = new double[ITERATIONS];
            var stopwatch = new Stopwatch();

            F result = null;

            for (var index = 0; index < ITERATIONS; index++)
            {
                stopwatch.Start();
                result = Reflection.Deserialize<F>(equal);
                stopwatch.Stop();

                var elapsedTime = stopwatch.Elapsed.TotalMilliseconds;
                times[index] = elapsedTime;
                stopwatch.Reset();
            }

            var average = times.Average();
            var sum = times.Sum();

            Debug.WriteLine($"Среднее время выполнения одного вызова: {average}");
            Debug.WriteLine($"Общее время выполнения для всех итераций: {sum}");
            Debug.WriteLine($"Полученная строка: {equal}");

            stopwatch.Reset();

            F resultNewtonsoft = null;
            for (var index = 0; index < ITERATIONS; index++)
            {
                stopwatch.Start();
                resultNewtonsoft = Newtonsoft.Json.JsonConvert.DeserializeObject<F>(equal);
                stopwatch.Stop();

                var elapsedTime = stopwatch.Elapsed.TotalMilliseconds;
                times[index] = elapsedTime;
                stopwatch.Reset();
            }

            var averageNewtonsoft = times.Average();
            var sumNewtonsoft = times.Sum();

            Debug.WriteLine($"Среднее время выполнения одного вызова: {averageNewtonsoft}");
            Debug.WriteLine($"Общее время выполнения для всех итераций: {sumNewtonsoft}");
            Debug.WriteLine($"Полученная строка: {equal}");

            Assert.Equivalent(result, resultNewtonsoft);
        }
    }
}
