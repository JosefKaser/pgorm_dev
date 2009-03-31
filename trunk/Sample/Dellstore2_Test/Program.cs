/*-------------------------------------------------------------------------
 * Program.cs
 *
 * This file is part of the PGORM project.
 * http://pgorm.googlecode.com/
 *
 * Copyright (c) 2002-2009, TrueSoftware B.V.
 *
 * IDENTIFICATION
 * 
 *  $Id$
 * 	$HeadURL$
 * 	
 *-------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Dellstore2.Data;
using Dellstore2.Objects;

namespace Dellstore2_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initialize the database
            DataAccess.InitializeDatabase("localhost",
                "dellstore2", "postgres", "postgres");

            // just to test whether all the data can be loaded with
            // the generated assemblies. 
            DeepLoader.Load();

            // get all records from this table
            customersRecordSet recordSet1 = customersFactory.GetAll();

            // get some records
            customersRecordSet recordSet2 = customersFactory.GetBy_customerid(10);

            // get more records
            customersRecordSet recordSet3 = customersFactory.GetBy_username("user21");

            // how to update an existing object
            customersObject customer1 = recordSet1[0];
            customer1.firstname = "John";

            // update the customer by customerid
            customersFactory.UpdateBy_customerid(customer1, customer1.customerid.Value);

            // call an stored procedure
            StoredProcedures.new_customer(
                "Eva",
                "Adams",
                "Main Street 1",
                "",
                "Glendale",
                "CA",
                0,
                "USA",
                0,
                "eva.adams@example.com",
                "818-1234567890",
                1,
                "1234567890",
                "01-01-2010",
                "eva_adams",
                "password",
                30,
                2500,
                "F");
        }
    }
}