using System.Collections.Generic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using CsvHelper;
using System.IO;
using NorthWind.Model;
using Microsoft.Win32;
using System.Data.SqlClient;
using System.Text;

namespace NorthWind.ViewModels
{
    public class ProductDatails : ViewModelBase
    {
        /// <summary>
        /// Данные получинные из бд
        /// </summary>
        private List<ProductsDatail> list;
        public List<ProductsDatail> List
        {
            get
            {
                using (northwind dc = new northwind())
                {
                    list = dc.ProductsDatails.SqlQuery(@"SELECT p.ProductName
                    , p.QuantityPerUnit
                    , c.CategoryName
                    , c.Description
                    , c.Picture
                    , s.CompanyName
                    , s.Country+','+ISNULL(s.Region,'')+isnull(s.Region,s.City)+','+s.Address+','+s.PostalCode as Address
                    , s.Phone
                      FROM [northwind].[dbo].[Products] p
                      join Categories c on c.CategoryID = p.CategoryID
                      join Suppliers s on p.SupplierID = s.SupplierID").ToListAsync().Result;

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
                    using (northwind dc = new northwind())
                    {
                        var rlist = dc.ProductsDatails.SqlQuery(@"SELECT TOP(@top) p.ProductName
                        , p.QuantityPerUnit
                        , c.CategoryName
                        , c.Description
                        , c.Picture
                        , s.CompanyName
                        , s.Country+','+ISNULL(s.Region,'')+isnull(s.Region,s.City)+','+s.Address+','+s.PostalCode as Address
                        , s.Phone
                          FROM [northwind].[dbo].[Products] p
                          join Categories c on c.CategoryID = p.CategoryID
                          join Suppliers s on p.SupplierID = s.SupplierID",new SqlParameter("@top", int.Parse(top))).ToListAsync().Result;
                        SaveFileDialog sd = new SaveFileDialog();
                        sd.ShowDialog();
                        using (var writer = new StreamWriter(sd.FileName + ".csv"))
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
