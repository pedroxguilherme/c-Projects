
using System;
using System.Data;
using System.DirectoryServices.Protocols;
using System.Linq.Expressions;
using Oracle.ManagedDataAccess.Client;


namespace Connect
{
    class Program
    {


        static void Main(string[] args)
        {
            bool menu = true;
            do
            {
                Console.WriteLine("1- Inserir dados");
                Console.WriteLine("2- SELECT ALL DATA");
                Console.WriteLine("3- Update Name");
                Console.WriteLine("4- Delete Row");
                Console.WriteLine("5- Exit");
                var read = Console.ReadLine();
                String name;
                int idInsert;
                bool validNumber;
                bool looping = true;

                string connString = "DATA SOURCE=CPTMDES;PASSWORD=12trein34;PERSIST SECURITY INFO=True;USER ID=TREIN;TNS_ADMIN=C:\\Oracle\\TNS_ADMIN";
                try
                {
                    // Please replace the connection string attribute settings

                    OracleConnection conn = new OracleConnection(connString);
                    conn.Open();
                    var cmd = conn.CreateCommand();
                    switch (read)
                    {

                        case "1":
                            do
                            {
                                Console.WriteLine("Inserir nome.");
                                read = Console.ReadLine();
                                name = read;
                                Console.WriteLine("Inserir id.");
                                read = Console.ReadLine();

                                validNumber = int.TryParse(read, out idInsert);
                                cmd.CommandText = $"SELECT * FROM TESTE_PG  WHERE ID={idInsert}";
                                if (validNumber)
                                {
                                    using (OracleDataReader dr = cmd.ExecuteReader())
                                    {
                                        if (!dr.HasRows)
                                        {



                                            cmd.CommandText = "INSERT INTO TESTE_PG (ID, NAME_TESTE) VALUES (:TESTE_ID, :NAME)";

                                            cmd.CommandType = CommandType.Text;

                                            cmd.Parameters.Clear();
                                            cmd.Parameters.Add(":TESTE_ID", OracleDbType.Int32).Value = idInsert;
                                            cmd.Parameters.Add(":NAME", OracleDbType.Varchar2).Value = name;
                                            cmd.ExecuteNonQuery();

                                            Console.WriteLine("Worked");
                                            looping = false;
                                        }
                                        else
                                        {
                                            Console.WriteLine("The Id already exist");
                                            Console.WriteLine("Type again id doesn't exist");
                                        }

                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Number is invalid");
                                    Console.WriteLine("Type again.");
                                }
                            } while (looping == true);
                            break;
                        case "2":
                            cmd.CommandText = "SELECT * FROM TESTE_PG";
                            using (OracleDataReader dr = cmd.ExecuteReader())
                            {
                                if (dr.HasRows)
                                {
                                    while (dr.Read())
                                    {


                                        Console.WriteLine($"ID: {dr["ID"]} || NAME_TESTE: {dr["NAME_TESTE"]}");
                                    }


                                }
                            }
                            break;

                        case "3":
                            Console.WriteLine("Type the id you want to change:");
                            read = Console.ReadLine();
                            validNumber = int.TryParse(read, out idInsert);
                           
                            cmd.CommandText = $"SELECT * FROM TESTE_PG  WHERE ID={idInsert}";
                            if (validNumber)
                            {
                                using (OracleDataReader dr = cmd.ExecuteReader())
                                {
                                    if (dr.HasRows)
                                    {
                                        // ID EXISTS

                                        Console.WriteLine("Type the new name:");
                                        read = Console.ReadLine();
                                        name = read;
                                        cmd.CommandText = $"UPDATE teste_pg SET name_teste = :name WHERE ID=:id";
                                        cmd.CommandType = CommandType.Text;
                                        cmd.Parameters.Clear();
                                        cmd.Parameters.Add("name", OracleDbType.Varchar2).Value = name;
                                        cmd.Parameters.Add("id", OracleDbType.Int32).Value = idInsert;
                                        cmd.ExecuteNonQuery();
                                        Console.WriteLine("Line updated");


                                    }
                                    else
                                    {
                                        Console.WriteLine("Id invalid");

                                    }
                                }
                            }
                            else{
                                Console.WriteLine("type a valid number");
                            }
                            break;
                        case "4":
                            Console.WriteLine("Insert the id of the row you want to delete:");
                            read = Console.ReadLine();
                            validNumber = int.TryParse(read, out idInsert);

                            if(validNumber)
                            {
                            cmd.CommandText = $"DELETE FROM TESTE_PG WHERE ID = :id ";
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.Clear();
                            cmd.Parameters.Add("id", idInsert);
                            cmd.ExecuteNonQuery();
                            Console.WriteLine("Deleted.");
                            }
                            else{
                                Console.WriteLine("Invalid Number");
                            }
                            break;
                        case "5":
                            menu = false;
                            Console.WriteLine("Exiting..");

                            break;
                        default:
                            Console.WriteLine("Type again");
                            break;
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error : {0}", ex);
                }


            } while (menu == true);
        }
    }
}
