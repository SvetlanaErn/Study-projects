﻿using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.Common;

namespace LudoTest
{
    public class TestDatabase : IDisposable
    {
        private DbConnection _connection;

        private DbContextOptions<TestContext> CreateOptions()
        {
            return new DbContextOptionsBuilder<TestContext>()
                .UseSqlite(_connection).Options;
        }

        public TestContext CreateContext()
        {
            if (_connection != null) return new TestContext(CreateOptions());
            _connection = new SqliteConnection("DataSource=:memory:;Foreign Keys=False");
            _connection.Open();

            var options = CreateOptions();
            using var context = new TestContext(options);
            context.Database.EnsureCreated();

            return new TestContext(CreateOptions());
        }

        public void Dispose()
        {
            if (_connection == null) return;
            _connection.Dispose();
            _connection = null;
        }
    }
}