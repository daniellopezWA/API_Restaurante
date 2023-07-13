using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using API_Restaurante.Models;
using System.Globalization;

namespace API_Restaurante.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropinasController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public PropinasController(IConfiguration configuration)
        {
            _configuration = configuration;

        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                            select id, idMesa, idMesero, ValorPropina, fecha 
                            from Propinas";
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

        [HttpGet("FullName")]
        public JsonResult Get1()
        {
            string query = @"
                            select CONCAT(Mes.Nombre, ' ', MES.Apellido) AS Nombre, m.descripccion as Mesa, ValorPropina as Valor, Fecha
                            from Propinas as p 
                            inner join Mesas as M 
                            on p.idMesa = M.id
                            inner join Mesero as Mes
                            on p.idMesero= mes.id";
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

        [HttpPost("Promedio")]
        public JsonResult Post1(Fechas fechas)
        {
            string formatoFecha = "yyyy-MM-dd";
            if (fechas.FechaInicio == null || fechas.FechaFin == null || !DateTime.TryParseExact(fechas.FechaInicio, formatoFecha, CultureInfo.InvariantCulture, DateTimeStyles.None, out _) || !DateTime.TryParseExact(fechas.FechaFin, formatoFecha, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))

            {
                Response.StatusCode = 400;
                return new JsonResult(new { error = "fechas invalidas o no Proporcionadas" });
            }
            string query = @"SELECT AVG(ValorPropina) AS Promedio FROM Propinas WHERE Fecha BETWEEN @fechaInicio AND @fechaFin ";
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

        [HttpPost]
        public JsonResult Post(Propina propina)
        {
            if (propina.idMesero == 0 | propina.idMesa== 0| propina.ValorPropina.ToString() == null | propina.Fecha == null)
            {
                Response.StatusCode = 400;
                return new JsonResult(new { error = "datos invalidos o no Proporcionados" });
            }
            string query = @"
                            insert into Propinas 
                            values (@idMesa, @idMesero, @ValorPropina, @Fecha)";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("RestaurantAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand sqlCommand = new SqlCommand(query, myCon)) 
                {
                    sqlCommand.Parameters.AddWithValue("@idMesa", propina.idMesa);
                    sqlCommand.Parameters.AddWithValue("@idMesero", propina.idMesero);
                    sqlCommand.Parameters.AddWithValue("@ValorPropina", propina.ValorPropina);
                    sqlCommand.Parameters.AddWithValue("@Fecha", propina.Fecha.ToString());
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
