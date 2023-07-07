using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using API_Restaurante.Models;

namespace API_Restaurante.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeserosController : ControllerBase
    {
        private readonly IConfiguration _configuration;

       
        public MeserosController(IConfiguration configuration)
        {
            _configuration = configuration;

        }

        [HttpGet]
        public JsonResult Get()
        {

            string query = @"
                            select id, nombre, apellido, cedula from Mesero";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand sqlCommand = new SqlCommand(query, myCon))
                {
                    myReader = sqlCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);

        }

        [HttpPost]

        public JsonResult Post2(Fechas fechas)
        {
            string query = @"
                            select top 1 CONCAT(Mes.Nombre, ' ', MES.Apellido) AS Nombre, SUM(p.ValorPropina) as totalPropina 
                            from propinas as p 
                            inner join Mesas as M 
                            on p.idMesa = M.id
                            inner join Mesero as Mes
                            on p.idMesero= mes.id
                            WHERE fecha BETWEEN @fechaInicio AND @fechaFin
                            GROUP BY Mes.Nombre, mes.Apellido
                            ORDER BY totalPropina DESC;";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand sqlCommand = new SqlCommand(query, myCon))
                {
                    sqlCommand.Parameters.AddWithValue("@fechaInicio", fechas.FechaInicio);
                    sqlCommand.Parameters.AddWithValue("@fechaFin", fechas.FechaFin);
                    myReader = sqlCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }
    }
}
