using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using FiretrekkersAPI.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace FiretrekkersAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlacesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public PlacesController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;

        }

        [HttpGet]
        public JsonResult get()
        {
            string query = @"
            select PlaceId, PlaceDate, PlaceName, PlaceArea, PlaceElevation,
            PlaceDescription, PlaceCoverImage, PlaceImages from dbo.PlacesVisited";
            DataTable table = new DataTable();
            //Define a Variable to Store Database Connection String
            string sqlDataSource = _configuration.GetConnectionString("FiretrekkersAppCon");
            SqlDataReader myReader;
            //Using Sql Connection and the SQl Command 
            //we will execute our query and fill the results into a DataTable
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); ;

                    myReader.Close();
                    myCon.Close();
                }
            }
            //Return DataTable as Json Result
            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(Places place)
        {
            string query = @"
            insert into dbo.PlacesVisited
            (PlaceDate,PlaceName,PlaceArea,PlaceElevation,PlaceDescription
            ,PlaceCoverImage,PlaceImages)
            values
            ('" + place.PlaceDate + @"',
            '" + place.PlaceName + @"',
            '" + place.PlaceArea + @"',
            '" + place.PlaceElevation + @"',
            '" + place.PlaceDescription + @"',
            '" + place.PlaceCoverImage + @"',
            '" + place.PlaceImages + @"')";
            DataTable table = new DataTable();
            //Define a Variable to Store Database Connection String
            string sqlDataSource = _configuration.GetConnectionString("FiretrekkersAppCon");
            SqlDataReader myReader;
            //Using Sql Connection and the SQl Command 
            //we will execute our query and fill the results into a DataTable
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); ;

                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("New Trek Added Successfully");
        }

        [HttpPut]
        public JsonResult put(Places place)
        {
            string query = @"
            update dbo.PlacesVisited set
            PlaceDate = '" + place.PlaceDate + @"',
            PlaceName = '" + place.PlaceName + @"',
            PlaceArea = '" + place.PlaceArea + @"',
            PlaceElevation = '" + place.PlaceElevation + @"',
            PlaceDescription = '" + place.PlaceDescription + @"',
            PlaceCoverImage = '" + place.PlaceCoverImage + @"',
            PlaceImages = '" + place.PlaceImages + @"'
            
            where PlaceId = " + place.PlaceId + @"
            ";
            DataTable table = new DataTable();
            //Define a Variable to Store Database Connection String
            string sqlDataSource = _configuration.GetConnectionString("FiretrekkersAppCon");
            SqlDataReader myReader;
            //Using Sql Connection and the SQl Command 
            //we will execute our query and fill the results into a DataTable
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); ;

                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Trek Data Updated Successfully");
        }


        //Since we are sending the ID in URL
        // We need to add it to Root Parameter
        [HttpDelete("{id}")]
        //Delete API will recive the id of the Place As Input
        public JsonResult Delete(int id)
        {
            string query = @"
            delete from dbo.PlacesVisited
            where PlaceId = " + id + @"
            ";
            DataTable table = new DataTable();
            //Define a Variable to Store Database Connection String
            string sqlDataSource = _configuration.GetConnectionString("FiretrekkersAppCon");
            SqlDataReader myReader;
            //Using Sql Connection and the SQl Command 
            //we will execute our query and fill the results into a DataTable
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); ;

                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Trek Data Deleted");
        }

        //Custom Route Name for Image Upload
        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string fileName = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/CoverPhotos/" + fileName;

                using(var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    //Save file to photos folder
                    postedFile.CopyTo(stream);
                }
                return new JsonResult(fileName);
            }
            catch(Exception)
            {

                return new JsonResult("annoumous.png");
            }

        }

        //Method to get all Places in Dropdown Menu
        [Route("GetAllPlaesNames")]
        public JsonResult GetAllPlaesNames()
        {
            string query = @"
            select PlaceName from dbo.PlacesVisited";
            DataTable table = new DataTable();
            //Define a Variable to Store Database Connection String
            string sqlDataSource = _configuration.GetConnectionString("FiretrekkersAppCon");
            SqlDataReader myReader;
            //Using Sql Connection and the SQl Command 
            //we will execute our query and fill the results into a DataTable
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); ;

                    myReader.Close();
                    myCon.Close();
                }
            }
            //Return DataTable as Json Result
            return new JsonResult(table);
        }
    }
}
