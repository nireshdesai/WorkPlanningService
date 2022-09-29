using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WorkPlanningService.Models;

namespace WorkPlanningService.Controllers
{
    public class EmployeeShiftController : ApiController
    {
        WorkPlanning wp = new WorkPlanning();
        EmployeeShiftDetail ES = new EmployeeShiftDetail();
        string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
        // GET api/values
        public HttpResponseMessage Get()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SPGetEmployeeShiftDetail"))
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {

                        sda.Fill(dt);
                        List<EmployeeShiftDetail> lstEMPShift = new List<EmployeeShiftDetail>();
                        if (dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                EmployeeShiftDetail empshift = new EmployeeShiftDetail();
                                empshift.EmpName = dt.Rows[i]["EmpName"].ToString();
                                empshift.ShiftDate = dt.Rows[i]["ShiftDate"].ToString();
                                empshift.Shifttime = dt.Rows[i]["Shifttime"].ToString();
                                lstEMPShift.Add(empshift);
                            }
                        }
                        if (lstEMPShift.Count > 0)
                        {
                            
                            return Request.CreateResponse(HttpStatusCode.OK, lstEMPShift);
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.NoContent); ;
                        }
                    }
                }
            }

        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public HttpResponseMessage Post([FromBody] WorkPlanning wpo)
        {
            string msg = "";
            if (wpo == null)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Parameters is empty");
            }
            if (!ModelState.IsValid)
            { //checking model state               
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Parameter value is not valid");
            }
            else
            {
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (var command = new SqlCommand("SPCheckValidation", con))
                    {
                        con.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@EmpId", wpo.EmployeeId);
                        command.Parameters.AddWithValue("@Date", wpo.DateOfWorking);
                        int result = Convert.ToInt32(command.ExecuteScalar());
                        if(result==1)
                        {
                             return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Employee never has multiple shift on same day. ");
                        }
                        con.Close();

                    }
                }
            }

            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SPSaveEmployeeShiftTime"))
                {
                    con.Open();
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EmpId", wpo.EmployeeId);
                    cmd.Parameters.AddWithValue("@Date", wpo.DateOfWorking);
                    cmd.Parameters.AddWithValue("@ShiftId", wpo.ShiftId);
                    try
                    {
                        int status = 0;
                        status = cmd.ExecuteNonQuery();
                        if (status > 0)
                            msg = "Work planning details added successfully.";
                        else
                            msg = "Something went wrong !";
                    }
                    catch (SqlException ex)
                    {

                        Console.WriteLine(ex.Message);

                        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);

                    }
                    con.Close();


                }
            }

            return Request.CreateResponse(HttpStatusCode.OK, msg);


        }

        // PUT api/values/5
        public string Put(int id, [FromBody] WorkPlanning workplanningupdate)
        {
            if (workplanningupdate == null)
            {
                return "Parameters is empty";
            }

            string msg = "";
            try
            {

                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("SPUpdateEmployeeShiftTime"))
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EmpId", id);
                        cmd.Parameters.AddWithValue("@Date", workplanningupdate.DateOfWorking);
                        cmd.Parameters.AddWithValue("@ShiftId", workplanningupdate.ShiftId);
                        int status = 0;
                        status = cmd.ExecuteNonQuery();
                        if (status > 0)
                            msg = "Work planning details updated successfully.";
                        else
                            msg = "No Records Updated !";
                        con.Close();


                    }
                }

                return msg;

            }

            catch (SqlException ex)
            {

                Console.WriteLine(ex.Message);

                return "Error while update record :" + ex.Message;

            }

        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
