﻿using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPD12_SuperExpress
{
    public class Database
    {
        MySqlConnection conn;

        public Database()
        {
            string dbConnectString = "server=den1.mysql1.gear.host;database=superexpress;user id=superexpress;password=Lf4g-Wc!wjBg";
            conn = new MySqlConnection(dbConnectString);
            conn.Open();
        }


        public void Addcountry(Country country)
        {
            using (MySqlCommand insertCommand = new MySqlCommand("INSERT INTO countries (code, name) VALUES (@code, @name)", conn))
            {
                insertCommand.Parameters.AddWithValue("@code", country.Code);
                insertCommand.Parameters.AddWithValue("@name", country.Name);
                insertCommand.ExecuteNonQuery();
            }

        }

        public void AddProvince(Province province)
        {
            using (MySqlCommand insertCommand = new MySqlCommand("INSERT INTO provinces (countrycode, code, name) VALUES (@countryCode, @code, @name)", conn))
            {
                insertCommand.Parameters.AddWithValue("@countryCode", province.CountryCode);
                insertCommand.Parameters.AddWithValue("@code", province.ProvinceStateCode);
                insertCommand.Parameters.AddWithValue("@name", province.ProvinceStateName);
                insertCommand.ExecuteNonQuery();
            }
        }

        public List<Country> GetAllCountry()
        {
            List<Country> list = new List<Country>();
            MySqlCommand command = new MySqlCommand("SELECT * FROM countries", conn);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Country country = new Country() { Code = (string)reader["code"], Name = (string)reader["name"] };
                    list.Add(country);
                }
                return list;
            }
        }

        public List<Province> GetAllProvice()
        {
            List<Province> list = new List<Province>();
            MySqlCommand command = new MySqlCommand("SELECT * FROM provinces", conn);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Province province = new Province() { CountryCode = (string)reader["countryCode"], ProvinceStateCode = (string)reader["code"], ProvinceStateName = (string)reader["name"] };
                    list.Add(province);
                }
                return list;
            }
        }



        /*
        public List<Person> GetAllPeople()
        {
            List<Person> list = new List<Person>();
            SqlCommand command = new SqlCommand("SELECT * FROM people", conn);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Person person = new Person((int)reader[0], (string)reader[1], (int)reader[2], (double)reader[3]);
                    list.Add(person);
                }
                return list;
            }

        }

        public void GetAllPeople(List<Person> list)
        {
            List<Person> list2 = new List<Person>();
            list = list2;
            SqlCommand command = new SqlCommand("SELECT * FROM people", conn);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Person person = new Person((int)reader[0], (string)reader[1], (int)reader[2], (double)reader[3]);
                    list2.Add(person);
                }
                //return list;
            }

        }

        public void AddPerson(Person person)
        {
            using (SqlCommand insertCommand = new SqlCommand("INSERT INTO people (name, age, height) VALUES (@name, @age, @height)", conn))
            {
                insertCommand.Parameters.AddWithValue(@"name", person.Name);
                insertCommand.Parameters.AddWithValue(@"age", person.Age);
                insertCommand.Parameters.AddWithValue(@"height", person.Height);
                insertCommand.ExecuteNonQuery();
            }

        }

        public void UpdatePerson(Person person)
        {
            using (SqlCommand insertCommand = new SqlCommand("UPDATE people SET name = @name, age = @age, height = @height WHERE id = @id", conn))
            {
                insertCommand.Parameters.AddWithValue(@"name", person.Name);
                insertCommand.Parameters.AddWithValue(@"age", person.Age);
                insertCommand.Parameters.AddWithValue(@"height", person.Height);
                insertCommand.Parameters.AddWithValue(@"id", person.Id);
                insertCommand.ExecuteNonQuery();
            }

        }

        public void DeletePerson(int id)
        {
            using (SqlCommand insertCommand = new SqlCommand("DELETE FROM people WHERE id = @id", conn))
            {
                insertCommand.Parameters.AddWithValue(@"id", id);
                insertCommand.ExecuteNonQuery();
            }

        }
        */
    }
}