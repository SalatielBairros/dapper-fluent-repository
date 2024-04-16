using System;
using FluentMigrator;

namespace Dapper.Fluent.ORM.Migrations
{
    public abstract class OnlyUpMigration : Migration
    {
        public sealed override void Down() => throw new InvalidOperationException();
        public override abstract void Up();
    }
}
