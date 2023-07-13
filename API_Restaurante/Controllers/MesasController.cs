using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using API_Restaurante.Models;

namespace API_Restaurante.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MesasController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public MesasController(IConfiguration configuration)
        {
            _configuration = configuration;

        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                            select id, descripccion from Mesas";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("RestaurantAppCon");
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

        public JsonResult Post(Fechas fechas)
        {
            if (fechas.FechaInicio == null | fechas.FechaFin == null)
            {
                Response.StatusCode = 400;
                return new JsonResult(new { error = "fechas invalidas o no Proporcionadas" });
            }
            string query = @"
                            select top 1 m.descripccion, SUM(p.ValorPropina) as totalPropina 
                            from propinas as p 
                            inner join Mesas as M 
                            on p.idMesa = M.id
                            inner join Mesero as Mes
                            on p.idMesero= mes.id
                            WHERE fecha BETWEEN @fechaInicio AND @fechaFin
                            GROUP BY m.descripccion
                            ORDER BY totalPropina DESC;";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("RestaurantAppCon");
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
