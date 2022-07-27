using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;

public class Database : MonoBehaviour
{
    public static Database _instance;

    string _playerDB = "URI = file:Player.db";

    SqliteConnection _connection;
    SqliteCommand _command;
    SqliteDataReader _reader;

    private void Awake()
    {
        _instance = this;

        _connection = new SqliteConnection(_playerDB);

        ExecuteSQLCommand("CREATE TABLE IF NOT EXISTS collectables (id INTEGER, quantity INTEGER NOT NULL,PRIMARY KEY(id AUTOINCREMENT))");
        ExecuteSQLCommand("CREATE TABLE IF NOT EXISTS inventory (id INTEGER, itemid INTEGER NOT NULL, quantity INTEGER NOT NULL, PRIMARY KEY(id AUTOINCREMENT))");
        ExecuteSQLCommand("CREATE TABLE IF NOT EXISTS playerinfo (id INTEGER, coins INTEGER NOT NULL, energy INTEGER NOT NULL, energyrestoretime INTEGER NOT NULL, PRIMARY KEY(id AUTOINCREMENT))");
    }

    public object ReadOneValueFromDB(string _table, string _column, int _id)
    {
        object _returnObject;
        _connection.Open();

        _command = _connection.CreateCommand();

        _command.CommandText = "SELECT " + _column + " FROM " +_table + " WHERE id = " + _id + ";";

        _reader = _command.ExecuteReader();

        _returnObject = _reader.GetValue(_reader.GetOrdinal(_column));

        _reader.Close();
        _connection.Close();


        return _returnObject;
    }

    public void SaveValueToDB<T>(string _table,string _column, int _id, T _value)
    {
        ExecuteSQLCommand("UPDATE " + _table + " SET " + _column + " = " + _value + " WHERE id = " + _id + ";");
        
    }

    public void ExecuteSQLCommand(string _commandToExecute)
    {
        _connection.Open();

        _command = _connection.CreateCommand();

        _command.CommandText = _commandToExecute;
        _command.ExecuteNonQuery();

        _connection.Close();
    }
}
