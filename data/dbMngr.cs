using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace data
{
    public class dbMngr
    {
        private string _ConnString;
        public dbMngr(string conn)
        {
            _ConnString = conn;
        }
        public List<Person> GetPeople()
        {
            SqlConnection conn = new(_ConnString);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"select * from People p";
            List<Person> people = new();
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                people.Add(new Person
                {
                    FirstName = (string)reader["FirstName"],
                    LastName = (string)reader["LastName"],
                    Id = (int)reader["Id"],
                    Balance = GetPersonTotalById((int)reader["Id"]),
                    Cell = (string)reader["Cell"],
                    Date = (DateTime)reader["date"],
                    AlwaysInclude = (bool)reader["alwaysInclude"]
                });
            }
            return people;
        }
        public List<History> GetPersonHistory(int id)
        {
            SqlConnection conn = new(_ConnString);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"select * from People p
                                join Deposit d 
                                on d.PersonId = p.Id
                                where p.Id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            List<History> h = new();
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                h.Add(new History
                {
                    Action = "Deposit",
                    Date = (DateTime)reader["date"],
                    Amount = (decimal)reader["Amount"]
                });
            }
            h.AddRange(GetPersonHistory2(id));
            return h;
        }
        public List<History> GetPersonHistory2(int id)
        {
            SqlConnection conn = new(_ConnString);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"select * from SimchaPeople
                                where PersonId = @id";
            cmd.Parameters.AddWithValue("@id", id);
            List<History> h = new();
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                h.Add(new History
                {
                    Action = GetSimchaById((int)reader["SimchaId"]).Name,
                    Date = (DateTime)reader["Date"],
                    Amount = -(decimal)reader["Amount"]
                });
            }
            return h;
        }
        public List<Simcha> GetSimchos()
        {
            SqlConnection conn = new(_ConnString);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from SimchaTable";
            List<Simcha> simchos = new();
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                simchos.Add(new Simcha
                {
                    Name = (string)reader["Name"],
                    Date = (DateTime)reader["Date"],
                    Id = (int)reader["Id"],
                    Total = GetSimchaTotalById((int)reader["Id"]),
                    NumOfContributers = NumOfContributers((int)reader["Id"]),
                });
            }
            return simchos;
        }
        public decimal GetSimchaTotalById(int id)
        {
            SqlConnection conn = new(_ConnString);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"select isnull(sum(sp.amount), 0) from SimchaTable s
                                left join SimchaPeople sp
                                on s.Id = sp.SimchaId
                                left join People p
                                on p.Id = sp.PersonId
                                where s.id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            return (decimal)cmd.ExecuteScalar();
        }
        public decimal GetPersonTotalById(int id)
        {
            SqlConnection conn = new(_ConnString);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"select isnull(sum(d.Amount), 0) from People p
                                join Deposit d 
                                on d.PersonId = p.Id
                                where p.Id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            decimal money = GetPersonTotalById2(id);
            return (decimal)cmd.ExecuteScalar() - money;
        }
        public decimal GetPersonTotalById2(int id)
        {
            SqlConnection conn = new(_ConnString);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"select isnull(sum(sp.amount), 0) from People p
                                join SimchaPeople sp
                                on sp.PersonId = p.Id
                                where p.Id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            return (decimal)cmd.ExecuteScalar();
        }
        public void AddPerson(Person p)
        {
            SqlConnection conn = new(_ConnString);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"insert into People
                                values(@fname, @lname, @cell, @date, @ai)select SCOPE_IDENTITY()";
            cmd.Parameters.AddWithValue("@fname", p.FirstName);
            cmd.Parameters.AddWithValue("@lname", p.LastName);
            cmd.Parameters.AddWithValue("@cell", p.Cell);
            cmd.Parameters.AddWithValue("@date", p.Date);
            cmd.Parameters.AddWithValue("@ai", p.AlwaysInclude);

            conn.Open();
            AddDeposit((int)(decimal)cmd.ExecuteScalar(), p.Balance, p.Date);
        }
        public void AddDeposit(int id, decimal amount, DateTime date)
        {
            SqlConnection conn = new(_ConnString);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"insert into Deposit
                                values(@amount, @date, @pId)";
            cmd.Parameters.AddWithValue("@amount", amount);
            cmd.Parameters.AddWithValue("@date", date);
            cmd.Parameters.AddWithValue("@pId", id);

            conn.Open();
            cmd.ExecuteNonQuery();
        }
        public Person GetPersonById(int id)
        {
            SqlConnection conn = new(_ConnString);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"select * from People where id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            Person people = new();
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                people.FirstName = (string)reader["FirstName"];
                people.LastName = (string)reader["LastName"];
                people.Id = (int)reader["Id"];
                people.Cell = (string)reader["Cell"];
                people.Date = (DateTime)reader["date"];
                people.AlwaysInclude = (bool)reader["alwaysInclude"];
                people.Balance = GetPersonTotalById((int)reader["Id"]);
            };

            return people;
        }
        public Simcha GetSimchaById(int id)
        {
            SqlConnection conn = new(_ConnString);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from SimchaTable where id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            Simcha s = new();
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                s.Name = (string)reader["Name"];
                s.Id = (int)reader["Id"];
                s.Date = (DateTime)reader["date"];
            };

            return s;
        }
        public void AddSimcha(Simcha s)
        {
            SqlConnection conn = new(_ConnString);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"insert into SimchaTable
                                values(@name, @date)";
            cmd.Parameters.AddWithValue("@name", s.Name);
            cmd.Parameters.AddWithValue("@date", s.Date);

            conn.Open();
            cmd.ExecuteNonQuery();
        }
        public void EditPerson(Person p)
        {
            SqlConnection conn = new(_ConnString);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"update People set FirstName = @fn, LastName = @ln, Cell = @cell, [Date] = @date, alwaysInclude = @incl
                                where id = @id";
            cmd.Parameters.AddWithValue("@fn", p.FirstName);
            cmd.Parameters.AddWithValue("@ln", p.LastName);
            cmd.Parameters.AddWithValue("@cell", p.Cell);
            cmd.Parameters.AddWithValue("@date", p.Date.ToString());
            cmd.Parameters.AddWithValue("@incl", p.AlwaysInclude);
            cmd.Parameters.AddWithValue("@id", p.Id);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void UpdateSimcha(int SimchaId, decimal amount, int PersonId)
        {
            SqlConnection conn = new(_ConnString);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"insert into SimchaPeople
                                values(@pId, @sId, @amount, @date)";
            cmd.Parameters.AddWithValue("@pId", PersonId);
            cmd.Parameters.AddWithValue("@sId", SimchaId);
            cmd.Parameters.AddWithValue("@amount", amount);
            cmd.Parameters.AddWithValue("@date", DateTime.Now);

            conn.Open();
            cmd.ExecuteNonQuery();
        }
        public int NumOfContributers(int id)
        {
            SqlConnection conn = new(_ConnString);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"select count(*) from SimchaPeople sp
                                join SimchaTable s
                                on s.Id = sp.SimchaId
                                where s.id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            return (int)cmd.ExecuteScalar();
        }
        public void DeleteSimchaContributers(int SimchaId)
        {
            SqlConnection conn = new(_ConnString);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"delete from SimchaPeople
                                where SimchaId = @id";
            cmd.Parameters.AddWithValue("@Id", SimchaId);

            conn.Open();
            cmd.ExecuteNonQuery();
        }
        public bool CheckIfContributed(int pId, int sId)
        {
            SqlConnection conn = new(_ConnString);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"select * from SimchaPeople
                                where PersonId = @pId and SimchaId = @sId";
            conn.Open();
            cmd.Parameters.AddWithValue("@pId", pId);
            cmd.Parameters.AddWithValue("@sId", sId);
            SqlDataReader reader = cmd.ExecuteReader();
            if(!reader.Read())
            {
                return false;
            }
            return true;
        }
    }

}
