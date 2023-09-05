using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace CarDealership
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MyDealership();
        }

        static void MyDealership()
        {
            Console.WriteLine("Output");
            Console.WriteLine("==============================================================================");
            Dealership dealership1 = new Dealership("D1_22_T501", "The Six Cars", "1 Main Street, Toronto");
            // Console.WriteLine();
            Console.WriteLine(dealership1.ToString());
            // dealership1.ShowCars();

            Dealership dealership2 = new Dealership("D2_22_B321", "Car Street", "5th avenue, Brampton");
            Console.WriteLine(dealership2.ToString());
            // dealership2.ShowCars();


            Console.WriteLine("\nToyota Cars available in Dealership 1");
            dealership1.ShowCars("Toyota");


            Console.WriteLine("\nToyota Cars available in Dealership 2");
            dealership2.ShowCars("Toyota");
            Car favCar = new Car("Hyundai", 2020, "Elantra", 30000.00, CarType.Sedan);
            Console.WriteLine($"\nCar to match : {favCar.ToString()}");

            Console.WriteLine("\nMatching car(s) from Dealership 1 : ");
            dealership1.ShowCars(favCar);


            Console.WriteLine("\nMatching car(s) from Dealership 2 : ");
            dealership2.ShowCars(favCar);

            //favCar = new Car("Honda", 2018, "Civic", 20000.00, CarType.SUV, CarSpecifications.FogLights | CarSpecifications.TintendGlasses);
            favCar = new Car("Honda", 2018, "Civic", 20000.00, CarType.SUV);

            Console.WriteLine($"\nCar to match : {favCar.ToString()}");

            Console.WriteLine("\nMatching car(s) from Dealership 1 : ");
            dealership1.ShowCars(favCar);

            Console.WriteLine("\nMatching car(s) from Dealership 2 : ");
            dealership2.ShowCars(favCar);

            Console.WriteLine("\nList of similiar car models available in both dealership : ");

            foreach (Car firstCar in dealership1.CarList)
            {
                foreach (Car secondsCar in dealership2.CarList)
                {
                    if (firstCar == secondsCar)
                    {
                        Console.WriteLine($"Dealership 1 : {firstCar.ToString()}");
                        Console.WriteLine($"Dealership 2 : {secondsCar.ToString()}");
                    }
                }
            }
        }
    }

    public enum CarType
    {
        SUV,
        Hatchback,
        Sedan,
        Truck
    }

    class Car
    {
        public string Manufacturer { get; }
        public int Make { get; }
        public string Model { get; }
        private static int VI_NUMBER = 1021;
        private int VIN;
        public double BasePrice { get; }
        public CarType Type { get; }

        public Car(string manufacturer, int make, string model, double basePrice, CarType type)
        {
            // This constructor will assign received parameters to the respective class properties
            this.Manufacturer = manufacturer;
            this.Make = make;
            this.Model = model;
            this.BasePrice = basePrice;
            this.Type = type;
            this.VIN = VI_NUMBER;
            VI_NUMBER = VI_NUMBER + 100;
            VIN += 100;
        }

        public static bool operator ==(Car first, Car second)
        {
            bool result = false;
            if ((first.Manufacturer == second.Manufacturer) && (first.Model == second.Model) && (first.Type == second.Type))
            {
                result = true;

            }
            return result;
        }

        public static bool operator !=(Car first, Car second)
        {
            bool result = false;
            if ((first.Manufacturer != second.Manufacturer) | (first.Model != second.Model) | (first.Type != second.Type))
            {
                result = true;
            }
            return result;
        }

        public override string ToString()
        {
            return $"\n{VIN} : {this.Manufacturer}, {this.Make}, {this.Model}, {this.BasePrice}, {this.Type}";
        }
    }

    class Dealership
    {
        public string ID { get; }
        public string Name { get; }

        public string Address { get; }

        public List<Car> CarList { get; }

        private static string FILENAME = "Dealership_Cars.txt";
        public Dealership(string ID, string name, string address)
        {
            this.CarList = new List<Car>();
            this.ID = ID;
            this.Name = name;
            this.Address = address;
            try
            {

                using (StreamReader reader = new StreamReader(FILENAME))
                {
                    string recordLine;
                    Car tempCar;
                    // Console.WriteLine(this.ToString());
                    Console.WriteLine();
                    while ((recordLine = reader.ReadLine()) != null)
                    {
                        string[] values = recordLine.Split(',');

                        if (this.ID == values[0])
                        {
                            string manufacturer = values[1];
                            int make = Convert.ToInt32(values[2]);
                            string model = values[3];
                            double basePrice = Convert.ToDouble(values[4]);
                            string type = values[5];
                            CarType carType = (CarType)Enum.Parse(typeof(CarType), type);
                            tempCar = new Car(manufacturer, make, model, basePrice, carType);
                            this.CarList.Add(tempCar);
                        }

                    }
                    reader.Close();

                }
            }
            catch (FileNotFoundException fne)
            {
                Console.WriteLine($"File Not found : Please check the file name/path again.");
                Console.WriteLine(fne.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Something went wrong : {ex.Message}");
            }
        }

        public String ShowCars(List<Car> cars)
        {
            string result = "";
            foreach (Car car in cars)
            {
                result += car.ToString();
            }
            return result;
        }

        public void ShowCars(String manufacturer)
        {
            bool found = false;
            foreach (Car car in this.CarList)
            {

                if (car.Manufacturer.ToLower() == manufacturer.ToLower())
                {
                    // Console.WriteLine();
                    Console.WriteLine($"{car.ToString()}");
                    found = true;
                }
            }
            if (!found)
            {
                Console.WriteLine($"None");
            }
        }

        public void ShowCars(Car carToBeSearched)
        {
            bool found = false;
            foreach (Car car in this.CarList)
            {
                if (car == carToBeSearched)
                {
                    Console.WriteLine($"{car.ToString()}");
                    found = true;
                }
            }
            if (!found)
            {
                Console.WriteLine($"None");
            }
        }

        public override string ToString()
        {
            return $"{this.ID}, {this.Name}, {this.Address}\n{ShowCars(this.CarList)}";
        }
    }
}
