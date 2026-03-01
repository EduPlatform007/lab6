using System;
using System.Collections.Generic;
о
namespace Module06_HW
{
   

    interface IPaymentStrategy
    {
        void Pay(decimal amount);
    }

    class CardPayment : IPaymentStrategy
    {
        private string cardNumber;

        public CardPayment(string cardNumber)
        {
            this.cardNumber = cardNumber;
        }

        public void Pay(decimal amount)
        {
            Console.WriteLine($"Оплата {amount} ₸ банковской картой ({cardNumber})");
        }
    }

    class PayPalPayment : IPaymentStrategy
    {
        private string email;

        public PayPalPayment(string email)
        {
            this.email = email;
        }

        public void Pay(decimal amount)
        {
            Console.WriteLine($"Оплата {amount} ₸ через PayPal ({email})");
        }
    }

    class CryptoPayment : IPaymentStrategy
    {
        private string wallet;

        public CryptoPayment(string wallet)
        {
            this.wallet = wallet;
        }

        public void Pay(decimal amount)
        {
            Console.WriteLine($"Оплата {amount} ₸ криптовалютой (кошелек: {wallet})");
        }
    }

    class PaymentContext
    {
        private IPaymentStrategy strategy;

        public void SetStrategy(IPaymentStrategy strategy)
        {
            this.strategy = strategy;
        }

        public void DoPayment(decimal amount)
        {
            if (strategy == null)
            {
                Console.WriteLine("Стратегия таңдалмаған!");
                return;
            }

            strategy.Pay(amount);
        }
    }


    interface IObserver
    {
        void Update(string currency, decimal rate);
    }

    interface ISubject
    {
        void Attach(IObserver ob);
        void Detach(IObserver ob);
        void Notify(string currency, decimal rate);
    }

    class CurrencyExchange : ISubject
    {
        private List<IObserver> observers = new List<IObserver>();

        public void Attach(IObserver ob)
        {
            observers.Add(ob);
        }

        public void Detach(IObserver ob)
        {
            observers.Remove(ob);
        }

        public void Notify(string currency, decimal rate)
        {
            for (int i = 0; i < observers.Count; i++)
            {
                observers[i].Update(currency, rate);
            }
        }

        public void ChangeRate(string currency, decimal rate)
        {
            Console.WriteLine($"\nКурс {currency} изменен на {rate}");
            Notify(currency, rate);
        }
    }

    class BankObserver : IObserver
    {
        public void Update(string currency, decimal rate)
        {
            Console.WriteLine($"Банк получил обновление: {currency} = {rate}");
        }
    }

    class InvestorObserver : IObserver
    {
        public void Update(string currency, decimal rate)
        {
            Console.WriteLine($"Инвестор анализирует новый курс {currency}: {rate}");
        }
    }

    class MobileAppObserver : IObserver
    {
        public void Update(string currency, decimal rate)
        {
            Console.WriteLine($"Мобильное приложение уведомляет пользователя: {currency} = {rate}");
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
          
            Console.WriteLine("===== STRATEGY PATTERN =====");
            PaymentContext ctx = new PaymentContext();

            Console.WriteLine("Выберите способ оплаты:");
            Console.WriteLine("1 - Банковская карта");
            Console.WriteLine("2 - PayPal");
            Console.WriteLine("3 - Криптовалюта");

            string ch = Console.ReadLine();

            Console.Write("Введите сумму: ");
            decimal sum;
            if (!decimal.TryParse(Console.ReadLine(), out sum))
            {
                Console.WriteLine("Сумма қате енгізілді!");
                return;
            }

            if (ch == "1")
            {
                ctx.SetStrategy(new CardPayment("1234-5678-9012-3456"));
            }
            else if (ch == "2")
            {
                ctx.SetStrategy(new PayPalPayment("user@mail.com"));
            }
            else if (ch == "3")
            {
                ctx.SetStrategy(new CryptoPayment("0xABC123"));
            }
            else
            {
                Console.WriteLine("Неверный выбор!");
                return;
            }

            ctx.DoPayment(sum);

          
            Console.WriteLine("\n===== OBSERVER PATTERN =====");

            CurrencyExchange ex = new CurrencyExchange();

            IObserver bank = new BankObserver();
            IObserver investor = new InvestorObserver();
            IObserver app = new MobileAppObserver();

            ex.Attach(bank);
            ex.Attach(investor);
            ex.Attach(app);

            ex.ChangeRate("USD", 470);
            ex.ChangeRate("EUR", 510);


            ex.Detach(investor);

            ex.ChangeRate("USD", 480);

            Console.WriteLine("\nБітті. Enter бас.");
            Console.ReadLine();
        }
    }
}