namespace NancyExample
{
    using Nancy;
    using Simple.Data;
    using System.Collections.Generic;
    
    public class IndexModule : NancyModule
    {
        public IndexModule()
        {
            Get["/"] = parameters =>
            {
                return "Home Directory";
            };

            Post["/gradeinfo"] = parameters =>
            {
                // Assign values from the form to variables
                string user = Request.Form.userName;
                string password = Request.Form.password;
                string grade = Request.Form.grade;

                // This is just a basic example so we will use a Dictionary
                Dictionary<string, string> dict = new Dictionary<string, string>();

                // Database connection string
                var db = Database.Opener.OpenConnection("Server=107.150.4.162;Port=3306;database=cordova_example;User ID=" + user + "; password=" + password + ";", "MySql.Data.MySqlClient");

                // Retreive all data from the 'users' table in the database where a user has a certain grade, this will retrieve a SimpleResultSet which we can iterate over
                 var users = db.users.FindAllBygrade(grade).OrderBy(db.users.employeelastname);

                // Loop through each record and add it to the Dictionary. We will convert them to strings as the dictionary can only output to JSON if it has string data types
                // Each record in the Dictionary should look like (employeeid is 1000, employeefirstname is John, employeelastname is Smith and grade is B): 1000:John Smith, B

                // Although for our example we will not utilise the employeeid/key value
                foreach (var loopy in users)
                {
                    dict.Add(loopy.employeeid.ToString(), loopy.employeefirstname.ToString() + " " + loopy.employeelastname.ToString() + ", " + loopy.grade.ToString());
                }

                return Response.AsJson(dict);
            };
        }
    }
}