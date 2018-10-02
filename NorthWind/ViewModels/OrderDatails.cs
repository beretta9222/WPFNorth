using System.Collections.Generic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using CsvHelper;
using System.IO;
using NorthWind.Model;
using Microsoft.Win32;
using System.Data.SqlClient;

namespace NorthWind.ViewModels
{
    public class OrderDatails : ViewModelBase
    {
        /// <summary>
        /// Данные получинные из бд
        /// </summary>
        private List<OrderDatail> list;
        public List<OrderDatail> List
        {
            get
            {
                using (northwind dc = new northwind())
                {
                    list = dc.OrderDatails.SqlQuery(@"SELECT c.CompanyName
                    , c.Country+','+c.City+','+c.Address+','+c.PostalCode as Address
                    , o.OrderDate, o.ShipCountry+','+o.ShipCity+','+o.ShipAddress as ShipAddress
                    , od.Quantity
                    , od.UnitPrice
                    , p.ProductName
                    , p.QuantityPerUnit 
                    FROM Customers c
                    join Orders o on c.CustomerID = o.CustomerID
                    JOIN [Order Details] od on o.OrderID = od.OrderID
                    JOIN Products p on od.ProductID = p.ProductID").ToListAsync().Result;

                }
                return list;
            }
        }

        private RelayCommand tocsv;
        public RelayCommand toCSV
        {
            get
            {
                return tocsv ?? (tocsv = new RelayCommand(() =>
                {
                    using (Model.northwind dc = new Model.northwind())
                    {
                        var rlist = dc.OrderDatails.SqlQuery(@"SELECT TOP(@top) c.CompanyName
                        , c.Country+','+c.City+','+c.Address+','+c.PostalCode as Address
                        , o.OrderDate, o.ShipCountry+','+o.ShipCity+','+o.ShipAddress as ShipAddress
                        , od.Quantity
                        , od.UnitPrice
                        , p.ProductName
                        , p.QuantityPerUnit 
                        FROM Customers c
                        join Orders o on c.CustomerID = o.CustomerID
                        JOIN [Order Details] od on o.OrderID = od.OrderID
                        JOIN Products p on od.ProductID = p.ProductID", new SqlParameter("@top", int.Parse(top))).ToListAsync().Result;
                        SaveFileDialog sd = new SaveFileDialog();
                        sd.ShowDialog();
                        using (var writer = new StreamWriter(sd.FileName+".csv"))
                        using (var csvWriter = new CsvWriter(writer))
                        {
                            csvWriter.Configuration.Delimiter = ",";
                            csvWriter.Configuration.HasHeaderRecord = true;
                            csvWriter.Configuration.AutoMap<ProductsDatail>();
                            csvWriter.WriteHeader<ProductsDatail>();
                            csvWriter.WriteRecords(rlist);
                            writer.Flush();                           
                        }
                    }
                }));
            }

        }
        /// <summary>
        ///"to" export to cst
        /// </summary>
        private string top;
        public string Top
        {
            get
            {
                return top;
            }
            set
            {
                top = value;
                RaisePropertyChanged(() => Top);
            }
        }
    }
}
