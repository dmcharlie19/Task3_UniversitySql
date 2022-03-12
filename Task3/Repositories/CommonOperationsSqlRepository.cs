using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace UniversitySql.Repositories
{
    public class CommonOperationsSqlRepository<T> : ICommonOperationsSqlRepository<T> where T : new()
    {
        protected readonly string _connectionString;

        public CommonOperationsSqlRepository( string connectionString )
        {
            _connectionString = connectionString;
        }

        public List<T> GetAll()
        {
            var result = new List<T>();

            using ( var connection = new SqlConnection( _connectionString ) )
            {
                connection.Open();

                using ( SqlCommand Command = connection.CreateCommand() )
                {
                    Command.CommandText = $@"select * from {typeof( T ).Name}";
                    using ( SqlDataReader reader = Command.ExecuteReader() )
                    {
                        while ( reader.Read() )
                        {
                            var properties = typeof( T ).GetProperties();
                            T obj = new T();
                            foreach ( var prop in properties )
                            {
                                prop.SetValue( obj, reader[ prop.Name ] );
                            }
                            result.Add( obj );
                        }
                    }

                }
            }
            return result;
        }

        public T GetById( int id )
        {
            T result = default( T );

            using ( var connection = new SqlConnection( _connectionString ) )
            {
                connection.Open();

                using ( SqlCommand сommand = connection.CreateCommand() )
                {
                    сommand.CommandText =
                        $@"select * from {typeof( T ).Name}
                            where [id] = @id";

                    сommand.Parameters.Add( "@id", SqlDbType.Int ).Value = id;

                    using ( SqlDataReader reader = сommand.ExecuteReader() )
                    {
                        if ( reader.Read() )
                        {
                            result = new T();
                            var properties = typeof( T ).GetProperties();
                            foreach ( var prop in properties )
                            {
                                prop.SetValue( result, reader[ prop.Name ] );
                            }
                        }
                    }
                }
            }
            return result;
        }

        public void Add( T obj )
        {
            using ( var connection = new SqlConnection( _connectionString ) )
            {
                connection.Open();
                using ( SqlCommand сommand = connection.CreateCommand() )
                {
                    var properties = typeof( T ).GetProperties();

                    string values = "";
                    for ( int i = 0; i < properties.Length; i++ )
                    {
                        if ( properties[ i ].Name == "Id" )
                            continue;

                        values += $@"@{properties[ i ].Name}";
                        if ( i + 1 != properties.Length )
                            values += ", ";
                    }

                    сommand.CommandText =
                        $@"insert into [{typeof( T ).Name}]
                        values
                            ({values})";

                    foreach ( var prop in properties )
                    {
                        if ( prop.Name == "Id" )
                            continue;

                        сommand.Parameters.AddWithValue( prop.Name, prop.GetValue( obj ) );
                    }

                    properties[ 0 ].SetValue( obj, Convert.ToInt32( сommand.ExecuteScalar() ) );
                }
            }
        }

        public void DeleteById( int id )
        {
            using ( var connection = new SqlConnection( _connectionString ) )
            {
                connection.Open();
                using ( SqlCommand command = connection.CreateCommand() )
                {
                    command.CommandText =
                         $@"delete from [{typeof( T ).Name}]
                            where [Id] = @id";

                    command.Parameters.Add( "@id", SqlDbType.Int ).Value = id;

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
