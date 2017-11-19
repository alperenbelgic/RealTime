//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace StockExchangeMachine.Tests
//{
//    public class Class1
//    {
//        public static void Main2(string[] args)
//        {
//            var c = new Class1();

//            c.a();


//            Thread.Sleep(10000000);
//        }


//        public async void a()
//        {


//            var stockProduct = new StockProduct() { StockProductCode = "google" };
//            stockProduct.Price = 100;

//            DateTime d = DateTime.Now;

//            RandomOrderInterval.GetObservable()
//                .Subscribe(
//                onNext: u =>
//                {
//                    var rog = new RandomOrderGenerator(stockProduct);
//                    rog.GenerateOrder();

//                    var ts = DateTime.Now - d;

//                    Console.WriteLine(ts);
//                    Console.WriteLine(stockProduct.Price);
//                    Console.WriteLine(" ");

//                }
//                );

//            await Task.Delay(10000000);

//        }

//    }
//}
