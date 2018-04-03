﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MySql.Data;
using MySql.Data.MySqlClient;
using ShipEngine.ApiClient.Api;
using ShipEngine.ApiClient.Client;
using ShipEngine.ApiClient.Model;

namespace IPD12_SuperExpress
{
    /// <summary>
    /// Interaction logic for CreateShipmentRequest.xaml
    /// </summary>
    public partial class CreateShipmentRequest : Window
    {
        List<Country> countryList = new List<Country>();
        List<Province> provinceList = new List<Province>();
        CostCalculator costCalculator;
        SuperExpressRate rate;

        public CreateShipmentRequest(CostCalculator cal, SuperExpressRate rate)
        {
            try
            {
                InitializeComponent();
                InitializeDataFromDatabase();
                costCalculator = cal;
                this.rate = rate;
                InitializeShipmentRequest();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine("Error opening database connection: " + ex.Message);
                Environment.Exit(1);

            }
        }

        private void InitializeDataFromDatabase()
        {
            try
            {
                countryList = Globals.db.GetAllCountry();
                //provinceList = Globals.db.GetAllProvice();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine("Error fetching country or provice data from database: " + ex.Message);
                Environment.Exit(1);
            }
        }

        private void InitializeShipmentRequest()
        {
            //List<String> countryNameList = (from country in countryList orderby country.Name select country.Name).ToList();
            lblServiceType.Content = rate.ServiceType;
            lblGuaranteedService.Content = rate.Guaranteed;
            lblEstimatedDate.Content = rate.EstimatedDeliveryDateTimeStr;
            lblWeight.Content = costCalculator.WeightStr;
            lblDimensions.Content = costCalculator.DimensionsStr;
            lblAmount.Content = rate.AmountStr + Globals.CURRENCY_CAD;

            cbCountryFrom.ItemsSource = countryList;//countryNameList;
            cbCountryFrom.Text = costCalculator.CountryFrom.Name;
            cbProvinceStateFrom.Text = costCalculator.ProvinceFrom.ProvinceStateName;
            tbCityFrom.Text = costCalculator.CityFrom;
            tbPostalCodeFrom.Text = costCalculator.PostalCodeFrom;

            cbCountryTo.ItemsSource = countryList;
            cbCountryTo.Text = costCalculator.CountryTo.Name;
            cbProvinceStateTo.Text = costCalculator.ProvinceTo.ProvinceStateName;
            tbCityTo.Text = costCalculator.CityTo;
            tbPostalCodeTo.Text = costCalculator.PostalCodeTo;


            /* for test start*/
            tbAddressFrom1.Text = "";
            tbAddressTo1.Text = "";
            /* for test end */

        }

        private void cbCountryFrom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string countryCode = ((Country)cbCountryFrom.SelectedItem).Code;
            List<Province> provinceInSelectedCountryList = Globals.db.GetAllProviceByCountryCode(countryCode);
            cbProvinceStateFrom.ItemsSource = provinceInSelectedCountryList;
            cbProvinceStateFrom.SelectedIndex = 0;
        }

        private void cbCountryTo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string countryCode = ((Country)cbCountryTo.SelectedItem).Code;
            List<Province> provinceInSelectedCountryList = Globals.db.GetAllProviceByCountryCode(countryCode);
            cbProvinceStateTo.ItemsSource = provinceInSelectedCountryList;
            cbProvinceStateTo.SelectedIndex = 0;
        }
    }
}