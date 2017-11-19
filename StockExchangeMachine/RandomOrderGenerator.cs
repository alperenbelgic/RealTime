using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Security.Cryptography;
using System.Text;

namespace StockExchangeMachine
{
    public class RandomOrderGenerator
    {
        public RandomOrderGenerator(StockProduct product)
        {
            this.Product = product;
        }

        CryptoRandom CryptoRandom = new CryptoRandom();

        public void GenerateOrder()
        {
            decimal price = this.Product.Price;

            int intervalPercentage = 1;

            decimal nextOrderPrice = price * (100 + (CryptoRandom.NextDecimal() * intervalPercentage * 2 - intervalPercentage)) / 100;

            bool isNextOrderBid =
                CryptoRandom.NextBool();

            int lotCount = CryptoRandom.Next(5, 25) * 100;

            decimal customerNameSuffix = CryptoRandom.Next(1, 1000);

            if (isNextOrderBid)
            {
                Product.Bid(
                  (lotCount),
                    nextOrderPrice,
                    "customer" + customerNameSuffix.ToString());
            }
            else
            {
                Product.Offer(
                                (lotCount),
                                  nextOrderPrice,
                                  "customer" + customerNameSuffix.ToString());
            }
        }

        public IDisposable StartGeneratingOrders()
        {
            return 
            RandomOrderInterval
                .GetObservable()
                .Subscribe(a =>
                {
                    GenerateOrder();
                });
            ;
        }

        StockProduct Product { get; }
    }

    class CryptoRandom : RandomNumberGenerator
    {
        private static RandomNumberGenerator r;
        ///<summary>
        /// Creates an instance of the default implementation of a cryptographic random number generator that can be used to generate random data.
        ///</summary>
        public CryptoRandom()
        {
            r = RandomNumberGenerator.Create();
        }
        ///<summary>
        /// Fills the elements of a specified array of bytes with random numbers.
        ///</summary>
        ///<param name=”buffer”>An array of bytes to contain random numbers.</param>
        public override void GetBytes(byte[] buffer)
        {
            r.GetBytes(buffer);
        }

        public decimal NextDecimal()
        {
            return Convert.ToDecimal(NextDouble());
        }

        ///<summary>
        /// Returns a random number between 0.0 and 1.0.
        ///</summary>
        public double NextDouble()
        {
            byte[] b = new byte[4];
            r.GetBytes(b);
            return (double)BitConverter.ToUInt32(b, 0) / UInt32.MaxValue;
        }
        ///<summary>
        /// Returns a random number within the specified range.
        ///</summary>
        ///<param name=”minValue”>The inclusive lower bound of the random number returned.</param>
        ///<param name=”maxValue”>The exclusive upper bound of the random number returned. maxValue must be greater than or equal to minValue.</param>
        public int Next(int minValue, int maxValue)
        {
            return (int)Math.Round(NextDouble() * (maxValue - minValue - 1)) + minValue;
        }

        public bool NextBool()
        {
            return NextDouble() > 0.5;
        }

        ///<summary>
        /// Returns a nonnegative random number.
        ///</summary>
        public int Next()
        {
            return Next(0, Int32.MaxValue);
        }
        ///<summary>
        /// Returns a nonnegative random number less than the specified maximum
        ///</summary>
        ///<param name=”maxValue”>The inclusive upper bound of the random number returned. maxValue must be greater than or equal 0</param>
        public int Next(int maxValue)
        {
            return Next(0, maxValue);
        }
    }

    public class RandomOrderInterval
    {
        static CryptoRandom random = new CryptoRandom();

        public static IObservable<Unit> GetObservable()
        {

            Func<Unit, TimeSpan> nextTriggerTimerCalculator =
                (Unit u) =>
                {
                    var nextMilliseconds = random.Next(100, 300);
                    return TimeSpan.FromMilliseconds(nextMilliseconds);
                };

            var unit = new Unit();

            var observable =
            Observable.Generate<Unit, Unit>(
            unit,
            (Unit u) => { return true; },
            (previous) => { return unit; },
            (state) => { return unit; },
             nextTriggerTimerCalculator)
             ;

            return observable;

        }




    }
}
